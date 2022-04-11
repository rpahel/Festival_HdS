using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfiniteStairsEffect : MonoBehaviour
{
    public float yAmount;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            (other.gameObject.GetComponent<CharacterController>()).enabled = false;
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + yAmount, other.gameObject.transform.position.z);
            (other.gameObject.GetComponent<CharacterController>()).enabled = true;
        }
    }
}
