using UnityEngine;
using UnityEngine.UI;

//manages the showing/hiding of UI Canvases
public class ManagerUI : MonoBehaviour
{
    //all UI panels
    public GameObject uiSettings;
    public GameObject uiHUD;
    public GameObject uiMore;
    public GameObject uiGameOver;
    public GameObject uiHighScore;
    public GameObject uiNewHighScore;
    public GameObject uiSaveLoad;
    public GameObject uiMenu;
    public GameObject uiMainMenu;
    public GameObject uiObjectives;
    public GameObject uiLoadingScreen;

    private ManagerGame manager;
    private ManagerUIAnimations managerAnimations;

    //singleton
    private static ManagerUI instance = null;
    public static ManagerUI Instance
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
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        managerAnimations = gameObject.GetComponent<ManagerUIAnimations>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (manager.status ==statusGame.playing || manager.status == statusGame.menu)
            {
                ShowMenu();
            }
        }
    }

    public void ShowObjectives()
    {
        HideAllUI();
        uiObjectives.SetActive(true);
        managerAnimations.AddAnimation(uiObjectives);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0.1f;
        HideAllUI();
        uiHUD.SetActive(true);
        uiGameOver.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //Show HUD + More
    public void ShowCinematic()
    {
        HideAllUI();
        uiHUD.SetActive(true);
        uiMore.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMenuOrMainMenu()
    {
        if(manager.status == statusGame.playing || manager.status == statusGame.menu)
        {
            ShowMenu();
        }
        else
        {
            ShowMainMenu();
        }
    }

    public void ShowSaveLoad()
    {
        HideAllUI();
        uiSaveLoad.SetActive(true);
        managerAnimations.AddAnimation(uiSaveLoad);

        //load content
        uiSaveLoad.GetComponent<ScriptUISaveLoad>().LoadContent();

        //can't save in main menu => enable/disable BtnSave
        if (manager.status == statusGame.mainMenu)
        {
            uiSaveLoad.transform.Find("BtnSave").GetComponent<Button>().enabled = false;
        }
        else
        {
            uiSaveLoad.transform.Find("BtnSave").GetComponent<Button>().enabled = true;
        }
    }

    public void ShowLoadingScreen()
    {
        uiLoadingScreen.SetActive(true);
    }

    public void ShowNewHighScore()
    {
        HideAllUI();
        uiNewHighScore.SetActive(true);
        managerAnimations.AddAnimation(uiNewHighScore);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ShowHighScores()
    {
        HideAllUI();
        uiHighScore.SetActive(true);
        managerAnimations.AddAnimation(uiHighScore);

        //load content
        uiHighScore.GetComponent<ScriptUIHighScore>().LoadContent("Level1");
    }

    public void ShowSettings()
    {
        HideAllUI();
        uiSettings.SetActive(true);
        managerAnimations.AddAnimation(uiSettings);
    }

    public void HideLoadingScreen()
    {
        uiLoadingScreen.SetActive(false);
    }

    //pause game and show menu (or unpause and resume)
    private void ShowMenu()
    {
        if (manager.status == statusGame.playing || (manager.status == statusGame.menu && uiMenu.activeSelf != true))
        {
            //pause game
            Time.timeScale = 0;
            managerAnimations.AddAnimation(uiMenu);
            manager.status = statusGame.menu;
            HideAllUI();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            uiMenu.SetActive(true);
            uiHUD.SetActive(true);
            if(Application.loadedLevelName == "Survival")
            {
                uiMenu.transform.Find("BtnSaveLoad").GetComponent<Button>().enabled = false;
            }
            else
            {
                uiMenu.transform.Find("BtnSaveLoad").GetComponent<Button>().enabled = true;
            }
        }
        //if in menu => return to game
        else
        {
            //let game continue
            Time.timeScale = 1;

            manager.status = statusGame.playing;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            HideAllUI();
            uiHUD.SetActive(true);
            uiMore.SetActive(true);
        }
    }

    private void ShowMainMenu()
    {
        Time.timeScale = 1;
        HideAllUI();
        uiMainMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideAllUI()
    {
        uiSettings.SetActive(false);
        uiMenu.SetActive(false);
        uiMainMenu.SetActive(false);
        uiMore.SetActive(false);
        uiHUD.SetActive(false);
        uiNewHighScore.SetActive(false);
        uiHighScore.SetActive(false);
        uiGameOver.SetActive(false);
        uiSaveLoad.SetActive(false);
        uiObjectives.SetActive(false);
        uiLoadingScreen.SetActive(false);
    }
}
