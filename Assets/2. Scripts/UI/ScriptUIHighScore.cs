using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScriptUIHighScore : MonoBehaviour {
    private ManagerGame manager;
    private ScriptSaveLoad saveLoad;
    private RectTransform highScoreContent;

    void Start () {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        saveLoad = manager.GetComponent<ScriptSaveLoad>();
        highScoreContent = transform.Find("ScrollView/Viewport/Content").GetComponent<RectTransform>();
    }

    public void LoadScoresLevel1()
    {
        LoadContent("Level1");
    }

    public void LoadScoresSurvival()
    {
        LoadContent("Survival");
    }

    //generate HighScores (called by ManagerUI)
    public void LoadContent(string levelName)
    {
        Start();
        //destroy existing scores
        for (int i = highScoreContent.childCount; i > 2; i--)
        {
            Destroy(highScoreContent.GetChild(i - 1).gameObject);
        }

        //set relative height for content window and text
        float headerHeight = Screen.height / 15;
        float scoreHeight = Screen.height / 20;

        //set header size/position
        RectTransform header = highScoreContent.GetChild(0).GetComponent<RectTransform>();
        header.sizeDelta = new Vector2(highScoreContent.sizeDelta.x / 2, headerHeight);
        header.localPosition = new Vector2(header.localPosition.x, -headerHeight / 2);
        //set score size/position
        RectTransform scoreTextTemplate = highScoreContent.GetChild(1).GetComponent<RectTransform>();
        scoreTextTemplate.sizeDelta = new Vector2(highScoreContent.sizeDelta.x, scoreHeight);
        scoreTextTemplate.localPosition = new Vector2(scoreTextTemplate.localPosition.x, -headerHeight - scoreHeight);

        //load and print scores
        List<HighScore> highScoreList = saveLoad.LoadScore(levelName);
        int x = 1;
        foreach (HighScore highScore in highScoreList)
        {
            RectTransform scoreText = Instantiate(scoreTextTemplate);
            scoreText.name = "score" + x;
            scoreText.transform.SetParent(highScoreContent, false);
            scoreText.transform.localPosition += new Vector3(0, -scoreHeight * x, 0);
            scoreText.transform.GetChild(0).GetComponent<Text>().text = "" + x++;
            scoreText.transform.GetChild(1).GetComponent<Text>().text = "" + highScore.Name;
            scoreText.transform.GetChild(2).GetComponent<Text>().text = "" + highScore.Time.ToString("0.##");
            scoreText.transform.GetChild(3).GetComponent<Text>().text = "" + highScore.Kills;
        }
        //set content window
        highScoreContent.sizeDelta = new Vector2(highScoreContent.sizeDelta.x, headerHeight + (x + 1) * scoreHeight);
    }
}
