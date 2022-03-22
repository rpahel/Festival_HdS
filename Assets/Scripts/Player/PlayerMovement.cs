using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController PlayerController;
    private SpriteRenderer srPlayer;
    public GameObject lampLight;

    public GameObject candy;
    private GameObject candyClone;

    public Camera mainCam;

    public float speed = 0f;
    public float speedTurn = 0f;
    public float rate;

    private float previousInputLook;
    private float angle;

    private bool canThrow;
    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        srPlayer = GetComponent<SpriteRenderer>();
        canThrow = true;
    }

    private void Update()
    {
        MovePlayer();
        MovingLight();
        TurningThePlayer();

        // Check if Input != 0 otherwise the player can't throw the candy
        if (Input.GetAxis("Horizontal") > 0)
        {
            previousInputLook = Input.GetAxis("Horizontal");
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            previousInputLook = Input.GetAxis("Horizontal");
        }

        if (Input.GetKey(KeyCode.E))
        {
            // Debug the path of the candy
        }

        if (Input.GetKeyUp(KeyCode.E) && canThrow)
        {
            ThrowCandy();
        }

    }

    private void MovePlayer()
    {
        // Move player
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement);
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, transform.position.y + 0.7f, mainCam.transform.position.z);
    }
    private void TurningThePlayer()
    {
        // Turn player
        if (Input.GetAxis("Horizontal") < 0)
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
    private void MovingLight()
    {
        // Move Light
        Vector3 toConvert = Input.mousePosition;
        toConvert.z = -mainCam.transform.position.z;
        Vector3 mousePos = mainCam.ScreenToWorldPoint(toConvert);
        angle = Mathf.Atan2(mousePos.y - lampLight.transform.position.y, mousePos.x - lampLight.transform.position.x);
        lampLight.transform.rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 90, 90);
    }

    private void ThrowCandy()
    {    
        if(previousInputLook < 0)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
            candyClone = Instantiate(candy, spawnPosition, Quaternion.identity);
            candyClone.GetComponent<Rigidbody>().velocity = new Vector3(-2, 5, 0);
        }

        if(previousInputLook > 0)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
            candyClone = Instantiate(candy, spawnPosition, Quaternion.identity);
            candyClone.GetComponent<Rigidbody>().velocity = new Vector3(2, 5, 0);
        }
        Destroy(candyClone, 5f);
        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        canThrow = false;
        yield return new WaitForSeconds(rate);
        canThrow = true;
    }
}
