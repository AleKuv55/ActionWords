using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchCW : MonoBehaviour
{
    private Crossword _crossword;

    [SerializeField] private readonly string _wordsFilePath = "Assets/Resources/crossword.txt";

    public void Awake()
    {
        _crossword = new Crossword (_wordsFilePath);

        _crossword.GetCrosswordTable();

        //Dictionary<string, CWword>.Enumerator i = _crossword.CWdict.GetEnumerator();
        //CWword obj = null;
        //Debug.Log("SDFSDFDSFS" + (CWword)_crossword.CWdict.Values.word);
        //i.MoveNext();
        //Debug.Log(obj);

    }
}
/*
 * 
 * 
 * 
 * 
7x7
0 0 0 0 0 м 0 
ш и н а 0 и 0
0 0 и 0 0 н 0
м а ш и н а 0 
и 0 а 0 0 0 0
ш 0 0 0 0 0 0 
а 0 0 0 0 0 0 
*/ 
