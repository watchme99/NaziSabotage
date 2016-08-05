using UnityEngine;
using System.Collections;

public class ScriptPickupAK47 : MonoBehaviour {
    public int ammo;
    public AudioClip pickupSound;
    private Transform icon;
    private Quaternion originalRotation;
    private Vector3 offset;

    void Awake()
    {
        icon = transform.GetChild(0);
        originalRotation = Quaternion.Euler(90,0,0);
        offset = new Vector3(0, 10, 0);//icon = 10m above ground
    }

    void LateUpdate()
    {
        icon.rotation = originalRotation;
        icon.position = transform.position + offset;
    }

	void OnTriggerEnter(Collider collision) {
        if(collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponentInChildren<ScriptGun>().ammoReserve += ammo;
            AudioSource.PlayClipAtPoint(pickupSound, gameObject.transform.position);
            Destroy(this.gameObject);
        }
    }
}
