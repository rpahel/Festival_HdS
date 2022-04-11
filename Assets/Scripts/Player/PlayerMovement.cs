using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController PlayerController;
    private LineRenderer lrTraj;
    public GameObject lampLight;
    public GameObject candy;
    private GameObject candyClone;

    public Animator animPlayer;
    public GameObject leftArm, rightArm;

    private Vector3 directionVelocity;
    private Vector3 mousePos;

    public Camera mainCam;
    private float cameraYOffset;
    private float cameraXValueToKeep;

    public float moveSpeed = 2f;
    public float speedTurn = 0f;
    public float rate;
    public float timeSimulatingLr;

    private float previousInputLook;
    private float angle;
    private const float gravity = -9.81f;

    private bool canThrow;

    private List<Vector3> positionsPredicted = new List<Vector3>();
    private void Start()
    {
        PlayerController = GetComponent<CharacterController>();
        lrTraj = GetComponent<LineRenderer>();
        canThrow = true;
        cameraXValueToKeep = mainCam.transform.position.x;
    }

    private void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal"));
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

        if (Input.GetKey(KeyCode.E) && canThrow)
        {
            if (previousInputLook < 0)
            {
                directionVelocity = new Vector3(-2, 5, 0);
            }
            if (previousInputLook > 0)
            {
                directionVelocity = new Vector3(2, 5, 0);
            }

            if (previousInputLook == 0 && (mousePos.x > transform.position.x && Input.GetAxis("Horizontal") == 0))
            {
                directionVelocity = new Vector3(2, 5, 0);
            }
            else if (previousInputLook == 0 && (mousePos.x <= transform.position.x && Input.GetAxis("Horizontal") == 0))
            {
                directionVelocity = new Vector3(-2, 5, 0);
            }
            positionsPredicted = PredictPositions();
            lrTraj.positionCount = positionsPredicted.Count;
            // Debug the path of the candy
            for(int i = 0; i < PredictPositions().Count; i++)
            {
                lrTraj.SetPosition(i, PredictPositions()[i]);
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && canThrow)
        {
            ThrowCandy();
            lrTraj.positionCount = 0;
        }
    }

    private void MovePlayer()
    {
        // Move player
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        PlayerController.SimpleMove(movement * moveSpeed);
        if(transform.position.x <= -4.6f || transform.position.x >= 3.93f)
        {
            mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + cameraYOffset, mainCam.transform.position.z);
        } else
        {
            mainCam.transform.position = new Vector3(cameraXValueToKeep, transform.position.y + cameraYOffset, mainCam.transform.position.z);
        }
    }
    private void TurningThePlayer()
    {
        // Turn player
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -180, 0), Time.deltaTime * speedTurn);
            animPlayer.SetBool("FaceLeft", true);
            animPlayer.SetFloat("Speed", 0.2f);
            rightArm.SetActive(false);
            leftArm.SetActive(true);
            
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speedTurn);
            animPlayer.SetBool("FaceLeft", false);
            animPlayer.SetFloat("Speed", 0.2f);
            rightArm.SetActive(true);
            leftArm.SetActive(false);
        }
        if (Input.GetAxis("Horizontal") == 0)
        {
            previousInputLook = 0;
            animPlayer.SetFloat("Speed", 0);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime * speedTurn);
            animPlayer.SetFloat("Speed", 0.2f);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * speedTurn);
            animPlayer.SetFloat("Speed", 0.2f);
        }
    }
    private void MovingLight()
    {
        // Move Light
        Vector3 toConvert = Input.mousePosition;
        toConvert.z = -mainCam.transform.position.z;
        mousePos = mainCam.ScreenToWorldPoint(toConvert);
        if(mousePos.x <= transform.position.x)
        {
            animPlayer.SetBool("FaceLeft", true);
            rightArm.SetActive(false);
            leftArm.SetActive(true);
        } else
        {
            animPlayer.SetBool("FaceLeft", false);
            rightArm.SetActive(true);
            leftArm.SetActive(false);
        }
        angle = Mathf.Atan2(mousePos.y - lampLight.transform.position.y, mousePos.x - lampLight.transform.position.x);
        lampLight.transform.rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 90, 90);
    }

    private void ThrowCandy()
    {    
        //throw candy with movement
        if(previousInputLook < 0)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
            candyClone = Instantiate(candy, spawnPosition, Quaternion.identity);
            directionVelocity = new Vector3(-2, 5, 0);
            candyClone.GetComponent<Rigidbody>().velocity = directionVelocity;
        }

        if(previousInputLook > 0)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 0.6f, transform.position.y, transform.position.z);
            candyClone = Instantiate(candy, spawnPosition, Quaternion.identity);
            directionVelocity = new Vector3(2, 5, 0);
            candyClone.GetComponent<Rigidbody>().velocity = directionVelocity;

        }

        // throw candy with speed = 0 but with the mousePos
        if (previousInputLook == 0 && (mousePos.x > transform.position.x && Input.GetAxis("Horizontal") == 0))
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + 0.6f, transform.position.y, transform.position.z);
            candyClone = Instantiate(candy, spawnPosition, Quaternion.identity);
            directionVelocity = new Vector3(2, 5, 0);
            candyClone.GetComponent<Rigidbody>().velocity = directionVelocity;
        } else if(previousInputLook == 0 && (mousePos.x <= transform.position.x && Input.GetAxis("Horizontal") == 0))
        {
            Vector3 spawnPosition = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
            candyClone = Instantiate(candy, spawnPosition, Quaternion.identity);
            directionVelocity = new Vector3(-2, 5, 0);
            candyClone.GetComponent<Rigidbody>().velocity = directionVelocity;
        }
        //Destroy(candyClone, 5f);
        StartCoroutine(Reloading());
    }

    private List<Vector3> PredictPositions()
    {
        float step = 0.1f;
        int maxSteps = (int)(timeSimulatingLr / step);

        List<Vector3> positions = new List<Vector3>();
        for(int i = 0; i < maxSteps; i++)
        {
            float simulationTime = i / (float)maxSteps;
            Vector3 displacement = directionVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime / 2f;
            Vector3 drawPoint;
            if (previousInputLook > 0)
            {
                drawPoint = new Vector3(transform.position.x + 0.6f, transform.position.y, transform.position.z) + displacement;
                positions.Add(drawPoint);
            }
            if (previousInputLook < 0)
            {
                drawPoint = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z) + displacement;
                positions.Add(drawPoint);
            }

            if (previousInputLook == 0 && (mousePos.x > transform.position.x && Input.GetAxis("Horizontal") == 0))
            {
                drawPoint = new Vector3(transform.position.x + 0.6f, transform.position.y, transform.position.z) + displacement;
                positions.Add(drawPoint);
            }
            if (previousInputLook == 0 && (mousePos.x <= transform.position.x && Input.GetAxis("Horizontal") == 0))
            {
                drawPoint = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z) + displacement;
                positions.Add(drawPoint);
            }
            displacement.z = transform.position.z;
        }
        return positions;
    }

    private IEnumerator Reloading()
    {
        canThrow = false;
        yield return new WaitForSeconds(rate);
        canThrow = true;
    }
}
