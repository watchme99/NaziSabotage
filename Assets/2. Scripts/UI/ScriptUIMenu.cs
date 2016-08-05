using UnityEngine;

public class ScriptUIMenu : MonoBehaviour {
    private ManagerGame manager;
    private ManagerUI ui;
    private ScriptSaveLoad saveLoad;

    void Start () {
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

    public void Continue()
    {
        //check if player is alive
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<ManagerPlayer>().currentHealth == 0)
        {
            manager.status = statusGame.gameOver;
        }
        else
        {
            ui.ShowMenuOrMainMenu();
        }
    }
}
