using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private Animator anim;
    public GameObject mesh;

    [Header("Managers")]
    public EvaManager manager;
    public AudioManager audioManager;

    [Header("Movement")]
    public Transform[] waypoints;
    public float waitBeforeMoving;
    private bool isWaiting;
    private int nextPosIndex;
    public float speed;
    private CapsuleCollider collider;
    private Vector3 direction;

    [Header("AI")]
    public float agroDistance;
    private Transform player;
    private bool chasingPlayer;

    void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        anim = mesh.GetComponent<Animator>();
        player = FindObjectOfType<Player>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (nextPosIndex >= waypoints.Length)
        {
            nextPosIndex = 0;
        }

        if (waypoints.Length > 0)
        {
            direction = waypoints[nextPosIndex].position - transform.position;
            direction.y = 0;
        }

        if (CheckPlayer())
        {
            
        }

        if(!isWaiting)
            MonsterMove();
    }

    private bool CheckPlayer()
    {
        Debug.DrawLine(transform.position, transform.position + (player.position - transform.position).normalized * agroDistance, Color.red);

        if ((transform.position - player.position).sqrMagnitude < (agroDistance * agroDistance))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, agroDistance))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    chasingPlayer = true;
                    return true;
                }
            }
        }

        chasingPlayer = false;
        return false;
    }

    public void CandyAlert(Vector3 position)
    {
        //if (!chasingPlayer)
        //{
        //    StopAllCoroutines();
        //    GoTo(position);
        //    isWaiting = false;
        //}
    }

    private void MonsterMove()
    {
        Vector3 velocity = direction.normalized * Time.deltaTime * speed;

        if (direction.sqrMagnitude <= 0.25f || ObstacleInFront())
        {
            velocity = Vector3.zero;
            isWaiting = true;
            StartCoroutine(WaitThenGo(waitBeforeMoving));
        }

        transform.position += new Vector3(velocity.x, 0, velocity.z);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private bool ObstacleInFront()
    {
        Debug.DrawLine(collider.bounds.center, collider.bounds.center + transform.forward * (collider.bounds.extents.x + 0.1f), Color.red);
        return (Physics.Raycast(collider.bounds.center, transform.forward, collider.bounds.extents.x + 0.1f));
    }

    IEnumerator WaitThenGo(float waitTime)
    {
        if (waypoints.Length > 0)
        {
            nextPosIndex++;
        }

        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        StopCoroutine(WaitThenGo(0));
    }
}
