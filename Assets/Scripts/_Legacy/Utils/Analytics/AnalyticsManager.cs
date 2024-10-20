using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class AnalyticsManager : SingletonMonobehaviour<AnalyticsManager>
    {
        private IAnalyticsTool[] _analyticTools;

        public void OnLevelStart() => SendMessage("Level_started");
        public void OnAddsWatch() => SendMessage("Adds_watch_button_clicked");
        public void OnMenuEnter() => SendMessage("Menu_started");

        protected override void OnStart()
        {
            _analyticTools = new IAnalyticsTool[]
            {
                new UnityAnalytics()
            };
        }

        private void SendMessage(string message, Dictionary<string, object> eventData = null)
        {
            foreach(IAnalyticsTool tool in _analyticTools)
                tool.SendMessage(message, eventData);
        }
    }
}