using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _mouseSenc;
    [SerializeField]
    private Animator _animator;

    private Vector3 _lookTarget;

    private void LateUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        if(xMove == 0 && zMove == 0)
        {
            _animator.SetBool("Move", false);
        }
        else
        {
            _animator.SetBool("Move", true);
        }

        Vector3 moveDir = new Vector3(xMove, 0, zMove).normalized;

        transform.Translate(moveDir * _moveSpeed * Time.deltaTime, Space.Self);
    }

    private void Rotate()
    {
        float xMove = Input.GetAxis("Mouse X");
        float zMove = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, xMove * _mouseSenc * Time.deltaTime);
    }
}
