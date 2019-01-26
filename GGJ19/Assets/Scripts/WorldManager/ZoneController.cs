using UnityEngine;
using System.Collections.Generic;

public class ZoneController
{
    public int ZoneID;
    public List<CellController> Cells;
    public List<ZoneController> Neighbors;

    private Color _color;

    public ZoneController()
    {
        Cells = new List<CellController>();
        Neighbors = new List<ZoneController>();

        _color = Color.white;
    }
    
    public int Size
    {
        get
        {
            return Cells.Count;
        }
    }

    public void DestroyZone()
    {
        foreach(var cell in Cells)
        {
            cell.Empty = true;
        }
        Cells = null;
    }


    public void SetColor()
    {
        if(_color == Color.white)
        {
            _color = new Color(Random.value, Random.value, Random.value);
        }
        
        foreach (var cell in Cells)
        {
            cell.SetColor(_color);
        }
    }
}
