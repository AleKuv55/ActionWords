using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientEvent : IClientEvent
{
    private static Action _eventCallback;

    public virtual string ConvertToJSon()
    {
        throw new System.NotImplementedException();
    }

    public void AddEventCallback(Action callback)
    {
        _eventCallback += callback;
    }

    public void OnEvent()
    {
        _eventCallback();
    }

    public ClientEvent ParseEventFromJSon(string json)
    {
        if (json[0] == '1')
            return new EventLoaded();

        if (json[0] == '0')
            return new EventConnected();

        if (json[0] == '2')
            return new EventWord();

        return null;
    }
}
