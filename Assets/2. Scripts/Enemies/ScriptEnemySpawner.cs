using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptEnemySpawner : MonoBehaviour {
    public GameObject enemy;
    public float minWaitTime;
    public float maxWaitTime;
    public float waitTimeDecrease;
    private Transform player;
    private List<Transform> allPatrolPoints;
    private float currentWaitTime;
    private float timer;

	void Start () {
        currentWaitTime = maxWaitTime;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //get all patrolpoint transforms
        allPatrolPoints = new List<Transform>();
        GameObject[] pps = GameObject.FindGameObjectsWithTag("PatrolPoint");
        foreach(GameObject pp in pps)
        {
            allPatrolPoints.Add(pp.GetComponent<Transform>());
        }
	}
	
	void Update () {
        timer += Time.deltaTime;
        if(currentWaitTime < minWaitTime)
        {
            currentWaitTime = minWaitTime;
        }
        if (timer > currentWaitTime)
        {
            timer = 0;
            currentWaitTime -= waitTimeDecrease;
            GameObject newEnemy = Instantiate(
                enemy,
                transform.position,
                transform.rotation) as GameObject;
            //add random patrolpoints
            List<Transform> newPatrols = new List<Transform>();
            for(int i = 1; i < 5; i++)
            {
                int test = Random.Range(0, allPatrolPoints.Count);
                Transform randomPP = allPatrolPoints[test];
                newPatrols.Add(randomPP);
            }
            //last patrolpoint is player
            newPatrols.Add(player.transform);
            newEnemy.GetComponent<ScriptEnemyPatrol>().patrolPoints = newPatrols.ToArray();
        }
	}
}
