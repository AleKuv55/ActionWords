using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClientEvent
{
    string      ConvertToJSon();
    ClientEvent ParseEventFromJSon(string json);
    void        OnEvent();
}
