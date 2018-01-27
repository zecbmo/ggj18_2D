using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameControls { Jump, Hit, FillWater, Sprint, None }
public enum AxisControls { Horizontal, Vertical, None }


public static class InputManager  {


    public static bool GetButtonDown(GameControls key, int controllerID)
    {

        if (Input.GetButtonDown(BuildInputID(key, controllerID)))
        {
            return true;
        }

        return false;
    }

    public static bool GetButtonDown(AxisControls axis, int controllerID)
    {

        if (Input.GetButtonDown(BuildInputID(axis, controllerID)))
        {
            return true;
        }

        return false;
    }

    public static bool GetButtonUp(GameControls key, int controllerID)
    {
        if (Input.GetButtonUp(BuildInputID(key, controllerID)))
        {
            return true;
        }

        return false;
    }

    public static bool GetButton(GameControls key, int controllerID)
    {
        if (Input.GetButton(BuildInputID(key, controllerID)))
        {
            return true;
        }

        return false;
    }


    public static float GetAxis(AxisControls axis, int controllerID)
    {
        return Input.GetAxis(BuildInputID(axis, controllerID));    
        
    }

    public static string BuildInputID(GameControls key, int controllerID)
    {
        return controllerID.ToString() + "_" + key.ToString();
    }

    public static string BuildInputID(AxisControls key, int controllerID)
    {
        return controllerID.ToString() + "_" + key.ToString();
    }

}




