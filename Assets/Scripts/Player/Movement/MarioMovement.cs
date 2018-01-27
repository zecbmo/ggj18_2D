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
    [SerializeField, Range(0, 500)]
    private float wallSlidingSpeed = 200f;

    private bool touchingWall = false;
    private bool wallSliding = false;
    private bool wallToTheRight = true;

    [Header("Other parameters")]
    [SerializeField]
    private LayerMask groundedLayerMask;




    /// <summary>
    /// The rigid body we a trying to move
    /// </summary>
    private Rigidbody2D rigidBody = null;

    /// <summary>
    /// The collider that represents out character
    /// </summary>
    private BoxCollider2D boxCollider = null;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        Assert.IsNotNull(rigidBody);

        boxCollider = GetComponent<BoxCollider2D>();
        Assert.IsNotNull(boxCollider);

        originalGravity = rigidBody.gravityScale;
    }

    private void Update()
    {
        bool jumpButtonDown = Input.GetButtonDown("Jump");
        bool jumpButtonUp = Input.GetButtonUp("Jump");
        bool pressedHorizontal = Input.GetButtonDown("Horizontal");


        if (jumpButtonDown && grounded)
        {
            jumping = true;
            jumped = true;
            jumpTimestamp = Time.time;
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
        grounded = IsGrounded();
        touchingWall = IsTouchingWall();
        wallSliding = false;


        float horizontalInput = Input.GetAxis("Horizontal");
        bool pressedTowardsWall = (horizontalInput > 0 && wallToTheRight) || (horizontalInput < 0 && !wallToTheRight);


        /*
        We input the horizontal speed that we desire based
        on the state of movement we are in
         */
         
        if (touchingWall &&
            !grounded &&
            pressedTowardsWall &&
            !jumped &&
            rigidBody.velocity.y < 0)
        {
            
            wallSliding = true;

            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -wallSlidingSpeed * Time.deltaTime);

        }
        else if (Input.GetButton("Sprint") &&
            Time.time - sprintTimeStamp > sprintDelay &&
            grounded || jumpedOnSprint)
        {
                rigidBody.velocity = new Vector2(horizontalInput * movementSpeed * sprintMultiplier * Time.deltaTime, rigidBody.velocity.y);

            if (jumped)
            {
                jumpedOnSprint = true;
            }
        }
        else
        {
                rigidBody.velocity = new Vector2(horizontalInput * movementSpeed * Time.deltaTime, rigidBody.velocity.y);
        }


        bool currentLookingRight = (rigidBody.velocity.x > 0);
        if (currentLookingRight != lookingRight && rigidBody.velocity.x != 0)
        {
            switchedDirections = true;
            lookingRight = currentLookingRight;
        }


        /*
         We add the initial jumping force, this is the
         "real" jump, the initial one.
         */
        if (jumped)
        {
            rigidBody.AddForce(new Vector2(0, initialJumpForce));
            jumped = false;
        }

        /*
         We add a bit of extra force if the button is still pressed.
         */
        if (jumping && (Time.time - jumpTimestamp) > delayForExtraForce && !wallSliding)
        {
            rigidBody.AddForce(new Vector2(0, extraJumpForce));
        }

        /*
         When the character is falling we multiply its gravity by
         a multiplier to make the jump feel more Super Marioish.
         */
        if (rigidBody.velocity.y < 0 && !wallSliding)
        {
            rigidBody.gravityScale = originalGravity * gravityMultiplier;
        }
        else
        {
            rigidBody.gravityScale = originalGravity;
        }

    }

    public bool IsLookingRight()
    {
        return lookingRight;
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
