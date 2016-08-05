using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System;


/*
all highscores and savegames are saved in the game folder (also possible in /documents with application.dataPath)
saving works by getting all the data, serializing and writing it to a file
loading is the other way around
*/

public class ScriptSaveLoad : MonoBehaviour
{
    private ManagerUI ui;
    private ScriptUILoadingScreen uiLoadingScreen;
    private ManagerGame manager;
    private ManagerMission1 managerMission1;
    private GameObject player;
    private ScriptGun playerGun;
    private GameObject mainCamera;

    void Awake()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
        uiLoadingScreen = ui.uiLoadingScreen.GetComponent<ScriptUILoadingScreen>();
    }

    void Start()
    {
        if (Application.loadedLevelName != "MainMenu")
        {
            player = GameObject.FindGameObjectWithTag("Player");
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            playerGun = mainCamera.transform.Find("PrefabAK47/Gun").GetComponent<ScriptGun>();
            if (Application.loadedLevelName != "Survival")
            {
                managerMission1 = GameObject.Find("MissionController").GetComponent<ManagerMission1>();
            }
        }
    }

    public void SaveGame(String saveName)
    {
        Start();
        SaveGame save = new SaveGame();

        //get game data
        save.GameKills = manager.kills;
        save.GameTime = manager.time;
        save.levelName = Application.loadedLevelName;

        //get player data
        save.PlayerHealth = player.GetComponent<ManagerPlayer>().currentHealth;
        save.PlayerTransform = new List<float>()
        {
            player.transform.position.x,
            player.transform.position.y,
            player.transform.position.z,
            player.transform.rotation.x,
            player.transform.rotation.y,
            player.transform.rotation.z,
            player.transform.rotation.w
        };
        save.PlayerAmmoInClip = playerGun.ammoInClip;
        save.PlayerAmmoReserve = playerGun.ammoReserve;

        //get enemies data
        save.EnemiesName = new List<string>();
        save.EnemiesHealth = new List<float>();
        save.EnemiesAmmoClip = new List<int>();
        save.EnemiesAmmoReserve = new List<int>();
        save.EnemiesTransform = new List<List<float>>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            ManagerEnemy manEnemy = enemy.GetComponent<ManagerEnemy>();
            ScriptGun enemyGun = enemy.transform.Find("hand_R/PrefabAK47/Gun").GetComponent<ScriptGun>();
            save.EnemiesName.Add(enemy.name);
            save.EnemiesHealth.Add(manEnemy.currentHealth);
            save.EnemiesAmmoClip.Add(enemyGun.ammoInClip);
            save.EnemiesAmmoReserve.Add(enemyGun.ammoReserve);
            save.EnemiesTransform.Add(new List<float>()
            {
                enemy.transform.position.x,
                enemy.transform.position.y,
                enemy.transform.position.z,
                enemy.transform.rotation.x,
                enemy.transform.rotation.y,
                enemy.transform.rotation.z,
                enemy.transform.rotation.w
            });
        }

        //get mission data
        save.Objectives = managerMission1.GetObjectives();

        //write to file
        BinaryFormatter bf = new BinaryFormatter();
        Directory.CreateDirectory(Application.dataPath + "/SaveGames");
        FileStream file = File.Create(Application.dataPath + "/SaveGames/" + saveName + ".gd");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Saved in: " + Application.dataPath + "/SaveGames");
    }

    public List<String> getSaves()
    {
        List<string> saves = Directory.GetFiles(Application.dataPath + "/SaveGames/", "*.gd").Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
        return saves;
    }
    
    public IEnumerator LoadNewGame(String levelName)
    {
        AsyncOperation load = Application.LoadLevelAsync(levelName);
        uiLoadingScreen.asyncOp = load;
        yield return load;
    }

    public IEnumerator LoadGame(String saveName)
    {
        String fileLocation = Application.dataPath + "/SaveGames/" + saveName + ".gd";
        if (File.Exists(fileLocation))
        {
            SaveGame save = new SaveGame();
            uiLoadingScreen.progressLoadSaveData = 0;

            //read savefile
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileLocation, FileMode.Open);
            save = (SaveGame)bf.Deserialize(file);
            file.Close();

            uiLoadingScreen.progressLoadSaveData = 0.1f;

            //load level, wait until load completes
            yield return StartCoroutine(LoadNewGame(save.levelName));

            Start();

            //set game data
            manager.kills = save.GameKills;
            manager.time = save.GameTime;

            //set player data
            player.GetComponent<ManagerPlayer>().currentHealth = save.PlayerHealth;
            playerGun.ammoInClip = save.PlayerAmmoInClip;
            playerGun.ammoReserve = save.PlayerAmmoReserve;
            Vector3 pos = new Vector3(
                save.PlayerTransform[0],
                save.PlayerTransform[1],
                save.PlayerTransform[2]);
            Quaternion rot = new Quaternion(
                save.PlayerTransform[3],
                save.PlayerTransform[4],
                save.PlayerTransform[5],
                save.PlayerTransform[6]);
            player.transform.position = pos;
            player.transform.rotation = rot;
            mainCamera.transform.position = pos;
            mainCamera.transform.rotation = rot;

            uiLoadingScreen.progressLoadSaveData = 0.5f;
            yield return 0;//wait 1 frame to update loadingscreen

            //set enemies data
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                int index = save.EnemiesName.FindIndex(x => x.StartsWith(enemy.name));
                ManagerEnemy manEnemy = enemy.GetComponent<ManagerEnemy>();
                ScriptGun enemyGun = enemy.transform.Find("hand_R/PrefabAK47/Gun").GetComponent<ScriptGun>();
                manEnemy.currentHealth = save.EnemiesHealth[index];
                enemyGun.ammoInClip = save.EnemiesAmmoClip[index];
                enemyGun.ammoReserve = save.EnemiesAmmoReserve[index];
                enemy.transform.position = new Vector3(
                    save.EnemiesTransform[index][0],
                    save.EnemiesTransform[index][1],
                    save.EnemiesTransform[index][2]);
                enemy.transform.rotation = new Quaternion(
                    save.EnemiesTransform[index][3],
                    save.EnemiesTransform[index][4],
                    save.EnemiesTransform[index][5],
                    save.EnemiesTransform[index][6]);
            }

            //set mission data
            managerMission1.SetObjectives(save.Objectives);
        }
        uiLoadingScreen.progressLoadSaveData = 1f;
        yield return 0; //wait 1 frame to load scene, then continue in UI
    }

    public void SaveScore(string level, string playerName)
    {
        Start();
        List<HighScore> highscoreList = new List<HighScore>();
        HighScore currentHighScore = new HighScore();

        //get player time, kills, name, health
        currentHighScore.Time = manager.time;
        currentHighScore.Kills = manager.kills;
        currentHighScore.Name = playerName;

        //create file (or open file + read data)
        Directory.CreateDirectory(Application.dataPath + "/HighScores");
        FileStream file;
        if (File.Exists(Application.dataPath + "/HighScores/HighScores" + level + ".gd"))
        {
            highscoreList = LoadScore(level);
            file = File.Create(Application.dataPath + "/HighScores/HighScores" + level + ".gd");
        }
        else
        {
            file = File.Create(Application.dataPath + "/HighScores/HighScores" + level + ".gd");
        }

        //add new highscore
        highscoreList.Add(currentHighScore);

        //sort highscores
        if(level == "Survival")
        {
            highscoreList = highscoreList.OrderByDescending(x => x.Time).ToList();
        }
        else
        {
            highscoreList = highscoreList.OrderBy(x => x.Time).ToList();
        }

        //write to file
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, highscoreList);
        file.Close();
    }

    public List<HighScore> LoadScore(string level)
    {
        List<HighScore> loadedData = new List<HighScore>();

        if (File.Exists(Application.dataPath + "/HighScores/HighScores" + level + ".gd"))
        {
            //read savefile
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/HighScores/HighScores" + level + ".gd", FileMode.Open);
            loadedData = (List<HighScore>)bf.Deserialize(file);
            file.Close();
        }

        return loadedData;
    }
}
