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
    private int _homeSize;

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

        yield return CreateZones();
        yield return FixZones();

        ConnectCollidingZones();

        bool success = AllZonesConnected();
        if (success)
        {
            foreach(var zone in _zones)
            {
                zone.SetColor();
            }
        }

        Debug.Log(success);
    }

    private IEnumerator CreateZones()
    {
        _zones = new List<ZoneController>();

        int idCounter = 0;

        ZoneController zone = new ZoneController();
        zone.ZoneID = idCounter;
        _zones.Add(zone);

        for (int i = _worldSize / 2 - _homeSize / 2; i <= _worldSize / 2 + _homeSize / 2; i++)
        {
            for (int j = _worldSize / 2 - _homeSize / 2; j <= _worldSize / 2 + _homeSize / 2; j++)
            {
                _cellsArr[i, j].ZoneID = idCounter;
                _cellsArr[i, j].Empty = false;

                zone.Cells.Add(_cellsArr[i, j]);
            }
        }

        idCounter++;

        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                CellController cell = _cellsArr[x, y];
                if (cell == null || cell.Empty || cell.ZoneID >= 0) continue;

                zone = new ZoneController();
                zone.ZoneID = idCounter;

                SetZone(idCounter, cell, zone, 0);

                zone.SetColor();

                _zones.Add(zone);

                idCounter++;

                yield return new WaitForEndOfFrame();
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

    private IEnumerator FixZones()
    {
        for (int i = _zones.Count - 1; i >= 0; i--)
        {
            if (_zones[i].Size < _minZoneSize)
            {
                _zones[i].DestroyZone();
                _zones.Remove(_zones[i]);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void ConnectCollidingZones()
    {
        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                CellController cell = _cellsArr[x, y];

                ConnectCells(cell, cell.RightCell);
                ConnectCells(cell, cell.LeftCell);
                ConnectCells(cell, cell.TopCell);
                ConnectCells(cell, cell.BottomCell);
            }
        }
    }

    private void ConnectCells(CellController cell1, CellController cell2)
    {
        if (cell1 != null && cell2 != null)
        {
            if (!cell1.Empty && !cell2.Empty)
            {
                if (cell1.ZoneID != cell2.ZoneID)
                {
                    var zone1 = _zones.Find(x => x.ZoneID == cell1.ZoneID);
                    var zone2 = _zones.Find(x => x.ZoneID == cell2.ZoneID);

                    if (zone1 == null || zone2 == null) return;
                    if (zone1.Neighbors.Contains(zone2)) return;

                    zone1.Neighbors.Add(zone2);
                    zone2.Neighbors.Add(zone1);
                }
            }
        }
    }

    private bool AllZonesConnected()
    {
        List<int> ids = new List<int>();
        var zone = _zones[0];

        CheckZone(zone, ids);

        int counter = 100 * _worldSize;
        while (ids.Count != _zones.Count)
        {
            FindNewConnection(ids);

            ids = new List<int>();
            CheckZone(zone, ids);
            ConnectCollidingZones();

            if (--counter == 0)
            {
                return false;
            }
        }

        return true;
    }

    private void CheckZone(ZoneController zone, List<int> ids)
    {
        ids.Add(zone.ZoneID);

        foreach (var connectedZone in zone.Neighbors)
        {
            if (ids.Contains(connectedZone.ZoneID)) continue;

            CheckZone(connectedZone, ids);
        }
    }

    private void FindNewConnection(List<int> ids)
    {
        foreach (var id in ids)
        {
            var zone = _zones.Find(z => z.ZoneID == id);
            SetFirstTempValues(zone);
        }

        var newCell = SearchNewCell(ids);

        if (newCell != null)
        {
            var newZone = _zones.Find(z => z.ZoneID == newCell.ZoneID);

            CreateConnection(newCell, newZone);
        }
    }

    private void SetFirstTempValues(ZoneController zone)
    {
        foreach (var cell in zone.Cells)
        {
            if (cell.LeftCell != null && cell.LeftCell.Empty)
            {
                if (cell.tempValue < 0)
                {
                    cell.LeftCell.tempValue = 0;
                }
            }
            if (cell.RightCell != null && cell.RightCell.Empty)
            {
                if (cell.tempValue < 0)
                {
                    cell.RightCell.tempValue = 0;
                }
            }
            if (cell.TopCell != null && cell.TopCell.Empty)
            {
                if (cell.tempValue < 0)
                {
                    cell.TopCell.tempValue = 0;
                }
            }
            if (cell.BottomCell != null && cell.BottomCell.Empty)
            {
                if (cell.tempValue < 0)
                {
                    cell.BottomCell.tempValue = 0;
                }
            }
        }
    }

    private CellController SearchNewCell(List<int> ids)
    {
        for (int i = 0; i < 100 * _worldSize; i++)
        {
            for (int x = 0; x < _worldSize; x++)
            {
                for (int y = 0; y < _worldSize; y++)
                {
                    var cell = _cellsArr[x, y];

                    if (cell.tempValue >= 0)
                    {
                        var targetCell = CheckNearCell(cell.LeftCell, cell, ids);
                        if (targetCell != null) return targetCell;

                        targetCell = CheckNearCell(cell.RightCell, cell, ids);
                        if (targetCell != null) return targetCell;

                        targetCell = CheckNearCell(cell.TopCell, cell, ids);
                        if (targetCell != null) return targetCell;

                        targetCell = CheckNearCell(cell.BottomCell, cell, ids);
                        if (targetCell != null) return targetCell;
                    }
                }
            }
        }

        return null;
    }

    private CellController CheckNearCell(CellController nearCell, CellController cell, List<int> ids)
    {
        if (nearCell != null)
        {
            if (nearCell.Empty)
            {
                if (nearCell.tempValue < 0)
                {
                    nearCell.tempValue = cell.tempValue + 1;
                }
            }
            else
            {
                if (!ids.Contains(nearCell.ZoneID))
                {
                    nearCell.tempValue = cell.tempValue + 1;
                    return nearCell;
                }
            }
        }

        return null;
    }

    private void ClearTempValues()
    {
        for (int x = 0; x < _worldSize; x++)
        {
            for (int y = 0; y < _worldSize; y++)
            {
                _cellsArr[x, y].tempValue = -1;
            }
        }
    }

    private void CreateConnection(CellController cell, ZoneController zone)
    {
        for (int i = 0; i < 100 * _worldSize; i++)
        {
            if (cell.LeftCell != null)
            {
                if (cell.LeftCell.tempValue >= 0)
                {
                    if (cell.tempValue > cell.LeftCell.tempValue)
                    {
                        cell = cell.LeftCell;
                        cell.Empty = false;
                        cell.ZoneID = zone.ZoneID;
                        zone.Cells.Add(cell);

                        if (cell.tempValue == 0)
                        {
                            return;
                        }
                    }
                }
            }
            if (cell.RightCell != null)
            {
                if (cell.RightCell.tempValue >= 0)
                {
                    if (cell.tempValue > cell.RightCell.tempValue)
                    {
                        cell = cell.RightCell;
                        cell.Empty = false;
                        cell.ZoneID = zone.ZoneID;
                        zone.Cells.Add(cell);

                        if (cell.tempValue == 0)
                        {
                            return;
                        }
                    }
                }
            }
            if (cell.TopCell != null)
            {
                if (cell.TopCell.tempValue >= 0)
                {
                    if (cell.tempValue > cell.TopCell.tempValue)
                    {
                        cell = cell.TopCell;
                        cell.Empty = false;
                        cell.ZoneID = zone.ZoneID;
                        zone.Cells.Add(cell);

                        if (cell.tempValue == 0)
                        {
                            return;
                        }
                    }
                }
            }
            if (cell.BottomCell != null)
            {
                if (cell.BottomCell.tempValue >= 0)
                {
                    if (cell.tempValue > cell.BottomCell.tempValue)
                    {
                        cell = cell.BottomCell;
                        cell.Empty = false;
                        cell.ZoneID = zone.ZoneID;
                        zone.Cells.Add(cell);

                        if (cell.tempValue == 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        ClearTempValues();
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
