using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private int _worldSize;
    [SerializeField]
    private float _cellSize;
    [SerializeField]
    private int _maxZoneSize;
    [SerializeField]
    private int _minZoneSize;

    [SerializeField]
    private CellController _cellTemplate;

    private CellController[,] _cellsArr;
    private List<ZoneController> _zones;

    [ContextMenu("Generate World")]
    public void GenerateWorld()
    {
        if (_cellsArr != null)
        {
            ClearWowrld();
        }

        _cellsArr = new CellController[_worldSize, _worldSize];

        StartCoroutine(GeneratorRoutine());
    }

    private IEnumerator GeneratorRoutine()
    {
        Vector3 spawnPos = Vector3.zero;
        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                spawnPos.x = x * _cellSize;
                spawnPos.z = y * _cellSize;

                _cellsArr[x, y] = Instantiate(_cellTemplate, spawnPos, Quaternion.identity, _cellTemplate.transform.parent);
                _cellsArr[x, y].Init(Random.value > 0.5f);

                if (x > 0)
                {
                    if (_cellsArr[x - 1, y] != null)
                    {
                        _cellsArr[x, y].LeftCell = _cellsArr[x - 1, y];
                        _cellsArr[x - 1, y].RightCell = _cellsArr[x, y];
                    }

                }
                if (y > 0)
                {
                    if (_cellsArr[x, y - 1] != null)
                    {
                        _cellsArr[x, y].BottomCell = _cellsArr[x, y - 1];
                        _cellsArr[x, y - 1].TopCell = _cellsArr[x, y];
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }

        CreateZones();
        FixZones();
        ConnectZones();
    }

    private void CreateZones()
    {
        _zones = new List<ZoneController>();

        int idCounter = 0;
        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                CellController cell = _cellsArr[x, y];
                if (cell == null || cell.Empty || cell.ZoneID >= 0) continue;

                ZoneController zone = new ZoneController();
                zone.ZoneID = idCounter;

                SetZone(idCounter, cell, zone, 0);

                zone.SetColor();

                _zones.Add(zone);

                idCounter++;
            }
        }
    }

    private void SetZone(int zoneID, CellController cell, ZoneController zone, int currentSize)
    {
        cell.ZoneID = zoneID;
        zone.Cells.Add(cell);

        currentSize++;
        if (currentSize > _maxZoneSize) return;

        if (cell.LeftCell != null && cell.LeftCell.ZoneID < 0 && !cell.LeftCell.Empty)
        {
            SetZone(zoneID, cell.LeftCell, zone, currentSize);
        }
        if (cell.RightCell != null && cell.RightCell.ZoneID < 0 && !cell.RightCell.Empty)
        {
            SetZone(zoneID, cell.RightCell, zone, currentSize);
        }
        if (cell.TopCell != null && cell.TopCell.ZoneID < 0 && !cell.TopCell.Empty)
        {
            SetZone(zoneID, cell.TopCell, zone, currentSize);
        }
        if (cell.BottomCell != null && cell.BottomCell.ZoneID < 0 && !cell.BottomCell.Empty)
        {
            SetZone(zoneID, cell.BottomCell, zone, currentSize);
        }
    }

    private void FixZones()
    {
        for (int i = _zones.Count - 1; i >= 0; i--)
        {
            if (_zones[i].Size < _minZoneSize)
            {
                _zones[i].DestroyZone();
                _zones.Remove(_zones[i]);
            }
        }
    }

    private void ConnectZones()
    {
        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                CellController cell = _cellsArr[x, y];
                if (!cell.Empty)
                {
                    if(cell.RightCell != null && cell.ZoneID != cell.RightCell.ZoneID)
                    {
                        //var zone1 = _zones.Find(zx => x.ZoneID == cell1.ZoneID);
                        //var zone2 = _zones.Find(x => x.ZoneID == cell2.ZoneID);

                        //if (zone1 == null || zone2 == null) return false;
                        //if (zone1.Neighbors.Contains(zone2)) return false;

                        //zone1.Neighbors.Add(zone2);
                        //zone2.Neighbors.Add(zone1);
                    }
                }
            }
        }

        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                CellController cell = _cellsArr[x, y];
                if (cell.Empty)
                {
                    if (ConnectCells(cell.RightCell, cell.LeftCell))
                    {
                        cell.Empty = false;
                        continue;
                    }
                    if (ConnectCells(cell.RightCell, cell.TopCell))
                    {
                        cell.Empty = false;
                        continue;
                    }
                    if (ConnectCells(cell.RightCell, cell.BottomCell))
                    {
                        cell.Empty = false;
                        continue;
                    }
                    if (ConnectCells(cell.LeftCell, cell.TopCell))
                    {
                        cell.Empty = false;
                        continue;
                    }
                    if (ConnectCells(cell.LeftCell, cell.BottomCell))
                    {
                        cell.Empty = false;
                        continue;
                    }
                    if (ConnectCells(cell.TopCell, cell.BottomCell))
                    {
                        cell.Empty = false;
                        continue;
                    }
                }
            }
        }
    }

    private bool ConnectCells(CellController cell1, CellController cell2)
    {
        if (cell1 != null && cell2 != null)
        {
            if (!cell1.Empty && !cell2.Empty)
            {
                if (cell1.ZoneID != cell2.ZoneID)
                {
                    var zone1 = _zones.Find(x => x.ZoneID == cell1.ZoneID);
                    var zone2 = _zones.Find(x => x.ZoneID == cell2.ZoneID);

                    if (zone1 == null || zone2 == null) return false;
                    if (zone1.Neighbors.Contains(zone2)) return false;

                    zone1.Neighbors.Add(zone2);
                    zone2.Neighbors.Add(zone1);

                    return true;
                }
            }
        }

        return false;
    }

    private void ClearWowrld()
    {
        for (int i = 0; i < _cellsArr.GetLength(0); i++)
        {
            for (int j = 0; j < _cellsArr.GetLength(1); j++)
            {
                Destroy(_cellsArr[i, j].gameObject);
            }
        }

        _cellsArr = null;
    }
}
