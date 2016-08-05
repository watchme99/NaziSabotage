using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptInteract : MonoBehaviour
{
    private Text textInteract;

    //Unity doesn't allow the selection of the "Halo" component
    //Selecting the "Halo" component as a behaviour is a workaround to enable/disable it
    private Behaviour lastHaloComponent;
    private ManagerMission1 mission;

    void Start()
    {
        textInteract = GameObject.Find("UI").transform.Find("More/TextInteract").GetComponent<Text>();
        textInteract.text = "";
        mission = GameObject.Find("MissionController").GetComponent<ManagerMission1>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, 3f))
        {
            if (hit.collider.tag == "Interactable")
            {
                textInteract.text = "Press \"E\" to interact";

                //If the interactable object has a "Halo" component, it will be enabled.
                if ((Behaviour)hit.transform.GetComponent("Halo") != null)
                {
                    lastHaloComponent = (Behaviour)hit.transform.GetComponent("Halo");
                    lastHaloComponent.enabled = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    mission.Interact(hit.transform.gameObject);
                }
            }
        }
        else
        {
            if (lastHaloComponent != null)
            {
                lastHaloComponent.enabled = false;
            }
            textInteract.text = "";
        }
    }
}
