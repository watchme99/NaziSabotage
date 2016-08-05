using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptInfo : MonoBehaviour {
    public float displayInfoTime;
    private float displayInfoTimer;
    private Text textInfo;

	void Start () {
        textInfo = GetComponent<Text>();
        textInfo.text = "";
	}

    void Update()
    {
        displayInfoTimer += Time.deltaTime;
        if (displayInfoTimer > displayInfoTime)
        {
            textInfo.text = "";
        }
    }

    public void ShowMessage(string msg)
    {
        displayInfoTimer = 0;
        textInfo.text = msg;
    }
}
