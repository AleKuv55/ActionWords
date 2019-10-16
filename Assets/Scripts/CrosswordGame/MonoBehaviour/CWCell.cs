using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CWCell : MonoBehaviour
{
    [SerializeField]
    private char _hiddenLetter;
    [SerializeField]
    private Text _text;


    public void OpenLetter ()
    {
        _text.text = _hiddenLetter.ToString().ToUpper();
    }

    public CWCell SetHiddenLetter (char hiddenLetter)
    {
        _hiddenLetter = hiddenLetter;
        return this;
    }

    public char GetHiddenLetter ()
    {
        return _hiddenLetter;
    }

    public CWCell SetSize (float size)
    {
        this.GetComponent<Transform>();
        var s = GetComponent<RectTransform>().sizeDelta;
        GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        return this;
    }

    public CWCell TakePlace (Transform newParent, Vector3 relativePosition)
    {
        RectTransform rt = GetComponent<RectTransform>();
        transform.SetParent(newParent);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
//        rt.anchoredPosition3D = relativePosition;

        transform.localPosition = relativePosition;
        return this;
    }

    /// <summary>
    /// Clears cell, preparing it to be stored to pool.
    /// </summary>
    public void Clear()
    {
        _hiddenLetter = ' ';
        _text.text = "";
    }
}
