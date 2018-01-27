using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTowerInteraction : MonoBehaviour
{
    TowerCollisionHelper collisionObject = null;
    [SerializeField]
    PlayerContainer container = null;

    enum WhatCollisonType { RemovingWater, AddingWater};
    WhatCollisonType type = WhatCollisonType.RemovingWater;


    private void Update()
    {
        if (collisionObject && InputManager.GetButton(GameControls.FillWater, 0))
        {
            switch (type)
            {
                case WhatCollisonType.RemovingWater:
                    {
                        container.AddFluid(5);
                        collisionObject.RemoveWater(5);
                        container.fluidBeingRemoved = false;                       
                    }
                    break;
                case WhatCollisonType.AddingWater:
                    {
                        collisionObject.AddWater(5);
                        container.RemoveFluid(5);
                        container.fluidBeingRemoved = true;

                    }
                    break;
                default:
                    break;
            }
            
        }
        else if (collisionObject)
        {
            collisionObject.ResetWaterAnim();
            container.fluidBeingRemoved = false;

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
            if (collisionObject)
            {
                collisionObject.ResetWaterAnim();
                container.fluidBeingRemoved = false;

                collisionObject = null;
            }
        }
        else if (collision.gameObject.tag == "AdditionArea")
        {
            if (collisionObject)
            {
                collisionObject.ResetWaterAnim();
                container.fluidBeingRemoved = false;

                collisionObject = null;
            }
        }
    }

}
