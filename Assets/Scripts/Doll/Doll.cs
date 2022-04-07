using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Doll : MonoBehaviour
{
    [Header("Item floating.")]
    [SerializeField] private float speed;
    [SerializeField] private float amplitude;
    [SerializeField] private float rotSpeed;
    private float yIniPos;

    // Update is called once per frame
    private void Awake()
    {
        yIniPos = transform.position.y;
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, yIniPos + Mathf.Sin(Time.time * speed) * amplitude, transform.position.z);
        transform.rotation = Quaternion.Euler( 15, Time.time * rotSpeed, (transform.rotation).eulerAngles.z);
    }
}
