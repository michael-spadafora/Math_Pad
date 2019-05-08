﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : GamePlay
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            RestartGame();
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.X))
            SceneManager.LoadScene(5);
    }
}
