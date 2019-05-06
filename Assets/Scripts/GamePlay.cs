using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : ToggleSelection
{
    public GameObject[] answers;
    public GameObject[] numbers;
    public GameObject[] addition;
    public GameObject[] subtraction;
    public GameObject[] dancepads;

    public Texture[] hundreds;
    public Texture[] tens;
    public Texture[] ones;

    private static List<Texture[]> textures;
    private static int currentQuestion = 0; // current question position, initialized at 0
    private static int[] modes = modesList.ToArray(); // array containing selected question types: number, addition, and subtraction
    private static bool plus = true; // boolean for the current dancepad mode, addition or subtraction mode, initialized at true
    private static bool paused = false; // checks whether the game was paused
    private static int[,] inputs = new int[questionNumber, difficulty + 1]; // array that stores the user's answers inputted
    private static Question[] questions; // array that contains Question objects which contain a question

    // Hides every asset that needs to be hidden
    void HideAssets()
    {
        //foreach (GameObject asset in answers)
          //  asset.SetActive(false);
        foreach (GameObject asset in numbers)
            asset.SetActive(false);
        foreach (GameObject asset in addition)
            asset.SetActive(false);
        foreach (GameObject asset in subtraction)
            asset.SetActive(false);
        //foreach (GameObject asset in dancepads)
          //  asset.SetActive(false);
    }

    // Creates an array that is filled with any combination of 0, 1, and 2 
    // which represents the question type: number, addition, subtraction
    private int[] ModesArray()
    {
        Debug.Log("QUESTIONS: " + questionNumber); // can delete
        int[] toReturn = new int[questionNumber];
        int remainder = questionNumber % modes.Length;
        for (int i = 0; i < questionNumber / modes.Length; i++)
            for (int j = 0; j < modes.Length; j++)
                toReturn[i * modes.Length + j] = modes[j];

        if (remainder > 0)
            for (int i = questionNumber - remainder; i < questionNumber; i++)
                toReturn[i] = modes[questionNumber - i];
        return toReturn;
    }

    // Shuffles the array - Fisher-Yates shuffle
    void ShuffleArray(int[] intArray)
    {
        for (int i = 0; i < intArray.Length; i++)
        {
            int temp = intArray[i];
            int r = Random.Range(i, intArray.Length);
            intArray[i] = intArray[r];
            intArray[r] = temp;
        }
    }

    // Sets up the questions
    void SetUp()
    {
        HideAssets();
        questions = new Question[questionNumber];
        int[] temp = ModesArray();
        ShuffleArray(temp);
        for (int i = 0; i < temp.Length; i++)
        {
            questions[i] = ScriptableObject.CreateInstance("Question") as Question;
            questions[i].init(temp[i], difficulty);
        }
        dancepads[1].SetActive(false);
    }

    // Shows a questions
    void Show(Question question)
    {
        HideAssets();
        int i = difficulty; int j = 0; int part = 0;
        if (question.Mode == 0)
        {
            foreach (GameObject number in numbers)
            {
                number.GetComponent<RawImage>().texture = textures[i--][questions[currentQuestion].Numbers[0, j++]];
                number.SetActive(true);            
            }
        }
        else if (question.Mode == 1)
        {
            
            foreach (GameObject asset in addition)
            {
                if (j == difficulty + 1)
                {
                    asset.SetActive(true);
                    part++; j++;
                    i = difficulty;
                }
                else
                {
                    asset.GetComponent<RawImage>().texture = textures[i--][questions[currentQuestion].Numbers[part, j++ % (difficulty + 2)]];
                    asset.SetActive(true);
                }
            }
        }
        else
        {
            foreach (GameObject asset in subtraction)
            {
                if (j == difficulty + 1)
                {
                    asset.SetActive(true);
                    part++; j++;
                    i = difficulty;
                }
                else
                {
                    asset.GetComponent<RawImage>().texture = textures[i--][questions[currentQuestion].Numbers[part, j++ % (difficulty + 2)]];
                    asset.SetActive(true);
                }
            }
        }
        UpdateAnswer();
    }

    // Updates the user's answers on screen
    void UpdateAnswer()
    {
        int i = difficulty; int j = 0;
        foreach (GameObject asset in answers)        
            asset.GetComponent<RawImage>().texture = textures[i--][inputs[currentQuestion, j++]];        
    }

    // Adds or subtracts hundred
    void Hundred()
    {
        if (difficulty == 2) // as long as hundreds place isn't already 9
        {
            if (plus && inputs[currentQuestion, 0] < 9)
            {
                inputs[currentQuestion, 0]++;
                UpdateAnswer();
            }
            else if (!plus && inputs[currentQuestion, 0] > 0)
            {
                inputs[currentQuestion, 0]--;
                UpdateAnswer();
            }
        }
    }

    // Adds or subtracts ten 
    void Ten()
    {
        if (difficulty == 2) // need this because adding tens can carry over to hundreds place 
        {
            if (plus)
            {
                if (!(inputs[currentQuestion, 0] == 9 && inputs[currentQuestion, 1] == 9))
                // can't add ten if answer input is 900 + 90 + x
                {
                    if (++inputs[currentQuestion, 1] == 10) // if tens place is now 10, carry over
                    {
                        inputs[currentQuestion, 0]++;
                        inputs[currentQuestion, 1] = 0;
                    }
                    UpdateAnswer();
                }
            }
            else
            {
                if (!(inputs[currentQuestion, 0] == 0 && inputs[currentQuestion, 1] == 0))
                // can't subtract ten if answer input is 000 + 00 + x
                {
                    if (--inputs[currentQuestion, 1] == -1) // if tens place is now -1, get tens from hundreds
                    {
                        inputs[currentQuestion, 0]--;
                        inputs[currentQuestion, 1] = 9;
                    }
                    UpdateAnswer();
                }
            }
            
        }

        else if (difficulty == 1 && inputs[currentQuestion, 1] < 9)
        {
            if (plus)
                inputs[currentQuestion, 1]++;
            else
                inputs[currentQuestion, 1]--;
            UpdateAnswer();
        }
    }

    // Adds or subtracts one
    void One()
    {
        if (difficulty == 2) // triple digits case
        {
            if (plus)
            {
                if (!(inputs[currentQuestion, 0] == 9 && inputs[currentQuestion, 1] == 9 && inputs[currentQuestion, 2] == 9))
                // can't add 1 if answer input is 999
                {
                    if (++inputs[currentQuestion, 2] == 10) // increase ones and if ones place is now 10
                    {
                        if (++inputs[currentQuestion, 1] == 10) // increase tens and if tens place is now 10
                        {
                            inputs[currentQuestion, 0]++; // increase hundreds
                            inputs[currentQuestion, 1] = 0; // set tens back to 0
                        }
                        inputs[currentQuestion, 2] = 0; // set ones back to 0
                    }
                    UpdateAnswer();
                }
            }
            else
            {
                if (!(inputs[currentQuestion, 0] == 0 && inputs[currentQuestion, 1] == 0 && inputs[currentQuestion, 2] == 0))
                // can't subtract 1 if answer input is 000
                {
                    if (--inputs[currentQuestion, 2] == -1) // subtract ones and if ones place is now -1
                    {
                        if (--inputs[currentQuestion, 1] == -1) // subtract tens and if tens place is now -1
                        {
                            inputs[currentQuestion, 0]--; // decrease hundreds
                            inputs[currentQuestion, 1] = 9; // set tens back to 9 since borrowed from hundreds
                        }
                        inputs[currentQuestion, 2] = 9; // set ones back to 9 since borrowed from tens
                    }
                    UpdateAnswer();
                }
            }
        }
        
        else if (difficulty == 1) // double digits case
        {
            if (plus)
            {
                if (!(inputs[currentQuestion, 1] == 9 && inputs[currentQuestion, 2] == 9))
                // can't add one if answer input is 99
                {
                    if (++inputs[currentQuestion, 2] == 10) // if ones place is now 10, carry over
                    {
                        inputs[currentQuestion, 1]++; // increase tens by one
                        inputs[currentQuestion, 2] = 0; // set ones back to 0
                    }
                    UpdateAnswer();
                }
            }
            else
            {
                if (!(inputs[currentQuestion, 1] == 0 && inputs[currentQuestion, 2] == 0))
                // can't subtract if answer input is 00
                {
                    if (--inputs[currentQuestion, 2] == -1) // if ones place is now -1, carry over
                    {
                        inputs[currentQuestion, 1]--; // subtract tens by one
                        inputs[currentQuestion, 2] = 9; // set ones back to 9
                    }
                    UpdateAnswer();
                }
            }
        }

        else
        {
            if (plus && inputs[currentQuestion, 2] < 9)
            {
                inputs[currentQuestion, 2]++;
                UpdateAnswer();
            }
            else if (!plus && inputs[currentQuestion, 2] > 0)
            {
                inputs[currentQuestion, 2]--;
                UpdateAnswer();
            }
        }
    }

    // Next question
    void NextQuestion()
    {
        if (currentQuestion < questionNumber - 1)
        {
            currentQuestion++;
            Debug.Log("Q " + currentQuestion);
            Show(questions[currentQuestion]);
        }
    }

    // Previous question
    void PreviousQuestion()
    {
        if (currentQuestion > 0)
        {
            currentQuestion--;
            Debug.Log("Q " + currentQuestion);
            Show(questions[currentQuestion]);
        }
    }

    // Finish or goes back to plus pad if in minus
    void BottomLeft()
    {
        if (plus)
        {
            if (currentQuestion == questionNumber - 1)
            {
                Done();
            }
        }
        else
        {
            Debug.Log("ASDFSA");
            plus = true; // plus mode
            dancepads[0].SetActive(true); // shows plus pad
            dancepads[1].SetActive(false); // hides minus pad
        }
    }

    // Prompts user if done
    void Done()
    {

    }

    // Pauses the game
    void Pause()
    {
        paused = true;
        SceneManager.LoadScene(5);
    }

    // Changes to minus pad if in plus
    void BottomRight()
    {
        if (plus)
        {
            plus = false; // minus mode
            dancepads[0].SetActive(false); // hides plus pad
            dancepads[1].SetActive(true); // shows minus pad
        }
    }

    // Changes Bun's words of encouragement
    void Encourage()
    {

    }

    

    // Start is called before the first frame update
    void Start()
    {
        if (!paused)
        {
            Debug.Log("first time Paused: " + paused);
            SetUp();
            textures = new List<Texture[]>() { ones, tens, hundreds };
        }
        else
        {
            Debug.Log("asdfasdfasdfa Paused: " + paused);
            paused = false;        
        }
        Encourage(); // change question text somewhere here
        Show(questions[currentQuestion]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Hundred();

        if (Input.GetKeyDown(KeyCode.W))
            Ten();

        if (Input.GetKeyDown(KeyCode.E))
            One();

        if (Input.GetKeyDown(KeyCode.A))
            PreviousQuestion();

        if (Input.GetKeyDown(KeyCode.D))
            NextQuestion();

        if (Input.GetKeyDown(KeyCode.Z))
            BottomLeft();

        if (Input.GetKeyDown(KeyCode.X))
            Pause();

        if (Input.GetKeyDown(KeyCode.C))
            BottomRight();
    }

}
