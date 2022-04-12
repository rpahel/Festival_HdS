using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpVent : MonoBehaviour
{
    public Transform endPoint;
    private void Start()
    {
        Debug.Log(endPoint.position);
        Debug.Log(endPoint.parent.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector3 endPointChanged = new Vector3(endPoint.position.x + 2f, endPoint.position.y, endPoint.position.z);
            GameObject.FindGameObjectWithTag("Player").transform.position = endPointChanged;
        }
    }
}
