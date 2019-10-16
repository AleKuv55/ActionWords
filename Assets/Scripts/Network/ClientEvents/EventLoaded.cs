using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLoaded : ClientEvent
{
    public override string ConvertToJSon()
    {
        return "1";
    }
}
