using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private WorldManager _worldManager;
    [SerializeField]
    private Player _player;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _worldManager.GenerateWorld();
    }
}
