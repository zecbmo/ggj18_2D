using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    MeshRenderer[] rends = null;

    private void Start()
    {
        ChangeColor(Color.red);
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.SetColor(0, color);
        }
    }
}
