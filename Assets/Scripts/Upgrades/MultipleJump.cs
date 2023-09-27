using System.Collections;
using UnityEngine;

namespace WizardsPlatformer
{
    internal class MultipleJump : BasicJump
    {
        private int repetitions;
        private float _jumpForce = 5f;
        public MultipleJump(UpgradeConfig config) : base(config)
        {
            repetitions = (int)_config.Value;
        }

        protected override void OnActivation()
        {
            bool b = _jumper.AccessContacts().HasContactDown;

            if (repetitions > 0 && !b)
            {
                _jumper.Jump(_jumpForce);
                repetitions--;
                if (repetitions == 0) (_jumper as MonoBehaviour).StartCoroutine(RefreshRepetitions(10f));
                OnFinish?.Invoke(true);
            }
        }

        private IEnumerator RefreshRepetitions(float time)
        {
            yield return new WaitForSeconds(time);
            repetitions = (int)_config.Value;
        }
    }
}