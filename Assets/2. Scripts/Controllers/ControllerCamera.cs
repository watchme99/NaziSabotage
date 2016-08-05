using UnityEngine;

public class ControllerCamera : MonoBehaviour
{
    //camera behaviour while playing
    public float rotationSpeed = 90;
    public float cameraSpeedX = 2;
    public float cameraSpeedY = 2;

    //cinematics force camera to look at ... 
    //while still able to control the camera a little
    private Vector3 cinematicCameraDirection;    //look in direction
    private Transform cinematicObject;           //look at object
    public float cinematicCamSpeed = 1;

    private Transform player;
    private ControllerPlayer playerController;
    private ManagerGame manager;
    private ScriptGun scriptGun;
    private Camera cam;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = player.GetComponent<ControllerPlayer>();
        scriptGun = gameObject.GetComponentInChildren<ScriptGun>();
        cam = GetComponent<Camera>();
    }

    //no fixedupdate, this causes a jumpy camera
    void LateUpdate()
    {
        if (manager.status == statusGame.playing)
        {
            cinematicCamSpeed = 1f;
            InputMouse();
            //set player y rotation to camera y rotation
            player.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
            CheckButtons();
        }
        else if (manager.status == statusGame.cinematic)
        {
            cinematicCamSpeed = 0.3f;
            InputMouse();
            //if cinematicObject is given => look at GO
            if (cinematicObject != null)
            {
                cinematicCameraDirection = Vector3.zero;
                Vector3 direction = cinematicObject.position - this.transform.position;
                //transform.rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    Time.deltaTime * 1.5f);
            }
            //if direction is given => look in that direction
            else if (cinematicCameraDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.Euler(cinematicCameraDirection),
                    Time.deltaTime * 1.5f);
            }
        }
    }

    public void SetCinematicObject(Transform t)
    {
        cinematicCameraDirection = Vector3.zero;
        cinematicObject = t;
    }

    public void SetCinematicDirection(Vector3 v)
    {
        cinematicCameraDirection = v;
        cinematicObject = null;
    }

    private void InputMouse()
    {
        //sneaking = 1m above ground 
        //walking = 1.5m above ground
        if (playerController.sneaking)
        {
            transform.position = player.position;
        }
        else
        {
            transform.position = player.position + (Vector3.up / 2);
        }

        //input for rotation => causes z rotation
        float h = 1 * cameraSpeedX * cinematicCamSpeed * Input.GetAxis("Mouse X");
        float v = -1 * cameraSpeedY * cinematicCamSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(v, h, 0);

        //recalculate z rotation to horizontal
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
    }

    private void CheckButtons()
    {
        //check if shooting
        if (Input.GetAxis("Fire1") == 1)
        {
            scriptGun.Shoot();
        }

        //check if aiming
        if (Input.GetButton("Fire2"))
        {
            cam.fieldOfView = 40;
            scriptGun.Aim(true);
        }
        else
        {
            scriptGun.Aim(false);
            cam.fieldOfView = 60;
        }

        //check if sprinting
        if (Input.GetButton("Sprint"))
        {
            scriptGun.Sprint(true);
        }
        else
        {
            scriptGun.Sprint(false);
        }

        //check if reloading
        if (Input.GetAxis("Reload") == 1)
        {
            scriptGun.Reload();
        }
    }
}
