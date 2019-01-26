using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventController : ScriptableObject, ISerializationCallbackReceiver
{
    private List<EventListener> _listenersList;
    
    public void StartListen(EventListener listener)
    {
        _listenersList.Add(listener);
    }

    public void StopListen(EventListener listener)
    {
        _listenersList.Add(listener);
    }

    public void Activate()
    {
        for (int i = _listenersList.Count-1; i >= 0; i--)
        {
            _listenersList[i].Activate();
        }
    }

    public void OnBeforeSerialize()
    {
        _listenersList = null;
    }

    public void OnAfterDeserialize()
    {
        _listenersList = new List<EventListener>();
    }
}
