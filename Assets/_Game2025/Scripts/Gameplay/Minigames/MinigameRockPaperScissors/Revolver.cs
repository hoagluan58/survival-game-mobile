using DG.Tweening;
using NFramework;
using Sirenix.OdinInspector;
using SquidGame.LandScape.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{
    public class Revolver : MonoBehaviour
    {
        [SerializeField] private Transform _zoomPoint;
        [SerializeField] private Transform _gun;
        [SerializeField] private Transform _magazine;
        [SerializeField] private Transform _centerMagazine;
        [SerializeField] private Transform _bullet;
        [SerializeField] private Transform _pointInBullet;
        [SerializeField] private Transform _pointIn_1Bullet;
        [SerializeField] private List<Vector3> _bulletPath;
        [Button]
        public void PlayAnimation()
        {
            StartCoroutine(CRPlayAnimation());
        }

        public IEnumerator CRPlayAnimation()
        {
            _gun.localPosition = Vector3.zero;
            _gun.localEulerAngles = Vector3.zero;
            _magazine.localEulerAngles = Vector3.zero;
            _bullet.localPosition = Vector3.zero;
            _bullet.localEulerAngles = Vector3.zero;

            _gun.DOLocalMove(_zoomPoint.localPosition, 1);
            _gun.DOLocalRotate(_zoomPoint.localEulerAngles, 1);
            yield return new WaitForSeconds(0.5f);
            _magazine.DOLocalRotate(Vector3.back * 40, 0.5f);
            _bullet.DOPath(_bulletPath.ToArray(), 1.25f);
            _bullet.DOLocalRotate(_pointInBullet.localEulerAngles, 1);
            yield return new WaitForSeconds(1.25f);
            _bullet.parent = _centerMagazine;
            _magazine.DOLocalRotate(Vector3.back, 0.5f);
            yield return new WaitForSeconds(0.5f);
            GameSound.I.PlaySFX(Define.SoundPath.SFX_ROCKPAPERSCISSOR_SPIN_GUN);
            _centerMagazine.DOLocalRotate(_centerMagazine.localEulerAngles + Vector3.back * 720f, 1.5f,RotateMode.LocalAxisAdd);
            yield return new WaitForSeconds(0.5f);
            _gun.DOLocalMove(Vector3.zero, 1f);
            _gun.DOLocalRotate(Vector3.zero, 1f);
            yield return new WaitForSeconds(1f);
        }


        [Button]
        public void SyncPath()
        {
            _bulletPath = new List<Vector3>();
            _bulletPath = _bullet.parent.GetComponent<DOTweenPath>().wps;
            _bulletPath.Add(_pointInBullet.position);
            _bulletPath.Add(_pointIn_1Bullet.position);
        }
    }
}
