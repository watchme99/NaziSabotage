using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManagerMissionSurvival : MonoBehaviour {
    void Start()
    {
        Transform ui = GameObject.Find("UI").transform;
        ui.FindChild("Objectives/Objective/ScrollView/Viewport/Content").GetComponent<Text>().text =
            "You need to survive as long as you can!\n" +
            "The first nazis come in 30sec.\n" +
            "There will be more nazis the longer you survive!\n";
        ui.FindChild("Objectives/Summary/ScrollView/Viewport/Content").GetComponent<Text>().text =
            "- Survive\n";
        Destroy(gameObject);
    }
}
