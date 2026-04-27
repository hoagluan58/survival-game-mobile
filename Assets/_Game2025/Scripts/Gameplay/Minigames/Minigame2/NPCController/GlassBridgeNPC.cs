using NFramework;
using Redcode.Extensions;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SquidGame.LandScape.Minigame2
{
    public class GlassBridgeNPC : NPC
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;

        private bool _isDeath, _isFalling;
        private CharacterAnimator _animator;
        private bool _isReachDestination;
        private GlassPanel[,] _glassPanels;
        private List<GlassPanel> _correctPath;
        private GlassPanel _curGlassPanel;
        public NavMeshAgent Agent => _agent;

        public bool IsReachDestination => _isReachDestination;
        public bool IsDeath => _isDeath;

        public GlassPanel CurGlassPanel
        {
            get => _curGlassPanel;
            set => _curGlassPanel = value;
        }

        public void Init(GlassPanel[,] glassPanels, List<GlassPanel> correctPath)
        {
            _glassPanels = glassPanels;
            _correctPath = correctPath;
            _isReachDestination = false;
            _animator = Model.GetCom<CharacterAnimator>();
            ToggleGravity(false);
        }

        public GlassPanel GetNextGlassBridge()
        {
            var nextRow = _curGlassPanel == null ? 0 : _curGlassPanel.Data.Row + 1;
            var glassPanelList = new List<GlassPanel>();
            var row = _glassPanels.GetLength(0);
            var columns = _glassPanels.GetLength(1);

            if (nextRow > row - 1)
            {
                return null;
            }

            for (var i = 0; i < columns; i++)
            {
                var panel = _glassPanels[nextRow, i];

                if (panel.IsBreak) continue;
                if (_correctPath.Contains(panel))
                {
                    return panel;
                }

                glassPanelList.Add(_glassPanels[nextRow, i]);
            }

            return glassPanelList.RandomItem();
        }

        // true
        public void ToggleGravity(bool value)
        {
            _agent.enabled = !value;
            _rigidbody.isKinematic = !value;
        }

        public void Falling()
        {
            if (_isFalling) return;
            _isFalling = true;
            ToggleGravity(true);
            _animator.PlayAnimation(EAnimStyle.Falling);
        }

        public void Die()
        {
            _isDeath = true;
            _rigidbody.isKinematic = false;
            _animator.PlayAnimation(EAnimStyle.Die);
            Model.ToggleGreyScale(true);
        }

        public void Win()
        {
            if (_isReachDestination) return;

            _isReachDestination = true;
            _agent.enabled = true;
            TransitionTo(INPCState.EState.Wander);
        }

        public void ToggleColliderTrigger(bool value) => _collider.isTrigger = value;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<CollidableType>(out var component))
            {
                switch (component.Type)
                {
                    case CollidableType.EObjectType.TriggerWin:
                        Win();
                        return;
                    case CollidableType.EObjectType.TriggerFalling:
                        return;
                    case CollidableType.EObjectType.TriggerDeath:
                        ToggleColliderTrigger(false);
                        Die();
                        return;
                    case CollidableType.EObjectType.GlassPanel:
                        GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_JUMP_ON_GLASS);
                        if (component.TryGetComponentInParent<GlassPanel>(out var glassPanel))
                        {
                            var isTrueMove = glassPanel.Data.IsTrueMove;
                            if (!isTrueMove)
                            {
                                glassPanel.Break();
                            }
                            else
                            {
                                CurGlassPanel = glassPanel;
                            }
                        }
                        return;
                }
            }
        }
    }
}
