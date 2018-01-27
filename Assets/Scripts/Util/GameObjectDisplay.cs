using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDisplay : MonoBehaviour
{

    [SerializeField]
    Color color = Color.blue;

    [SerializeField]
    float radius = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
