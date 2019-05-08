using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : GamePlay
{
    private int[] fromPauseScenes = new int[4] {6, 7, difficulty + 2, 10};
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            SceneManager.LoadScene(fromPauseScenes[0]);

        if (Input.GetKeyDown(KeyCode.A))
            SceneManager.LoadScene(fromPauseScenes[1]);

        if (Input.GetKeyDown(KeyCode.D))
            SceneManager.LoadScene(fromPauseScenes[2]);

        if (Input.GetKeyDown(KeyCode.X))
            SceneManager.LoadScene(fromPauseScenes[3]);
    }
}
