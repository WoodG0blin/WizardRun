using System;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class Controller : IDisposable
    {
        private List<IDisposable> _disposables;
        private List<Controller> _childControllers;
        private bool _disposed;

        protected IView _animatedView;

        public Controller(IView view) { _animatedView = view; }
        public Controller() { }

        //TODO remove update from controller
        public void Update()
        {
            if(_childControllers != null) foreach(Controller controller in _childControllers) controller.Update();
            OnUpdate();
        }
        protected virtual void OnUpdate() { }

        public void Dispose()
        {
            if(_disposed) return;
            _disposed = true;

            _animatedView?.Dispose();

            if (_disposables != null)
            {
                foreach (IDisposable disposable in _disposables) disposable.Dispose();
                _disposables.Clear();
            }

            if (_childControllers != null)
            {
                foreach (Controller disposable in _childControllers) disposable.Dispose();
                _childControllers.Clear();
            }

            OnDispose();
        }
        protected virtual void OnDispose() { }

        protected void Register(IDisposable disposable)
        {
            _disposables ??= new List<IDisposable>();
            _disposables.Add(disposable);
        }
        //TODO remove when update is removed
        protected void AddController(Controller controller)
        {
            _childControllers??=new List<Controller>();
            _childControllers.Add(controller);
        }
    }
}
