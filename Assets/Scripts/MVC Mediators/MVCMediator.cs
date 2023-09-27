using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal abstract class MVCMediator : IDisposable
    {
        private bool _disposed;
        private List<IDisposable> _disposables;

        public void Dispose()
        {
            if (_disposed) return;

            foreach(IDisposable disposable in _disposables) disposable.Dispose();
            OnDispose();

            _disposed = true;
        }

        protected virtual void OnDispose() { }

        protected void RegisterOnDispose(IDisposable disposable)
        {
            _disposables ??= new List<IDisposable>();
            _disposables.Add(disposable);
        }
    }
}
