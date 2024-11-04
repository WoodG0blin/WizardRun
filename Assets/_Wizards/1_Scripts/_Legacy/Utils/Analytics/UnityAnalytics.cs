using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class UnityAnalytics : IAnalyticsTool
    {
        public void SendMessage(string message, Dictionary<string, object> eventData = null)
        {
            if(eventData != null) Analytics.CustomEvent(message, eventData);
            else Analytics.CustomEvent(message);

            Debug.Log("Message sent to UnityAnalytics: " + message);
        }
    }
}
