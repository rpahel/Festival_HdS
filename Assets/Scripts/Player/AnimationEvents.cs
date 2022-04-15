using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public Player player;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    public void Throw()
    {
        player.ThrowCandy();
    }

    public void Jump()
    {
        player.Jump();
    }


    public void LandingJump()
    {
        audioManager.LandingJump();
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
