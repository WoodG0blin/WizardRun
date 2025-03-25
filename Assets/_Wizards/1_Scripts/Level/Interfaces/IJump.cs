using UnityEngine;

namespace WizardsPlatformer
{
    public interface IJump
    {
        void Jump(float force);
        bool IsGrounded { get; }
        //IContactsPuller AccessContacts();
    }
}