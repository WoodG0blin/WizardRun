using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    {
        protected static SingletonMonobehaviour<T> instance;

        private void Awake()
        {
            if(instance == null)
            {
                var t = GameObject.FindObjectsOfType(typeof(T));
                foreach(T t2 in t)
                {
                    if (t2.GetType().ToString().Contains(t2.gameObject.name))
                    {
                        instance = t2;
                        break;
                    }
                }

                instance ??= this;
            }
            
            if (instance != this) GameObject.Destroy(this.gameObject);
            else OnStart();
        }

        protected virtual void OnStart() { }
    }
}
