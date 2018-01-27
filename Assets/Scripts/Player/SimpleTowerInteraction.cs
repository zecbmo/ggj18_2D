using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTowerInteraction : MonoBehaviour
{
    TowerCollisionHelper collisionObject = null;


    enum WhatCollisonType { RemovingWater, AddingWater};
    WhatCollisonType type = WhatCollisonType.RemovingWater;


    private void Update()
    {
        if (collisionObject && Input.GetKey(KeyCode.X))
        {
            switch (type)
            {
                case WhatCollisonType.RemovingWater:
                    collisionObject.RemoveWater(5);
                    break;
                case WhatCollisonType.AddingWater:
                    collisionObject.AddWater(5);
                    break;
                default:
                    break;
            }
            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "RemovableArea")
        {
            collisionObject = collision.gameObject.GetComponent<TowerCollisionHelper>();
            type = WhatCollisonType.RemovingWater;
        }
        else if (collision.gameObject.tag == "AdditionArea")
        {
            collisionObject = collision.gameObject.GetComponent<TowerCollisionHelper>();
            type = WhatCollisonType.AddingWater;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "RemovableArea")
        {
            collisionObject = null;
        }
        else if (collision.gameObject.tag == "AdditionArea")
        {
            collisionObject = null;
        }
    }

}
