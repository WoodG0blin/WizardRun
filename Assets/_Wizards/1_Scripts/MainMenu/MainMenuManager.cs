using UnityEngine;

namespace WizardsPlatformer
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private StartUIView _startUI;

        private IMenuInfo _menuInfo;

        private void Awake()
        {
            //Replace with DIc
            _menuInfo = FindFirstObjectByType<GameManager>();
            _startUI.Init(_menuInfo, OnStart);
            _menuInfo.SceneLoader.FinishSceneLoad();
        }

        private void OnStart(Location location)
        {
            _menuInfo.SetActiveLocation(location);
            _startUI.SetActive(false);
            _menuInfo.SceneLoader.LoadLevel();
        }
    }
}
