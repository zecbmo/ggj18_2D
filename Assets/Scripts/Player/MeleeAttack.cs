using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MeleeAttack : MonoBehaviour
{

    [SerializeField]
    float meleeAttackDuration;

    Collider2D meleeAttackCollider;
    SpriteRenderer weaponSpriteRenderer;
    MarioMovement marioMovement;
    bool attacking;
    Vector2 weaponPosition;

    void Awake()
    {
        meleeAttackCollider = GetComponent<Collider2D>();
        Assert.IsNotNull(meleeAttackCollider);

        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        Assert.IsNotNull(weaponSpriteRenderer);

        marioMovement = transform.parent.GetComponent<MarioMovement>();
        Assert.IsNotNull(marioMovement);
    }

    void Start()
    {
        weaponSpriteRenderer.enabled = false;
        weaponPosition = transform.localPosition;    
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
        if (!attacking)
        {
            weaponPosition.x = marioMovement.IsLookingRight() == true ? Mathf.Abs(weaponPosition.x) : -Mathf.Abs(weaponPosition.x);
            transform.localPosition = weaponPosition;
            StartCoroutine(MeleeAttackCoroutine());
        }
    }

    IEnumerator MeleeAttackCoroutine()
    {
        float startTime = Time.time; // Change this if we use custom Time class

        // Enable melee collider
        meleeAttackCollider.enabled = true;
        weaponSpriteRenderer.enabled = true;
        attacking = true;

        while (Time.time - startTime < meleeAttackDuration)
        {
            yield return false;
        }

        // Disable it after the duration
        meleeAttackCollider.enabled = false;
        weaponSpriteRenderer.enabled = false;
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IHittable otherIHittable = collider.gameObject.GetComponent<IHittable>();
        if (otherIHittable != null && collider.transform.parent != transform.parent)
        {
            otherIHittable.OnHit(gameObject);
        }
    }
}
