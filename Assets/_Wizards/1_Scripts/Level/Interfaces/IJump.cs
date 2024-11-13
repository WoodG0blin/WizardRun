using UnityEngine;

namespace WizardsPlatformer
{
    public interface IJump
    {
        void Jump(float force);
        IContactsPuller AccessContacts();
    }
}