using UnityEngine;
using System.Collections;

public class ScriptGun : MonoBehaviour
{
    public float rateOfFire = 0.1f;
    public float reloadTime = 2f;
    public float bulletSpeed = 500f;
    public int clipSize = 15;
    public int ammoInClip = 15;
    public int ammoReserve = 30;
    public GameObject bullet;
    public AudioClip audioShot;
    public AudioClip audioReload;
    public Renderer muzzleFlash;
    public Light muzzleLight;

    private float rateOfFireTimer;
    private float reloadTimer;
    private AudioSource audioSource;
    private ManagerGame manager;
    private Transform bulletSpawnPoint;
    private Animator anim;

    void Start()
    {
        reloadTimer = reloadTime; 
        bulletSpawnPoint = this.transform.GetChild(0).transform;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (manager.status == statusGame.playing)
        {
            rateOfFireTimer += Time.deltaTime;
            reloadTimer += Time.deltaTime;
        }
    }

    //give target to aim at that target
    public void Shoot(Vector3 target = default(Vector3))
    {
        if (rateOfFireTimer > rateOfFire && reloadTimer > reloadTime)
        {
            if (ammoInClip > 0)
            {
                if (target != Vector3.zero)
                {
                    //Transform gun = gameObject.GetComponentInParent<Transform>();
                    //gun.LookAt(target);
                    bulletSpawnPoint.LookAt(target);
                    //Quaternion rotate = Quaternion.LookRotation(target);
                    //rotate *= Quaternion.Euler(0, 90, 0);
                    //bulletSpawnPoint.transform.rotation = rotate;
                    
                }
                GameObject newBullet = Instantiate(
                    bullet,
                    bulletSpawnPoint.position,
                    bulletSpawnPoint.rotation) as GameObject;
                newBullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.VelocityChange);
                newBullet.GetComponent<ScriptBullet>().maxVelocity = bulletSpeed;
                rateOfFireTimer = 0;
                ammoInClip--;
                anim.SetTrigger("Fire");
                audioSource.PlayOneShot(audioShot, PlayerPrefs.GetFloat("Volume"));
                ReactToSound();
                StartCoroutine(MuzzleFlash());
            }
            else
            {
                Reload();
            }
        }
    }

    private void ReactToSound()
    {
        //look at direction where sound came from
        Collider[] colliders = Physics.OverlapSphere(transform.position, 50);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                collider.GetComponent<ScriptEnemyAttack>().InvestigateSound(transform.position);
            }
        }
    }

    public void Aim(bool aim)
    {
        anim.SetBool("Aim", aim);
    }

    public void Sprint(bool sprint)
    {
        anim.SetBool("Sprint", sprint);
    }

    public void Reload()
    {
        if (reloadTimer >= reloadTime)
        {
            int ammoToReload = clipSize - ammoInClip;
            //enough ammo in reserve to load clip
            if (ammoReserve >= ammoToReload)
            {
                ammoInClip += ammoToReload;
                ammoReserve -= ammoToReload;
            }
            else
            {
                ammoInClip += ammoReserve;
                ammoReserve = 0;
            }
            reloadTimer = 0;
            anim.SetBool("Aim", false);
            anim.SetBool("Sprint", false);
            anim.SetTrigger("Reload");
            audioSource.PlayOneShot(audioReload, PlayerPrefs.GetFloat("Volume"));
        }
    }

    private IEnumerator MuzzleFlash()
    {
        muzzleFlash.enabled = true;
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(0.02f);
        muzzleFlash.enabled = false;
        muzzleLight.enabled = false;
    }
}
