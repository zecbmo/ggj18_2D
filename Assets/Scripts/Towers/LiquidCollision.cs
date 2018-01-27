using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidCollision : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Hitting player");    
        }
    }


}
