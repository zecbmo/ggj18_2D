using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField]
    AudioClip levelMusic = null;

    private void Start()
    {
        if(levelMusic != null)
        AudioManager.Instance().PlayBackgroundMusic(levelMusic);
    }
}
