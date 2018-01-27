using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ReceiveDamage : MonoBehaviour, IHittable
{

    private MarioMovement marioMovement;

    [SerializeField]
    private Vector2 knockbackForce = Vector2.zero;
    [SerializeField]
    private float knockbackTime = 0f;

    private void Awake()
    {
        marioMovement = GetComponentInParent<MarioMovement>();
        Assert.IsNotNull(marioMovement);
    }

    public void OnHit(GameObject otherObject)
    {
        marioMovement.ApplyKnockback(new Vector2((otherObject.transform.position.x > this.transform.position.x)
            ? -knockbackForce.x : knockbackForce.x, knockbackForce.y),
            knockbackTime);

    }


}
