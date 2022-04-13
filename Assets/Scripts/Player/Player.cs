using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float walkJump;
    public float runJump;
    private CharacterController playerController;
    private Collider playerCollider;

    [Header("Animations")]
    public Animator animPlayer;

    [Header("Lamp")]
    public GameObject armPivot, leftArm, rightArm;
    private Vector3 walkArmPivot;
    private Vector2 mousePos;
    public Slider lampStaminaBar;
    public Light lamp1;
    public Light lamp2;
    public float speedStaminaDown;
    public float maxValueStamina;
    public float valueReload;
    public float zoomSpeed;
    private bool isPressed;
    private bool canBePressed = true;

    [Header("Candy throwing")]
    public GameObject candy;
    public float throwForce;
    public float throwCooldown;
    private LineRenderer lrTraj;
    private Vector3 beginTraj;
    private Vector3 endTraj;
    private float angle;
    private bool canThrow;

    [Header("Camera")]
    public Camera mainCam;
    private float cameraXValueToKeep;

    [Header("Audio")]
    private AudioSource audioSourcePlayer;
    private AudioSource audioSourceLightPlayer;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        lrTraj = GetComponent<LineRenderer>();
        playerCollider = GetComponent<Collider>();
        audioSourcePlayer = GetComponent<AudioSource>();
        audioSourceLightPlayer = gameObject.AddComponent<AudioSource>();
        canThrow = true;
    }

    private void Start()
    {
        cameraXValueToKeep = mainCam.transform.position.x;

        walkArmPivot = armPivot.transform.localPosition;

        lrTraj.startWidth = 0.05f;
        lrTraj.endWidth = 0.05f;

        lampStaminaBar.maxValue = maxValueStamina;
        lampStaminaBar.value = lampStaminaBar.maxValue;
    }

    private void Update()
    {
        mousePos = (mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)) - armPivot.transform.position).normalized;
        
        MovingLight();
        MovePlayer();
        OnOffLamp();
        StaminaManager();

        if (Input.GetButton("Throw") && canThrow)
        {
            ThrowTrajectory();
        }

        if (Input.GetButtonUp("Throw") && canThrow)
        {
            lrTraj.enabled = false;
            animPlayer.SetTrigger("Throw");
        }

        lampStaminaBar.value = Mathf.Clamp(lampStaminaBar.value, 0, maxValueStamina);
    }

    private void MovePlayer()
    {
        // Move player
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButton("Run"))
        {
            playerController.SimpleMove(movement.normalized * runSpeed);
        }
        else
        {
            playerController.SimpleMove(movement.normalized * walkSpeed);
        }


        //Footsteps
        if (playerController.velocity == Vector3.zero)
        {
            if(audioSourcePlayer.clip != null)
                audioSourcePlayer.Stop();

            audioSourcePlayer.clip = null;
        }
        else
        {
            audioSourcePlayer.clip = AudioManager.Instance.audioClipPlayer[0];
            
            if (!audioSourcePlayer.isPlaying && audioSourcePlayer.clip != null)
            {
                audioSourcePlayer.Play();
                if(playerController.velocity.magnitude <= 2)
                {
                    audioSourcePlayer.pitch = 1.3f;
                } else if(playerController.velocity.magnitude > 2.2f)
                {
                    audioSourcePlayer.pitch = 2f;
                }
            } 
        }

        if (transform.position.x <= -4.6f || transform.position.x >= 3.93f)
        {
            mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, mainCam.transform.position.z);
        }
        else
        {
            mainCam.transform.position = new Vector3(cameraXValueToKeep, transform.position.y + 1f, mainCam.transform.position.z);
        }

        animPlayer.SetFloat("Speed", playerController.velocity.sqrMagnitude);
    }

    private void MovingLight()
    {
        angle = Mathf.Atan2(mousePos.y, mousePos.x);

        if (mousePos.x < 0)
        {
            armPivot.transform.rotation = Quaternion.Euler(armPivot.transform.rotation.x, armPivot.transform.rotation.y, angle * Mathf.Rad2Deg + 180);
            animPlayer.SetBool("FaceLeft", true);
            rightArm.SetActive(false);
            leftArm.SetActive(true);
        }
        else
        {
            armPivot.transform.rotation = Quaternion.Euler(armPivot.transform.rotation.x, armPivot.transform.rotation.y, angle * Mathf.Rad2Deg);
            animPlayer.SetBool("FaceLeft", false);
            rightArm.SetActive(true);
            leftArm.SetActive(false);
        }

        if (playerController.velocity.sqrMagnitude > 10)
        {
            if (mousePos.x >= 0)
            {
                armPivot.transform.localPosition = new Vector3(0.316f, 0.20f, armPivot.transform.localPosition.z);
            }
            else
            {
                armPivot.transform.localPosition = new Vector3(0.169f, 0.20f, 0.04f);
            }
        }
        else
        {
            armPivot.transform.localPosition = walkArmPivot;
        }
    }

    private void ThrowTrajectory()
    {
        beginTraj = armPivot.transform.position + Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * (Vector3.right * 0.2f);
        endTraj = armPivot.transform.position + Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg) * (Vector3.right * 1.5f);

        RaycastHit hit;
        if (Physics.Raycast(armPivot.transform.position, endTraj - beginTraj, out hit, 1f))
        {
            endTraj = hit.point;
        }

        lrTraj.SetPosition(0, beginTraj);
        lrTraj.SetPosition(1, endTraj);

        lrTraj.enabled = true;
    }

    public void ThrowCandy()
    {
        GameObject candyClone = Instantiate(candy, beginTraj, Quaternion.identity);
        candyClone.GetComponent<Rigidbody>().velocity = (endTraj - beginTraj).normalized * throwForce;
        Destroy(candyClone, 2f);
        StartCoroutine(Reloading());
    }

    private IEnumerator Reloading()
    {
        canThrow = false;
        yield return new WaitForSeconds(throwCooldown);
        canThrow = true;
    }

    private void OnOffLamp()
    {
        if (lampStaminaBar.value > 0)
        {
            canBePressed = true;
        }

        if (Input.GetButtonDown("Lamp") && canBePressed)
        {
            isPressed = !isPressed;
            audioSourceLightPlayer.clip = AudioManager.Instance.audioClipPlayer[3];
            audioSourceLightPlayer.Play();
        }

        if (isPressed && lampStaminaBar.value > 0)
        {
            lamp1.intensity = 3f;
            lamp2.intensity = 3f;
        }
        else if (lampStaminaBar.value <= 0)
        {
            lamp1.intensity = 0f;
            lamp2.intensity = 0f;
            canBePressed = false;
        }
        else if (!isPressed)
        {
            lamp1.intensity = 0f;
            lamp2.intensity = 0f;
        }
    }

    private void StaminaManager()
    {
        if (lamp1.intensity > 0 || lamp2.intensity > 0)
        {
            lampStaminaBar.value -= speedStaminaDown * Time.deltaTime;
        }

        if (lampStaminaBar.value <= 0)
        {
            Camera.main.fieldOfView -= Time.deltaTime * zoomSpeed; // calcul to be determined with the value of the stamina
        }
        else
        {
            Camera.main.fieldOfView = 32.8f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LampCharge"))
        {
            lampStaminaBar.value += valueReload;
            Destroy(other.gameObject);
        }
    }

    private bool OnGround()
    {
        Debug.DrawLine(playerCollider.bounds.center, playerCollider.bounds.center + Vector3.down * (playerCollider.bounds.extents.y + 0.5f), Color.red);

        if (Physics.Raycast(playerCollider.bounds.center, Vector3.down,
                playerCollider.bounds.extents.y + 0.5f))
        {
            return true;
        }
        
        return false;
    }

    public void Jump()
    {
        float axis = Input.GetAxis("Horizontal"); 

        if (axis == 0)
        {
            //playerController.
        }
    }
}
