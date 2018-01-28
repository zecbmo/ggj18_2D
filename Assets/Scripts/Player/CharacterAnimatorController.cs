using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{

    [SerializeField]
    private float runDivider = 2.0f;

    private MarioMovement marioMovement;

    private Animator animator;

    private float initialScale;

    // Use this for initialization
    void Awake()
    {

        marioMovement = GetComponentInParent<MarioMovement>();
        UnityEngine.Assertions.Assert.IsNotNull(marioMovement);

        animator = GetComponentInChildren<Animator>();
        UnityEngine.Assertions.Assert.IsNotNull(animator);


    }

    private void Start()
    {
        initialScale = this.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        float velocity = marioMovement.GetRigidBodyVelocity();

        animator.SetBool("isGrounded", marioMovement.IsGrounded());
        animator.SetBool("isWallSliding", marioMovement.IsWallSliding());
        animator.SetFloat("speed", Mathf.Abs(velocity));
        animator.SetFloat("runMult", Mathf.Clamp(Mathf.Abs(velocity / runDivider),0.5f,float.MaxValue));


        if (marioMovement.IsLookingRight())
        {
            this.transform.localScale = new Vector3(initialScale, transform.localScale.y, transform.localScale.z );
        }
        else
        {
            this.transform.localScale = new Vector3(-initialScale, transform.localScale.y, transform.localScale.z);
        }


    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
}
