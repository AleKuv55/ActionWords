using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelManager : MonoBehaviour
{
    Action _startCutsceneFinished;
    Action _endCutsceneFinished;

    public void ShowCutscene
        (int locationNumber,
         int levelNumber,
         LoadingParameter lp,
         Action cutsceneFinishedCallback
        ) {
        Debug.Log("A: Sanya verni sotku");
        Debug.Log("S: Net");
        Debug.Log("A: Pozhaluista");
        Debug.Log("S: Net");
        Debug.Log("S: Ladno");

        cutsceneFinishedCallback();
    }


    void Start()
    {

    }
}
