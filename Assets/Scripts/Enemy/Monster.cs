using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    public GameObject mesh;
    private AudioManager audioManager;

    [Header("Movement")]
    public Transform[] waypoints;
    public float waitBeforeMoving;
    private bool isWaiting;
    private int nextPosIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        anim = mesh.GetComponent<Animator>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[0].position);
            nextPosIndex++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //MonsterMove();
    }

    public void GoTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    IEnumerator WaitBeforeGo(Vector3 nextPosition)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitBeforeMoving);
        GoTo(nextPosition);
        yield return new WaitForSeconds(.25f);
        isWaiting = false;
    }

    public void MonsterMove()
    {
        if (nextPosIndex >= waypoints.Length)
        {
            nextPosIndex = 0;
        }

        if (waypoints.Length > 0 && agent.velocity == Vector3.zero && !isWaiting)
        {
            StartCoroutine(WaitBeforeGo(waypoints[nextPosIndex].position));
            nextPosIndex++;
            audioManager.MonsterStop();
        }

        anim.SetFloat("Speed", agent.velocity.magnitude);

    }
}
