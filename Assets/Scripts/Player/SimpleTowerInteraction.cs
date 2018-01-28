using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SimpleTowerInteraction : MonoBehaviour
{
    TowerCollisionHelper collisionObject = null;
    [SerializeField]
    PlayerContainer container = null;

    MarioMovement marioMovement;

    enum WhatCollisonType { RemovingWater, AddingWater };
    WhatCollisonType type = WhatCollisonType.RemovingWater;

    MarioMovement player;

    [SerializeField]
    AudioClip bubbleSFX;
    [SerializeField]
    AudioSource dribleSFX;
    [SerializeField]
    float dribbleVolume = 1.0f;

    bool playSFX = true;

    private void Start()
    {
        player = GetComponent<MarioMovement>();
    }

    private void Update()
    {
        dribleSFX.volume = 0;

        if (collisionObject && InputManager.GetButton(GameControls.FillWater, player.controllerId))
        {


            dribleSFX.volume = AudioManager.Instance().MasterVolume * AudioManager.Instance().SXFVolume * dribbleVolume;

            switch (type)
            {
                case WhatCollisonType.RemovingWater:
                    {
                        if (container.GetPercentageFilled() >= 99)
                        {
                            playSFX = true;
                            collisionObject.ResetWaterAnim();
                            container.fluidBeingRemoved = false;
                            return;
                        }


                        container.AddFluid(5);
                        collisionObject.RemoveWater(5);
                        container.fluidBeingRemoved = false;
                        if (playSFX)
                        {
                            AudioManager.Instance().PlaySFX(bubbleSFX, 0.2f);
                            playSFX = false;
                        }
                                           
                    }
                    break;
                case WhatCollisonType.AddingWater:
                    {
                        if (container.GetPercentageFilled() <= 0)
                        {
                            return;
                            container.fluidBeingRemoved = false;
                        }

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
            playSFX = true;
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
