using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField]
    AudioClip levelMusic = null;

    private void Start()
    {
        AudioManager.Instance().PlayBackgroundMusic(levelMusic);
    }
}
