using System;

namespace WizardsPlatformer
{
    public class SubscribtableProperty<T> : ISubscriptable<T>
    {
        protected T _value;
        protected Action<T> _onValueChange;

        public virtual T Value
        {
            get => _value;
            set
            {
                _value = value;
                _onValueChange?.Invoke(_value);
            }
        }
        public void SubscribeOnValueChange(Action<T> onValueChange) => _onValueChange += onValueChange;
        public void UnsubscribeOnValueChange(Action<T> onValueChange) => _onValueChange -= onValueChange;
    }

    public class SubscriptableTrigger : SubscribtableProperty<bool>
    {
        public override bool Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    _onValueChange?.Invoke(_value);
                    _value = !value;
                }
            }
        }
    }

    public class SubscribrablePropertyWithEqualsCheck<T> : SubscribtableProperty<T>
    {
        public override T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                _value = value;
                _onValueChange?.Invoke(_value);
                }
            }
        }
    }
}
