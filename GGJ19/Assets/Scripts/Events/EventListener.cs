using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField]
    private EventController _eventToListen;
    [SerializeField]
    private UnityEvent _response;

    private void OnEnable()
    {
        _eventToListen.StartListen(this);
    }

    private void OnDisable()
    {
        _eventToListen.StopListen(this);
    }

    public void Activate()
    {
        _response.Invoke();
    }
}
