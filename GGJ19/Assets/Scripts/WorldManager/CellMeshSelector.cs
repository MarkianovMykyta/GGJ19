using UnityEngine;
using System.Collections;

public class CellMeshSelector : MonoBehaviour
{
    [SerializeField]
    private Transform _center;
    [SerializeField]
    private Transform _deadEnd;
    [SerializeField]
    private Transform _straightWay;
    [SerializeField]
    private Transform _angle;
    [SerializeField]
    private Transform _wall;

    [SerializeField]
    private GameObject _baseObj;

    public void SetMesh(bool top, bool bottom, bool left, bool right)
    {
        _baseObj.SetActive(false);

        if(top && bottom && left && right)
        {
            _center.gameObject.SetActive(true);
            return;
        }

        if(top && bottom && !left && !right)
        {
            _straightWay.gameObject.SetActive(true);
            _straightWay.Rotate(Vector3.forward, 90f);
            return;
        }

        if(left && right && !top && !bottom)
        {
            _straightWay.gameObject.SetActive(true);
            return;
        }

        if(left && top && !right && !bottom)
        {
            _angle.gameObject.SetActive(true);
            return;
        }

        if(left && bottom && !top && !right)
        {
            _angle.gameObject.SetActive(true);
            _angle.Rotate(Vector3.forward, 270f);
            return;
        }

        if(right && top && !left && !bottom)
        {
            _angle.gameObject.SetActive(true);
            _angle.Rotate(Vector3.forward, 90f);
            return;
        }

        if(right && bottom && !top && !left)
        {
            _angle.gameObject.SetActive(true);
            _angle.Rotate(Vector3.forward, 180f);
            return;
        }

        if(top && bottom && left && !right)
        {
            _wall.gameObject.SetActive(true);
            _wall.Rotate(Vector3.forward, 270f);
            return;
        }

        if(top && bottom && right && !left)
        {
            _wall.gameObject.SetActive(true);
            _wall.Rotate(Vector3.forward, 90f);
            return;
        }

        if(left && right && bottom && !top)
        {
            _wall.gameObject.SetActive(true);
            _wall.Rotate(Vector3.forward, 180f);
            return;
        }

        if(left && right && top && !bottom)
        {
            _wall.gameObject.SetActive(true);
            return;
        }

        if (top)
        {
            _deadEnd.gameObject.SetActive(true);
            _deadEnd.Rotate(Vector3.forward, 270f);
            return;
        }

        if (bottom)
        {
            _deadEnd.gameObject.SetActive(true);
            _deadEnd.Rotate(Vector3.forward, 90f);
            return;
        }

        if (left)
        {
            _deadEnd.gameObject.SetActive(true);
            _deadEnd.Rotate(Vector3.forward, 180f);
            return;
        }

        if (right)
        {
            _deadEnd.gameObject.SetActive(true);
            
            return;
        }
    }
}
