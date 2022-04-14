using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private AudioSource[] audioSourceMonsters;
    private AudioSource[] audioSourcePlayer;
    private AudioSource[] audioSourceAmbiance;
    private AudioSource[] audioSourcePhone;
    private AudioSource[] audioSourceDoll;
    private AudioSource[] audioSourceDoor;

    public AudioMixerGroup audioMixer; 

    [Header("Monstre")]
    public List<AudioClip> audioClipMonstre;

    [Header("Ambiance")]
    public List<AudioClip> audioClipAmbiance;

    [Header("Player")]
    public List<AudioClip> audioClipPlayer;

    [Header("Phone")]
    public List<AudioClip> audioClipPhone;

    [Header("Doll")]
    public List<AudioClip> audioClipDoll;
    
    [Header("Door")]
    public List<AudioClip> audioClipDoor;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        audioSourceMonsters = new AudioSource[audioClipMonstre.Count];
        audioSourcePlayer = new AudioSource[audioClipPlayer.Count];
        audioSourceAmbiance = new AudioSource[audioClipAmbiance.Count];
        audioSourcePhone = new AudioSource[audioClipPhone.Count];
        audioSourceDoll = new AudioSource[audioClipDoll.Count];
        audioSourceDoor = new AudioSource[audioClipDoor.Count];
    }

    // Audio initialisation
    void Start()
    {
        for(int i = 0; i < audioClipMonstre.Count; i++)
        {
            audioSourceMonsters[i] = gameObject.AddComponent<AudioSource>();
            audioSourceMonsters[i].outputAudioMixerGroup = audioMixer;
            audioSourceMonsters[i].volume = 0.1f;
            audioSourceMonsters[i].spatialBlend = 1;
            audioSourceMonsters[i].rolloffMode = AudioRolloffMode.Linear;
            audioSourceMonsters[i].clip = audioClipMonstre[i];
        }
        for (int i = 0; i < audioClipPlayer.Count; i++)
        {
            audioSourcePlayer[i] = gameObject.AddComponent<AudioSource>();
            audioSourcePlayer[i].outputAudioMixerGroup = audioMixer;
            audioSourcePlayer[i].clip = audioClipPlayer[i];
        }
        for (int i = 0; i < audioClipAmbiance.Count; i++)
        {
            audioSourceAmbiance[i] = gameObject.AddComponent<AudioSource>();
            audioSourceAmbiance[i].outputAudioMixerGroup = audioMixer;
            audioSourceAmbiance[i].clip = audioClipAmbiance[i];
        }
        for (int i = 0; i < audioClipPhone.Count; i++)
        {
            audioSourcePhone[i] = gameObject.AddComponent<AudioSource>();
            audioSourcePhone[i].outputAudioMixerGroup = audioMixer;
            audioSourcePhone[i].clip = audioClipPhone[i];
        }
        for (int i = 0; i < audioClipDoll.Count; i++)
        {
            audioSourceDoll[i] = gameObject.AddComponent<AudioSource>();
            audioSourceDoll[i].outputAudioMixerGroup = audioMixer;
            audioSourceDoll[i].clip = audioClipDoll[i];
        }
        for (int i = 0; i < audioClipDoor.Count; i++)
        {
            audioSourceDoor[i] = gameObject.AddComponent<AudioSource>();
            audioSourceDoor[i].outputAudioMixerGroup = audioMixer;
            audioSourceDoor[i].clip = audioClipDoor[i];
        }
    }

    #region MonsterSounds
    public void MonsterBreathAndWalk()
    {
        for(int i = 0; i < audioSourceMonsters.Length; i++)
        {
            if (!audioSourceMonsters[i].isPlaying)
            {
                audioSourceMonsters[i].Play();
            }
        }
    }

    public void MonsterStop()
    {
        if (audioSourceMonsters[1].isPlaying)
        {
            audioSourceMonsters[1].Stop();
        }
    }

    #endregion

    #region PlayerSounds
    public void PlayerWalk()
    {
        if (!audioSourcePlayer[0].isPlaying)
        {
            audioSourcePlayer[0].Play();
            audioSourcePlayer[0].pitch = 1.3f;
        }
    }

    public void PlayerRun()
    {
        if (!audioSourcePlayer[0].isPlaying)
        {
            audioSourcePlayer[0].Play();
            audioSourcePlayer[0].pitch = 2f;
        }
    }

    public void StopPlayerWalk()
    {
        if(audioSourcePlayer[0].clip != null && audioSourcePlayer[0].isPlaying)
        {
            audioSourcePlayer[0].Stop();
        }
    }

    public void Jump()
    {
        if (!audioSourcePlayer[6].isPlaying)
        {
            audioSourcePlayer[6].Play();
        }
    }

    public void LandingJump()
    {
        if (!audioSourcePlayer[7].isPlaying)
        {
            audioSourcePlayer[7].Play();
        }
    }

    public void LightOnAndOff()
    {
        audioSourcePlayer[3].Play();
    }

    public void ReloadLight()
    {
        if (!audioSourcePlayer[5].isPlaying)
        {
            audioSourcePlayer[5].Play();
        }
    }

    public void PlayThrowCandy()
    {
        audioSourcePlayer[1].Play();
    }
    #endregion

    #region DeathSounds

    public void CamZooming()
    {
        if (!audioSourcePlayer[4].isPlaying)
        {
            audioSourcePlayer[4].pitch = 0.6f;
            audioSourcePlayer[4].Play();
        }
    }

    public void CamNotZooming()
    {
        if (audioSourcePlayer[4].isPlaying)
        {
            audioSourcePlayer[4].Stop();
        }
    }

    #endregion

    #region DoorSounds

    public void DoorOpening()
    {
        if (!audioSourceDoor[0].isPlaying)
        {
            audioSourceDoor[0].Play();
        }
    }

    public void DoorLocked()
    {
        if (!audioSourceDoor[1].isPlaying)
        {
            audioSourceDoor[1].Play();
        }
    }

    #endregion

    #region DollSounds

    public void FindDoll()
    {
        if (!audioSourceDoll[0].isPlaying)
        {
            audioSourceDoll[0].Play();
        }
    }

    #endregion
}
