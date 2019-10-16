using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NovelManager : MonoBehaviour
{
    Action _startCutsceneFinished;
    Action _endCutsceneFinished;

    Action _temporaryCallback;

    public Text text;

    public void ShowCutscene
                            (int locationNumber,
                             int levelNumber,
                             LoadingParameter lp,
                             Action cutsceneFinishedCallback
                            ) 
    {
        Debug.Log("A: Sanya verni sotku");
        Debug.Log("S: Net");
        Debug.Log("A: Pozhaluista");
        Debug.Log("S: Net");
        Debug.Log("S: Ladno");

        gameObject.SetActive(true);
        if (lp == LoadingParameter.START)
        {
            switch (levelNumber)
            {
                case 0: { text.text = "Цель \n\nНабери воду \nв колодце"; break; }
                case 1:
                    {
                        text.text = "Цель \n\nПомоги \nдобыть рыбу"; break;
                    }
                case 2:
                    {
                        text.text = "Цель \n\nСпаси \nовечку"; break;
                    }
                case 3:
                    {
                        text.text = "У тебя есть шанс \n\nПриручить \n зверька"; break;
                    }
            }
        }
        else if (lp == LoadingParameter.END)
            text.text = "Молодец!!!";

        _temporaryCallback = cutsceneFinishedCallback;
        //cutsceneFinishedCallback();
    }

    public void OnCutSceneButtonClick()
    {
        _temporaryCallback();
        gameObject.SetActive(false);
    }


    void Start()
    {
        Debug.Log("Message from master");
    }
}
