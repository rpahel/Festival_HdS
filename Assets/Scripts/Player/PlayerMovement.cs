using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController PlayerController;
    public float speed = 0f;
    private float gravity = 9.81f;
    public Transform mainCam;

    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement);
        mainCam.position = new Vector3(mainCam.position.x, transform.position.y + 0.7f, mainCam.position.z);
    }
}
