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
    public Text questionText;
    public Text bunsText;

    private static List<Texture[]> textures;
    private static int currentQuestion = 0; // current question position, initialized at 0
    private static int[] modes = modesList.ToArray(); // array containing selected question types: number, addition, and subtraction
    private static bool plus = true; // boolean for the current dancepad mode, addition or subtraction mode, initialized at true
    private static bool paused = false; // checks whether the game was paused
    private static int[,] inputs = new int[questionNumber, difficulty + 1]; // array that stores the user's answers inputted
    private static bool[] inputsAnswered = new bool[questionNumber]; // array that checks whether each question has an answer
    private static Question[] questions; // array that contains Question objects which contain a question
    public static int score = questionNumber * 5;
    private static string[] bunsWords =
        new string[7]{ "You're doing great!", "Awesome job!", "Keep it up!", "Way to go!", "Marvelous!", "Fantastic!", "Superb!" };

    public static int[,] Inputs { get => inputs; set => inputs = value; }
    public static int CurrentQuestion { get => currentQuestion; set => currentQuestion = value; }
    public static bool Plus { get => plus; set => plus = value; }
    public static bool Paused { get => paused; set => paused = value; }

    // Restarts inputs
    public void RestartGame()
    {
        plus = true;
        paused = false;
        currentQuestion = 0;
        for (int i = 0; i < questionNumber; i++)
        {
            for (int j = 0; j < difficulty; j++)
                inputs[i, j] = 0;
        }
    }

    // Hides every asset that needs to be hidden
    void HideAssets()
    {
        foreach (GameObject asset in numbers)
            asset.SetActive(false);
        foreach (GameObject asset in addition)
            asset.SetActive(false);
        foreach (GameObject asset in subtraction)
            asset.SetActive(false);
    }

    // Creates an array that is filled with any combination of 0, 1, and 2 
    // which represents the question type: number, addition, subtraction
    private int[] ModesArray()
    {
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
        questionText.text = "1";
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
        int i = difficulty; int j = difficulty;
        foreach (GameObject asset in answers)        
            asset.GetComponent<RawImage>().texture = textures[i--][inputs[currentQuestion, j--]];        
    }

    // Adds or subtracts hundred
    void Hundred()
    {
        if (difficulty == 2) // as long as hundreds place isn't already 9
        {
            if (plus && inputs[currentQuestion, difficulty] < 9)
            {
                inputs[currentQuestion, difficulty]++;
                UpdateAnswer();
            }
            else if (!plus && inputs[currentQuestion, difficulty] > 0)
            {
                inputs[currentQuestion, difficulty]--;
                UpdateAnswer();
            }
            inputsAnswered[currentQuestion] = true;
        }
    }

    // Adds or subtracts ten 
    void Ten()
    {
        if (difficulty == 2) // need this because adding tens can carry over to hundreds place 
        {
            if (plus)
            {
                if (!(inputs[currentQuestion, difficulty] == 9 && inputs[currentQuestion, difficulty - 1] == 9))
                // can't add ten if answer input is 900 + 90 + x
                {
                    if (++inputs[currentQuestion, difficulty - 1] == 10) // if tens place is now 10, carry over
                    {
                        inputs[currentQuestion, difficulty]++;
                        inputs[currentQuestion, difficulty - 1] = 0;
                    }
                    UpdateAnswer();
                }
            }
            else
            {
                if (!(inputs[currentQuestion, difficulty] == 0 && inputs[currentQuestion, difficulty - 1] == 0))
                // can't subtract ten if answer input is 000 + 00 + x
                {
                    if (--inputs[currentQuestion, difficulty - 1] == -1) // if tens place is now -1, get tens from hundreds
                    {
                        inputs[currentQuestion, difficulty]--;
                        inputs[currentQuestion, difficulty - 1] = 9;
                    }
                    UpdateAnswer();
                }
            }
            inputsAnswered[currentQuestion] = true;
        }

        else if (difficulty == 1)
        {
            if (plus && inputs[currentQuestion, difficulty] < 9)
                inputs[currentQuestion, difficulty]++;
            else if (!plus && inputs[currentQuestion, difficulty] > 0)
                    inputs[currentQuestion, difficulty]--;
            UpdateAnswer();
            inputsAnswered[currentQuestion] = true;
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
                    if (++inputs[currentQuestion, 0] == 10) // increase ones and if ones place is now 10
                    {
                        if (++inputs[currentQuestion, 1] == 10) // increase tens and if tens place is now 10
                        {
                            inputs[currentQuestion, 2]++; // increase hundreds
                            inputs[currentQuestion, 1] = 0; // set tens back to 0
                        }
                        inputs[currentQuestion, 0] = 0; // set ones back to 0
                    }
                    UpdateAnswer();
                }
            }
            else
            {
                if (!(inputs[currentQuestion, 0] == 0 && inputs[currentQuestion, 1] == 0 && inputs[currentQuestion, 2] == 0))
                // can't subtract 1 if answer input is 000
                {
                    if (--inputs[currentQuestion, 0] == -1) // subtract ones and if ones place is now -1
                    {
                        if (--inputs[currentQuestion, 1] == -1) // subtract tens and if tens place is now -1
                        {
                            inputs[currentQuestion, 2]--; // decrease hundreds
                            inputs[currentQuestion, 1] = 9; // set tens back to 9 since borrowed from hundreds
                        }
                        inputs[currentQuestion, 0] = 9; // set ones back to 9 since borrowed from tens
                    }
                    UpdateAnswer();
                }
            }
            inputsAnswered[currentQuestion] = true;
        }
        
        else if (difficulty == 1) // double digits case
        {
            if (plus)
            {
                if (!(inputs[currentQuestion, 1] == 9 && inputs[currentQuestion, 0] == 9))
                // can't add one if answer input is 99
                {
                    if (++inputs[currentQuestion, 0] == 10) // if ones place is now 10, carry over
                    {
                        inputs[currentQuestion, 1]++; // increase tens by one
                        inputs[currentQuestion, 0] = 0; // set ones back to 0
                    }
                    UpdateAnswer();
                }
            }
            else
            {
                if (!(inputs[currentQuestion, 1] == 0 && inputs[currentQuestion, 2] == 0))
                // can't subtract if answer input is 00
                {
                    if (--inputs[currentQuestion, 0] == -1) // if ones place is now -1, carry over
                    {
                        inputs[currentQuestion, 1]--; // subtract tens by one
                        inputs[currentQuestion, 0] = 9; // set ones back to 9
                    }
                    UpdateAnswer();
                }
            }
            inputsAnswered[currentQuestion] = true;
        }

        else
        {
            if (plus && inputs[currentQuestion, difficulty] < 9)
            {
                inputs[currentQuestion, difficulty]++;
                UpdateAnswer();
            }
            else if (!plus && inputs[currentQuestion, difficulty] > 0)
            {
                inputs[currentQuestion, difficulty]--;
                UpdateAnswer();
            }
            inputsAnswered[currentQuestion] = true;
        }
    }

    // Next question
    void NextQuestion()
    {
        if (currentQuestion < questionNumber - 1)
        {
            currentQuestion++;
            Show(questions[currentQuestion]);
            questionText.text = (currentQuestion + 1).ToString();
        }
    }

    // Previous question
    void PreviousQuestion()
    {
        if (currentQuestion > 0)
        {
            currentQuestion--;
            Show(questions[currentQuestion]);
            questionText.text = (currentQuestion + 1).ToString();
        }
    }

    // Finish or goes back to plus pad if in minus
    void BottomLeft()
    {
        if (plus)
        {
            Done();
        }
        else
        {
            plus = true; // plus mode
            dancepads[0].SetActive(true); // shows plus pad
            dancepads[1].SetActive(false); // hides minus pad
        }
    }

    // Check if every question has an answer
    bool AllAnswered()
    {
        for (int i = 0; i < inputsAnswered.Length; i++)
        {
            if (!inputsAnswered[i])
                return inputsAnswered[i];
        }
        return true;
    }

    bool CheckAnswer(int i)
    {
        for (int j = 0; j < questions[i].Answer.Length; i++)
        {
            if (inputs[i, j] != questions[i].Answer[j])
                return false;
        }
        return true;
    }
    // Calculates score
    int CalculateScore()
    {
        for (int i = 0; i < questionNumber; i++)
        {
            if (CheckAnswer(i))
            {
                score += 10;
            }
        }
        return score;
    }

    // Prompts user if done
    void Done()
    {
        if (AllAnswered())
        {
            CalculateScore();
            SceneManager.LoadScene(8);
        }
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
        bunsText.text = bunsWords[(int)Random.Range(0, 7)];
    }

    

    // Start is called before the first frame update
    void Start()
    {
        if (!paused)
        {
            SetUp();
            textures = new List<Texture[]>() { ones, tens, hundreds };
            InvokeRepeating("Encourage", 10.0f, 5); // supposed to start in 10 seconds
        }
        else
        {
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
