using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pentagram
{
    private int _wordsCount;
    protected char[] _letters; public char[] Letters() { return _letters; }

    public int GetWordsCount ()
    {
        return _wordsCount;
    }

    public Pentagram (char[] letters, int wordsCount)
    {
        _wordsCount = wordsCount;
        _letters = letters;
    }
}
