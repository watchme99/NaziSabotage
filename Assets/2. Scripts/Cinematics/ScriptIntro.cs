using UnityEngine;

public class ScriptIntro : MonoBehaviour {
    public AudioSource parachuteSound;
    public AudioSource parachuteOpenSound;
    public Animation introAnimation;
    public GameObject planeSound;

    private ManagerGame manager;

    public void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        if (manager.playIntro)
        {
            StartIntro();
        }
        else
        {
            EndIntro();
        }
    }

    public void StartIntro()
    {
        manager.status = statusGame.cinematic;
        ControllerCamera controllerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ControllerCamera>();
        controllerCam.SetCinematicDirection(new Vector3(65, 0, 0));
        introAnimation.enabled = true;
        introAnimation["Level1Intro"].speed = 0.1f;
        parachuteOpenSound.enabled = true;
        parachuteSound.enabled = true;
        planeSound.SetActive(true);
    }

    //At the end of the intro animation is a trigger that calls EndIntro()
    public void EndIntro()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>().status = statusGame.playing;
        Destroy(parachuteSound);
        Destroy(parachuteOpenSound);
        Destroy(introAnimation);
        Destroy(planeSound);
        Destroy(this);
    }
}
