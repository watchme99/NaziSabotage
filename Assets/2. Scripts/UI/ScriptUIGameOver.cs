using UnityEngine;
using System.Collections;

public class ScriptUIGameOver : MonoBehaviour {
    private ManagerGame manager;
    private ManagerUI ui;
    private ScriptSaveLoad saveLoad;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
        saveLoad = manager.GetComponent<ScriptSaveLoad>();
    }

    public void MainMenu()
    {
        manager.status = statusGame.mainMenu;
        StartCoroutine(saveLoad.LoadNewGame("MainMenu"));
        ui.ShowMenuOrMainMenu();
        ui.ShowLoadingScreen();
    }
}
