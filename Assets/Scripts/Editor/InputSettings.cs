﻿using UnityEngine;
using UnityEditor;



public class InputSettings : EditorWindow
{
    GameControls A_Button = GameControls.Sprint;
    GameControls B_Button = GameControls.Hit;
    GameControls X_Button = GameControls.Jump;
    GameControls Y_Button = GameControls.FillWater;
    GameControls Right_Bumper = GameControls.Hit;
    GameControls Left_Bumper = GameControls.Sprint;
    GameControls Right_Trigger = GameControls.Hit;
    GameControls Left_Trigger = GameControls.Sprint;

    AxisControls LeftStick_Y = AxisControls.Vertical;
    AxisControls LeftStick_X = AxisControls.Horizontal;
    AxisControls RightStick_Y = AxisControls.None;
    AxisControls RightStick_X = AxisControls.None;
    AxisControls DPad_X = AxisControls.Horizontal;
    AxisControls DPad_Y = AxisControls.Vertical;


    string[] names = new string[] { "One", "Two", "Three", "Four" };
    int[] sizes = { 1, 2, 3, 4 };


    int noControllers = 1;

    SerializedProperty defaultAxis = null;

    [MenuItem("Input Options/Change Controls")]
    private static void UpdateControllers()
    {
        EditorWindow.GetWindow(typeof(InputSettings));
    }

    void OnGUI()
    {
        GUILayout.Label("Input Settings", EditorStyles.boldLabel);
        GUILayout.Label("Face Buttons", EditorStyles.boldLabel);
        A_Button = (GameControls)EditorGUILayout.EnumPopup("A button:", A_Button);
        B_Button = (GameControls)EditorGUILayout.EnumPopup("B Button:", B_Button);
        X_Button = (GameControls)EditorGUILayout.EnumPopup("X Button:", X_Button);
        Y_Button = (GameControls)EditorGUILayout.EnumPopup("Y Button:", Y_Button);
        GUILayout.Label("triggers and bumpers", EditorStyles.boldLabel);
        Right_Bumper = (GameControls)EditorGUILayout.EnumPopup("Right Bumper:", Right_Bumper);
        Left_Bumper = (GameControls)EditorGUILayout.EnumPopup("Left Bumper:", Left_Bumper);
        //Right_Trigger = (GameControls)EditorGUILayout.EnumPopup("Right Trigger:", Right_Trigger);
        //Left_Trigger = (GameControls)EditorGUILayout.EnumPopup("Left Trigger:", Left_Trigger);
        GUILayout.Label("Number of Controllers", EditorStyles.boldLabel);
        noControllers = EditorGUILayout.IntPopup("How Many Controllers: ", noControllers, names, sizes);


        if (GUILayout.Button("Update Controls"))
        {
            UpdateControls();
        }

        if (GUILayout.Button("Reset To Default"))
        {
            ResetControls();
        }

        if (GUILayout.Button("Delete all inputs"))
        {
            DeleteControls();
        }

    }

    void DeleteControls()
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        axesProperty.arraySize = 0;
        serializedObject.ApplyModifiedProperties();
    }

    void UpdateControls()
    {
        //ResetControls();
        for (int i = 0; i < noControllers; i++)
        {
            AddController(i);
        }
    }

    void AddController(int controller_id)
    {
        //Face Buttons
        ButtonCreationHelper(controller_id, A_Button, 0);
        ButtonCreationHelper(controller_id, B_Button, 1);
        ButtonCreationHelper(controller_id, X_Button, 2);
        ButtonCreationHelper(controller_id, Y_Button, 3);

        //Bumber buttons Buttons
        ButtonCreationHelper(controller_id, Left_Bumper, 4);
        ButtonCreationHelper(controller_id, Right_Bumper, 5);
        //AxisCreationHelper(controller_id, Left_Trigger, 0);
        //AxisCreationHelper(controller_id, Right_Trigger, 0);

        //Sticks
        AxisCreationHelper(controller_id, LeftStick_X, 1);
        AxisCreationHelper(controller_id, LeftStick_Y, 2);

        AxisCreationHelper(controller_id, DPad_X, 6);
        AxisCreationHelper(controller_id, DPad_Y, 7);

    }


    void ButtonCreationHelper(int controller_id, GameControls control, int buttonID)
    {

        AddAxis(new InputAxis()
        {
            name = ammend(controller_id, control.ToString()),
            positiveButton = JoyStickButton(buttonID, controller_id + 1),
            joyNum = controller_id + 1 //reset to zero

        });

    }

    void AxisCreationHelper(int controller_id, AxisControls control, int axisID)
    {
        AddAxis(new InputAxis()
        {
            name = ammend(controller_id, control.ToString()),
            axis = axisID,
            joyNum = controller_id + 1, //reset to zero
            type = AxisType.JoystickAxis,
            gravity = 0,
            dead = 0.19f,
            sensitivity = 1
        });
    }


    string ammend(int id, string b)
    {
        return id.ToString() + "_" + b;
    }

    string JoyStickButton(int id, int joystickId)
    {
        return "joystick " + joystickId  + " button " + id.ToString();
    }



    void ResetControls()
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        if (defaultAxis == null)
        {
            defaultAxis = axesProperty;
        }
        axesProperty.arraySize = 18;
        serializedObject.ApplyModifiedProperties();
    }

    public enum AxisType
    {
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    };

    public class InputAxis
    {
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity;
        public float dead;
        public float sensitivity;

        public bool snap = false;
        public bool invert = false;

        public AxisType type;

        public int axis;
        public int joyNum;
    }


    private static bool AxisDefined(string axisName)
    {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false))
        {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    private static void AddAxis(InputAxis axis)
    {
        //if (AxisDefined(axis.name)) return;

        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }


}