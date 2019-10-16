using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    public List<Phrase> dialog;

    public Phrase GetNextPhrase()
    {
        var numerator =  dialog.GetEnumerator();
        numerator.MoveNext();
        return numerator.Current;
    }

    public void AddPhrase(Phrase phrase)
    {
        dialog.Add(phrase);
    }
}
