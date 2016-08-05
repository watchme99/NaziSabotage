using UnityEngine;
using System.Collections;

public class ScriptDrawMiniMap : MonoBehaviour
{
    public RenderTexture miniMapTexture;
    public Material miniMapMaterial;
    public Texture miniMapBorder;

    // Update is called once per frame
    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            float offset = Screen.width / 100;
            float size = Screen.width / 5;
            Rect r = new Rect(offset, Screen.height - size - offset, size, size);
            Graphics.DrawTexture(r, miniMapTexture, miniMapMaterial);
            GUI.DrawTexture(r, miniMapBorder);
        }
    }
}
