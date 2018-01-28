using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{
    [Header("Current volume level")]
    /// <summary>
    /// master volume, will effect all audio 
    /// </summary>
    [SerializeField,
    Range(0, 1)]
    float masterVolume = 1.0f;
    /// <summary>
    /// base sfx over volume
    /// </summary>
    [SerializeField,
    Range(0, 1)]
    float sfxVolume = 1.0f;
    /// <summary>
    /// base voice over volume
    /// </summary>
    [SerializeField,
    Range(0, 1)]
    float voiceOverVolume = 1.0f;
    /// <summary>
    /// base background music volume
    /// </summary>
    [SerializeField,
    Range(0, 1)]
    float backgroundMusicVolume = 1.0f;


    public float MasterVolume
    {
        set
        {
            masterVolume = value;
            Mathf.Clamp(masterVolume, 0, 1);
            UpdateAudioVolumes();
        }
        get
        {
            return masterVolume;
        }
    }

    public float SXFVolume
    {
        set
        {
            sfxVolume = value;
            Mathf.Clamp(sfxVolume, 0, 1);
            UpdateAudioVolumes();
        }
        get
        {
            return sfxVolume;
        }
    }

    public float VoiceOverVolume
    {
        set
        {
            voiceOverVolume = value;
            Mathf.Clamp(voiceOverVolume, 0, 1);
            UpdateAudioVolumes();
        }
    }

    public float BackgroundMusicVolume
    {
        set
        {
            backgroundMusicVolume = value;
            Mathf.Clamp(backgroundMusicVolume, 0, 1);
            UpdateAudioVolumes();
        }
        get { return backgroundMusicVolume; }
    }

    /// <summary>
    /// Audio sources used for music 
    /// </summary>
    private AudioSource backGroundMusicSouce = new AudioSource();


    /// <summary>
    /// Audio Souce Used for voice over effects
    /// </summary>
    private AudioSource voiceOverSouce = new AudioSource();

    /// <summary>
    /// List of all the sfx 
    /// </summary>
    private List<AudioSource> sFXSources = new List<AudioSource>();

    /// <summary>
    /// How many diffrent sfx sources should be playable at any given point 
    /// </summary>
    [Header("SFX Settings"),
    Tooltip("How many SFX Souce that is required at any given point"),
    SerializeField]
    int maxNumberSFXSouces = 15;


    protected override void Awake()
    {
        base.Awake();
        // add all audio souces for each type of audio

        backGroundMusicSouce = gameObject.AddComponent<AudioSource>();
        voiceOverSouce = gameObject.AddComponent<AudioSource>();

        for (int i = 0; i < maxNumberSFXSouces; i++)
        {
            sFXSources.Add(gameObject.AddComponent<AudioSource>());

        }



    }

    /// <summary>
    /// Updates the audio volume settings
    /// </summary>
    protected virtual void UpdateAudioVolumes()
    {



        // Update all audio sources
        backGroundMusicSouce.volume = masterVolume * backgroundMusicVolume;

        voiceOverSouce.volume = masterVolume * voiceOverVolume;

        foreach (var audo in sFXSources)
        {
            audo.volume = masterVolume * sfxVolume;
        }

    }

    /// <summary>
    /// Play Background Music Track
    /// </summary>
    /// <param name="clip">Clip that is wanting to be played</param>
    /// <param name="bshouldLoop">if clip should loop</param>
    /// <param name="pitch">Pitch of the clip</param>
    public void PlayBackgroundMusic(AudioClip clip, bool bshouldLoop = true, float pitch = 1.0f)
    {

        if (clip != null)
        {
            backGroundMusicSouce.loop = bshouldLoop;
            backGroundMusicSouce.Stop();
            backGroundMusicSouce.clip = clip;
            backGroundMusicSouce.pitch = pitch;
            backGroundMusicSouce.volume = masterVolume * backgroundMusicVolume;
            backGroundMusicSouce.Play();
        }
        else
        {
        }
    }

    /// <summary>
    /// Play voice over Music Track
    /// </summary>
    /// <param name="clip">Clip that is wanting to be played</param>
    /// <param name="pitch">Pitch of the clip</param>
    public void PlayVoiceOver(AudioClip clip, float pitch = 1.0f)
    {

        if (clip != null)
        {

            voiceOverSouce.Stop();
            voiceOverSouce.clip = clip;
            voiceOverSouce.pitch = pitch;
            voiceOverSouce.volume = masterVolume * voiceOverVolume;
            voiceOverSouce.Play();
        }
        else
        {

        }
    }

    /// <summary>
    /// Plays a sfx effect 
    /// </summary>
    /// <param name="clip">Clip to be played</param>
    /// <param name="pitch">pitch of the clip</param>
    /// <param name="atPoint">at a given location</param>
    /// <returns>The audio source that is playing the clip</returns>
    public AudioSource PlaySFX(AudioClip clip, float pitchModulation = 0.0f, float volume = 1,  Vector3? atPoint = null)
    {
        // if wanting to play at a posision then set up 
        if (atPoint.HasValue)
        {
            AudioSource.PlayClipAtPoint(clip, atPoint.Value, masterVolume * sfxVolume);

        }

        float pitch = Random.Range(1 - pitchModulation, 1 + pitchModulation);

        foreach (var item in sFXSources)
        {
            if (item != null)
            {

                if (!item.isPlaying)
                {
                    item.clip = clip;
                    item.pitch = pitch;
                    item.volume = masterVolume * sfxVolume * volume;
                    item.Play();

                    return item;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Will pause all audio in game
    /// </summary>
    void PauseAllAudio()
    {

        PauseAllSFX();
        PauseVoiceOverAudio();
        PauseBackgroundMusic();
    }

    /// <summary>
    /// Will pause all sfx playing currently 
    /// </summary>
    public void PauseAllSFX()
    {

        foreach (var item in sFXSources)
        {
            if (item != null)
            {
                item.Pause();
            }
        }
    }

    /// <summary>
    /// Pauses Background Music effects
    /// </summary>
    public void PauseBackgroundMusic()
    {

        backGroundMusicSouce.Pause();
    }

    /// <summary>
    /// Pause all voice over volumes
    /// </summary>
    public void PauseVoiceOverAudio()
    {

        voiceOverSouce.Pause();
    }

    /// <summary>
    /// Will unpause all audio in game
    /// </summary>
    void UnPauseAllAudio()
    {

        UnPauseAlLSFXEffects();
        UnPauseBackgroundMusic();
        UnPauseVoiceOverBackgroundMusic();
    }

    /// <summary>
    /// unpauses all sfx 
    /// </summary>
    void UnPauseAlLSFXEffects()
    {

        foreach (var item in sFXSources)
        {
            if (item != null)
            {
                item.UnPause();
            }
        }
    }

    /// <summary>
    /// unpauses current background music
    /// </summary>
    void UnPauseBackgroundMusic()
    {

        backGroundMusicSouce.UnPause();
    }
    /// <summary>
    /// unpause voice over currently playing
    /// </summary>
    void UnPauseVoiceOverBackgroundMusic()
    {

        backGroundMusicSouce.UnPause();
    }

    /// <summary>
    /// Will stop all audio being played
    /// </summary>
    public void StopAllAudio()
    {

        StopAllSFX();
        StopBackgroundMusic();
        StopVoiceOver();
    }

    /// <summary>
    /// Will stop all sfx playing currently 
    /// </summary>
    public void StopAllSFX()
    {

        foreach (var item in sFXSources)
        {
            if (item != null)
            {
                item.Stop();
                item.clip = null;
            }
        }
    }

    /// <summary>
    /// Will stop all current background music being played
    /// </summary>
    public void StopBackgroundMusic()
    {

        backGroundMusicSouce.Stop();
        backGroundMusicSouce.clip = null;
    }

    /// <summary>
    /// Will stop all current voice over  being played
    /// </summary>
    public void StopVoiceOver()
    {

        voiceOverSouce.Stop();
        voiceOverSouce.clip = null;


    }


}

