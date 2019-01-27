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
    private GameObject _centerDecoration;
    [SerializeField]
    private GameObject _daedEndDecoration;
    [SerializeField]
    private GameObject _straightWayDecoration;
    [SerializeField]
    private GameObject _angleDecoration;
    [SerializeField]
    private GameObject _wallDecoration;

    [SerializeField]
    private GameObject _baseObj;

    public void SetMesh(bool top, bool bottom, bool left, bool right)
    {
        _baseObj.SetActive(false);

        if(top && bottom && left && right)
        {
            SetCenter(90f * Random.Range(0, 4));
            return;
        }

        if(top && bottom && !left && !right)
        {
            SetStraightWay(90f);
            return;
        }

        if(left && right && !top && !bottom)
        {
            SetStraightWay(0);
            return;
        }

        if(left && top && !right && !bottom)
        {
            SetAngle(0);
            return;
        }

        if(left && bottom && !top && !right)
        {
            SetAngle(270f);
            return;
        }

        if(right && top && !left && !bottom)
        {
            SetAngle(90f);
            return;
        }

        if(right && bottom && !top && !left)
        {
            SetAngle(180f);
            return;
        }

        if(top && bottom && left && !right)
        {
            SetWall(270f);
            return;
        }

        if(top && bottom && right && !left)
        {
            SetWall(90f);
            return;
        }

        if(left && right && bottom && !top)
        {
            SetWall(180f);
            return;
        }

        if(left && right && top && !bottom)
        {
            SetWall(0);
            return;
        }

        if (top)
        {
            SetDeadEnd(270f);
            return;
        }

        if (bottom)
        {
            SetDeadEnd(90f);
            return;
        }

        if (left)
        {
            SetDeadEnd(180f);
            return;
        }

        if (right)
        {
            SetDeadEnd(0);
            return;
        }
    }

    private void SetCenter(float angle)
    {
        _center.gameObject.SetActive(true);
        _center.Rotate(Vector3.forward, angle);
        _centerDecoration.SetActive(true);
    }

    private void SetDeadEnd(float angle)
    {
        _deadEnd.gameObject.SetActive(true);
        _deadEnd.Rotate(Vector3.forward, angle);
        _daedEndDecoration.SetActive(true);
    }

    private void SetStraightWay(float angle)
    {
        _straightWay.gameObject.SetActive(true);
        _straightWay.Rotate(Vector3.forward, angle);
        _straightWayDecoration.SetActive(true);
    }

    private void SetAngle(float angle)
    {
        _angle.gameObject.SetActive(true);
        _angle.Rotate(Vector3.forward, angle);
        _angleDecoration.SetActive(true);
    }

    private void SetWall(float angle)
    {
        _wall.gameObject.SetActive(true);
        _wall.Rotate(Vector3.forward, angle);
        _wallDecoration.SetActive(true);
    }
}
