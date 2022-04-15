using System.Collections;
using UnityEngine;

public class Phone : MonoBehaviour
{
    private bool canInteract = true;
    private EvaManager manager;
    private GameObject player;
    public GameObject phone2_1;
    public GameObject phone2_2;

    private void Start()
    {
        manager = FindObjectOfType<EvaManager>();
    }

    private void Update()
    {
        if (Input.GetButtonDown(("Interact")) && canInteract)
        {
            canInteract = false;
            manager.playerScript.onPhone = true;
            manager.playerScript.armPivot.SetActive(false);
            if ((manager.player.transform.position - transform.position).x >= 0)
            {
                manager.playerScript.animPlayer.SetBool("FaceLeft", true);
            }
            else
            {
                manager.playerScript.animPlayer.SetBool("FaceLeft", false);
            }
            manager.playerScript.animPlayer.SetTrigger("onPhone");
            player.transform.position = new Vector3((transform.position + transform.right * .75f).x, player.transform.position.y, transform.position.z);
            manager.playerSpawn.position = transform.position + transform.right * .75f;
            StartCoroutine(PhoneStuff());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
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

    IEnumerator PhoneStuff()
    {
        yield return new WaitForSeconds(.3f);
        phone2_1.SetActive(false);
        phone2_2.SetActive(false);
        yield return new WaitForSeconds(3.3f);
        phone2_1.SetActive(true);
        phone2_2.SetActive(true);
        StopCoroutine(PhoneStuff());
    }
}
