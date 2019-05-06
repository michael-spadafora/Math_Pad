using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : ScriptableObject
{
    private int mode, difficulty;
    private int[,] numbers;
    private int[] answer;

    public int[] Answer { get => answer; set => answer = value; }
    public int[,] Numbers { get => numbers; set => numbers = value; }
    public int Mode { get => mode; set => mode = value; }

    public void init(int mode, int difficulty)
    {
        this.Mode = mode;
        this.difficulty = difficulty;
        numbers = new int[2, difficulty + 1];
        answer = new int[difficulty + 1];
        Generate();
    }

    public void Generate()
    {
        int first;
        if (Mode == 2)
            first = Random.Range((int)Mathf.Pow(10, difficulty) * 2, (int)Mathf.Pow(10, difficulty + 1));
        else
            first = Random.Range((int)Mathf.Pow(10, difficulty), (int)Mathf.Pow(10, difficulty) * 9);
        Debug.Log(first);
        int temp = first;
        for (int i = 0; i < difficulty + 1; i++)
        {
            numbers[0, i] = temp / (int)Mathf.Pow(10, difficulty - i);
            if (Mode == 0)
                answer[i] = temp;
            temp %= (int)Mathf.Pow(10, difficulty - i);
        }
        if (Mode == 1)
        {
            int second = Random.Range((int)Mathf.Pow(10, difficulty), (int)Mathf.Pow(10, difficulty + 1) - first);
            temp = second;
            for (int i = 0; i < difficulty + 1; i++)
            {
                numbers[1, i] = temp / (int)Mathf.Pow(10, difficulty - i);
                temp %= (int)Mathf.Pow(10, difficulty - i);
            }
            temp = first + second;
            for (int i = 0; i < difficulty + 1; i++)
            {
                answer[i] = temp / (int)Mathf.Pow(10, difficulty - i);
                temp %= (int)Mathf.Pow(10, difficulty - i);
            }
            Debug.Log(second + " add");
        }
        else if (Mode == 2)
        {
            int second = Random.Range((int)Mathf.Pow(10, difficulty), first - ((int)Mathf.Pow(10, difficulty)));
            temp = second;
            for (int i = 0; i < difficulty + 1; i++)
            {
                numbers[1, i] = temp / (int)Mathf.Pow(10, difficulty - i);
                temp %= (int)Mathf.Pow(10, difficulty - i);
            }
            temp = first - second;
            for (int i = 0; i < difficulty + 1; i++)
            {
                answer[i] = temp / (int)Mathf.Pow(10, difficulty - i);
                temp %= (int)Mathf.Pow(10, difficulty - i);
            }
            Debug.Log(second + " sub");
        }
    }
}
