using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWword : MonoBehaviour
{
    public enum Direction
    {
        horizontal,
        vertical
    }

    private string _word;
    private int _x;
    private int _y;
    private CWword.Direction _direction;


    public CWword(string word, int x, int y, Direction dir)
    {
        _x = x;
        _y = y;
        _direction = dir;
        _word = word;
    }

    public int x { get { return _x; }}
    public int y { get { return _y; } }
    public string word { get { return _word; }}
    public Direction direction { get { return _direction; }}
}
