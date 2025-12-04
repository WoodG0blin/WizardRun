using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class CameraController
    {
        private ICameraView _camera;

        public CameraController(Camera camera, Sprite[] backGroundSprites)
        {
            if (!camera.TryGetComponent<ICameraView>(out _camera)) _camera = camera.transform.AddComponent<CameraView>();
            _camera.InitBackGrounds(backGroundSprites, Screen.height / camera.orthographicSize);
        }

        public void UpdateToPlayerPosition(Vector2 newPosition)
        {
            float targetX = newPosition.x;
            float targetY = Mathf.Clamp(newPosition.y, -1f, newPosition.y);
            _camera.SetNewTargetPosition(targetX, targetY);
        }
    }
}
