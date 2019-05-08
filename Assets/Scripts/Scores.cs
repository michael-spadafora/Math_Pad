using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scores : ToggleSelection
{
    private string[] difficultiesArray = new string[3] { "Easy ", "Medium ", "Hard " };
    public Text title;

    // Start is called before the first frame update
    void Start()
    {
        title.text = difficultiesArray[difficulty] + questionNumber;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            SceneManager.LoadScene(1);
    }
}
