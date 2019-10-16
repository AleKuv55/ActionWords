using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordLevelManager : MonoBehaviour
{
    [SerializeField] private PentaPuzzleManager _pentaPuzzleManager;

    private Action _levelFinishedCallback;

    public void AddLevelFinishedCallback(Action onLevelFinished)
    { _levelFinishedCallback += onLevelFinished; }


    private void Awake()
    {
        Debug.Log("CrosswordLevelManager is alive.");
        _pentaPuzzleManager.AddWordActivationCallback(OnWordActivation);
        _pentaPuzzleManager.AddExtraActionCallback(() => { });
    }

    private void OnEnable()
    {
    }


    private void OnWordActivation(string word, SpellEffect SpellEffect)
    {
    }

    internal void LoadLevel(int level)
    {
        _pentaPuzzleManager.StartBoardGame();
    }


}
