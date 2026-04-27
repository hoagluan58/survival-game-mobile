using SquidGame.LandScape.Game;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SquidGame.LandScape.Minigame1
{
    public class RunToTargetState : BaseStateBot
    {
        [Header("CONFIGS")]
        [SerializeField] private bool _isInit;
        [SerializeField] private float _speed;
        [SerializeField] private float _endgameZ = 52.35f;
        [SerializeField] private BotBrain _botBrain;
        

        private bool _isTriggerStay;
        private bool _isTriggerRun;
        private float _delayMoving;
        private int _indexTargetPosition = 0;
        private List<Vector3> _targetPostions;


        public override void OnEnter()
        {
            base.OnEnter();
            _isInit = false;
            transform.localEulerAngles = Vector3.zero;
            _botBrain.Init();
            _targetPostions = _botBrain.Positions;
            _isInit = true; 
        }

        public override void OnExit()
        {
            base.OnExit();
            
        }

        private void Freeze()
        {
            _isTriggerRun = false;
            if (_isTriggerStay) return;
            _delayMoving = Random.Range(0f, 0.5f);
            _isTriggerStay = true;
            _bot.Animator.PlayAnimation(EAnimStyle.Stand_Still_Pose_1, EAnimStyle.Stand_Still_Pose_2);
        }


        private void Run()
        {
            _delayMoving -= Time.deltaTime;
            if (_delayMoving >= 0) return;

            _isTriggerStay = false;
            PlayRunAnimationInUpdate();

            transform.position = Vector3.MoveTowards(transform.position, _targetPostions[_indexTargetPosition], _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPostions[_indexTargetPosition]) < 0.1f)
            {
                _indexTargetPosition++;
                if (_indexTargetPosition >= _targetPostions.Count)
                {
                    OnExit();
                    return; 
                }
                transform.LookAt(_targetPostions[_indexTargetPosition]);
            }
        }

        private void PlayRunAnimationInUpdate()
        {
            if (_isTriggerRun) return;
            _isTriggerRun = true;
            _bot.Animator.PlayAnimation(EAnimStyle.Running);
        }

        public override void OnContinuing()
        {
            base.OnContinuing();
            _isTriggerRun = false;
            PlayRunAnimationInUpdate();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!_isInit) return;
            if (_bot.IsDie || _bot.IsWin) return;

            if (_bot.Hunter.IsSilent)
            {
                Freeze();
            }
            else
            {
                Run();
            }

            if(transform.position.z >= _endgameZ)
            {
                _bot.RunCompleted();
            }
        }

        
    }

}