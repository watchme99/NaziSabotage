using UnityEngine;
using System.Collections;

//Music has 3 running clips in loop: base, search and attack clip
//Enemies that are attacking/searching reset timers
//If 2 enemies are searching and 1 is attacking => music is attacking
public class ManagerMusic : MonoBehaviour
{
    public AudioSource audioBase;
    public AudioSource audioSearch;
    public AudioSource audioAttack;
    public float musicChangeTime = 5;

    private float musicAttackTimer = 10;
    private float musicSearchTimer = 10;

    void Update()
    {
        musicSearchTimer += Time.deltaTime;
        musicAttackTimer += Time.deltaTime;
        float volume = PlayerPrefs.GetFloat("Volume");
        audioBase.volume = volume;

        if (musicSearchTimer > musicChangeTime) //base clip => not searching for xx sec
        {
            audioSearch.volume = 0;
            audioAttack.volume = 0;
        }
        else if (musicAttackTimer > musicChangeTime && musicSearchTimer < musicChangeTime) //search clip => not attacking for xx sec && still searching
        {
            audioSearch.volume = volume;
            audioAttack.volume = 0;
        }
        else if (musicAttackTimer < musicChangeTime) //attack clip => still attacking
        {
            audioSearch.volume = volume;
            audioAttack.volume = volume;
        }
    }

    public void setMusicAttack()
    {
        musicAttackTimer = 0;
        musicSearchTimer = 0;
    }

    public void setMusicSearch()
    {
        musicSearchTimer = 0;
    }
}
