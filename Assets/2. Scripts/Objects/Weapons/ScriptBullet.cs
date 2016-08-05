using UnityEngine;
using System.Collections;

public class ScriptBullet : MonoBehaviour
{
    public float maxDamage;
    public float maxVelocity;
    public float despawnTime;

    private float despawnTimer;
    private ManagerGame manager;
    private Rigidbody rigidBody;
    private bool hasHitEnemyOrPlayer = false;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
    }

    void FixedUpdate()
    {
        if (manager.status == statusGame.playing)
        {
            despawnTimer += Time.deltaTime;
            if (despawnTimer > despawnTime)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //multiple collisions at once are possible because of continuous collision detections
            //these collisions will all trigger before the gameobject is destroyed 
            //workaround => only hit player/enemy once
            if (!hasHitEnemyOrPlayer)
            {
                rigidBody = gameObject.GetComponent<Rigidbody>();
                float dmg = maxDamage * 2 * rigidBody.velocity.magnitude / maxVelocity;
                collision.gameObject.GetComponent<ManagerEnemy>().currentHealth -= dmg;
                hasHitEnemyOrPlayer = true;
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (!hasHitEnemyOrPlayer)
            {
                rigidBody = gameObject.GetComponent<Rigidbody>();
                float dmg = maxDamage * 2 * rigidBody.velocity.magnitude / maxVelocity;
                collision.gameObject.GetComponent<ManagerPlayer>().currentHealth -= dmg;
                hasHitEnemyOrPlayer = true;
                Destroy(gameObject);
            }
        }
    }
}
