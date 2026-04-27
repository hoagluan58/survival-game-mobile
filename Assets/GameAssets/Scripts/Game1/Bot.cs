using Animancer;
using NFramework;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using System.Collections.Generic;
using UnityEngine;

namespace Game1
{
    public class Bot : MonoBehaviour
    {
        public float speed;
        public bool firsttime;
        float a;
        bool die, onetime, win;
        public bool femal;

        [SerializeField] private GameObject _fxBloodSplat;
        [SerializeField] private GameObject _fxBloodPool;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private Transform _headPos;


        [Header("ANIMATION")]
        [SerializeField] private BaseCharacter _character;
        [SerializeField] private AnimancerComponent _animancer;
        [SerializeField] private AnimationClip _idleClip;
        [SerializeField] private AnimationClip _runClip;
        [SerializeField] private AnimationClip _deadClip;
        [SerializeField] private AnimationClip[] _standstillClips;

        [Header("CONFIGS")]
        [SerializeField] private BotBrain _botBrain;

        public bool IsDie => die;
        public bool IsWin => win;
        public Transform HeadPos => _headPos;

        private bool _isInit;
        private HunterController _enemyController;
        private PlayerController _playerController;
        private Game1Control _controller;
        private BotManager _botManager;
        private AnimancerState _state;

        private int _indexTargetPosition = 0; 
        private List<Vector3> _targetPostions;
        void Start()
        {
            speed = Random.Range(4f, 9f);
            _animancer.Play(_idleClip);
        }

        public void Init(Game1Control controller, BotManager botManager)
        {
            _controller = controller;
            _botManager = botManager;
            _enemyController = _controller.HunterController;
            _playerController = _controller.PlayerController;
            _fxBloodSplat.gameObject.SetActive(false);
            _fxBloodPool.gameObject.SetActive(false);
            _botBrain.Init(this);
            _targetPostions = _botBrain.Positions;
            _isInit = true;
        }

        public void StopGame()
        {
            _animancer.Stop();
            enabled = false;
        }

        void Update()
        {
            if (!_isInit) return;
            if (!_controller.IsStartGame) return; 

            if (_playerController.IsPlaying && !die && !win)
            {
                if (!_enemyController.IsSilent)
                {
                    onetime = false;
                    if (!firsttime)
                    {
                        firsttime = true;
                        a = Random.Range(1, 1.3f);
                        _state = _animancer.Play(_runClip);
                    }
                    else
                    {
                        _state = _animancer.Play(_runClip);
                    }
                    transform.position = Vector3.MoveTowards(transform.position, _targetPostions[_indexTargetPosition], speed * Time.deltaTime);
                    _state.Speed = a;

                    if(Vector3.Distance(transform.position, _targetPostions[_indexTargetPosition]) < 0.1f)
                    {
                        _indexTargetPosition++;
                        if (_indexTargetPosition >= _targetPostions.Count)
                        {
                            win = true;
                        }
                    }
                }
                else
                {
                    speed = Random.Range(3f, 4f);
                    _state.Speed = 0f;
                    if (!onetime)
                    {
                        RandomStay();
                        //RandomDieOrStay();
                    }

                }
            }

            if (_controller.TimeLeft <= 0 && !win && !die)
            {
                die = true;
            }
        }


        private void RandomStay()
        {
            onetime = true;
            _state = _animancer.Play(_standstillClips.RandomItem());
            _state.Speed = 0f;
        }


        void RandomDieOrStay()
        {
            onetime = true;

            var rndValue = Random.value <= 0.25f;

            if (rndValue)
            {
                die = true;
                _state.Speed = Random.Range(1, 2);
            }
            else
            {
                _state = _animancer.Play(_standstillClips.RandomItem());
                _state.Speed = 0f;
            }
        }

        public void HandleDie()
        {
            die = true;
            _state.Speed = 0f;
            _boxCollider.isTrigger = true;
            var rndValue = Random.value;
            GameSound.I.PlaySFX(rndValue > 0.5f ? Define.SoundPath.SFX_MG01_M4_SHOOT : Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(rndValue > 0.5f ? Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01 : Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01);
            _fxBloodSplat.gameObject.SetActive(true);
            _animancer.Play(_deadClip);
            _character.ToggleGreyScale(true);
            _botManager.RemoveBot(this);
            this.InvokeDelay(1f, () => _fxBloodPool.gameObject.SetActive(true));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "win")
            {
                win = true;
                _boxCollider.isTrigger = true;
                transform.eulerAngles = new Vector3(0, 180, 0);
                _animancer.Play(_idleClip);
            }
        }
    }

}