using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ScriptUISaveLoad : MonoBehaviour {
    private ManagerGame manager;
    private ScriptSaveLoad saveLoad;
    private ManagerUI ui;
    private RectTransform savesContent;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        saveLoad = manager.GetComponent<ScriptSaveLoad>();
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
        savesContent = transform.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
    }

    public void FillInput(Text text)
    {
        InputField input = transform.Find("SaveName").GetComponent<InputField>();
        input.text = text.text;
    }

    public void SaveGame()
    {
        Text input = transform.Find("SaveName/Text").GetComponent<Text>();
        if (!String.IsNullOrEmpty(input.text))
        {
            saveLoad.SaveGame(input.text);
        }
        ui.ShowSaveLoad();
    }

    //show loadingscreen, start loading game async
    public void LoadGame()
    {
        ui.ShowLoadingScreen();
        manager.playIntro = false;
        Text input = transform.Find("SaveName/Text").GetComponent<Text>();
        StartCoroutine(saveLoad.LoadGame(input.text));
    }

    //generate SaveGames (called by ManagerUI)
    public void LoadContent()
    {
        Start();
        //destroy existing saveButtons
        for (int i = savesContent.childCount; i > 1; i--)
        {
            Destroy(savesContent.GetChild(i - 1).gameObject);
        }

        //load new saveButtons
        List<String> saves = saveLoad.getSaves();
        float buttonHeight = Screen.height / 20;
        RectTransform saveTemplate = savesContent.GetChild(0).GetComponent<RectTransform>();
        int x = 0;
        foreach (String save in saves)
        {
            RectTransform newSave = Instantiate(saveTemplate);
            newSave.name = "Save" + (x + 1);
            newSave.transform.SetParent(savesContent, false);
            newSave.sizeDelta = new Vector2(newSave.sizeDelta.x, buttonHeight);
            newSave.transform.localPosition += new Vector3(0, -buttonHeight * x, 0);
            newSave.transform.GetChild(0).GetComponent<Text>().text = save;
            newSave.gameObject.SetActive(true);
            x++;
        }
        savesContent.sizeDelta = new Vector2(savesContent.sizeDelta.x, buttonHeight + buttonHeight * Mathf.Floor(saves.Count / 2));
    }
}
