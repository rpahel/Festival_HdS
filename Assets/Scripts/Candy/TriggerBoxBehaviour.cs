using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxBehaviour : MonoBehaviour
{
    public float distanceFromCandy;
    public float speed = 0.2f;

    private bool isMoving = false;

    private GameObject gameObjectCollision;

    public AnimationCurve animCurve;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObjectCollision = collision.gameObject;
            isMoving = true;
        }
    }

    private void Update()
    {
        if(isMoving == true)
        {
            gameObjectCollision.transform.position = Vector3.MoveTowards(gameObjectCollision.transform.position, transform.parent.position, Time.deltaTime * speed); // Enemy goes to the candy's position
            if(Vector3.Distance(gameObjectCollision.transform.position, transform.parent.position) <= distanceFromCandy)
            {
                gameObjectCollision.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Destroy(gameObject);
                isMoving = false;
            }
        }
    }
}
