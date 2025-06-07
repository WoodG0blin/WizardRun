using System.Collections.Generic;

namespace WizardsPlatformer
{
    //public interface IArtifactExecutorsContainer
    //{
    //    void ExecuteFor(ArtifactExecutorType type, IArtifactHolder holder);
    //}

    //internal class ArtifactExecutorsContainer : IArtifactExecutorsContainer
    //{
    //    private Dictionary<ArtifactExecutorType, List<IArtifactExecutor>> _executors;

    //    public ArtifactExecutorsContainer(List<IArtifact> artifacts)
    //    {
    //        _executors = new();
    //        FillExecutorsByType(ArtifactExecutorType.Modifier, artifacts);
    //        FillExecutorsByType(ArtifactExecutorType.Attack, artifacts);
    //        FillExecutorsByType(ArtifactExecutorType.Jump, artifacts);
    //        FillExecutorsByType(ArtifactExecutorType.ExplicitAction, artifacts);
    //    }

    //    private void FillExecutorsByType(ArtifactExecutorType type, List<IArtifact> artifacts)
    //    {
    //        if (!_executors.ContainsKey(type)) _executors.Add(type, new());
    //        foreach(var a in artifacts)
    //        {
    //            var ex = a.GetExecutor(type);
    //            if(ex != null) _executors[type].Add(ex);
    //        }
    //        if (_executors[type].Count == 0) _executors.Remove(type);
    //    }

    //    public void ExecuteFor(ArtifactExecutorType type, IArtifactHolder holder)
    //    {
    //        if (!_executors.ContainsKey(type)) return;
    //        foreach (var ex in _executors[type])
    //            if(ex.IsReady) ex.Use(holder);
    //    }
    //}
}
