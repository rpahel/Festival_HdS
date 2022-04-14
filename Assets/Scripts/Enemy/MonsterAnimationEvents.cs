using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEvents : MonoBehaviour
{
    public Monster monster;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    private void Update()
    {
        MonsterMove();
    }

    public void MonsterMove()
    {
        monster.MonsterMove();
        audioManager.MonsterBreathAndWalk();

    }
}
