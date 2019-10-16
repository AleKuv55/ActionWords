using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Phrase
{
    private Text _phrase;
    private Sprite _phraseSpeaker;

    public Phrase(Text phrase, Sprite sprite)
    {
        _phrase = phrase;
        _phraseSpeaker = sprite;
    }

    public Text phrase { get { return _phrase; } }
    public Sprite sprite { get { return _phraseSpeaker; } }
}

