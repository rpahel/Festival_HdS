using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController PlayerController;
    private SpriteRenderer srPlayer;
    public Light lampLight;

    public Transform mainCam;

    public float speed = 0f;
    public float speedTurn = 0f;

    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        srPlayer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement);
        mainCam.position = new Vector3(mainCam.position.x, transform.position.y + 0.7f, mainCam.position.z);

        //lampLight.transform.rotation = Quaternion.Euler(Input.mousePosition.x, lampLight.transform.rotation.y, 90);
        //lampLight.transform.LookAt(Input.mousePosition);
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
