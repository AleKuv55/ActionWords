using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PentaPuzzleManager : MonoBehaviour, IBoardGame
{
    private Action<string, SpellEffect> _wordActivationCallback;
    private Action _nextScrollCallback;

    [SerializeField] private Animator _rewardAnimator;
    [SerializeField] private GameObject _changeScrollButton;

    [SerializeField] private Text _pentagramsLeftText;
    [SerializeField] private Text _wordsSelectedText;
    [SerializeField] private Text _selectedWordText;

    private int _maxWords;
    private int _wordsSelected;

    private PentaLoader _pentaLoader;
    private PoolOfAll _pool;
    private ScrollManager _scrollManager;
    private Liner _liner;

    //private int _pentagramCounter;
    //private Pentagram[] _pentagrams;

    private void Awake()
    {
        _pentaLoader = new PentaLoader();

        _liner = FindObjectOfType<Liner>();
        _pool = GetComponentInChildren<PoolOfAll>();
        _scrollManager = GetComponentInChildren<ScrollManager>();


        if (_liner == null || _pool == null || _scrollManager == null)
        { Debug.LogError("PuzzleManager couldn't find something."); }

        _selectedWordText.text = "";

        Scroll[] pents = FindObjectsOfType<Scroll>();
        foreach (Scroll pent in pents)
        {
            pent.AddSpellActivatedCallBack(OnWordActivation);
            pent.AddSelectedWordChangedCallback(OnSelectedWordChanged);
        }
    }

    public void StartBoardGame ()
    {
        _pentaLoader.LoadPentagrams();
        NextScroll();
    }

    public void InitiateBoardGame (Crossword crossword)
    {
        _pentaLoader.LoadPentagrams(crossword);
    }

    public void EndBoardGame()
    {
        gameObject.SetActive(false);
    }

    public void AddWordActivationCallback(Action<string, SpellEffect> callback)
    {
        _wordActivationCallback = null;
        _wordActivationCallback += callback;
    }

    public void AddExtraActionCallback(Action callback)
    {
        _nextScrollCallback = null;
        _nextScrollCallback += callback;
    }

    public void OnWordActivation(string word, int[] letterNumbersSequence)
    {
        SetupRewardAnimation();

        IncreaseWordsSelectedText();
        _wordActivationCallback(word, SpellEffect.None);
    }

    private void OnSelectedWordChanged (string word)
    {
        Debug.Log("" + word);
        _selectedWordText.text = word.ToUpper();
    }

    public void NextScroll()
    {
        Debug.Log("PuzzleManager: NextScroll");
        SmartPentagram newPentagram = _pentaLoader.GetNextPentagram();
        _scrollManager.ChangeScroll(newPentagram);
        if (_pentaLoader.OutOfPentagrams()) { _changeScrollButton.SetActive(false); }

        _wordsSelectedText = _scrollManager.GetActiveScroll().GetWordsSelectedCounter();

        _pentagramsLeftText.text = _pentaLoader.PentagramsLeft() + " свитков осталось.";

        _maxWords = newPentagram.GetSelectableWords().Count;
        _wordsSelected = 0;
        _wordsSelectedText.text = "0 / " + _maxWords + " слов найдено.";

        _nextScrollCallback();
    }

    /// <summary>
    /// Govnocode Bad Code Remake this Refactor required
    /// </summary>
    /// <param name="number"></param>
    public void SetScroll(int number)
    {
        Debug.Log("PuzzleManager: SetScroll");
        SmartPentagram newPentagram = _pentaLoader.GetPentagram(number);
        _scrollManager.ChangeScroll(newPentagram);
        if (_pentaLoader.OutOfPentagrams()) { _changeScrollButton.SetActive(false); }

        _wordsSelectedText = _scrollManager.GetActiveScroll().GetWordsSelectedCounter();

        _pentagramsLeftText.text = _pentaLoader.PentagramsLeft() + " свитков осталось.";

        _maxWords = newPentagram.GetSelectableWords().Count;
        _wordsSelected = 0;
        _wordsSelectedText.text = "0 / " + _maxWords + " слов найдено.";

        _nextScrollCallback();
    }


    public List<string> GetSelectableWords()
    {
        return _scrollManager
                .GetActiveScroll()
                .GetPentagram()
                .GetSelectableWords();
    }

    public IEnumerator EmulateWordActivation(string word)
    {
        Scroll activeScroll = _scrollManager.GetActiveScroll();
        yield return StartCoroutine(activeScroll.SelectWord(word));

        //if (word != activeScroll.GetSelectedWord())
        //{
        //    Debug.LogError("Somehow word \"" + word + "\" couldn't have been selected in a pentagram, but the Mage doesn't give a fuck");
        //    yield break;
        //}

        activeScroll.GetPentagram().TryToUseWord(word);
        IncreaseWordsSelectedText();

        activeScroll.UnselectLetters();
        _liner.Clear(null);
        _wordActivationCallback(word, SpellEffect.None);
    }
    

    private void IncreaseWordsSelectedText()
    {
        _wordsSelectedText.text = ++_wordsSelected + " / " + _maxWords + " слов найдено.";
    }
    

    private SpellEffect EffectOfSequence(int wordLength, int[] sequence)
    {
        // Less than 3 letters -> Stun
        if (wordLength <= 2) return SpellEffect.Stun;


        //Cycle -> Shield
        for (int i = 0; i < wordLength; i++)
        {
            for (int j = i + 1; j < wordLength; j++)
            {
                if (sequence[i] == sequence[j])
                {
                    Debug.Log("SHIELD!");
                    return SpellEffect.Shield;
                }
            }
        }

        //None -> None
        return SpellEffect.None;
    }

    private void SetupRewardAnimation()
    {
        /*
        string[] s = { "Flame1", "Explosion" };
        if (word.Length >= 5)
        {
            int i = UnityEngine.Random.Range((int)0, 2);
            _rewardAnimator.SetTrigger(s[i]);
        }
        */
    }

    public void ExtraAction()
    {
        NextScroll();
    }
}
