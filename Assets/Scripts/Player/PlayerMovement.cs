using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController PlayerController;
    private SpriteRenderer srPlayer;
    public Light lampLight;

    public Camera mainCam;

    public float speed = 0f;
    public float speedTurn = 0f;
    public float mouseSensibility;

    private Vector2 mousePos;
    private float mouseY;
    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        srPlayer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement);
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, transform.position.y + 0.7f, mainCam.transform.position.z);

        mouseY = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;
        lampLight.transform.Rotate(Vector3.up * mouseY); 

        if(Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -180, 0), Time.deltaTime * speedTurn);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speedTurn);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime * speedTurn);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * speedTurn);
        }
    }
}
