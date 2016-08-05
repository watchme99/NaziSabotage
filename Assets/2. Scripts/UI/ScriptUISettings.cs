using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptUISettings : MonoBehaviour
{
    public GameObject treeLQ;
    public GameObject treeMQ;
    public GameObject treeHQ;
    private TreePrototype[] treeType;
    private Slider sliderVolume;
    private Dropdown dropdownTrees;
    private Dropdown dropdownQuality;
    private Toggle toggleDynamic;

    // Use this for initialization
    void Start()
    {
        treeType = Terrain.activeTerrain.terrainData.treePrototypes;
        sliderVolume = transform.Find("SliderVolume").GetComponent<Slider>();
        sliderVolume.value = PlayerPrefs.GetFloat("Volume");
        dropdownTrees = transform.Find("DropdownTrees").GetComponent<Dropdown>();
        dropdownQuality = transform.Find("DropdownQuality").GetComponent<Dropdown>();
        toggleDynamic = transform.Find("ToggleDynamic").GetComponent<Toggle>();
    }

    //update settings menu (can change with dynamic quality)
    void OnGUI()
    {
        dropdownTrees.value = PlayerPrefs.GetInt("Trees");
        dropdownQuality.value = PlayerPrefs.GetInt("Quality");
        toggleDynamic.isOn = PlayerPrefs.GetInt("DynamicQuality") == 1 ? true : false;
    }

    public void SetQualityTrees(int x)
    {
        PlayerPrefs.SetInt("Trees", x);
        switch (x)
        {
            case 0:
                treeType[0].prefab = treeLQ;
                break;
            case 1:
                treeType[0].prefab = treeMQ;
                break;
            default:
                treeType[0].prefab = treeHQ;
                break;
        }
        Terrain.activeTerrain.terrainData.treePrototypes = treeType;
    }

    public void SetQuality(int x)
    {
        PlayerPrefs.SetInt("Quality", x);
        QualitySettings.SetQualityLevel(x);
    }

    public void SetQualityDynamic(bool x)
    {
        if (x)
        {
            PlayerPrefs.SetInt("DynamicQuality", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DynamicQuality", 0);
        }
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        //get all audiosources and update volume
        foreach (AudioSource a in FindObjectsOfType<AudioSource>())
        {
            a.volume = volume;
        }
    }
}
