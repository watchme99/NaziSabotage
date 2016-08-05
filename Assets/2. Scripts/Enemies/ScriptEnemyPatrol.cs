using UnityEngine;
using System.Collections;

public class ScriptEnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints = null;
    public float waitTime = 3;
    //when close enough to patrolpoint (radius), movement will stop, wait and move to next point
    public float patrolPointRadiusSQRD = 3;
    public float patrolSpeed;
    public float searchSpeed;

    private float waitTimer;
    private float checkTimer;
    private int patrolPoint = 0;
    private NavMeshAgent agent;
    private ManagerEnemy enemy;

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemy = gameObject.GetComponent<ManagerEnemy>();
    }

    void FixedUpdate()
    {
        enemy.speed = agent.desiredVelocity.magnitude;

        if (enemy.status == statusEnemy.idle)
        {
            Idle();
        }
        else if (enemy.status == statusEnemy.patrol)
        {
            Patrol();
        }
        else if (enemy.status == statusEnemy.search)
        {
            Search();
        }
    }

    void Idle()
    {
        if (waitTimer > waitTime)
        {
            waitTimer = 0;
            if (patrolPoint >= patrolPoints.Length - 1)
            {
                patrolPoint = 0;
            }
            else
            {
                patrolPoint++;
            }
            enemy.status = statusEnemy.patrol;
        }
        waitTimer += Time.deltaTime;
        //look in the direction of patrolpoint rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, patrolPoints[patrolPoint].rotation, 5 * Time.deltaTime);
    }

    void Patrol()
    {
        agent.Resume();
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[patrolPoint].position);
        //if enemy reaches patrolpoint => idle
        if (Vector3.Distance(transform.position, patrolPoints[patrolPoint].position) < patrolPointRadiusSQRD)
        {
            enemy.status = statusEnemy.idle;
        }
    }

    void Search()
    {
        Vector3 lastPlayerPosition = enemy.GetLastPlayerPosition();
        agent.SetDestination(lastPlayerPosition);
        agent.Resume();
        agent.speed = searchSpeed;
        if (Vector3.Distance(transform.position, lastPlayerPosition) < patrolPointRadiusSQRD)
        {
            //look left (90°) takes 1 second, look right takes 2 seconds (180°)
            //look left
            if (checkTimer < 1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 90, 0), 3 * Time.deltaTime);
            }
            //look right
            else if (checkTimer < 3)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, -90, 0), 3 * Time.deltaTime);
            }
            else
            {
                checkTimer = 0;
                enemy.status = statusEnemy.patrol;
            }
            checkTimer += Time.deltaTime;
        }
    }
}

