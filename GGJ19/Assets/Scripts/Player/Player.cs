using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;
    [SerializeField]
    private Home _home;

    private bool _ready = false;

    public bool Ready
    {
        get
        {
            return _ready;
        }
        set
        {
            _ready = value;
            gameObject.SetActive(_ready);

            if (_ready)
            {
                Spawn(_home.Position);
            }
        }
    }

    public void Spawn(Vector3 spawnPos)
    {
        transform.position = spawnPos;
    }
}
