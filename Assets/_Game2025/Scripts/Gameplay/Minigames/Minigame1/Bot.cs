using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame1
{

    public class Bot : MonoBehaviour
    {
        private bool _die, _win;
 
        [SerializeField] private GameObject _fxBloodSplat;
        [SerializeField] private GameObject _fxBloodPool;
        [SerializeField] private Transform _headPos;


        [Header("ANIMATION")]
        public CharacterAnimator _characterAnimator;
        [SerializeField] private BaseCharacter _baseCharacter;
        [SerializeField] private BaseStateBot _randomRunState;
        [SerializeField] private BaseStateBot _runToTargetState;
        
        private BaseStateBot _currentState;

        public bool IsDie => _die;
        public bool IsWin => _win;
        public Transform HeadPos => _headPos;

        private bool _isInit;
        private HunterController _enemyController;
        private BotManager _botManager;
        
        public BotManager Manager => _botManager;
        public CharacterAnimator Animator => _characterAnimator;
        public HunterController Hunter => _enemyController;



        [Button]
        public void AssignHead()
        {
            //BaseCharacter:Head
            var target = FindDeepChild(this.transform, "BaseCharacter:Head");
            _headPos = target;
        }

        [Button]
        public void AssignBaseCharacter()
        {
            _baseCharacter = _characterAnimator.GetComponent<BaseCharacter>();
        }


        Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;
                Transform found = FindDeepChild(child, name);
                if (found != null)
                    return found;
            }
            return null;
        }



        public void SetToMoveRandomState()
        {

            _currentState?.OnExit();
            _currentState = _randomRunState;
            _currentState.OnEnter();
        }

        public void SetToMoveTargetState()
        {
            _currentState?.OnExit();
            _currentState = _runToTargetState;
            _currentState.OnEnter();
        }


        internal void OnRevive()
        {
            if (_die || _win) return;
            //_baseCharacter.ToggleGreyScale(true);
            _currentState = _runToTargetState;
            _currentState.OnContinuing();
        }


        public void OnStart()
        {
            SetToMoveTargetState(); 
        }


        public void Init(MinigameController controller, BotManager botManager)
        {
            _botManager = botManager;
            _enemyController = controller.HunterController;
            _fxBloodSplat.gameObject.SetActive(false);
            _fxBloodPool.gameObject.SetActive(false);

            _randomRunState.Init(this);
            _runToTargetState.Init(this);
            SetToMoveRandomState();
            _baseCharacter.ToggleGreyScale(false);
            _isInit = true;
        }


        public void StopGame()
        {
            _currentState = null;
            if (!_die)
                _characterAnimator.PlayAnimation( EAnimStyle.Idle);
        }


        void Update()
        {
            if (!_isInit) return;
            if(_currentState == null) return;
            _currentState?.OnUpdate();
        }


        public void HandleDie()
        {
            _die = true;
            var rndValue = Random.value;
            GameSound.I.PlaySFX(rndValue > 0.5f ? Define.SoundPath.SFX_MG01_M4_SHOOT : Define.SoundPath.SFX_MG01_GUNSHOT);
            GameSound.I.PlaySFX(rndValue > 0.5f ? Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01 : Define.SoundPath.SFX_MG01_HOSTAGE_F_HIT_01);
            _fxBloodSplat.gameObject.SetActive(true);
            _baseCharacter.ToggleGreyScale(true);
            _characterAnimator.PlayAnimation(EAnimStyle.Die);
            this.InvokeDelay(1f, () => _fxBloodPool.gameObject.SetActive(true));
        }


        public void RunCompleted()
        {
            _currentState = null;
            _win = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
            _characterAnimator.PlayAnimation(EAnimStyle.Victory_1,EAnimStyle.Victory_2,EAnimStyle.Victory_3,EAnimStyle.Idle);
        }

    }

}