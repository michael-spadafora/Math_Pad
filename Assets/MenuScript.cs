using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    string[,] imgTags;
    //is something selected or not
    bool[,] active; //persistent state across scenes
    
    //cursor location
    int currRow = 0;
    int currCol = 0;


    // Start is called before the first frame update
    void Start()
    {

        imgTags = new string[,]{ {"easy", "numbers", "five", "scores"},
                                {"medium", "addition", "ten", "play"}, //currently need to press left twice to go to scores
                                {"hard", "subtraction", "fifteen", "play"} };


        active = new bool[3,3];
        active[0,0] = true;
        active[0,1] = true;
        active[0,2] = true;


            updateActiveButtons();


        // GameObject ParentGameObject = GameObject.FindGameObjectWithTag("Canvas"); 
        // GameObject ChildGameObject0 = ParentGameObject.transform.GetChild (0).gameObject; 
        // GameObject ChildGameObject1 = ParentGameObject.transform.GetChild (1).gameObject;

        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Space))
        {
            selectCurr();        
            updateActiveButtons();
        }


        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            currCol = (currCol+1) % 3;
            if (currRow == 3 && currCol == 1) {
                currCol = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {           
            currCol = (currCol+2) % 3;
            if (currRow == 3 && currCol == 1) {
                currCol = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currRow = (currRow+1) % 4;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            currRow = (currRow+3) % 4;
        }
        updateCursor();
    }

    void updateCursor() {
        Vector3 pos = GameObject.Find(imgTags[currCol,currRow]).transform.position;
        GameObject.Find ("cursor").transform.position = new Vector3(pos.x-40, pos.y, pos.z);

    }

    void selectCurr() {
            //if play button
            if (currRow == 3 && currCol == 2) {
                for(int i = 0 ; i < 3; i++) {
                    //get Difficulty
                    if (active[i, 0]) {
                        PlayerPrefs.SetString("difficulty", imgTags[i,0]);
                    }
                    if (active[i, 1]) {
                        PlayerPrefs.SetInt(imgTags[i,1], 1);
                    } else {
                        PlayerPrefs.SetInt(imgTags[i,1], 0);
                    }
                    if (active[i,2]) {
                        PlayerPrefs.SetString("numQuestions", imgTags[i,2]);
                    }

                }

                Application.LoadLevel("gameplay screen");
            }
            //elif score button
            else if (currRow == 3 && currCol == 0) {
                Application.LoadLevel("score screen");
            }
            else {
                if (currRow == 0 || currRow == 2) {
                    for (int i = 0; i < 3; i++) {
                        active[i,currRow] = false;
                    }
                    active[currCol, currRow] = true;


                } else if (currRow == 1) {
                    active[currCol,currRow] = !active[currCol,currRow];    
                    bool tes = false;
                    for (int i = 0; i < 3; i++) {
                        if (active[i,currRow]) {
                            tes = true;
                        }
                    }
                    if (!tes) {
                        active[0,1] = true;
                    }
                }
                
            }

    }

    void updateActiveButtons() {
        for (int row= 0; row < 3; row=row+1 ) {
            for (int col = 0; col < 3; col=col+1) {
                //why the fuck does this think everything is not active? 
                //fucking unity is garbage
                Debug.Log(imgTags[col,row] + ": " + active[col,row]);

                if (!active[col,row]){
                    GameObject.Find (imgTags[col,row]).transform.localScale = new Vector3(0, 0, 0);
                } else {
                    GameObject.Find (imgTags[col,row]).transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }
}
