using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum statusGame { mainMenu, cinematic, menu, playing, gameOver, win }

public class ManagerGame : MonoBehaviour
{
    public statusGame status;
    public float time;
    public int kills;
    public bool playIntro; //when a level is loaded and it has an intro, it will look here if it has to be played or not (new level or load level)
    private Text textTime;
    private Text textKills;

    //singleton
    private static ManagerGame instance = null;
    public static ManagerGame Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        textTime = GameObject.Find("UI").transform.Find("HUD/Time/TextTime").GetComponent<Text>();
        textKills = GameObject.Find("UI").transform.Find("HUD/Kills/TextKills").GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if (status == statusGame.playing)
        {
            time += Time.deltaTime;
        }
    }

    void Update()
    {
        textTime.text = time.ToString("0");
        textKills.text = kills.ToString();
    }

    void OnLevelWasLoaded()
    {
        time = 0;
        kills = 0;
    }
}
