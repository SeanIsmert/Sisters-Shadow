using System;
using UnityEngine;

public abstract class DatabaseElement : ScriptableObject
{
    [SerializeField] protected int _index;

    public int GetIndex { get { return _index; } }

    public void SetIndex(int i)
    {
        _index = i;
    }
}
