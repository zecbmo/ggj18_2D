using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiveDamage : MonoBehaviour, IHittable
{
    public void OnHit(GameObject otherObject)
    {
        Debug.Log("[" + gameObject.name + "] Got hit by \"" + otherObject.name + "\"");
    }
}
