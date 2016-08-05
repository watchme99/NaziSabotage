using UnityEngine;
using System.Collections;

public class ScriptEnemyAttack : MonoBehaviour
{
    public float attackDistanceView = 400;
    public float attackDistanceHear = 100;
    public float fieldOfView = 120;
    public float waitBeforeAttack = 10f;

    private Transform player;
    private ControllerPlayer controllerPlayer;
    private ScriptGun gunScript;
    private ManagerEnemy enemy;
    private NavMeshAgent agent;
    private float waitBeforeAttackTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        controllerPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllerPlayer>();
        gunScript = GetComponentInChildren<ScriptGun>();
        enemy = gameObject.GetComponent<ManagerEnemy>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.SqrMagnitude(player.position - transform.position);
        if (distance < attackDistanceView)
        {
            //check if player is in field of view and if anything is blocking vision
            Vector3 targetDir = player.position - transform.position;
            Vector3 forward = transform.forward;
            float angle = Vector3.Angle(targetDir, forward);
            if ((angle < fieldOfView / 2) && !Physics.Linecast(transform.position, player.position, 1))
            {
                Attack();
            }
            //check if player is close enough for hearing
            else if (distance < attackDistanceHear && !controllerPlayer.sneaking)
            {
                enemy.status = statusEnemy.search;
                waitBeforeAttackTimer = 0;
                enemy.SetLastPlayerPosition(player.position);
            }
            //enemy was attacking => now searching
            else if (enemy.status == statusEnemy.attack)
            {
                enemy.status = statusEnemy.search;
                waitBeforeAttackTimer = 0;
            }
        }
        //change enemy status to search for player
        else if (enemy.status == statusEnemy.attack)
        {
            enemy.status = statusEnemy.search;
            waitBeforeAttackTimer = 0;
        }
    }

    private void Attack()
    {
        agent.Stop();
        enemy.status = statusEnemy.attack;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), 10 * Time.deltaTime);
        enemy.SetLastPlayerPosition(player.position);
        waitBeforeAttackTimer += Time.deltaTime;
        if (waitBeforeAttackTimer > waitBeforeAttack)
        {
            gunScript.Shoot(player.position);
        }
    }

    public void InvestigateSound(Vector3 sound)
    {
        if (enemy.status != statusEnemy.attack)
        {
            enemy.SetLastPlayerPosition(sound);
            enemy.status = statusEnemy.search;
        }
    }
}
