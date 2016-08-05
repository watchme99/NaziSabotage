using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class SaveGame
{
    //Game Data
    public float GameTime { get; set; }
    public int GameKills { get; set; }
    public String levelName { get; set; }
    //Player Data
    public float PlayerHealth { get; set; }
    public List<float> PlayerTransform { get; set; }
    public int PlayerAmmoInClip { get; set; }
    public int PlayerAmmoReserve { get; set; }
    //Enemies Data
    public List<String> EnemiesName { get; set; }
    public List<List<float>> EnemiesTransform { get; set; }
    public List<float> EnemiesHealth { get; set; }
    public List<int> EnemiesAmmoClip { get; set; }
    public List<int> EnemiesAmmoReserve { get; set; }
    //Mission Data
    public List<bool> Objectives { get; set; }
}

