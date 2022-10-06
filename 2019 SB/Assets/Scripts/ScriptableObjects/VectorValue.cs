using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class VectorValue : ScriptableObject
{
    [Header("Value while game running")]
    public Vector2 initialValue;

    [Header("Default Value at game start")]
    public Vector2 defaultValue;

}
