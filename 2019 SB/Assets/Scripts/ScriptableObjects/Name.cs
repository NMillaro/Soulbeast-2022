using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class Name : ScriptableObject
{
    [Header("Value while game running")]
    public string initialName;

    [Header("Default Value at game start")]
    public string defaultName;

}
