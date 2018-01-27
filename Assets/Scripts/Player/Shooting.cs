using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    float currentAngle;
    
    [SerializeField]
    Vector2 vector;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            float deltaPressure = Time.deltaTime * pressureGrowthSpeed;
            weaponPressure = Mathf.Lerp(0, maximumPressure, (weaponPressure + deltaPressure) / maximumPressure);

            currentAngle = weaponAngle * (1 - weaponPressure / maximumPressure);
            vector = Quaternion.AngleAxis(currentAngle, Vector3.forward) * vector;
        }
        else if (Input.GetButtonUp("Fire1"))
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
            if (Input.GetButton("Fire1"))
            {
                break;
            }
            float deltaPressure = (Time.time - startTime) * pressureLossSpeed;

            weaponPressure = Mathf.Lerp(initialPressure, 0, deltaPressure / initialPressure);

            currentAngle = weaponAngle * (1 - weaponPressure / maximumPressure);
            vector = Quaternion.AngleAxis(currentAngle, Vector3.forward) * vector;

            yield return false;
        }
    }
}
