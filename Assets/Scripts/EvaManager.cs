using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public Transform playerSpawn;

    [Header("Monsters")]
    public GameObject monster;
    [HideInInspector] public Monster[] monsters;
    public Transform[] monstersSpawn;

    [Header("Dolls")]
    public GameObject doll;
    [HideInInspector] public Doll[] dolls;
    public Transform[] dollSpawn;

    [Header("Doors")]
    public Door[] doors;

    void Start()
    {
        monsters = FindObjectsOfType<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
