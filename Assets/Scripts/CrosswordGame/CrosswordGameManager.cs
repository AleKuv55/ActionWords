using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosswordGameManager : MonoBehaviour
{
    /// <summary>
    /// Canvases that are necessary on scene
    /// </summary>
    [SerializeField] private GameObject _puzzleCanvas;
    [SerializeField] private GameObject _mapCanvas;
    
    /// <summary>
    /// Managers that are necessary on scene
    /// </summary>
    [SerializeField] private CrosswordManager       _crosswordManager;
    [SerializeField] private PentaPuzzleManager     _pentaPuzzleManager;
    [SerializeField] private NovelManager           _novelManager;

    [SerializeField] private GameObject _levelEndScreen;
    [SerializeField] private Text _resultText;
    [SerializeField] private Text _actionText;


    private int _locationNumber = 0;
    private int _locationLevelsCount;
    private int _levelNumber;
    private const string _levelsFile = @"Assets/Resources/crossword.txt";

    private void Awake()
    {
    }

    void Start()
    {
        gameObject.SetActive(false);
        _crosswordManager.AddCrosswordFinishedCallback(OnCrosswordFinished);
        _locationLevelsCount = _crosswordManager.LoadLocation(_levelsFile);
    }

    public void OnMapLevelButtonPressed (int levelNumber)
    {
        _levelNumber = levelNumber-1;
        LaunchCutscene(LoadingParameter.START, NextLevel);
        _mapCanvas.gameObject.SetActive(false); 

    }

    public void NextLevel()
    {
        SwitchOnLevel(++_levelNumber);
    }

    public void SwitchOnLevel(int level)
    {
        _mapCanvas.gameObject.SetActive(false);
        _levelEndScreen.SetActive(false);
        _puzzleCanvas.gameObject.SetActive(true);

        _levelNumber = level;
        _crosswordManager.LoadLevel(level);
    }


    private void OnCrosswordFinished()
    {
        LaunchCutscene(LoadingParameter.END, ShowLevelFinishedDialogue);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lp">Parse START or END</param>
    private void LaunchCutscene(LoadingParameter lp, Action afterCutscene)
    {
        if (LoadingParameter.END != lp &&
            LoadingParameter.START != lp
            ) {
            throw new InvalidOperationException("For now can work with only START or END parameters.");
        }
        int locationNumber = 0;
        _novelManager.ShowCutscene(locationNumber, _levelNumber, lp, afterCutscene);
    }

    private void ShowLevelFinishedDialogue()
    {
        _levelEndScreen.SetActive(true);
        gameObject.SetActive(true);

        _resultText.text = "Просто волшебно!";
        _actionText.text = "Продолжить путь";
    }


    private void GoToMap ()
    {
        _mapCanvas.gameObject.SetActive(true);
        _puzzleCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Occurs after Player accepted the end of level (pushed the button)
    /// </summary>
    public void OnLevelFinished()
    {
        _crosswordManager.LoadLevelOut();

        LoadingParameter loadingParameter = LoadingParameter.MAP;

        if ((loadingParameter == LoadingParameter.AUTO ||
             loadingParameter == LoadingParameter.NEXT_LEVEL) &&
            _levelNumber < _locationLevelsCount - 1
            ) {
            NextLevel();
            return;
        }
        _levelEndScreen.SetActive(false);

        GoToMap();
    }
}
