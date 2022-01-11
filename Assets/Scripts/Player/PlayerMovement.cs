using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController PlayerController;
    public Light lampLight;

    public Transform mainCam;

    public float speed = 0f;

    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement);
        mainCam.position = new Vector3(mainCam.position.x, transform.position.y + 0.7f, mainCam.position.z);

        Vector3 mousePosForward = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lightPosUpward = new Vector3(Input.mousePosition.x, lampLight.transform.position.y, lampLight.transform.position.z);
        lampLight.transform.rotation = Quaternion.LookRotation(mousePosForward);

    }
}
