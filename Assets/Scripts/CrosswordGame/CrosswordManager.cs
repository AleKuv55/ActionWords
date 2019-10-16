using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordManager : MonoBehaviour, ILevelManager
{
    [SerializeField] private PentaPuzzleManager _puzzle;
    [SerializeField] private CrosswordGrid      _grid;

    private Crossword[]     _locationCrosswords;
    private CWCell[][]      _crosswordCells;
    private Crossword       _curCrossword;
    private int             _wordsFound;

    private Action _crosswordFinished;

    /// <summary>
    /// Adds an external method to be called on an event "LevelFinished"
    /// </summary>
    /// <param name="onCrosswordFinished"></param>
    public void AddCrosswordFinishedCallback(Action callback)
    {
        _crosswordFinished += callback;
    }

    private void Start()
    {
        _puzzle.AddWordActivationCallback(OnWordActivation);
        _puzzle.AddExtraActionCallback(() => { });
    }

    public void LoadLevelOut()
    {
        _grid.ReturnCellsToPool();
    }

    public void OnCrosswordFinished()
    {
        _crosswordFinished();
    }

    /// <summary>
    /// Also tells if the crossword is finished
    /// </summary>
    /// <param name="word"></param>
    /// <param name="SpellEffect"></param>
    private void OnWordActivation(string word, SpellEffect SpellEffect)
    {
        CWword cwWord = _curCrossword.GetCWword(word);
        int x = cwWord.x;
        int y = cwWord.y;
        CWword.Direction dir = cwWord.direction;

        _grid.OpenWord(x, y, dir);

        ++_wordsFound;
        if (_curCrossword.CWdict.Count == _wordsFound)
        {
            OnCrosswordFinished();
        }
    }

    /// <summary>
    /// Reads the <i>file</i> to load all levels of related location.
    /// </summary>
    /// <param name="file"> Name of the resourse file </param>
    public int LoadLocation(string file)
    {
        _locationCrosswords = Crossword.LoadCrosswords(file);
        return _locationCrosswords.Length;
    }

    /// <summary>
    /// Switches on the level indicated with <i>levelNumber</i> parameter.
    /// </summary>
    /// <param name="levelNumber">Number of a level to switch to</param>
    public void LoadLevel(int levelNumber)
    {
        _grid.ReturnCellsToPool();
        _wordsFound = 0;
        _curCrossword = _locationCrosswords[levelNumber];
        _grid.BuildCrossword(_curCrossword.GetCrosswordTable(), (char)0);
        _puzzle.InitiateBoardGame(_curCrossword);
        _puzzle.NextScroll();
    }
}
