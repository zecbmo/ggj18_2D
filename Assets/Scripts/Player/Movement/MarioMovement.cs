using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

/// <summary>
/// 
/// Component for a modified Super Mario like movement
/// for a 2D plattformer uning Unity's 2D physics system.
/// 
/// </summary>
public class MarioMovement : MonoBehaviour
{


    [Header("Horizontal movement parameters")]
    [SerializeField, Range(0, 500)]
    private float movementSpeed = 200.0f;

    private bool switchedDirections = false;
    private bool lookingRight = true;


    [Header("Jumping parameters")]
    [SerializeField, Range(0, 500)]
    private float initialJumpForce = 0.0f;

    [SerializeField, Range(0, 40)]
    private float extraJumpForce = 0.0f;

    [SerializeField, Range(0, 1.0f)]
    private float extraJumpTime = 0.3f;

    [SerializeField, Range(0, 0.5f)]
    private float delayForExtraForce = 0.05f;

    [SerializeField, Range(1, 5)]
    private float gravityMultiplier = 2.0f;

    [SerializeField, Range(0, 20)]
    private float maxVerticalVelocity = 10f;

    private bool jumping = false;
    private bool jumped = false;
    private float jumpTimestamp = -1.0f;
    private float originalGravity = 1.0f;

    private const float colliderThreshold = 0.1f;
    private bool grounded = false;


    [Header("Sprinting parameters")]
    [SerializeField, Range(1, 3)]
    private float sprintMultiplier = 1.3f;
    [SerializeField, Range(0, 0.5f)]
    private float sprintDelay = 0.3f;

    private float sprintTimeStamp = -1.0f;
    private bool jumpedOnSprint = false;

    [Header("Wall jumping parameters")]
    [SerializeField, Range(0, -1.0f)]
    private float wallSlidingTargetSpeed = -0.2f;
    [SerializeField, Range(0, 1.0f)]
    private float wallSlidingTime = 1.0f;

    [SerializeField, Range(0, 1.0f)]
    private float wallJumpControlDelay = 0.2f;
    [SerializeField, Range(0, 500)]
    private float wallJumpUpForce = 200f;
    [SerializeField, Range(0, 500)]
    private float wallJumpSideForce = 200f;
    private float wallJumpTimeStamp = -1.0f;
    private bool wallJumpedRight = true;

    private float wallSlideTimeStamp = -1.0f;
    private float wallSlideInitialVelocity = -1.0f;

    private bool touchingWall = false;
    private bool wallSliding = false;
    private bool wallToTheRight = true;

    /*
     
     @@DOING: Apply a force up and out from the wall
     if you are sliding and you press jump. Use a timer
     to keep the player from changing directions too quickly
     so it can actually go up from the wall.
         
     */


    [Header("Other parameters")]
    [SerializeField]
    private LayerMask groundedLayerMask;
    [SerializeField]
    private int playerId = 0;

    private int controllerId = -1;


    private bool inKnockback = false;
    private bool canMoveCharacter = true;

    /// <summary>
    /// The rigid body we a trying to move
    /// </summary>
    private Rigidbody2D rigidBody = null;

    /// <summary>
    /// The collider that represents out character
    /// </summary>
    private BoxCollider2D boxCollider = null;

    private CharacterAnimatorController animatorController = null;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidBody);

        boxCollider = GetComponent<BoxCollider2D>();
        Assert.IsNotNull(boxCollider);

        animatorController = GetComponentInChildren<CharacterAnimatorController>();
        Assert.IsNotNull(animatorController);

        originalGravity = rigidBody.gravityScale;
    }

    private void Update()
    {

        if (controllerId < 0)
        {
            return;
        }
        bool jumpButtonDown = InputManager.GetButtonDown(GameControls.Jump, controllerId);
        bool jumpButtonUp = InputManager.GetButtonUp(GameControls.Jump, controllerId);
        bool pressedHorizontal = InputManager.GetButtonDown(AxisControls.Horizontal, controllerId);

        if (jumpButtonDown && (grounded || touchingWall))
        {
            jumping = true;
            jumped = true;
            jumpTimestamp = Time.time;
            animatorController.TriggerJump();
        }

        if (jumpButtonUp || (Time.time - jumpTimestamp) > extraJumpTime)
        {
            jumping = false;
        }


        if (pressedHorizontal || switchedDirections)
        {
            sprintTimeStamp = Time.time;
            jumpedOnSprint = false;
        }

        switchedDirections = false;

    }

    private void FixedUpdate()
    {
        if (controllerId < 0)
        {
            controllerId = PlayerManager.Instance().GetControllerByPlayerId(playerId);
            if (controllerId < 0)
            {
                return;
            }

//            GetComponent<Renderer>().enabled = true;
            

        }
        grounded = IsGrounded();
        touchingWall = IsTouchingWall();
        rigidBody.sharedMaterial.friction = 0.0f;

        float horizontalInput = InputManager.GetAxis(AxisControls.Horizontal, controllerId);
        bool pressedTowardsWall = (horizontalInput > 0 && wallToTheRight) || (horizontalInput < 0 && !wallToTheRight);

        bool inWallJumpDelay = (Time.time - wallJumpTimeStamp) < wallJumpControlDelay;

        /*
         This is in case we want to add more movement mechanics
         that would keep the player from controlling the character.

         E.g. a dash mechanic or some debuff.
         */
        canMoveCharacter = (!inWallJumpDelay &&
                                    !inKnockback);


        if (canMoveCharacter)
        {   // --- MOVEMENT SECTION ---
            if (touchingWall &&
                !grounded &&
                pressedTowardsWall &&
                !jumped &&
                rigidBody.velocity.y < 0)
            {   // --- Wall Sliding ---

                if (wallSliding == false)
                {
                    wallSlideTimeStamp = Time.time;
                    wallSliding = true;
                    wallSlideInitialVelocity = rigidBody.velocity.y;
                }

                rigidBody.velocity = new Vector2(rigidBody.velocity.x,
                    Mathf.Lerp(wallSlideInitialVelocity,
                    wallSlidingTargetSpeed,
                    (Time.time - wallSlideTimeStamp) / wallSlidingTime));

            }
            else
            {
                wallSliding = false;


                if (InputManager.GetButton(GameControls.Sprint, controllerId) &&
                   Time.time - sprintTimeStamp > sprintDelay &&
                   grounded || jumpedOnSprint)
                {   // --- Sprinting Movement ---
                    rigidBody.velocity = new Vector2(horizontalInput * movementSpeed * sprintMultiplier * Time.deltaTime, rigidBody.velocity.y);

                    if (jumped)
                    {
                        jumpedOnSprint = true;
                    }
                }
                else
                {   // --- Normal Movement ---
                    rigidBody.velocity = new Vector2(horizontalInput * movementSpeed * Time.deltaTime, rigidBody.velocity.y);
                }
            }
        }
        else
        {  // --- We can't control the character ---

            if (inWallJumpDelay)
            {
                rigidBody.velocity = new Vector2((wallJumpedRight ? 1 : -1) * movementSpeed * Time.deltaTime, rigidBody.velocity.y);
            }

        }

        /*
         We update the position that we re looking towards
         */
        bool currentLookingRight = (horizontalInput > 0);
        if (currentLookingRight != lookingRight && horizontalInput != 0)
        {
            switchedDirections = true;
            lookingRight = currentLookingRight;
        }

        /*
         We deal with the initial jumping forces in this section
         */
        if (touchingWall &&
            !grounded &&
            jumped)
        {   // --- Wall Jumping ---
            wallJumpTimeStamp = Time.time;

            wallJumpedRight = !wallToTheRight;

            rigidBody.AddForce(new Vector2(0 /*(lookingRight) ? -wallJumpSideForce : wallJumpSideForce*/, wallJumpUpForce));
            jumped = false;
        }
        else if (jumped)
        {   // --- Normal Jumping ---
            /*
             We add the initial jumping force, this is the
             "real" jump, the initial one.
             */

            rigidBody.AddForce(new Vector2(0, initialJumpForce));
            jumped = false;
        }

        /*
         We add a bit of extra force if the button is still pressed.
         */
        if (jumping && (Time.time - jumpTimestamp) > delayForExtraForce && !wallSliding)
        {   // --- Extra Jumping Force ---
            rigidBody.AddForce(new Vector2(0, extraJumpForce));
        }

        /*
         When the character is falling we multiply its gravity by
         a multiplier to make the jump feel more Super Marioish.
         */
        if (rigidBody.velocity.y < 0 && !wallSliding)
        {   // --- Falling with faster Gravity ---
            rigidBody.gravityScale = originalGravity * gravityMultiplier;
        }
        else
        {
            rigidBody.gravityScale = originalGravity;
        }



        if (Mathf.Abs(rigidBody.velocity.y) > maxVerticalVelocity)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,
                rigidBody.velocity.y > 0 ? maxVerticalVelocity : -maxVerticalVelocity);
        }

    }

    public bool IsLookingRight()
    {
        return lookingRight;
    }

    public int GetControllerId()
    {
        return PlayerManager.Instance().GetControllerByPlayerId(playerId);
    }

    public void SetPlayerId(int id)
    {
        playerId = id;
    }

    public bool IsGrounded()
    {
        /*
         We check if a square from the center of our body to
         our "feet" is "touching" the another collider that we can
         consider ground.
         */


        return Physics2D.OverlapArea(
            new Vector3(boxCollider.bounds.min.x + colliderThreshold,
                boxCollider.bounds.center.y,
                boxCollider.bounds.center.z),
            new Vector3(boxCollider.bounds.max.x - colliderThreshold,
                boxCollider.bounds.min.y - colliderThreshold,
                boxCollider.bounds.center.z),
            groundedLayerMask);

    }

    public bool IsTouchingWall()
    {

        bool result = false;
        /*
         We check if a square from the center of our body to
         our "face" is "touching" the another collider that we can
         consider a wall.
         */


        if (lookingRight)
        {

            result = Physics2D.OverlapArea(
                           new Vector3(boxCollider.bounds.center.x,
                               boxCollider.bounds.max.y - colliderThreshold,
                               boxCollider.bounds.center.z),
                           new Vector3(boxCollider.bounds.max.x + colliderThreshold,
                               boxCollider.bounds.min.y + colliderThreshold,
                               boxCollider.bounds.center.z),
                           groundedLayerMask);

            wallToTheRight = true;

        }
        else
        {
            result = Physics2D.OverlapArea(
                            new Vector3(boxCollider.bounds.min.x - colliderThreshold,
                                boxCollider.bounds.max.y - colliderThreshold,
                                boxCollider.bounds.center.z),
                            new Vector3(boxCollider.bounds.center.x,
                                boxCollider.bounds.min.y + colliderThreshold,
                                boxCollider.bounds.center.z),
                            groundedLayerMask);

            wallToTheRight = false;

        }

        return result;
    }

    public void ApplyKnockback(Vector2 force, float time)
    {
        rigidBody.AddForce(force);
        StartCoroutine(KnockbackCoroutine(time));
    }

    IEnumerator KnockbackCoroutine(float time)
    {
        inKnockback = true;
        yield return new WaitForSeconds(time);
        inKnockback = false;
    }

    public bool GetCanMoveCharacter()
    {
        return canMoveCharacter;
    }


    public bool IsWallSliding() {
        return wallSliding;
    }


    public float GetRigidBodyVelocity() {
        return rigidBody.velocity.x;
    }

    /*
     @@TODO: If there is time.
     */
    public void StartSliding()
    {

    }

    public void EndSliding()
    {

    }

}
