using UnityEngine;

//Outro uses maincamera to look at plane
//Plane uses animation to fly away
//After the animation ends => option to add highscore
public class ScriptOutro : MonoBehaviour {
    private ManagerGame manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
    }

    public void StartOutro()
    {
        manager.status = statusGame.cinematic;
        //hide gun
        GameObject mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        mainCam.transform.GetChild(0).gameObject.SetActive(false);
        mainCam.GetComponent<ControllerCamera>().SetCinematicObject(this.transform);
        GetComponent<Animator>().SetTrigger("StartOutro");
        GetComponent<AudioSource>().enabled = true;
    }

    //At the end of the outro animation is a trigger that calls EndOutro()
    public void EndOutro()
    {
        manager.status = statusGame.menu;
        GameObject.Find("UI").GetComponent<ManagerUI>().ShowNewHighScore();
        Destroy(this.gameObject);
    }
}
