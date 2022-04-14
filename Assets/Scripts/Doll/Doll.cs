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

    private AudioManager audioManager;

    // Update is called once per frame
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        yIniPos = transform.position.y;
    }
    private void Update()
    {
        transform.position = new Vector3(transform.position.x, yIniPos + Mathf.Sin(Time.time * speed) * amplitude, transform.position.z);
        transform.rotation = Quaternion.Euler( 15, Time.time * rotSpeed, (transform.rotation).eulerAngles.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioManager.FindDoll();
            Destroy(gameObject);
        }
    }
}
