using UnityEngine;
using System.Collections;

public class CellController : MonoBehaviour
{
    public int ZoneID;
    public CellController LeftCell;
    public CellController RightCell;
    public CellController TopCell;
    public CellController BottomCell;

    private bool _empty;

    public bool Empty
    {
        get
        {
            return _empty;
        }
        set
        {
            _empty = value;
            gameObject.SetActive(!_empty);
        }
    }

    public void Init(bool empty)
    {
        ZoneID = -1;

        Empty = empty;
    }
}
