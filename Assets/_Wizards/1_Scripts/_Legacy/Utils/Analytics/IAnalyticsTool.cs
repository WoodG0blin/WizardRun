using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface IAnalyticsTool
{
    void SendMessage(string message, Dictionary<string, object> eventData = null); 
}
