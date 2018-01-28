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

    [SerializeField]
    AudioClip punchAttemptSFX = null;

    [SerializeField]
    float punchSFXModulation = 0.2f;

    Collider2D meleeAttackCollider;
    MeshRenderer weaponMeshRenderer;
    MarioMovement marioMovement;
    Vector2 weaponPosition;
    bool attackOnCooldown = false;
    bool attacking;

    private Vector3 originalScale;
    private Vector3 mirroredScale;


    void Awake()
    {
        meleeAttackCollider = GetComponent<Collider2D>();
        Assert.IsNotNull(meleeAttackCollider);

        weaponMeshRenderer = GetComponentInChildren<MeshRenderer>();
        Assert.IsNotNull(weaponMeshRenderer);

        marioMovement = transform.parent.GetComponent<MarioMovement>();
        Assert.IsNotNull(marioMovement);
    }

    void Start()
    {
        weaponMeshRenderer.enabled = false;
        weaponPosition = transform.localPosition;
        originalScale = this.transform.localScale;
        mirroredScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
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
            AudioManager.Instance().PlaySFX(punchAttemptSFX, punchSFXModulation);
            this.transform.localScale = marioMovement.IsLookingRight() == true ? originalScale : mirroredScale;

            StartCoroutine(MeleeAttackCoroutine());
            StartCoroutine(MeleeAttackCooldown(meleeAttackCooldown));
        }
    }

    private void OnEnable()
    {
        meleeAttackCollider.enabled = false;
        weaponMeshRenderer.enabled = false;
        attacking = false;
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
        weaponMeshRenderer.enabled = true;
        attacking = true;

        yield return new WaitForSeconds(meleeAttackDuration);

        // Disable it after the duration
        meleeAttackCollider.enabled = false;
        weaponMeshRenderer.enabled = false;
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
