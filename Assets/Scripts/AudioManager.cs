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

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < audioClipMonstre.Count; i++)
        {
            audioSourceMonsters[i] = gameObject.AddComponent<AudioSource>();
            audioSourceMonsters[i].outputAudioMixerGroup = audioMixer;
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

    public void LightOnAndOff()
    {
        audioSourcePlayer[3].Play();
    }

    public void PlayThrowCandy()
    {
        audioSourcePlayer[1].Play();
    }
}
