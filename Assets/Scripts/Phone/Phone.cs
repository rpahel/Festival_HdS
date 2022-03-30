using UnityEngine;

public class Phone : MonoBehaviour
{
    private bool isInTrigger = false;
    private bool canInteract = true;
    public GameObject interact;
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isInTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (canInteract)
                {
                    canInteract = false;
                    interact.SetActive(false);
                    //DO THINGS
                    source.Play();
                    GameManager.instance.SaveData(); //save
                }
            }
        }

        if(source.isPlaying == false)
        {
            canInteract = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interact.SetActive(true);
        isInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = true;
        interact.SetActive(false);
        source.Stop();
        isInTrigger = false;
    }
}
