﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListenerModule
{
    private List<GameObject> listeners = new List<GameObject>();


    public void AddListener(GameObject _listener)
    {
        if (!listeners.Contains(_listener))
            listeners.Add(_listener);
    }


    public void RemoveListener(GameObject _listener)
    {
        listeners.Remove(_listener);
    }


    public void NotifyListeners(string _method, object _parameter = null)
    {
        GarbageCollectListeners();

        foreach (GameObject listener in listeners)
        {
            listener.SendMessage(_method, _parameter, SendMessageOptions.DontRequireReceiver);
        }
    }


    public void RemoveAllListeners()
    {
        listeners.Clear();
    }


    void GarbageCollectListeners()
    {
        listeners.RemoveAll(item => item == null);
    }

}
