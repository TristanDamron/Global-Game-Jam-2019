﻿using System.Collections;
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
        });
        quit.onClick.AddListener(() => {
            Application.Quit();
        });
       DestroyMemoryCounter();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void DestroyMemoryCounter()
    {
        var memoryCounter = FindObjectOfType<MemoryCounter>();
        if (!memoryCounter) return;
        Destroy(memoryCounter.gameObject);
    }

}
