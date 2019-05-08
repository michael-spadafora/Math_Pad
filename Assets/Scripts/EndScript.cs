using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScript : GamePlay
{
    private int noClick = 0;
    private int noLetter = 0;
    public Text[] letters;
    private string[] alphabet = new string[26];
    public GameObject[] boxes;
    public Texture[] textures;
    public Text scoreText;

    //changes current letter's texture
    public void UpdateTexture()
    {
        if (noLetter == 0)
        {
            boxes[1].GetComponent<RawImage>().texture = textures[1];
            boxes[2].GetComponent<RawImage>().texture = textures[1];

        }
        else if (noLetter == 1)
        {
            boxes[0].GetComponent<RawImage>().texture = textures[1];
            boxes[2].GetComponent<RawImage>().texture = textures[1];
        }
        else
        {
            boxes[0].GetComponent<RawImage>().texture = textures[1];
            boxes[1].GetComponent<RawImage>().texture = textures[1];
        }
        boxes[noLetter].GetComponent<RawImage>().texture = textures[0];
    }

    //for UP button
    public void NextLetterForward()
    {
        if (++noClick >= 26)
        {
            noClick = 0;
        }
        letters[noLetter].text = alphabet[noClick];
    }

    //for DOWN button
    public void NextLetterBackward()
    {

        if (--noClick < 0)
        {
            noClick = 25;
        }
       letters[noLetter].text = alphabet[noClick];
    }

    //for RIGHT button
    public void NextLetterRight()
    {
        //makes sure correct letter is selected (first, second or third)
        if (++noLetter > 2) 
            noLetter = 0;

        UpdateTexture();

        //makes sure program starts incrementing at the next     
        //letter by finding current letter
        for (int i = 0; i < 26; i++)
        {
            if (letters[noLetter].text == alphabet[i])
            {
                noClick = i;
            }
        }
    }

    //for LEFT button
    public void NextLetterLeft()
    {
        //makes sure correct letter is selected (first, second or third)
        if (--noLetter < 0)
            noLetter = 2;

        UpdateTexture();

        //makes sure program starts incrementing at the next     
        //letter by finding current letter
        for (int i = 0; i < 26; i++)
        {
            if (letters[noLetter].text == alphabet[i])
            {
                noClick = i;
            }
        }
    }

    // for Q
    public void Done()
    {
        SceneManager.LoadScene(1);
        RestartGame();
    }

    // Start is called before the first frame update
    void Start()
    {
        alphabet[0] = "A";
        alphabet[1] = "B";
        alphabet[2] = "C";
        alphabet[3] = "D";
        alphabet[4] = "E";
        alphabet[5] = "F";
        alphabet[6] = "G";
        alphabet[7] = "H";
        alphabet[8] = "I";
        alphabet[9] = "J";
        alphabet[10] = "K";
        alphabet[11] = "L";
        alphabet[12] = "M";
        alphabet[13] = "N";
        alphabet[14] = "O";
        alphabet[15] = "P";
        alphabet[16] = "Q";
        alphabet[17] = "R";
        alphabet[18] = "S";
        alphabet[19] = "T";
        alphabet[20] = "U";
        alphabet[21] = "V";
        alphabet[22] = "W";
        alphabet[23] = "X";
        alphabet[24] = "Y";
        alphabet[25] = "Z";

        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            NextLetterForward();

        if (Input.GetKeyDown(KeyCode.X))
            NextLetterBackward();

        if (Input.GetKeyDown(KeyCode.A))
            NextLetterLeft();

        if (Input.GetKeyDown(KeyCode.D))
            NextLetterRight();

        if (Input.GetKeyDown(KeyCode.Q))
            Done();
    }





}

