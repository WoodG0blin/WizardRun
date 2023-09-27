using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardsPlatformer
{
    internal interface IModel
    {
        Model ParentModel { get; }
    }

    internal abstract class Model : IModel
    {
        public Model ParentModel { get; protected set; }
    }
}
