using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Shooting : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    float maximumPressure;

    [SerializeField, Range(0, 200)]
    float pressureGrowthSpeed;

    [SerializeField, Range(0, 200)]
    float pressureLossSpeed;

    [SerializeField]
    float weaponPressure = 0f;

    [SerializeField]
    float weaponAngle;

    MarioMovement marioMovement;

    float currentAngle;
    Ray shootingRay;

    void Awake()
    {
        marioMovement = transform.parent.GetComponent<MarioMovement>();
        Assert.IsNotNull(marioMovement);
    }

    void Start()
    {
        UpdateWeaponAngle();

    }

    void Update()
    {

        if (InputManager.GetButton(GameControls.Hit, 0))
        {
            float deltaPressure = Time.deltaTime * pressureGrowthSpeed;
            weaponPressure = Mathf.Lerp(0, maximumPressure, (weaponPressure + deltaPressure) / maximumPressure);
            UpdateWeaponAngle();
        }
        else if (InputManager.GetButtonUp(GameControls.Hit, 0))
        {
            StartCoroutine(StopShootingCoroutine());
        }

        
    }


    IEnumerator StopShootingCoroutine()
    {
        float initialPressure = weaponPressure;
        float startTime = Time.time;

        while (weaponPressure > 0f)
        {
            if (InputManager.GetButton(GameControls.Hit, 0))
            {
                break;
            }

            float deltaPressure = (Time.time - startTime) * pressureLossSpeed;

            weaponPressure = Mathf.Lerp(initialPressure, 0, deltaPressure / initialPressure);

            UpdateWeaponAngle();

            yield return false;
        }
    }

    void UpdateWeaponAngle()
    {
        Vector3 vector = new Vector3();
        currentAngle = -weaponAngle * (1 - weaponPressure / maximumPressure);
        vector.z = currentAngle;
        transform.eulerAngles = vector;

        UpdateShootingRay();
    }

    void UpdateShootingRay()
    {
        float radians = currentAngle * Mathf.Deg2Rad;
        int sign = marioMovement.IsLookingRight() ? 1 : -1;
        Vector2 weaponPosition = transform.localPosition;
        weaponPosition.x = marioMovement.IsLookingRight() == true ? Mathf.Abs(weaponPosition.x) : -Mathf.Abs(weaponPosition.x);
        transform.localPosition = weaponPosition;
        shootingRay = new Ray(transform.position, new Vector3(Mathf.Cos(radians) * sign, Mathf.Sin(radians), 0));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(shootingRay);
    }
}
