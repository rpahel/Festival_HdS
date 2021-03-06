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
    private bool isDead;

    [Header("GameManager")]
    public EvaManager manager;
    private bool godMode;
    [HideInInspector] public bool onPhone;
    [HideInInspector] public bool isPaused;

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpHeight;
    private CharacterController playerController;
    private Collider playerCollider;
    private Vector3 jumpDir = Vector3.zero;

    [Header("Animations")]
    public Animator animPlayer;

    [Header("Lamp")]
    public GameObject armPivot, leftArm, rightArm;
    private MeshRenderer leftArmMesh, rightArmMesh;
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
    private GameObject candyClone;
    public float throwForce;
    public float throwCooldown;
    private LineRenderer lrTraj;
    private Vector3 beginTraj;
    private Vector3 endTraj;
    private float angle;
    private bool canThrow;
    private bool isLanding = false;
    private bool isCandyLanding;

    [Header("Camera")]
    public Camera mainCam;
    private float cameraXValueToKeep;

    [Header("Audio")]
    private AudioManager audioManager;

    [Header("Vent")]
    private Vector3 destination;
    private bool isEntering;
    private bool ventActivated;

    private void Awake()
    {
        playerController = GetComponent<CharacterController>();
        lrTraj = GetComponent<LineRenderer>();
        playerCollider = GetComponent<Collider>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        canThrow = true;
    }

    private void Start()
    {
        leftArmMesh = leftArm.GetComponent<MeshRenderer>();
        rightArmMesh = rightArm.GetComponent<MeshRenderer>();

        cameraXValueToKeep = mainCam.transform.position.x;

        walkArmPivot = armPivot.transform.localPosition;

        lrTraj.startWidth = 0.05f;
        lrTraj.endWidth = 0.05f;

        lampStaminaBar.maxValue = maxValueStamina;
        lampStaminaBar.value = lampStaminaBar.maxValue;
    }

    private void Update()
    {
        if (onPhone)
        {
            lampStaminaBar.value = maxValueStamina;
            StaminaManager();
        }

        if (isDead || isPaused)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Y) && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.V))
        {
            godMode = !godMode;

            if (godMode)
            {
                Debug.Log("GodMode is Enabled.");
            }
            else
            {
                Debug.Log("GodMode is Disabled.");
            }
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            Death();
        }

        if (mainCam.fieldOfView <= 0.7f)
        {
            Death();
        }

        if (!ventActivated && !onPhone)
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
        else
        {
            if (isEntering)
            {
                transform.position += Vector3.forward * Time.deltaTime * 1f;
            }
        }

        if(candyClone != null)
        {
            isCandyLanding = Physics.Raycast(candyClone.transform.position, -Vector3.up, 0.2f);
        }

        if (isLanding)
        {

            if (isCandyLanding)
            {
                audioManager.CandyLanding();
                StartCoroutine(SoundIsPlaying());
            }
        }

        if (godMode)
        {
            lampStaminaBar.value = maxValueStamina;
        }
    }

    private IEnumerator SoundIsPlaying()
    {
        yield return new WaitForSeconds(0.1f);
        isLanding = false;
    }

    private void MovePlayer()
    {
        // Move player
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (Input.GetButton("Run"))
        {
            playerController.Move(movement.normalized * runSpeed * Time.deltaTime);
        }
        else
        {
            playerController.Move(movement.normalized * walkSpeed * Time.deltaTime);
        }


        //Footsteps
        if (playerController.velocity == Vector3.zero)
        {
            audioManager.StopPlayerWalk();
        }
        else
        {
            if (playerController.velocity.magnitude <= 2f)
            {
                audioManager.PlayerWalk();
            }
            else if (playerController.velocity.magnitude > 2.2f)
            {
                audioManager.PlayerRun();
            }
        } 

        if (transform.position.x <= -4.6f || transform.position.x >= 4.6f)
        {
            mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, mainCam.transform.position.z);
        }
        else
        {
            mainCam.transform.position = new Vector3(cameraXValueToKeep, transform.position.y + 1f, mainCam.transform.position.z);
        }

        animPlayer.SetFloat("Speed", playerController.velocity.sqrMagnitude);

        if (Input.GetButtonDown("Jump") && isGrounded(.1f))
        {
            animPlayer.SetTrigger("Jump");
            ArmMesh(false);
        }
        
        jumpDir.y += -9.81f * 2f * Time.deltaTime;

        if (isGrounded(.1f) && jumpDir.y < 0)
        {
            jumpDir.y = 0;
            animPlayer.SetBool("isFalling", false);
        }
        else if (!isGrounded(.1f) && jumpDir.y < 0)
        {
            if (!isGrounded(.3f)) // Juste pour ?tre s?r qu'on est pas sur une pente
            {
                animPlayer.SetBool("isFalling", true);
                ArmMesh(false);
            }
        }

        playerController.Move(jumpDir * Time.deltaTime);
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

        if (playerController.velocity.sqrMagnitude > 20)
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
        isLanding = true;
        candyClone = Instantiate(candy, beginTraj, Quaternion.identity);
        candyClone.GetComponent<Rigidbody>().velocity = (endTraj - beginTraj).normalized * throwForce;
        audioManager.PlayThrowCandy();
        
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
            audioManager.LightOnAndOff();
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
            mainCam.fieldOfView -= Time.deltaTime * zoomSpeed;
            mainCam.transform.position = new Vector3(transform.position.x, transform.position.y + (mainCam.fieldOfView / 32.8f) + 1 / (mainCam.fieldOfView + 0.01f), mainCam.transform.position.z);
            audioManager.CamZooming();
        }
        else
        {
            audioManager.CamNotZooming();
            mainCam.fieldOfView = 32.8f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LampCharge"))
        {
            lampStaminaBar.value += valueReload;
            audioManager.ReloadLight();
            Destroy(other.gameObject);
        }
    }

    private bool isGrounded(float distance)
    {
        //Debug.DrawLine(playerCollider.bounds.center, playerCollider.bounds.center + Vector3.down * (playerCollider.bounds.extents.y + distance), Color.red);
        //Debug.DrawLine(playerCollider.bounds.center + (Vector3.down * (playerCollider.bounds.extents.y - playerCollider.bounds.extents.x)),
        //    (playerCollider.bounds.center + (Vector3.down * (playerCollider.bounds.extents.y - playerCollider.bounds.extents.x))) + (new Vector3(-1f, -1f, 0)).normalized * (playerCollider.bounds.extents.x + distance),
        //    Color.red);
        //Debug.DrawLine(playerCollider.bounds.center + (Vector3.down * (playerCollider.bounds.extents.y - playerCollider.bounds.extents.x)),
        //    (playerCollider.bounds.center + (Vector3.down * (playerCollider.bounds.extents.y - playerCollider.bounds.extents.x))) + (new Vector3(1f, -1f, 0)).normalized * (playerCollider.bounds.extents.x + distance),
        //    Color.red);

        if (Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + distance) ||
            Physics.Raycast(playerCollider.bounds.center + (Vector3.down * (playerCollider.bounds.extents.y - playerCollider.bounds.extents.x)), (new Vector3(-1f,-1f,0)).normalized, (playerCollider.bounds.extents.x + distance)) ||
            Physics.Raycast(playerCollider.bounds.center + (Vector3.down * (playerCollider.bounds.extents.y - playerCollider.bounds.extents.x)), (new Vector3(1f,-1f,0)).normalized, (playerCollider.bounds.extents.x + distance)))
        {
            return true;
        }

        return false;
    }

    public void Jump()
    {
        if (isGrounded(.1f))
        {
            jumpDir.y = Mathf.Sqrt(jumpHeight * -3.0f * -9.81f);
            audioManager.Jump();
        }
    }

    public void ArmMesh(bool active)
    {
        leftArmMesh.enabled = active;
        rightArmMesh.enabled = active;
    }

    public void Vent(Vector3 beginPoint, Vector3 endPoint)
    {
        ventActivated = true;
        transform.position = beginPoint - Vector3.right * .25f;
        destination = endPoint;
        animPlayer.SetTrigger("Vent");
        playerController.enabled = false;
        armPivot.SetActive(false);
    }

    public void GoingInside()
    {
        isEntering = true;
    }

    public void Teleport()
    {
        transform.position = destination - Vector3.right * .25f + Vector3.up * .5f;
        isEntering = false;
        playerController.enabled = true;
        armPivot.SetActive(true);
        ventActivated = false;
    }

    public void Death()
    {
        if (godMode)
        {
            Debug.Log("You are in god mode and therefore can't die.");
            return;
        }

        if (!isDead)
        {
            isDead = true;
            armPivot.SetActive(false);
            animPlayer.SetTrigger("Death");
            Destroy(gameObject, 5f);
        }
    }

    public void QuitPhone()
    {
        onPhone = false;
        armPivot.SetActive(true);
    }
}
