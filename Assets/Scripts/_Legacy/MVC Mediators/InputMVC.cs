using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

namespace WizardsPlatformer
{
    internal class InputMVC : MVCMediator
    {

        public InputController Controller { get; private set; }

        public InputMVC(InputConfig config, LevelModel levelModel, Transform UIContainer)
        {
            GameObject temp = GameObject.Instantiate(config.Prefab);
            InputView input = temp.GetComponent<InputView>() ?? temp.AddComponent<InputView>();
            //RegisterOnDispose(input);

            Controller = new InputController(levelModel, input);
            RegisterOnDispose(Controller);
        }
        public void SetActive(bool active) => Controller.SetActive(active);
    }
}
