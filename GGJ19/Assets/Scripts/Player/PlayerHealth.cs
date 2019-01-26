using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private EventController _playerDeadEvent;
    [SerializeField]
    private GameObject[] _healthPointObjects;
    [SerializeField]
    private float _blowRadius;
    [SerializeField]
    private float _blowForce;
    [SerializeField]
    private float _cooldownTimer;

    [SerializeField]
    private int _hp;

    private bool _immortal = false;

    public int HP
    {
        get
        {
            return _hp;
        }
    }

    public void Damage()
    {
        if (_immortal) return;

        _immortal = true;

        _hp--;

        if (HP < 0)
        {
            _playerDeadEvent.Activate();
        }
        else
        {
            _healthPointObjects[_hp].SetActive(false);

            var colliders = Physics.OverlapSphere(transform.position, _blowRadius);

            foreach (var col in colliders)
            {
                if (col.CompareTag("Player")) continue;

                var rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 blowDir = (col.transform.position - transform.position).normalized;
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    distance = 1f - (distance / (_blowRadius / 100f)) / 100f;
                    
                    rb.AddForce(blowDir * _blowForce * distance, ForceMode.Acceleration);
                }
            }

            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(_cooldownTimer);

        _immortal = false;
    }
}
