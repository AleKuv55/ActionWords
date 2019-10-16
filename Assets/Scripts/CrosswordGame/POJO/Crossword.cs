using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class Crossword : ICrossword
{
    public Dictionary<string, CWword> CWdict;

    public CWword[] arrayCWwords;

    private char[,]     _crosswordTable;
    private char[]      _letters;
    private string[]    _words;


    private class StringComparer : IEqualityComparer<String>
    {
        public bool Equals(string x, string y)
        {
            Debug.Log("StringComparer!");
            Debug.Log("X: " + x); Debug.Log("Y: " + y);
            return x.ToUpper().Equals(y.ToUpper());
        }
        public int GetHashCode(string obj)
        {
            return obj.Length;
        }
    }

    public Crossword (char[] letters, string[] words, CWword.Direction[] dirs, int[] x, int[] y)
    {
        _letters = letters;
        _words = words;

        CWdict = new Dictionary<string, CWword>(new StringComparer());
        for (int i = 0; i < words.Length; i++)
        {
            CWword word = new CWword(words[i], x[i], y[i], dirs[i]);
            CWdict.Add(words[i], word);
        }
    }
    
    public Crossword(string filepath)
    {
        _crosswordTable = new char[7, 7];
        _words = new string[10];
        CWdict = new Dictionary<string, CWword>();

        CWLoad(filepath);
    }

    public Crossword(int x, int y)
    {
        _crosswordTable = new char[x, y];
        CWdict = new Dictionary<string, CWword>(new StringComparer());
    }

    private int CWLoad(string filepath)
    {

        System.IO.StreamReader MyReader = new System.IO.StreamReader(filepath);
        int i = 0; int j = 0; int k = 0; string durword; string allText;
        while ((allText = MyReader.ReadLine()) != null)
        {
            string durstring = allText;
            durstring = Regex.Replace(durstring, @" ", "");
            Debug.Log(durstring);

            if (i % 4 == 1) //1 string - horizontal words
            {
                FillCWdict(durstring, CWword.Direction.horizontal);
            }
            else if (i % 4 == 3) // 3 string - vertical words
            {
                FillCWdict(durstring, CWword.Direction.vertical);
            }
            else if (i % 4 == 0 && durstring[0] != 'h') // last string - all letters
            {
                FillLetters(durstring);
            }
            i++;
        }

        MyReader.Close();
        return 1;
    }

    public int CWLoad1(System.IO.StreamReader MyReader)
    {
        int i = 0; string allText;

        while (!(allText = MyReader.ReadLine()).Equals("FINITE"))
        {
            Debug.Log(allText);
            string durstring = allText;
            durstring = Regex.Replace(durstring, @" ", "");
            switch (i % 5)
            {
                case 1:
                    FillCWdict(durstring, CWword.Direction.horizontal);
                    break;
                case 3:
                    FillCWdict(durstring, CWword.Direction.vertical);
                    break;
                case 4:
                    FillLetters(durstring);
                    break;
            }

            i++;
        }

        return 1;
    }

    private void FillCWdict(string str, CWword.Direction dir)
    {
        int j = 0; int x = 0; int y = 0; int k = 0; 
        while (str[k] != '!')
        {
            StringBuilder word = new StringBuilder();
            while (str[j] != '(')
            {
                word.Append(str[j]);
                j++;
            }
            x = str[j + 1] - '0';
            y = str[j + 3] - '0';
            j += 5;

            Debug.Log("word = " + word + " x =" + x + " y =" + y);
            CWword CWword = new CWword(word.ToString(), x, y, dir);

            CWdict.Add(word.ToString(), CWword);
            k = j;

            FillCrosswordTable(CWword);

            //Dictionary<string, CWword>.Enumerator i = CWdict.GetEnumerator();
            //CWword obj = null;
            //Debug.Log("SDFSDFDSFS" + ((CWword)CWdict.Values).word);
            //i.MoveNext();
        }
    }

    private void FillCrosswordTable(CWword cWword)
    {
        int i = 0;
        for (i = 0; i < cWword.word.Length; i++)
        {
            if (cWword.direction == CWword.Direction.vertical)
                _crosswordTable[cWword.x + i, cWword.y] = cWword.word[i];
            else if(cWword.direction == CWword.Direction.horizontal)
                _crosswordTable[cWword.x, cWword.y + i] = cWword.word[i];

        }
    }
    private void PrintfCT()
    {
        for (int i = 0; i < _crosswordTable.GetLength(0); i++)
            for (int j = 0; j < _crosswordTable.GetLength(1); j++)
                    Debug.Log(_crosswordTable[i, j]);

    }

    public char[,] GetCrosswordTable()
    {
        PrintfCT();
        return _crosswordTable;
    }

    /*
 * Put in letters all symbols from durstring  
*/
    private void FillLetters(string durstring)
    {
        Debug.Log("FillLetters");
        _letters = durstring.ToCharArray();
    }

    public char[] GetLetters()
    {
        return _letters;
    }

    public CWword GetCWword(string word)
    {
        Debug.Log("Check word " + word);
        var en = CWdict.Keys.GetEnumerator();
        while (en.MoveNext())
        {
            Debug.Log(en.Current + "_");
            Debug.Log(word.ToUpper().Equals(en.Current.ToUpper()));
        }
        return CWdict[word];
    }

    public string[] GetWords()
    {
        if (null != _words) return _words;

        var enumerator = CWdict.Values.GetEnumerator();
        int n = CWdict.Count;
        _words = new string[n];

        for (int i = 0; i < n; i++)
        {
            enumerator.MoveNext();
            _words[i] = enumerator.Current.word;
        }
        return _words;
    }

    public static Crossword[] LoadCrosswords(string filepath)
    {
        int n = 0; int x_size; int y_size;
        System.IO.StreamReader MyReader = new System.IO.StreamReader(filepath);
        n = MyReader.ReadLine()[0] - '0';
        Debug.Log("NNNNNNN" + n);

        Crossword[] a = new Crossword[n];
        for (int i = 0; i < n; i++)
        {
            string size = MyReader.ReadLine();
            x_size = size[0] - '0';
            y_size = size[2] - '0';

            a[i] = new Crossword(x_size, y_size);
            a[i].CWLoad1(MyReader);
        }

        return a;
    }
}

/* 7х7
 * 5, н, и, ш, м, а, ниша, шина, мина, миша, машина, наши
 * 
 * 0 0 0 0 0 м 0 
 * ш и н а 0 и 0
 * 0 0 и 0 0 н 0
 * м а ш и н а 0 
 * и 0 а 0 0 0 0
 * ш 0 0 0 0 0 0 
 * а 0 0 0 0 0 0 
 * 
 */


            /*
        System.IO.StreamReader MyReader = new System.IO.StreamReader(filepath);
        string firststr = MyReader.ReadLine();

        int x = firststr[0] - '0';
        int y = firststr[2] - '0';
        Debug.Log("x =" + x + "y =" + y);
        crosswordTable = new char[x + 1, y + 1];

        int i = 0; int j = 0;
        while ((allText = MyReader.ReadLine()) != null)
        {
            string durstring = allText;
            durstring = Regex.Replace(durstring, @" ", "");
            Debug.Log(durstring);

            for (j = 0; j < y; j++)
            {
                crosswordTable[x, j] = durstring[j];
            }
            i++;
        }*/