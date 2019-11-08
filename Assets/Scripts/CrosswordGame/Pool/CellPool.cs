using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPool : MonoBehaviour
{
    [SerializeField]
    private int _itemsToInstantiate;
    [SerializeField]
    private GameObject _cellPrefab;
    private List<CWCell> _cells;

    // Start is called before the first frame update
    void Awake()
    {
        _cells = new List<CWCell>();
        for (int i = 0; i < _itemsToInstantiate; i++)
        {
            Debug.Log("Instantiating letter " + i);
            GameObject cell = Instantiate(_cellPrefab, transform);
            cell.GetComponent<CWCell>();
            cell.transform.localPosition = Vector3.zero;
            _cells.Add(cell.GetComponent<CWCell>());
        }
    }


    public CWCell GetCell()
    {
        CWCell cell = _cells[0];
        _cells.RemoveAt(0);
        return cell;
    }

    public void StoreCell (CWCell cell)
    {
        _cells.Add(cell);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = Vector3.zero;
    }
}
