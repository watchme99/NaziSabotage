using UnityEngine;
using System.Collections;

public class ScriptUIMainMenu : MonoBehaviour {
    private ManagerGame manager;
    private ManagerUI ui;
    private ScriptSaveLoad saveLoad;

    void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
        saveLoad = manager.GetComponent<ScriptSaveLoad>();
    }

    public void Play()
    {
        manager.status = statusGame.cinematic;
        manager.playIntro = true;
        ui.ShowLoadingScreen();
        StartCoroutine(saveLoad.LoadNewGame("Level1"));
        //ui.ShowCinematic();
    }

    public void Survival()
    {
        manager.status = statusGame.playing;
        manager.playIntro = true;
        ui.ShowLoadingScreen();
        StartCoroutine(saveLoad.LoadNewGame("Survival"));
    }

    public void Quit()
    {
        Application.Quit();
    }
}
