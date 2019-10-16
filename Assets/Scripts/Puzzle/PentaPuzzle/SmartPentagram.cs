using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPentagram : Pentagram
{
    private   List<string>  _words;    public List<string> GetSelectableWords() { return _words; }

    public SmartPentagram(char[] letters, string[] words) : base(letters, words.Length)
    {
        Debug.Log("SmartPentagram constructor:");
        _words = new List<string>();
        if (words != null)
        {
            Debug.Log("Letters: " + letters.ToString());
            Debug.Log("Words: " + words.Length + " in total.");
            foreach (string word in words)
            {
                Debug.Log("Adding word " + word);
                _words.Add(word.ToUpper());
            }
            _words.Sort();
        }
    }


    public bool TryToUseWord(string word)
    {
        if (_words != null)
        {
            Debug.Log("Checking for word " + word);
            return _words.Remove(word.ToUpper());
        }
        else
        {
            return true;
        }
    }
}
