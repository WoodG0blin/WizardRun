using System;
using UnityEngine.UI;

namespace WizardsPlatformer
{
    internal static class ButtonTweenInit
    {
        internal static void SetAction(Button target, Action action)
        {
            if (target == null) return;

            //if (target.TryGetComponent<TweenAnimation>(out TweenAnimation anim)) anim.OnClickEnd += new DG.Tweening.TweenCallback(action);
            //else target.onClick.AddListener(() => action.Invoke());
            target.onClick.AddListener(() => action.Invoke());
        }
    }
}
