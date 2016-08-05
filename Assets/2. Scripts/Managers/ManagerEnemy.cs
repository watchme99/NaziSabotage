using UnityEngine;
using System.Collections;

public enum statusEnemy { idle, patrol, attack, search }

public class ManagerEnemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float regenerate;
    public statusEnemy status = statusEnemy.idle;
    public float speed;
    public GameObject ragDoll;
    public GameObject pickupAK47;

    private ManagerGame manager;
    private ManagerMusic music;
    private Vector3 lastPlayerPosition;
    private Animator anim;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        music = GameObject.Find("MusicController").GetComponent<ManagerMusic>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regenerate * Time.deltaTime;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Dying();
        }
        checkStatus();
    }

    //check status + do animations and sounds
    private void checkStatus()
    {
        if (speed < 0.5)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }
        else if (speed < 3)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", true);
        }

        if (status == statusEnemy.attack)
        {
            anim.SetBool("Attack", true);
            music.setMusicAttack();
        }
        else
        {
            anim.SetBool("Attack", false);
        }

        if (status == statusEnemy.search)
        {
            music.setMusicSearch();
        }
    }

    //ragdoll + gun to pick up
    void Dying()
    {
        manager.kills++;
        Instantiate(ragDoll, transform.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0));
        Transform gun = this.gameObject.transform.Find("hand_R/PrefabAK47/Gun");
        GameObject newAK47 = Instantiate(pickupAK47, gun.position, gun.rotation) as GameObject;
        ScriptGun oldgun = gun.GetComponent<ScriptGun>();
        newAK47.GetComponent<ScriptPickupAK47>().ammo = oldgun.ammoInClip + oldgun.ammoReserve;
        Destroy(gameObject);
    }

    public void SetLastPlayerPosition(Vector3 lastPlayerPosition)
    {
        this.lastPlayerPosition = lastPlayerPosition;
    }

    public Vector3 GetLastPlayerPosition()
    {
        return lastPlayerPosition;
    }
}
