using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private float _maxDistanceToAgent;
    [SerializeField]
    private float _minDistanceToAgent;
    [SerializeField]
    private NavMeshAgent _agentPrefab;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Animator _animator;

    private NavMeshAgent _agent;

    private float _targetSpeed;
    private float _currentSpeed;

    public bool Wait;

    private void OnEnable()
    {
        _agent = Instantiate(_agentPrefab, transform.position, Quaternion.identity);
    }

    public void SetTargetToFollow(Transform target)
    {
        _target = target;
        _agent.isStopped = false;
    }

    private void LateUpdate()
    {
        if (Wait) return;

        if (_rb.velocity.y < -0.1f) return;

        if (_target != null)
        {
            FollowAgent();
            Move();

            _animator.SetBool("Move", true);
        }
        else
        {
            _animator.SetBool("Move", false);
            StopFollowing();
        }
    }

    private void FollowAgent()
    {
        if (_agent.isOnNavMesh)
        {
            _agent.destination = _target.position;
            _agent.isStopped = false;
        }

        float distance = Vector3.Distance(transform.position, _agent.transform.position);

        if (distance > _maxDistanceToAgent)
        {
            ResetAgent();
        }
        else if (distance > _maxDistanceToAgent - 1f)
        {
            _agent.isStopped = true;
        }
        

        if (distance < _minDistanceToAgent)
        {
            _targetSpeed = 0;
        }
        else
        {
            _targetSpeed = _moveSpeed;
        }
    }

    private void StopFollowing()
    {
        _agent.isStopped = true;
    }

    public void ResetAgent()
    {
        _agent.updatePosition = false;
        _agent.isStopped = true;
        _agent.gameObject.SetActive(false);
        _agent.transform.position = transform.position;
        _agent.gameObject.SetActive(true);
        _agent.updatePosition = true;

        //Destroy(_agent.gameObject);
        //_agent = Instantiate(_agentPrefab, transform.position, Quaternion.identity);
    }

    private void Move()
    {
        transform.LookAt(new Vector3(_agent.transform.position.x, transform.position.y, _agent.transform.position.z));

        var moveDir = (new Vector3(_agent.transform.position.x, transform.position.y, _agent.transform.position.z) - transform.position).normalized;

        _currentSpeed = Mathf.SmoothStep(_currentSpeed, _targetSpeed, 0.3f);

        transform.Translate(moveDir * _currentSpeed * Time.deltaTime);
    }
}
