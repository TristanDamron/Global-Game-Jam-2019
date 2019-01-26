using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public static DebugUI instance;
    public float gravityStrength = 0.0f;
    public Vector3 playerVelocity = Vector3.zero;
    DebugUI()
    {
        if (instance)
            Destroy(this);
        else
            instance = this;
    }
    private void OnGUI()
    {
        GUILayout.Label("DEBUG UI");
        GUILayout.Label("PLAYER VELOCITY: " + playerVelocity.ToString());
        GUILayout.Label("YOYO GRAVITY: " + gravityStrength.ToString());
    }
}
