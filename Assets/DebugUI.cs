using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public static DebugUI instance;
    public float gravityStrength = 0.0f;
    DebugUI()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }
    private void OnGUI()
    {
        GUILayout.Label("Debug UI");
        GUILayout.Label(gravityStrength.ToString());
    }
}
