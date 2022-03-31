using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoxBehaviour : MonoBehaviour
{
    private float timer, alpha;
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
        timer += Time.deltaTime / 3;
        alpha = animCurve.Evaluate(timer);
        if(isMoving == true)
        {
            gameObjectCollision.transform.position = Vector3.Lerp(gameObjectCollision.transform.position, gameObject.transform.parent.position, alpha); // Enemy goes to the candy's position
            //isMoving = false;
            //timer = 0f;
        }
    }
}
