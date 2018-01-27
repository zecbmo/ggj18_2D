using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MeleeAttack : MonoBehaviour
{

    [SerializeField]
    float meleeAttackDuration;

    [SerializeField]
    float meleeAttackCooldown;


    Collider2D meleeAttackCollider;
    SpriteRenderer weaponSpriteRenderer;
    MarioMovement marioMovement;
    Vector2 weaponPosition;
    bool attackOnCooldown = false;
    bool attacking;

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
        if (marioMovement.GetControllerId() < 0)
        {
            return;
        }
        if (InputManager.GetButtonDown(GameControls.Hit, marioMovement.GetControllerId()))
        {
            PerformMeleeAttack();
        }
    }

    void PerformMeleeAttack()
    {
        if (!attacking && !attackOnCooldown && marioMovement.GetCanMoveCharacter())
        {
            weaponPosition.x = marioMovement.IsLookingRight() == true ? Mathf.Abs(weaponPosition.x) : -Mathf.Abs(weaponPosition.x);
            transform.localPosition = weaponPosition;
            StartCoroutine(MeleeAttackCoroutine());
            StartCoroutine(MeleeAttackCooldown(meleeAttackCooldown));
        }
    }

    IEnumerator MeleeAttackCooldown(float cooldown)
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        attackOnCooldown = false;
    }

    IEnumerator MeleeAttackCoroutine()
    {
        // Enable melee collider
        meleeAttackCollider.enabled = true;
        weaponSpriteRenderer.enabled = true;
        attacking = true;

        yield return new WaitForSeconds(meleeAttackDuration);

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
