using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGame;
    public Button quit;
    public string firstLevelName;

    // Start is called before the first frame update
    void Start()
    {

        startGame.onClick.AddListener(() => {
            SceneManager.LoadScene(firstLevelName);
            AudioController.PlaySFX("sfx_ui_click");
        });
        quit.onClick.AddListener(() => {
            Application.Quit();
            AudioController.PlaySFX("sfx_ui_click");
        });
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
