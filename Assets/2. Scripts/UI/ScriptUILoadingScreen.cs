using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ScriptUILoadingScreen : MonoBehaviour
{
    public AsyncOperation asyncOp;
    public float? progressLoadSaveData;

    private ManagerGame manager;
    private ManagerUI ui;
    private Slider sliderLoadingProgress;
    private float progress;

    void Start()
    {
        progress = 0;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
        sliderLoadingProgress = transform.Find("LoadingBar").GetComponent<Slider>();
    }

    void OnGUI()
    {
        if (asyncOp != null)
        {
            UpdateLoadingProgress();
        }
    }

    void Update()
    {
        CheckProgress();//don't check in OnGUI => disable gameobject in OnGUI gives "isactive getrunineditmode" error
    }

    public void UpdateLoadingProgress()
    {
        int count = 1; //1 or 2 progresses (1=newgame, 2=loadgame)
        progress = 0;
        progress += asyncOp.progress;
        if (progressLoadSaveData != null)//progress for loading savedata (custom method)
        {
            progress += (float)progressLoadSaveData;
            count++;
        }
        progress /= count;
        sliderLoadingProgress.value = progress;
    }

    private void CheckProgress()
    {
        //if finished loading...
        if (progress == 1)
        {
            //if no savedata is loaded and not loading MainMenu=> show Cinematic
            if (progressLoadSaveData == null && manager.status != statusGame.mainMenu)
            {
                ui.ShowCinematic();
            }
            else //if new game is loaded => show menu
            {
                ui.ShowMenuOrMainMenu();
            }
            progressLoadSaveData = null;
            asyncOp = null;
        }
    }
}
