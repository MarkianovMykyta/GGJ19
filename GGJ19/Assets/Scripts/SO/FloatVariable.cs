using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private float InitValue;

    [NonSerialized]
    public float Value;

    public void OnAfterDeserialize()
    {
        Value = InitValue;
    }

    public void OnBeforeSerialize() { }
}
