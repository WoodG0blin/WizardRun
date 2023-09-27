using UnityEngine;

namespace WizardsPlatformer
{
    internal class DontDestroyOnLoad : MonoBehaviour
    {
        void Start()
        {
            if (enabled) DontDestroyOnLoad(gameObject);
        }

    }
}
