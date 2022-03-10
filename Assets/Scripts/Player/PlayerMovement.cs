using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController PlayerController;
    private SpriteRenderer srPlayer;
    public GameObject lampLight;

    public Camera mainCam;

    public float speed = 0f;
    public float speedTurn = 0f;

    private float angle;
    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        srPlayer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Move player
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement);
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, transform.position.y + 0.7f, mainCam.transform.position.z);

        // Move Light
        Vector3 toConvert = Input.mousePosition;
        toConvert.z = -mainCam.transform.position.z;
        Vector3 mousePos = mainCam.ScreenToWorldPoint(toConvert);
        angle = Mathf.Atan2(mousePos.y - lampLight.transform.position.y, mousePos.x - lampLight.transform.position.x);
        lampLight.transform.rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 90, 90);

        // Turn player
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
