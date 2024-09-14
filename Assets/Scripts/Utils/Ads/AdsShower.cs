using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    //internal class StubAdsShower : IAdsShower
    //{
    //    public event Action Started;
    //    public event Action Finished;
    //    public event Action Failed;
    //    public event Action Skipped;
    //    public event Action BecomeReady;

    //    public void Play()
    //    {
            
    //    }
    //}

    //internal class AdsShower : IAdsShower, IUnityAdsListener
    //{
    //    public event Action Started;
    //    public event Action Finished;
    //    public event Action Failed;
    //    public event Action Skipped;
    //    public event Action BecomeReady;

    //    protected readonly string _id;

    //    internal AdsShower(string ID)
    //    {
    //        _id = ID;
    //        Advertisement.AddListener(this);
    //    }

    //    public void Play()
    //    {
    //        Load();
    //        OnPlay();
    //        Load();
    //    }

    //    protected void Load() { if (!_id.Equals("")) Advertisement.Load(_id); }
    //    protected void OnPlay() { if (!_id.Equals("")) Advertisement.Show(_id); }

    //    void IUnityAdsListener.OnUnityAdsReady(string placementId)
    //    {
    //        if (!placementId.Equals(_id)) return;
    //        Debug.Log($"{GetType().Name} is ready");
    //        BecomeReady?.Invoke();
    //    }

    //    void IUnityAdsListener.OnUnityAdsDidError(string message)
    //    {
    //        Debug.Log($"{GetType().Name} has error: {message}");
    //        //Failed?.Invoke();
    //    }

    //    void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
    //    {
    //        if (!placementId.Equals(_id)) return;
    //        Debug.Log($"{GetType().Name} is started");
    //        Started?.Invoke();
    //    }

    //    void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    //    {
    //        if (!placementId.Equals(_id)) return;
            
    //        switch(showResult)
    //        {
    //            case ShowResult.Finished:
    //                Debug.Log($"{GetType().Name} is finished");
    //                Finished?.Invoke();
    //                break;

    //            case ShowResult.Skipped:
    //                Debug.Log($"{GetType().Name} is skipped");
    //                Skipped?.Invoke(); 
    //                break;

    //            case ShowResult.Failed:
    //                Debug.Log($"{GetType().Name} has failed");
    //                Failed?.Invoke(); 
    //                break;
    //        }
    //    }
    //}
}
