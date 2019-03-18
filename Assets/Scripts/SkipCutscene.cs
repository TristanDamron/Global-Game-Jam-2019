using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button skip;
    public string firstLevelName;

    // Start is called before the first frame update
    void Start()
    {

        skip.onClick.AddListener(() => {
            Time.timeScale = 1;
            SceneManager.LoadScene(firstLevelName);
        });


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }

    }
}
