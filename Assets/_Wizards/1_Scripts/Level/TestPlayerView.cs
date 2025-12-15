using UnityEngine;

namespace WizardsPlatformer
{
    internal class TestPlayerView : MonoBehaviour
    {
        ViewMover mover;

        private void Start()
        {
            mover = new(transform, v => { });
        }

        private void Update()
        {
            mover.Update(Time.deltaTime);
        }
    }
}
