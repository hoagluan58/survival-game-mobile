using DG.Tweening;
using SquidGame.LandScape.Game;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.Minigame2
{
    public class JumpBridgeState : INPCState
    {
        private NPCController _controller;
        private GlassBridgeNPC _npc;

        private NavMeshAgent _agent;
        private CharacterAnimator _animator;

        // Config 
        private float _jumpHeight = 2.2f;
        private float _gravity = -30f;
        private float _moveSpeed = 3f;

        private bool _canJump;
        private bool _isJumping = false;
        private float _waitTime = 0f;

        public JumpBridgeState(NPCController controller, GlassBridgeNPC npc)
        {
            _controller = controller;
            _npc = npc;
        }

        public INPCState.EState StateName => INPCState.EState.MoveToDestination;

        public void OnInit()
        {
            _agent = _npc.Agent;
            _animator = _npc.Model.GetCom<CharacterAnimator>();
        }

        public void OnEnter()
        {
            _agent.enabled = false;
            MoveToJumpPos();
        }

        public void OnExit()
        {
        }

        public void OnUpdate()
        {
            StartJumping();
        }

        private void MoveToJumpPos()
        {
            var targetPos = _controller.MinJumpPos.position;
            var dir = (targetPos - _npc.transform.position).normalized;
            SetRotation(dir);

            _npc.transform.DOMove(targetPos, _moveSpeed)
                .SetSpeedBased(true)
                .SetEase(Ease.Linear)
                .OnStart(() => _animator.PlayAnimation(EAnimStyle.Running))
                .OnComplete(() =>
                {
                    _canJump = true;
                });
        }

        private void StartJumping()
        {
            if (!_canJump) return;
            if (_isJumping) return;

            if (_waitTime > 0f)
            {
                _waitTime -= Time.deltaTime;
                return;
            }

            _isJumping = true;
            var jumpDuration = Mathf.Sqrt(2 * _jumpHeight / -_gravity) * 2f;

            _animator.PlayAnimation(EAnimStyle.Jump);
            var glassPanel = _npc.GetNextGlassBridge();
            var target = glassPanel == null ? _controller.DestinationPos.position : glassPanel.RandomPositionOnGlass();

            var dir = (target - _npc.transform.position).normalized;
            SetRotation(dir);

            _npc.transform.DOJump(target, _jumpHeight, 1, jumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _isJumping = false;
                    _waitTime = 1f;
                    _animator.PlayAnimation(EAnimStyle.Idle);

                    if (glassPanel == null)
                    {
                        return;
                    }

                    // Check true glass panel
                    if (glassPanel.Data.IsTrueMove)
                    {
                        _controller.TryAddCorrectPath(glassPanel);
                    }
                    else
                    {
                        _npc.Falling();
                        _canJump = false;
                    }
                });
        }

        private void SetRotation(Vector3 dir)
        {
            var lookRotation = Quaternion.LookRotation(dir);
            _npc.transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
    }
}