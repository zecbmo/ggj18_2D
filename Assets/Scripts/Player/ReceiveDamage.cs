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
    [SerializeField]
    AudioClip HitSFX = null;

    [SerializeField]
    int numeberOfHitsbeforeReSpawn = 3;

    [SerializeField]
    float percentToDropWhenHIt = 25.0f;

    Tower container = null;

 
    int hitamount = 0;


    private void Awake()
    {
        container = GetComponentInParent<Tower>();
        marioMovement = GetComponentInParent<MarioMovement>();
        Assert.IsNotNull(marioMovement);
    }

    public void OnHit(GameObject otherObject)
    {


        container.RemoveHardAmount(percentToDropWhenHIt);
        AudioManager.Instance().PlaySFX(HitSFX, 0.2f);


        hitamount++;
        if (hitamount > numeberOfHitsbeforeReSpawn)
        {
            container.RemoveHardAmount(200);
            hitamount = 0;
            if(GameManager.Exist)
                            GameManager.Instance().RespawnPlayer(gameObject.transform.parent.gameObject);
            return;
        }

        marioMovement.ApplyKnockback(new Vector2((otherObject.transform.position.x > this.transform.position.x)
            ? -knockbackForce.x : knockbackForce.x, knockbackForce.y),
            knockbackTime);

        

    }

   


}
