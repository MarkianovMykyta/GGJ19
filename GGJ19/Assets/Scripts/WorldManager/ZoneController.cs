using UnityEngine;
using System.Collections.Generic;

public class ZoneController
{
    public int ZoneID;
    public List<CellController> Cells;
    public List<ZoneController> Neighbors;

    public ZoneController()
    {
        Cells = new List<CellController>();
        Neighbors = new List<ZoneController>();
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
            GameObject.Destroy(cell.gameObject);
        }
        Cells = null;
    }


    public void SetColor()
    {
        Color color = new Color(Random.value, Random.value, Random.value);
        foreach (var cell in Cells)
        {
            cell.GetComponent<Renderer>().material.color = color;
        }
    }
}
