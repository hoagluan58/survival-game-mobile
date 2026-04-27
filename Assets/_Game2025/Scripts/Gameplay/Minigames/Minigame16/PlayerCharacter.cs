using DG.Tweening;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Game;
using UnityEngine;

namespace SquidGame.LandScape.Minigame16
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private Transform _head;
        [SerializeField] private CharacterAnimator _model;
        [SerializeField] private ParticleSystem _fxBlood;

        private BaseCharacter _character;

        private void Awake()
        {
            _character = _model.GetComponent<BaseCharacter>(); 
        }

        public void Init()
        {
            _model.PlayAnimation(EAnimStyle.Idle);
            _fxBlood.gameObject.SetActive(false);
        }

        public Transform Head() => _head;

        public void DORotate(Vector3 vector3, float duration) => transform.DORotate(vector3, duration);

        public void PlayWinAnim() => _model.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);

        public void PlayIdleAnim()
        {
            _character.ToggleGreyScale(false);
            _fxBlood.gameObject.SetActive(false);
            _fxBlood.Stop();
            _model.PlayAnimation(EAnimStyle.Idle);
        }

        public void PlayDieAnim()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _model.PlayAnimation(EAnimStyle.Die);
            _fxBlood.gameObject.SetActive(true);
            _fxBlood.Play(); 
            _character.ToggleGreyScale(true);
        }
    }
}
