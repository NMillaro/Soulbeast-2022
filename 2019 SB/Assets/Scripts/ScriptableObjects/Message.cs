using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Message : ScriptableObject
{
    public List<MessageListener> listeners = new List<MessageListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >=0; i--)
        {
            listeners[i].OnMessageRaised();
        }
    }

    public void RegisterListener(MessageListener message)
    {
        listeners.Add(message);
    }

    public void DeRegisterListener(MessageListener message)
    {
        listeners.Remove(message);
    }
}
