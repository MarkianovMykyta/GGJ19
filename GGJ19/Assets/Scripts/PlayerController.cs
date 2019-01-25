using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private Transform _playerCamera;


    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");
        
        transform.Translate(new Vector3(xMove * _moveSpeed * Time.deltaTime, 0, zMove * _moveSpeed * Time.deltaTime), Space.Self);
    }

    private void Rotate()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            Vector3 lookDir = transform.position - new Vector3(_playerCamera.position.x, transform.position.y, _playerCamera.position.z);
            lookDir.Normalize();
            transform.forward = lookDir;
        }
    }
}
