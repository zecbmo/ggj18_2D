﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a tower object that will be placed into the scene. 
/// Players will be able to collect water/mineral from these towers
/// </summary>
public class Tower : MonoBehaviour {

    [Header("Tower Settings")]
    [SerializeField, Range(0,100)]
    protected float percentageFilled = 100.0f;

    [SerializeField]
    float removeFluidSpeedModifier = 1.0f;

    [SerializeField]
    float addFluidSpeedModifier = 1.0f;

    [SerializeField,Tooltip("This objects pivot point should be at its bottom fior scaling")]
    GameObject fluidInidcator = null;

    [Header("Water Particles for draining")]
    [SerializeField]
    ParticleSystem waterParticles = null;
    ParticleSystem.EmissionModule emitter;
    ParticleSystem.TrailModule trail;
    Vector3 waterParticlesStartScale = Vector3.zero;
    float waterStopSpeed = 5f;
    bool waterCanBeStared = false;
    bool waterHasBeenStopped = false;

    //mesured in Y axis only
    Vector3 fluidInidcatorStartingScale = Vector3.zero;

    //bool used to say if it can be modifeid or not
     protected bool canBeModified = true;

    //bool used to say if it can be modifeid or not
    protected bool canBeFilled = true;

    public bool fluidBeingRemoved = false;

    // Use this for initialization
    protected virtual void Start ()
    {
        //set the max scale as is in the scene view
        fluidInidcatorStartingScale = fluidInidcator.transform.localScale;


        emitter = waterParticles.emission;
        trail = waterParticles.trails;
        waterParticlesStartScale = waterParticles.gameObject.transform.localScale;
      

    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        
        ManageWaterParticles();
        UpdateVisualIndicator();

        if (Input.GetKey(KeyCode.V))
        {
           /// RemoveFluid(removeFluidSpeedModifier);
        }


    }

    private void LateUpdate()
    {
       

    }

    public float AddFluid(float additionSpeed)
    {


        if (canBeFilled)
        {
            //change amount py positive value
            return ChangeFluidAmount(additionSpeed, addFluidSpeedModifier);
        }

        return 0.0f;
    }

    public float RemoveFluid(float removalSpeed)
    {
        fluidBeingRemoved = true;
        //remove fluid with a negative amount
        return ChangeFluidAmount(-removalSpeed, removeFluidSpeedModifier);
    }


    //modifiy fluid by a given speed and a modifier for that speed
    //Changes the percentage amount of fluid 
    //returns the amount removed that frame
    float ChangeFluidAmount(float changeSpeed, float modifier)
    {
        //get the amount to change
        float amountToChange = Time.deltaTime * changeSpeed * modifier; ;

        //modify percentage
        percentageFilled += amountToChange;

        //clamp percentage to 0/100
        percentageFilled = Mathf.Clamp(percentageFilled, 0.0f, 100.0f);

        return amountToChange;
    }

    void UpdateVisualIndicator()
    {
        if (fluidInidcator)
        {
            //get the percentage value of the currentScale
            float newY = (fluidInidcatorStartingScale.y/100.0f) * percentageFilled;
            Vector3 currentScale = fluidInidcator.transform.localScale;

            //change the scale of the visual indicator to match the percentage
            fluidInidcator.transform.localScale = new Vector3(currentScale.x, newY, currentScale.z);
        }
    }


    protected bool IsTowerEmpty()
    {
        if (percentageFilled <= 0.0f)
        {
            return true;
        }

        return false;
    } 

    //Getters and setters with a coroutine for the modification bool
    public bool CanTowerAmountBeChanged()
    {
        return canBeModified;
    }

    public void SetIfTowerCanBeChanged(bool newValue)
    {
        canBeModified = newValue;
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


    protected IEnumerator SetCanBeModifed(bool newValue, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        canBeModified = newValue;
    }

    public float GetPercentageFilled()
    {
        return percentageFilled;
    }
}
