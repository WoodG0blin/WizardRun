using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class CameraController : Controller
    {
        private ICameraView _camera;
        private LevelModel _level;

        public CameraController(Camera camera, LevelModel level)
        {
            if(!camera.TryGetComponent<ICameraView>(out _camera)) _camera = camera.transform.AddComponent<CameraView>();
            //Register(_camera);
            _level = level;

            //TODO remove to model?
            _level.PlayerPosition.SubscribeOnValueChange(OnPlayerPositionChange);
        }

        public void SetActive(bool active) => _camera.SetActive(active);

        protected override void OnDispose() =>
            _level.PlayerPosition.UnsubscribeOnValueChange(OnPlayerPositionChange);
        

        private void OnPlayerPositionChange(Vector3 newPosition)
        {
            float targetX = newPosition.x;
            float targetY = Mathf.Clamp(newPosition.y, -3.5f, 3.5f);
            _camera.SetNewTargetPosition(targetX, targetY);
        }
    }
}
