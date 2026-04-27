using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SquidGame.LandScape.MinigameRockPaperScissors
{


    public class MagazineUI : MonoBehaviour
    {
        [SerializeField] private BulletUI _angle;
        [SerializeField] private Transform _center;
        [SerializeField] private TextMeshProUGUI _bulletCountText;
        [SerializeField][ReadOnly] private int _maxBullet;
        [SerializeField][ReadOnly] private List<BulletUI> _bullets;

        private Vector3 _defaultEulerangle = new Vector3(0, 0, 90);

        public void MoveTo(int index , float duration)
        {
            if(index <0 || index >= _bullets.Count) return;
            var targetAngle =  _defaultEulerangle.z + (60 * index);

            if(duration < 0 ) _center.eulerAngles = Vector3.forward* targetAngle;
            _center.DORotate(Vector3.forward * targetAngle, duration);
        }

        public void Initialized(int maxBullet)
        {
            _center.eulerAngles = _defaultEulerangle;
            _maxBullet = maxBullet;
            _bulletCountText.SetText($"0/{_maxBullet}");
            _bullets.ForEach(x => x.Initialized());
        }

        //[Button]
        //private void Gen()
        //{
        //    _bullets = new List<BulletUI>();
        //    for (int i = 0; i < 6; i++)
        //    {
        //        var clone = (BulletUI)PrefabUtility.InstantiatePrefab(_angle,_center);
        //        clone.transform.localEulerAngles = Vector3.back*(60*i);
        //        clone.name = "angle_" + i;
        //        _bullets.Add(clone.GetComponent<BulletUI>());
        //    }
        //}


        public void UpdateStatusBullet(int index , BulletState state)
        {
            if (index < 0 || index > _bullets.Count) return;
            _bullets[index].ChangeStatus(state);
            _bulletCountText.SetText($"{index+1}/{_maxBullet}");
        }
    }
}
