using UnityEngine;
using System.Collections;

public class ScriptMiniMap : MonoBehaviour
{
    public float thisHeight = 50;
    public GameObject miniMap;
    public Camera miniMapCam;
    private GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //extra check => loading level is async (player doesn't exist)
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, thisHeight, player.transform.position.z);
            if (Input.GetKeyUp(KeyCode.M))
            {
                miniMapCam.enabled = !miniMapCam.enabled;
                miniMap.SetActive(!miniMap.activeSelf);
            }
        }
    }
}
