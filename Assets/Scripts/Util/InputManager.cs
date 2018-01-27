using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameControls { Jump, Hit, FillWater }

public class InputManager : MonoBehaviour {


    bool GetButtonDown(GameControls key, int controllerID)
    {

        if (Input.GetButtonDown(BuildInputID(key, controllerID)))
        {
            return true;
        }

        return false;
    }

    bool GetButtonUp(GameControls key, int controllerID)
    {
        if (Input.GetButtonUp(BuildInputID(key, controllerID)))
        {
            return true;
        }

        return false;
    }

    bool GetButton(GameControls key, int controllerID)
    {
        if (Input.GetButton(BuildInputID(key, controllerID)))
        {
            return true;
        }

        return false;
    }

    string BuildInputID(GameControls key, int controllerID)
    {
        return controllerID.ToString() + "_" + key.ToString();
    }

}




