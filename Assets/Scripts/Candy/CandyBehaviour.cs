using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyBehaviour : MonoBehaviour
{
    public GameObject triggerBox;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            triggerBox.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(RemoveFromParent());
        }
    }

    private IEnumerator RemoveFromParent()
    {
        yield return new WaitForSeconds(2f);
        triggerBox.transform.parent = null;
    }
}
