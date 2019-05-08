using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleSelection : UnityEngine.MonoBehaviour
{
    public GameObject[] objectArray = new GameObject[11]; // used for the visual cursor
    public GameObject[] textArray = new GameObject[9]; // used to show what options the user has 
    private int[] scenes = new int[3] { 2, 3, 4 }; // holds the scene numbers for easy, medium, and hard scenes
    private int position = 0; // position in the objectArray and textArray
    public static int difficulty; // 0, 1, or 2 which represents easy, medium, and hard difficulties respectively
    public static List<int> modesList = new List<int>(); // List containing selected question types: number, addition, and subtraction
    public static int questionNumber; // number of questions selected from menu

    // Toggle used to toggle any of the settings as a selection for the game
    void Toggle(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        DeactivateToggle(position);
    }

    // DeactiveToggle removes selection from others in the same row if for game difficulty or questions
    void DeactivateToggle(int position)
    {
        if (position / 3 == 0 || position / 3 == 2)
            for (int i = position/3 * 3; i < position / 3 * 3 + 3; i++)
                if (i != position)
                    textArray[i].SetActive(false);
    }

    bool CheckSelected()
    {
        bool selectedOne = false; bool selectedTwo = false; bool selectedThree = false;
        for (int i = 0; i < 3; i++)
            if (textArray[i].activeSelf == true)
                selectedOne = true;
        for (int i = 3; i < 6; i++)
            if (textArray[i].activeSelf == true)
                selectedTwo = true;
        for (int i = 6; i < 9; i++)
            if (textArray[i].activeSelf == true)
                selectedThree = true;
        return selectedOne && selectedTwo && selectedThree;
    }
    // finalizes the data fields so that Play can create a Game object
    void FinalizeSelections()
    {
        for (int i = 0; i < 3; i++)
            if (textArray[i].activeSelf == true)
                difficulty = i;
        for (int i = 3; i < 6; i++)
            if (textArray[i].activeSelf == true)
                modesList.Add(i % 3);
        for (int i = 6; i < 9; i++)
            if (textArray[i].activeSelf == true)
                questionNumber = ((i % 3) + 1) * 5;
    }

    // Play method create the game object and changes the scene
    void Play()
    {
        if (CheckSelected())
        {
            FinalizeSelections();
            SceneManager.LoadScene(scenes[difficulty]);
        }
    }

    // Scores moves player to the scores screen
    void Scores()
    {
        if (CheckSelected())
        {
            FinalizeSelections();
            SceneManager.LoadScene(9);
        }
    }

    // Hides assets
    void HideAssets()
    {
        foreach (GameObject gameObject in objectArray)
            gameObject.SetActive(false);
        foreach (GameObject gameObject in textArray)
            gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        HideAssets();
        objectArray[0].SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            objectArray[position].SetActive(false);
            if (position == 10)
            {
                Play();
            }
            else if (position % 3 == 2)
                position -= 2;
            else
                position += 1;
            objectArray[position].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            objectArray[position].SetActive(false);
            if (position == 9)
            {
                Scores();
            }
            else if (position % 3 == 0)
                position += 2;
            else
                position -= 1;
            objectArray[position].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            objectArray[position].SetActive(false);
            if (position / 3 == 0)
            {
                //do nothing
            }
            else if (position == 10)
                position = 8;
            else
                position -= 3;
            objectArray[position].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            objectArray[position].SetActive(false);
            if (position / 3 == 3)
            {
                // do nothing
            }
            else if (position == 8)
                position = 10;
            else
                position += 3;
            objectArray[position].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (position == 9)
            {
                Scores();
            }
            else if (position == 10)
            {
                Play();
            }
            else
                Toggle(textArray[position]);
        }
    }
}
