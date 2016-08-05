using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class ScriptUINewHighScore : MonoBehaviour {
    private ManagerGame manager;
    private ScriptSaveLoad saveLoad;
    private ManagerUI ui;

    void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        saveLoad = manager.GetComponent<ScriptSaveLoad>();
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
    }

    public void SaveScore()
    {
        String playerName = GameObject.Find("TextPlayerName").GetComponent<Text>().text;
        if (playerName != null && playerName != "")
        {
            saveLoad.SaveScore(Application.loadedLevelName, playerName);
        }
        MainMenu();
    }
    
    public void MainMenu()
    {
        manager.status = statusGame.mainMenu;
        StartCoroutine(saveLoad.LoadNewGame("MainMenu"));
        ui.ShowMenuOrMainMenu();
        ui.ShowLoadingScreen();
    }
}
