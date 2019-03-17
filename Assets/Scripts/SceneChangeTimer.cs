using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTimer : MonoBehaviour
{
    public float time = 30.0f;
    public string nextLevelName;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SceneChange());
    }

    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(nextLevelName);
    }
}
