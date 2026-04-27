using CnControls;
using NFramework;
using SquidGame;
using SquidGame.Core;
using SquidGame.Gameplay;
using SquidGame.LandScape;
using SquidGame.UI;
using System.Collections;
using UnityEngine;

namespace Game1
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header(" Control Settings ")]
        public Rigidbody thisRigidbody;
        public float moveSpeed;
        public float RotSpeed;
        public float maxX;
        public float maxZ;
        public static bool canMove;
        public bool IsPlaying, IsDie, IsWin, IsRunning;

        [SerializeField] private Transform _headPos;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private GameObject _winFX;
        [SerializeField] private GameObject _fxBloodSplat;
        [SerializeField] private GameObject _fxBloodPool;
        [SerializeField] private BaseCharacter _character;
        [SerializeField] private CharacterAnimationController _animationController;

        private bool _isMoving;

        private Minigame01MenuUI _ui;
        private HunterController _hunter;
        private Game1Control _controller;
        private SimpleJoystick _joystick;

        public void Init(Game1Control controller, Minigame01MenuUI ui)
        {
            _ui = ui;
            _joystick = _ui.Joystick;
            _controller = controller;
            _hunter = _controller.HunterController;
            _animationController.PlayAnimation(EAnimStyle.Idle);
            _fxBloodPool.SetActive(false);
            _fxBloodSplat.SetActive(false);
            canMove = true;
            IsRunning = false;
        }

        private void Update()
        {
            //joystick.
            if (IsPlaying)
            {
                if (_joystick.HorizintalAxis.Value != 0 || _joystick.VerticalAxis.Value != 0)
                {
                    // If is red light
                    if (_hunter.IsSilent && _hunter.IsRotateToBot && IsRunning)
                    {
                        StartCoroutine(DieCoroutine());
                        return;
                    }

                    // Move Player
                    _isMoving = true;
                    _animationController.PlayAnimation(EAnimStyle.Run);
                    transform.forward = new Vector3(_joystick.HorizintalAxis.Value * Time.deltaTime, 0,
                        _joystick.VerticalAxis.Value * Time.deltaTime);
                }
                else
                {
                    if (_isMoving)
                    {
                        _animationController.PlayAnimation(EAnimStyle.StandStill_1, EAnimStyle.StandStill_2, EAnimStyle.StandStill_3);
                    }
                    _isMoving = false;
                }
            }
        }

        private void FixedUpdate()
        {
            if (IsPlaying)
            {
                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
                pos.z = Mathf.Clamp(pos.z, -maxZ, maxZ);
                transform.position = pos;

                if (canMove)
                {
                    if (_isMoving)
                        Move();
                    else
                        thisRigidbody.velocity = Vector3.zero;
                }
            }
        }


        public void Move()
        {
            Vector3 movement = new Vector3(_joystick.HorizintalAxis.Value, 0, _joystick.VerticalAxis.Value);
            movement *= moveSpeed * Time.deltaTime;

            thisRigidbody.velocity = movement;
        }

        public void Revive()
        {
            IsDie = false;
            _fxBloodPool.SetActive(false);
            _fxBloodSplat.SetActive(false);
            _boxCollider.isTrigger = false;
            _hunter.SetActive(true);
            IsPlaying = true;
            _animationController.PlayAnimation(EAnimStyle.StandStill_1, EAnimStyle.StandStill_2, EAnimStyle.StandStill_3);
            _character.ToggleGreyScale(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsPlaying || IsDie || IsWin) return;
            if (collision.gameObject.CompareTag("win")) Win();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.LogError("OnTriggerEnter : " + other.name);
            var playerCollect = other.GetComponent<PlayerCollect>();
            if (playerCollect != null)
            {
                switch (playerCollect.TypeItem)
                {
                    case TypeItemCollect.Start:
                        if (IsRunning) return;
                        IsRunning = true;
                        break;
                    case TypeItemCollect.Win:
                        break;
                    case TypeItemCollect.Obstacle:
                        StartCoroutine(DieCoroutine());
                        break;
                    default:
                        break;
                }
            }
        }

        private void Win()
        {
            if (!IsPlaying) return;

            IsWin = true;
            IsPlaying = false;
            transform.eulerAngles = new Vector3(0, 180, 0);
            _animationController.PlayAnimation(EAnimStyle.Victory_1, EAnimStyle.Victory_2, EAnimStyle.Victory_3);
            _hunter.SetActive(false);
            StartCoroutine(CRPlayWinSound());
            _winFX.SetActive(true);
            GameManager.I.Win(3f);

            IEnumerator CRPlayWinSound()
            {
                var audioSource = GameSound.I.PlaySFX(Define.SoundPath.SFX_WIN_DANCE);
                yield return new WaitForSeconds(3f);
                audioSource.Stop();
            }
        }

        public IEnumerator DieCoroutine()
        {
            if (!IsPlaying) yield break;

            _boxCollider.isTrigger = true;
            IsDie = true;
            IsPlaying = false;
            _hunter.ShootSingle(_headPos).Forget();
            _animationController.PlayAnimation(EAnimStyle.Die);
            _character.ToggleGreyScale(true);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_GUNSHOT);
            yield return new WaitForSeconds(0.1f);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG01_HOSTAGE_M_HIT_01);
            _fxBloodSplat.SetActive(true);
            this.InvokeDelay(0.5f, () => { _fxBloodPool.SetActive(true); });
            _hunter.SetActive(false);
            yield return new WaitForSeconds(2.5f);
            GameManager.I.Lose();
        }
    }
}