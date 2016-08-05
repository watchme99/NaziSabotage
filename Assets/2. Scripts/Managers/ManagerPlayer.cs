using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManagerPlayer : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float regenerate;

    private Text textHealth;
    private Slider healthBar;
    private Image imgDamage;
    private Text textAmmoClip;
    private Text textAmmoReserve;
    private ManagerGame manager;
    private ScriptGun gun;
    private ManagerUI ui;

    void Start()
    {
        //textHealth + textAmmo can only be found this way => GO.Find() doesn't find inactive GO's (deactivated because of menu)
        ui = GameObject.Find("UI").GetComponent<ManagerUI>();
        textHealth = ui.transform.Find("HUD/Health/TextHealth").GetComponent<Text>();
        healthBar = ui.transform.Find("HUD/Health/HealthBar").GetComponent<Slider>();
        imgDamage = ui.transform.Find("HUD/Damage").GetComponent<Image>();
        textAmmoClip = ui.transform.Find("HUD/Ammo/TextAmmoClip").GetComponent<Text>();
        textAmmoReserve = ui.transform.Find("HUD/Ammo/TextAmmoReserve").GetComponent<Text>();
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ManagerGame>();
        gun = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetChild(0).GetComponent<ScriptGun>();
        healthBar.maxValue = maxHealth;
    }

    void Update()
    {
        if (manager.status == statusGame.playing)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += regenerate * Time.deltaTime;
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                textHealth.text = "" + currentHealth;
                if (Application.loadedLevelName == "Survival")
                {
                    manager.status = statusGame.gameOver;
                    ui.ShowNewHighScore();
                }
                else
                {
                    manager.status = statusGame.gameOver;
                    ui.ShowGameOver();
                }
            }
        }
        healthBar.value = currentHealth;
        if (currentHealth < 0)
        {

        }
        Color c = imgDamage.color;
        c.a = (maxHealth - currentHealth) / maxHealth;
        imgDamage.color = c;
        textHealth.text = "" + Mathf.Round(currentHealth);
        textAmmoClip.text = "" + gun.ammoInClip + "/";
        textAmmoReserve.text = "" + gun.ammoReserve;
    }
}
