using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaManager : MonoBehaviour
{
    [Header("Manager")]
    public AudioManager audioManager;

    [Header("Player")]
    public GameObject player;
    [HideInInspector] public Player playerScript;
    public Transform playerSpawn;
    public Slider lampStaminaBar;
    public Camera mainCam;

    [Header("Monsters")]
    public GameObject monster;
    [HideInInspector] public List<Monster> monsters;
    public Transform[] monstersSpawn;
    public Transform[] monstersWayPoints;

    [Header("Dolls")]
    public GameObject doll;
    [HideInInspector] public List<Doll> dolls;
    public Transform[] dollSpawn;

    [Header("Doors")]
    public List<Door> doors;

    void Start()
    {
        GameObject playerClone = Instantiate(player, playerSpawn.position, Quaternion.identity);
        playerScript = playerClone.GetComponent<Player>();
        playerScript.manager = this;
        playerScript.lampStaminaBar = lampStaminaBar;
        playerScript.mainCam = mainCam;

        for (int i = 0; i < monstersSpawn.Length; i++)
        {
            GameObject monsterClone = Instantiate(monster, monstersSpawn[i].position, monstersSpawn[i].rotation);
            monsterClone.GetComponent<Monster>().manager = this;
            monsterClone.GetComponent<Monster>().audioManager = audioManager;
            switch (i)
            {
                case 6:
                    Transform[] wpts0 = new Transform[4];
                    wpts0[0] = monstersWayPoints[6];
                    wpts0[1] = monstersWayPoints[7];
                    wpts0[2] = monstersWayPoints[8];
                    wpts0[3] = monstersWayPoints[9];
                    monsterClone.GetComponent<Monster>().waypoints = wpts0;
                    break;

                case 7:
                    Transform[] wpts1 = new Transform[4];
                    wpts1[0] = monstersWayPoints[0];
                    wpts1[1] = monstersWayPoints[3];
                    wpts1[2] = monstersWayPoints[4];
                    wpts1[3] = monstersWayPoints[3];
                    monsterClone.GetComponent<Monster>().waypoints = wpts1;
                    break;

                case 8:
                    Transform[] wpts2 = new Transform[4];
                    wpts2[0] = monstersWayPoints[5];
                    wpts2[1] = monstersWayPoints[2];
                    wpts2[2] = monstersWayPoints[1];
                    wpts2[3] = monstersWayPoints[2];
                    monsterClone.GetComponent<Monster>().waypoints = wpts2;
                    break;

                default: // aucun waypoint
                    break;
            }
            monsters.Add(monsterClone.GetComponent<Monster>());
        }

        for (int i = 0; i < dollSpawn.Length; i++)
        {
            GameObject dollClone = Instantiate(doll, dollSpawn[i].position, Quaternion.identity);
            dollClone.GetComponent<Doll>().id = i;
            dolls.Add(dollClone.GetComponent<Doll>());
        }
    }
}
