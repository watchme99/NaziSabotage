using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptBorders : MonoBehaviour
{
    private ScriptInfo info;

    // Use this for initialization
    void Start()
    {
        info = GameObject.Find("UI").transform.Find("More/TextInfo").GetComponent<ScriptInfo>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            info.ShowMessage("I'm not going to run from those nazis!");
        }
    }
}
