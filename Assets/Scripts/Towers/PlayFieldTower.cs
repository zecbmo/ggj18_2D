using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFieldTower : Tower
{
    [SerializeField, Tooltip("Visual Component of the Tower. Should be a child of the main game object")]
    GameObject visualObject = null;

    [SerializeField, Tooltip("The visual component will move by this height when coming into play and going out of play")]
    float heightOffset = 5.0f;

    [SerializeField]
    float moveInSpeed = 5.0f;

    [SerializeField, Tooltip("Timer to allow tower to animate in before coming active")]
    float delayBecomingAtStartTime = 1.0f;

    Vector2 visualObjectActivePosition = Vector3.zero;
    Vector2 visualObjectOffPosition = Vector3.zero;


    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        //Turn off interaction and enable it again after given time
        canBeModified = false;
        StartCoroutine(SetCanBeModifed(true, delayBecomingAtStartTime));

        //say it cant be filled as in game towers cant
        canBeFilled = false;

        //Set the acitive and off positions
        visualObjectActivePosition = visualObject.transform.localPosition;
        visualObjectOffPosition = new Vector2(visualObjectActivePosition.x, visualObjectActivePosition.y - heightOffset);

        //set the location to off screen
        visualObject.transform.localPosition = visualObjectOffPosition;

        //Animate the tower in
        ShowTower();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (IsTowerEmpty())
        {

        }
    }

    public void ShowTower()
    {
        StartCoroutine(MathUtil.MoveObjectTowardsLocation(visualObject, visualObjectActivePosition, moveInSpeed, 0.01f, true));
    }

    public void HideTower(bool destroObjectWhenFinished = false)
    {
        DestroyOptions destroyOptions = DestroyOptions.DontDestory;

        if (destroObjectWhenFinished)
        {
            destroyOptions = DestroyOptions.Parent;
        }

        StartCoroutine(MathUtil.MoveObjectTowardsLocation(visualObject, visualObjectOffPosition, moveInSpeed, 0.01f, true, destroyOptions));

    }



}
