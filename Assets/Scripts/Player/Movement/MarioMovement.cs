using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class MarioMovement : MonoBehaviour
{

    [SerializeField]
    private LayerMask groundedLayerMask;

    [Header("Horizontal movement parameters")]
    [SerializeField, Range(0, 500)]
    private float movementSpeed = 1.0f;

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
        bool grounded = IsGrounded();
        bool jumpButtonDown = Input.GetButtonDown("Jump");
        bool jumpButtonUp = Input.GetButtonUp("Jump");


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
        
    }

    private void FixedUpdate()
    {

        float horizontalInput = Input.GetAxis("Horizontal");

        /*
        We input the horizontal speed that we desire. 
         */
        rigidBody.velocity = new Vector2(horizontalInput * movementSpeed * Time.deltaTime, rigidBody.velocity.y);

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
        if (jumping && (Time.time - jumpTimestamp) > delayForExtraForce)
        {
            rigidBody.AddForce(new Vector2(0, extraJumpForce));
        }

        /*
         When the character is falling we multiply its gravity by
         a multiplier to make the jump feel more Super Marioish.
         */
        if (rigidBody.velocity.y < 0)
        {
            rigidBody.gravityScale = originalGravity * gravityMultiplier;
        }
        else {
            rigidBody.gravityScale = originalGravity;
        }

    }

    public bool IsGrounded()
    {
        /*
         We check if a square from the center of our body to
         our "feet" is "touching" the another collider that we can
         consider ground.
         */

        return Physics2D.OverlapArea(
            new Vector3(boxCollider.bounds.min.x,
                boxCollider.bounds.center.y,
                boxCollider.bounds.center.z),
            new Vector3(boxCollider.bounds.max.x,
                boxCollider.bounds.min.y - 0.1f,
                boxCollider.bounds.center.z),
            groundedLayerMask);

    }



}
