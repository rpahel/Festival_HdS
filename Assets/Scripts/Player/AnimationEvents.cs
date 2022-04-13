using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Player player;

    private AudioSource audioSourcePlayerCandy;
    private void Start()
    {
        audioSourcePlayerCandy = GetComponent<AudioSource>();
    }

    public void Throw()
    {
        player.ThrowCandy();
        audioSourcePlayerCandy.clip = AudioManager.Instance.audioClipPlayer[1];
        audioSourcePlayerCandy.Play();
    }

    public void Jump()
    {
        player.Jump();
    }

    public void HoldingPhone()
    {

    }

    public void Death()
    {

    }

    public void ArmMeshActivate()
    {
        player.ArmMesh(true);
    }

    public void GoingInside()
    {
        player.GoingInside();
    }

    public void Teleport()
    {
        player.Teleport();
    }
}
