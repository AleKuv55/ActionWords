using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswordGrid : MonoBehaviour
{
    private char _emptySymbol;
    private float _unitLength;
    private char[,] _scheme;
    private CWCell[,] _cells;
    private RectTransform rectTransform;

    [SerializeField]
    private float CELL_SIZE_ON_SPACE = 4;
    [SerializeField]
    private CellPool _cellPool;

    /// <summary>
    /// Gets <b>rect</b> of RectTransform to set the scaling parameter of CWCells, so that
    /// the Cells can fit both width and height of the space allowed by CrosswordGrid size.
    /// </summary>
    /// <param name="n">Number of cells vertically</param>
    /// <param name="m">Number of cells horizontally</param>
    /// <returns></returns>
    private void CalculateUnitLength(int n, int m)
    {
        float height = rectTransform.rect.height;
        float width = rectTransform.rect.width;

        float horizUnitSize = width  / (m * (CELL_SIZE_ON_SPACE + 1) - 1);
        float vertiUnitSize = height / (n * (CELL_SIZE_ON_SPACE + 1) - 1);

        _unitLength = (horizUnitSize < vertiUnitSize) ? horizUnitSize : vertiUnitSize;
    }

    internal void ReturnCellsToPool()
    {
        if (null == _cells) { return; }

        foreach (CWCell cell in _cells)
        {
            if (null == cell) continue;
            cell.Clear();
            _cellPool.StoreCell(cell);
        }
        _cells = null;
    }

    /// <summary>
    /// Gets necessary ammount of CWCells from pool to accuratly place them on scene,
    /// creating a crossvord field.
    /// </summary>
    /// <param name="scheme">The scheme of a crossword</param>
    public void BuildCrossword (char[,] scheme, char emptySymbol)
    {
        _scheme = scheme;
        _emptySymbol = emptySymbol;
        int n = scheme.GetUpperBound(0) + 1;
        int m = scheme.GetUpperBound(1) + 1;

        CalculateUnitLength(n, m);
        _cells = new CWCell[n, m];

        float vertiOffset  = +_unitLength * (n - 1) * (CELL_SIZE_ON_SPACE + 1) / 2;
        float horizOffset0 = -_unitLength * (m - 1) * (CELL_SIZE_ON_SPACE + 1) / 2;
        float horizOffset  = horizOffset0;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (scheme[i,j] != _emptySymbol) {
                    CWCell cell = _cellPool.GetCell();
                    cell.SetHiddenLetter(scheme[i, j])
                        .SetSize(_unitLength * CELL_SIZE_ON_SPACE)
                        .TakePlace(transform, relativePosition: new Vector3(horizOffset,
                                                                            vertiOffset));
                    _cells[i, j] = cell;

                    char a = cell.GetHiddenLetter();
                    Debug.Log(i + ", " + j + ": " + a);
                }
                horizOffset += _unitLength * (CELL_SIZE_ON_SPACE + 1);
            }
            vertiOffset -= _unitLength * (CELL_SIZE_ON_SPACE + 1);
            horizOffset = horizOffset0;
        }
    }


    /// <summary>
    /// Opens the word starting from [x,y] cell to the end.
    /// </summary>
    /// <param name="x">X of the first cell of a word in a grid.</param>
    /// <param name="y">Y of the first cell of a word in a grid.</param>
    /// <param name="direction">Tells whether to open vertically or horizontally.</param>
    public void OpenWord(int x, int y, CWword.Direction direction)
    {
        Debug.Log("Opening word at " + x + ", " + y);
        CWCell nextCell = _cells[x, y];
        while (nextCell != null)
        {
            nextCell.OpenLetter();
            if (direction == CWword.Direction.vertical) {
                if (++x > _cells.GetUpperBound(0)) { break; }
            } else {
                if (++y > _cells.GetUpperBound(1)) { break; }
            }
            nextCell = _cells[x, y];
            Debug.Log("Next cell (" + x + ", " + y + ") is null: " + (nextCell == null));
            if (null != nextCell)
                Debug.Log("Letter is " + nextCell.GetHiddenLetter());
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
