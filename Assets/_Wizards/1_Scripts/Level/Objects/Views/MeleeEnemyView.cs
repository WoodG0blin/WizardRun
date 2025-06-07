using System;
using UnityEngine;
using TMPro;

namespace WizardsPlatformer
{
    internal class MeleeEnemyView : LevelObjectView
    {
        [SerializeField] private TextMeshProUGUI stateText;
        [field: SerializeField] public Transform Barrel { get; private set; }

        [SerializeField] Vector3 PatrolPointValue;
        [SerializeField] Vector3 PositionValue;

        [SerializeField] bool HasContactLeft;
        [SerializeField] bool HasContactRight;
        [SerializeField] bool NoGap;
        [SerializeField] float xDir;


        private LayerMask _layerMask;

        public Vector3 PatrolPoint { get; private set; }

        public void Init(LevelObjectConfig config)
        {
            PatrolPoint = Position;
            _layerMask = LayerMask.GetMask("Background");
            Barrel ??= transform;
        }

        protected override void OnInitiation()
        {
            Mover = new ViewMover(visualBody);
        }

        protected override void OnUpdate()
        {
            //animator.UpdateValues(rigidbody.velocity);
            PatrolPointValue = PatrolPoint;
            PositionValue = Position;

            HasContactLeft = AccessContacts().HasContactLeft;
            HasContactRight = AccessContacts().HasContactRight;
            NoGap = CheckNoGap(XDirection);

            xDir = XDirection;
        }


        public bool CheckNoGap(float XDirection) =>
            Physics.Raycast(Position, new(XDirection, -0.5f, 0), 1.2f, _layerMask);


        public void DisplayAttack(Action onAttackPositionReady) =>
            animator.TriggerAnimation(ActionState.Attack, () => onAttackPositionReady());

        public void DisplayState(string state) => stateText.text = state;
    }
}