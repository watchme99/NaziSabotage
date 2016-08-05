using UnityEngine;
using System.Collections;

public class ManagerSettings : MonoBehaviour
{
    public int currentQualityLevel;
    public bool dynamicQuality;
    private float fps;
    private int countForLower;
    private int countForHigher;

    // Use this for initialization
    void Start()
    {
        fps = 0;
        currentQualityLevel = 5;
    }

    void Update()
    {
        if (PlayerPrefs.GetInt("DynamicQuality") == 1)
        {
            fps = 1.0f / Time.deltaTime;
            ChangeQualityDynamic();
        }
    }

    private void ChangeQualityDynamic()
    {
        currentQualityLevel = PlayerPrefs.GetInt("Quality");
        if (currentQualityLevel > 0 && fps < 30)
        {
            countForLower++;
            countForHigher = 0;
            if (countForLower > 30)
            {
                currentQualityLevel--;
                QualitySettings.SetQualityLevel(currentQualityLevel, true);
                countForLower = 0;
            }
        }
        else if (currentQualityLevel < 5 && fps > 50)
        {
            countForHigher++;
            countForLower = 0;
            if (countForHigher > 120)
            {
                currentQualityLevel++;
                QualitySettings.SetQualityLevel(currentQualityLevel, true);
                countForHigher = 0;
            }
        }
    }
}
