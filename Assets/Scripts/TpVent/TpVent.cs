using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpVent : MonoBehaviour
{
    public Transform endPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 endPointChanged = new Vector3(endPoint.position.x + 2f, endPoint.position.y, endPoint.position.z);
            Debug.Log(endPointChanged);
            Debug.Log(other.gameObject);
            other.transform.position = endPointChanged;
        }
        Debug.Log(other.gameObject);
    }
}
