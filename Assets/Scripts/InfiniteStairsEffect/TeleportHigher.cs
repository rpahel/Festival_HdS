using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TeleportHigher : MonoBehaviour
{
    public float yAmount;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Joueur touch� !");
            (other.gameObject.GetComponent<CharacterController>()).enabled = false;
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + yAmount, other.gameObject.transform.position.z);
            (other.gameObject.GetComponent<CharacterController>()).enabled = true;
        }
    }
}
