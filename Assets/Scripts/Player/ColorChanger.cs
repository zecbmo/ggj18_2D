using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    [SerializeField]
    MeshRenderer[] rends = null;

    [SerializeField]
    Color testColour = Color.red;

    private void Start()
    {
        //ChangeColor(testColour);
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = color;
        }
    }
}
