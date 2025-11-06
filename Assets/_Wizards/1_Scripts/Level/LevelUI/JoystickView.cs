using UnityEngine;
using UnityEngine.EventSystems;

namespace WizardsPlatformer
{
    public class JoystickView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform _joystick;
        [SerializeField] private int _dragRadius = 200;

        private float _jumpThreshold = 0.1f;
        private float _lastVerticalInput = 0f;

        private bool _currentDrag;
        private Vector2 _startPosition;
        private int _snapRadius;

        public bool Jump;
        public Vector2 Input { get; private set; }

        private void Awake()
        {
            _startPosition = _joystick.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentDrag = true;
            //_startPosition = eventData.position;
            _snapRadius = CalculateSnapRadius();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_currentDrag)
            {
                Vector2 pos = eventData.position;
                float xDelta = pos.x - _startPosition.x;
                float yDelta = pos.y - _startPosition.y;

                Vector2 current = new(xDelta, yDelta);
                float distance = current.magnitude;
                
                if (distance > _snapRadius)
                    FinishCurrentDrag();
                else
                {
                    if (distance < _dragRadius) distance = _dragRadius;
                    current *= (_dragRadius / distance);

                    Input = current.normalized;
                    //Jump = JoystickPosition.y > _jumpThreshold && JoystickPosition.y > _lastVerticalInput;
                    Jump = (Input.y - _lastVerticalInput) > _jumpThreshold;
                    _lastVerticalInput = Input.y;

                    _joystick.position = _startPosition + current;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(_currentDrag) FinishCurrentDrag();
        }

        private void FinishCurrentDrag()
        {
            _currentDrag = false;
            _joystick.position = _startPosition;
            Input = Vector2.zero;
            Jump = false;
        }

        private int CalculateSnapRadius()
        {
            //Rect zone = transform.parent.GetComponent<RectTransform>().rect;
            //return Mathf.RoundToInt(Mathf.Min( zone.width, zone.height ) / 2);
            return _dragRadius * 30;
        }
    }
}
