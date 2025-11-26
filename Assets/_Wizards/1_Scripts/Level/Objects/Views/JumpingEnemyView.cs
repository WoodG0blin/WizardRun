using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardsPlatformer
{
    internal class JumpingEnemyView : PatrollingEnemyView
    {
        private Vector2 _fixedMoveInput;
        private float _speed = 1f;
        private float _jumpAngleThreshold = 30f;

        [SerializeField] private string targetAccessable;

        public override bool TryMoveTo(Vector2 target, float speed)
        {
            if (CheckApproach(target, Position)) return false;
            if (!Mover.IsGrounded)
            {
                Mover?.SetInput(_fixedMoveInput, speed);
                return true;
            }

            _fixedMoveInput = target - Position;
            _speed = speed;

            bool canJump = CheckJump(_fixedMoveInput, Position, speed, out var landing);
            CheckStepText = $"dir {_fixedMoveInput}. CanJump {canJump}, landing {landing}. ShouldJump {ShouldJump(_fixedMoveInput, landing)}. CheckStep{CheckStep(_fixedMoveInput, Position)}";

            if (ShouldJump(_fixedMoveInput, landing) && canJump)
            {
                Mover?.SetInput(_fixedMoveInput, speed);
                Mover?.Jump(speed);
                return true;
            }

            if (CheckStep(target, Position))
            {
                Mover?.SetInput(_fixedMoveInput, speed);
                return true;
            }
            
            if(canJump)
            {
                Mover?.SetInput(_fixedMoveInput, speed);
                Mover?.Jump(speed);
                return true;
            }

            return false;
        }

        private bool CheckJump(Vector2 moveInput, Vector2 origin, float speed, out Vector2 landing)
        {
            float jumpImpulse = Mover.CalculateJumpImpulse(speed);
            float moveImpulse = Mover.CalculateMoveImpulse(moveInput, speed);
            bool canJump = false;

            float stepDeltaTime = 0.2f;
            Vector2 nextPos = origin;

            for (float t = 0; t < 3; t += stepDeltaTime)
            {
                if (jumpImpulse > 0)
                {
                    bool obstacle = Physics.Raycast(nextPos, (new Vector3(moveInput.x, jumpImpulse, 0)).normalized, 0.5f);
                    if (obstacle) break;
                }
                nextPos += new Vector2(moveImpulse, jumpImpulse) * stepDeltaTime;
                if (jumpImpulse <= 0)
                {
                    bool hasLanding = Physics.Raycast(nextPos, Vector3.down, 0.6f, _groundsLayerMask);
                    if (hasLanding)
                    {
                        canJump = nextPos.y > 0;
                        break;
                    }
                }
                jumpImpulse -= stepDeltaTime * 9.81f;
            }
            landing = nextPos;
            return canJump;
        }

        private bool ShouldJump(Vector2 moveInput, Vector2 landing)
        {
            Vector2 target = Position + moveInput;
            if(moveInput.magnitude < (target-landing).magnitude) return false;

            Vector2 reference = (new Vector2(moveInput.x, 0)).normalized;
            float angle = Vector2.Angle(moveInput.normalized, reference);
            return angle > _jumpAngleThreshold;
        }

        public override bool IsPointAccessable(Vector2 targetPoint)
        {
            bool res = false;

            float stepDeltaTime = 0.2f;
            Vector2 nextPos = Position;

            for (float t = 0; t < 3; t += stepDeltaTime)
            {
                if (CheckApproach(targetPoint, nextPos))
                {
                    res = true;
                    break;
                }

                Vector2 nextDir = targetPoint - nextPos;
                bool canJump = CheckJump(nextDir, nextPos, _speed, out var landing);

                if (ShouldJump(nextDir, landing) && canJump)
                {
                    nextPos = landing;
                    continue;
                }

                if (CheckStep(nextDir, nextPos))
                {
                    nextPos += new Vector2(Mover.CalculateMoveImpulse(nextDir, _speed), 0) * stepDeltaTime;
                    continue;
                }

                if (canJump)
                {
                    nextPos = landing;
                    continue;
                }

                break;
            }

            targetAccessable = $"{targetPoint}. final pos {nextPos}. {res}";

            return res;
        }
    }
}