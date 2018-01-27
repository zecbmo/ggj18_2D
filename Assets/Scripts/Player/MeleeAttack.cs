using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MeleeAttack : MonoBehaviour
{

    [SerializeField]
    float meleeAttackDuration;

    Collider2D meleeAttackCollider;

    void Awake()
    {
        meleeAttackCollider = GetComponent<Collider2D>();
        Assert.IsNotNull(meleeAttackCollider);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            PerformMeleeAttack();
        }
    }

    void PerformMeleeAttack()
    {
        StartCoroutine(MeleeAttackCoroutine());
    }

    IEnumerator MeleeAttackCoroutine()
    {
        float startTime = Time.time; // Change this if we use custom Time class

        // Enable melee collider
        meleeAttackCollider.enabled = true;

        while (Time.time - startTime < meleeAttackDuration)
        {
            yield return false;
        }

        // Disable it after the duration
        meleeAttackCollider.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IHittable otherIHittable = collision.gameObject.GetComponent<IHittable>();
        if (otherIHittable != null)
        {
            otherIHittable.OnHit(gameObject);
        }
    }
}
