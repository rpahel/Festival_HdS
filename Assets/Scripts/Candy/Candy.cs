using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    private EvaManager manager;
    private List<Monster> monsters;
    public float soundDistance;

    void Start()
    {
        manager = FindObjectOfType<EvaManager>();
        monsters = manager.monsters;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HardSurface"))
        {
            for(int i = 0; i < monsters.Count; i++)
            {
                float distance = (monsters[i].gameObject.transform.position - transform.position).magnitude;
                if(distance < soundDistance)
                {
                    monsters[i].CandyAlert(transform.position);
                }
            }
        }
    }
}
