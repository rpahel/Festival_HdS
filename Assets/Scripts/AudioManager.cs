using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
            instance = this;    
    }

    private AudioSource audioSource;

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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
