using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFieldTower : Tower
{
    [SerializeField, Tooltip("Visual Component of the Tower. Should be a child of the main game object")]
    GameObject visualObject = null;

    [SerializeField]
    ParticleSystem waterParticles = null;
    ParticleSystem.EmissionModule emitter;
    ParticleSystem.TrailModule trail;
    Vector3 waterParticlesStartScale = Vector3.zero;
    float waterStopSpeed = 5f;
    bool waterCanBeStared = false;
    bool waterHasBeenStopped = false;

    

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

        emitter = waterParticles.emission;
        trail = waterParticles.trails;
        waterParticlesStartScale = waterParticles.gameObject.transform.localScale;
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

        ManageWaterParticles();

        if (IsTowerEmpty())
        {

        }
    }

    void ManageWaterParticles()
    {

        if (canBeModified && fluidBeingRemoved && !IsTowerEmpty())
        {
            waterParticles.gameObject.SetActive(true);
            waterCanBeStared = true;
            emitter.enabled = true;

        }
        else if (!waterHasBeenStopped && waterCanBeStared)
        {
            // waterParticles.Stop();
            StartCoroutine(ScaleLerp(waterParticles.gameObject, new Vector3(0, 0, 0), waterStopSpeed));
        }
    }

    public void ShowTower()
    {
        StartCoroutine(MathUtil.MoveObjectTowardsLocation(visualObject, visualObjectActivePosition, moveInSpeed, 0.01f, true));
    }

    public void HideTower(bool destroObjectWhenFinished = false)
    {
        canBeModified = false;
        DestroyOptions destroyOptions = DestroyOptions.DontDestory;

        if (destroObjectWhenFinished)
        {
            destroyOptions = DestroyOptions.Parent;
        }

        StartCoroutine(MathUtil.MoveObjectTowardsLocation(visualObject, visualObjectOffPosition, moveInSpeed, 0.01f, true, destroyOptions));

    }

    public IEnumerator ScaleLerp(GameObject objectToLerp, Vector3 newScale, float speed)
    {
        float elapsedTime = 0;
        Vector3 startingScale = objectToLerp.transform.localScale;
        while (elapsedTime < 1)
        {
            objectToLerp.transform.localScale = Vector3.Lerp(startingScale, newScale, (elapsedTime / 1));
            elapsedTime += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        waterParticles.gameObject.SetActive(false);
        waterParticles.gameObject.transform.localScale = waterParticlesStartScale;

    }


}
