using System;

namespace WizardsPlatformer
{
    internal interface IAdsShower
    {
        event Action Started;
        event Action Finished;
        event Action Failed;
        event Action Skipped;
        event Action BecomeReady;

        void Play();
    }
}
