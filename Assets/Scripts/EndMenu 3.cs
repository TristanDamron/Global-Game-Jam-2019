using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public Button quit;
    public Button mainMenu;
    public string mainMenuName;

    // Start is called before the first frame update
    void Start()
    {

        mainMenu.onClick.AddListener(() => {
            SceneManager.LoadScene(mainMenuName);
        });

        quit.onClick.AddListener(() => {
            Application.Quit();
        });


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
