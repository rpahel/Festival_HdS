using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController PlayerController;
    public float speed = 0f;
    public Transform mainCam;

    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.Move(movement * speed * Time.deltaTime);
        mainCam.position = new Vector3(mainCam.position.x, transform.position.y, mainCam.position.z);
    }
}
