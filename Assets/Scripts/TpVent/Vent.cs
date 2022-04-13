using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public Transform endPoint;
    private bool canInteract;
    private Collider otherCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = true;
            otherCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    void Update()
    {
        if (otherCollider != null && canInteract && Input.GetButtonDown("Interact"))
        {
            canInteract = false;
            otherCollider.gameObject.GetComponent<Player>().Vent(gameObject.transform.position, endPoint.position);
        }
    }
}
