using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEvents : MonoBehaviour
{
    public Monster monster;
    private AudioSource audioSourceMonsterBreathe;
    private AudioSource audioSourceMonsterWalk;

    private void Start()
    {
        audioSourceMonsterWalk = gameObject.AddComponent<AudioSource>();
        audioSourceMonsterBreathe = gameObject.AddComponent<AudioSource>();

        audioSourceMonsterBreathe.maxDistance = 25f;
        audioSourceMonsterWalk.maxDistance = 25f;
        audioSourceMonsterBreathe.spatialBlend = 1;
        audioSourceMonsterWalk.spatialBlend = 1;
        audioSourceMonsterBreathe.rolloffMode = AudioRolloffMode.Linear;
        audioSourceMonsterWalk.rolloffMode = AudioRolloffMode.Linear;
    }

    private void Update()
    {
        MonsterMove();
    }

    public void MonsterMove()
    {
        monster.MonsterMove();

        audioSourceMonsterBreathe.clip = AudioManager.Instance.audioClipMonstre[0];
        
        if (!audioSourceMonsterBreathe.isPlaying)
        {
            audioSourceMonsterBreathe.volume = 0.1f;
            audioSourceMonsterBreathe.Play();
        }

        audioSourceMonsterWalk.clip = AudioManager.Instance.audioClipMonstre[1];
        if (!audioSourceMonsterWalk.isPlaying)
        {

            audioSourceMonsterWalk.volume = 0.1f;
            audioSourceMonsterWalk.Play();
        }
    }
}
