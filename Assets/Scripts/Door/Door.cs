using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [HideInInspector] public bool isOpen = false; //pour le saveSystem

    private bool canInteract;
    private float alpha;
    private Quaternion iniRot;
    private Quaternion finalRot;
    private AudioSource audioSourceDoor;
    public bool isLeftDoor;
    public bool isLocked;
    public float openingSpeed;


    private void Start()
    {
        audioSourceDoor = gameObject.AddComponent<AudioSource>();

        iniRot = Quaternion.Euler(0, -90, 0);

        if (isLeftDoor)
        {
            finalRot = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            finalRot = Quaternion.Euler(0, -180, 0);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && canInteract && !isLocked)
        {
            alpha = 0;
            isOpen = !isOpen;
        }

        alpha += Time.deltaTime * openingSpeed;
        alpha = Mathf.Clamp01(alpha);

        if (isOpen)
        {
            transform.rotation = Quaternion.Lerp(iniRot, finalRot, alpha);
            audioSourceDoor.clip = AudioManager.Instance.audioClipDoor[0];
            if (!audioSourceDoor.isPlaying)
            {
                audioSourceDoor.Play();
            }
        }
        else
        {
            transform.rotation = Quaternion.Lerp(finalRot, iniRot, alpha);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
