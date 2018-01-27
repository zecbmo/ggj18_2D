using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to the collision component of the tower. Gives the players easier access to the specific tower functions
/// </summary>
public class TowerCollisionHelper : MonoBehaviour
{
    [SerializeField]
    Tower tower;

    public float RemoveWater(float speed)
    {
        return tower.RemoveFluid(speed);
    }

    public float AddWater(float speed)
    {
        return tower.AddFluid(speed);
    }

    public void ResetWaterAnim()
    {
        tower.fluidBeingRemoved = false;
    }
}
