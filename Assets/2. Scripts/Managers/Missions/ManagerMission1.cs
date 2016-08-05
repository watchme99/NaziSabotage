using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ManagerMission1 : MonoBehaviour {
    private bool paper = false;
    private bool plane = false;
    private bool fuel = false;
    private GameObject interactedObject;
    private ScriptInfo info;
    private Text textObjective;
    private Text textSummary;

    // Use this for initialization
    void Start () {
        info = GameObject.Find("UI").transform.Find("More/TextInfo").GetComponent<ScriptInfo>();
        textObjective = GameObject.Find("UI").transform.FindChild("Objectives/Objective/ScrollView/Viewport/Content").GetComponent<Text>();
        textSummary = GameObject.Find("UI").transform.FindChild("Objectives/Summary/ScrollView/Viewport/Content").GetComponent<Text>();
        textObjective.text =
            "Nazi's have taken over a remote village in the mountains and setup camp.\n" +
            "You will be dropped close to the enemy nazi camp.\n" +
            "Once there, you'll make your way into their camp and steal their secret plans.\n" +
            "Make your way to the airport and escape with the old plane that the villagers use for supplies.\n";
        textSummary.text =
            "- Steal plans\n" + 
            "- Escape with plane\n";
    }


    public void Interact(GameObject obj)
    {
        interactedObject = obj;
        switch (obj.name)
        {
            case "PrefabPaper":
                SetPaper();
                break;
            case "PrefabPlane":
                SetPlane();
                break;
            case "PrefabFuel":
                SetFuel();
                break;
            default:
                break;
        }
    }

    public void SetPaper()
    {
        paper = true;
        Destroy(interactedObject);
        info.ShowMessage("Got the papers!");
    }

    public void SetPlane()
    {
        if (!paper)
        {
            info.ShowMessage("I must first get the papers before escaping!");
        }
        else if (paper && !fuel)
        {
            plane = true;
            info.ShowMessage("The plane is out of fuel! (Objectives updated*)");
            textObjective.text =
                "Nazi's have taken over a remote village in the mountains and setup camp. " +
                "You will be dropped close to the enemy nazi camp. " +
                "Once there, you'll make your way into their camp and steal their secret plans. " +
                "Make your way to the airport and escape with the old plane that the villagers use for supplies." + 
                "The plane is out of fuel, you'll probably find some in the village.";
            textSummary.text =
                "- Steal plans\n" +
                "- Find fuel in the village\n" +
                "- Escape with plane\n";
        } else if (plane && fuel)
        {
            //manager.status = statusGame.win;
            GameObject.Find("PrefabPlane").GetComponent<ScriptOutro>().StartOutro();
            info.ShowMessage("Off we go!");
        }
    }

    public void SetFuel()
    {
        if (!paper || (paper && !plane))
        {
            info.ShowMessage("A fuel can, don't need that now.");
        }
        else
        {
            fuel = true;
            Destroy(interactedObject);
            info.ShowMessage("Got the fuel can, now back to the plane");
        }
    }

    public List<bool> GetObjectives()
    {
        return new List<bool>() { paper, plane, fuel };
    }

    public void SetObjectives(List<bool> obj)
    {
        paper = obj[0];
        plane = obj[1];
        fuel = obj[2];
    }
}
