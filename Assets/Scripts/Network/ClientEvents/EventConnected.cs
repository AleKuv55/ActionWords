using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConnected : ClientEvent
{

    public override string ConvertToJSon()
    {
        return "0";
    }
}
