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
    private int nextPosIndex;
    public float speed;
    private CapsuleCollider coll;
    private Vector3 direction;
    private Vector3 candyPos;

    [Header("AI")]
    public float waitBeforeMoving;
    private bool isWaiting;
    public float agroDistance;
    private bool heardCandy;
    private Transform player;

    [Header("Attack")]
    private bool playerInRange;

    void Awake()
    {
        coll = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        anim = mesh.GetComponent<Animator>();
        player = FindObjectOfType<Player>().gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length > 0 && !heardCandy)
        {
            if (nextPosIndex >= waypoints.Length)
            {
                nextPosIndex = 0;
            }

            direction = waypoints[nextPosIndex].position - transform.position;
            direction.y = 0;
        }
        else if(heardCandy)
        {
            direction = candyPos - transform.position;
            direction.y = 0;
        }
        else
        {
            direction = Vector3.zero;
        }

        if (CheckPlayer())
        {
            direction = player.position - transform.position;
            direction.y = 0;
        }

        if(!isWaiting)
            MonsterMove();
    }

    private bool CheckPlayer()
    {
        if ((transform.position - player.position).sqrMagnitude < (agroDistance * agroDistance) && Vector3.Dot((transform.position - player.position), transform.forward) < 0)
        {
            Debug.DrawLine(transform.position, transform.position + (player.position - transform.position).normalized * agroDistance, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, agroDistance))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void CandyAlert(Vector3 position)
    {
        heardCandy = true;
        candyPos = position;
        StartCoroutine(Forget());
    }

    private void MonsterMove()
    {
        Vector3 velocity = direction.normalized * Time.deltaTime * speed;
        anim.SetFloat("Speed", 1f);

        if (direction.sqrMagnitude <= 0.25f || ObstacleInFront())
        {
            velocity = Vector3.zero;
            anim.SetFloat("Speed", 0);
            if (!heardCandy)
            {
                isWaiting = true;
                StartCoroutine(WaitThenGo(waitBeforeMoving));
            }
        }

        transform.position += new Vector3(velocity.x, 0, velocity.z);
        if(direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

    }

    private bool ObstacleInFront()
    {
        Debug.DrawLine(coll.bounds.center, coll.bounds.center + transform.forward * (coll.bounds.extents.x + 0.1f), Color.red);
        return (Physics.Raycast(coll.bounds.center, transform.forward, coll.bounds.extents.x + 0.1f));
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

    IEnumerator Forget()
    {
        yield return new WaitForSeconds(5f);
        heardCandy = false;
        StopCoroutine(Forget());
    }

    public void Attack()
    {
        if (playerInRange)
        {
            Debug.Log("Mort");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            anim.SetTrigger("Attack");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
