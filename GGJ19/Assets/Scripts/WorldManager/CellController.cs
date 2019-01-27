using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CellController : MonoBehaviour
{
    public int ZoneID;
    public CellController LeftCell;
    public CellController RightCell;
    public CellController TopCell;
    public CellController BottomCell;

    [SerializeField]
    private NavMeshSurface _navMechSurface;
    [SerializeField]
    private Renderer _renderer;
    [SerializeField]
    private CellMeshSelector _meshSelector;
    
    public int tempValue;

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

            if (_empty)
            {
                ZoneID = -1;
            }
        }
    }

    public void Init(bool empty)
    {
        ZoneID = -1;
        tempValue = -1;
        Empty = empty;
    }

    [ContextMenu("Generate NavMesh")]
    private void GenerateNavMesh()
    {
        _navMechSurface.BuildNavMesh();
    }

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }

    public void SetupContent()
    {
        bool top = TopCell != null && !TopCell.Empty;
        bool bottom = BottomCell != null && !BottomCell.Empty;
        bool left = LeftCell != null && !LeftCell.Empty;
        bool right = RightCell != null && !RightCell.Empty;

        _meshSelector.SetMesh(top, bottom, left, right);

        _navMechSurface.BuildNavMesh();
    }
}
