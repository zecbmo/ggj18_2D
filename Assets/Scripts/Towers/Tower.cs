using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a tower object that will be placed into the scene. 
/// Players will be able to collect water/mineral from these towers
/// </summary>
public class Tower : MonoBehaviour {


    [SerializeField, Range(0,100)]
    float percentageFilled = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    float removeFluid(float removalSpeed)
    {
        float amountRemoved = 0;


        return amountRemoved;
    }
}
