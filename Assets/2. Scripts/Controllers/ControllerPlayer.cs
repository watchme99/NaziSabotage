using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ControllerPlayer : MonoBehaviour
{
    public float speedForward = 10.0f;
    public float speedSideways = 5.0f;
    public float jumpHeight = 6.0f;
    public float speedSneaking = 0.75f;
    public float speedSprinting = 1.75f;
    public bool sneaking = false;
    public AudioSource runningSound;
    private CharacterController controller;
    private Vector3 move;
    private float gravity;
    private ManagerGame manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        controller = GetComponent<CharacterController>();
        move = Vector3.zero;
        gravity = Physics.gravity.y;
    }

    void Update()
    {
        if (manager.status == statusGame.playing)
        {
            //don't multiply move.y by anything => buggy results (sneaking, sprinting)
            if (controller.isGrounded)
            {
                move.x = speedSideways * Input.GetAxis("Horizontal");
                move.z = speedForward * Input.GetAxis("Vertical");
                move = transform.TransformDirection(move);
                if (Input.GetButton("Jump"))
                {
                    move.y = jumpHeight;
                    sneaking = false;
                }
                if (Input.GetButton("Sneak"))
                {
                    sneaking = true;
                }
                if (Input.GetButton("Sprint"))
                {
                    sneaking = false;
                    move.x *= speedSprinting;
                    move.z *= speedSprinting;
                }
                runningSound.pitch = controller.velocity.magnitude / 10f;
                if (sneaking)
                {
                    move.x *= speedSneaking;
                    move.z *= speedSneaking;
                }
            }
            else
            {
                runningSound.pitch = 0;
            }
            move.y += gravity * Time.deltaTime;
            controller.Move(move * Time.deltaTime);
        }
        else
        {
            runningSound.pitch = 0;
        }
    }


}
