using UnityEngine;

namespace Game10
{
    public class PanelInGame : PanelBase
    {
        [SerializeField] private Transform _pointBar;
        [SerializeField] private Vector2 _rangePoint;

        [SerializeField] private UITapButton _uITapLeft;
        [SerializeField] private UITapButton _uITapRight;


        public UITapButton UITapLeft => _uITapLeft;
        public UITapButton UITapRight => _uITapRight;


        public void UpdateValueBar(float val)
        {
            Vector3 rot = Vector3.zero;
            rot.z = Mathf.Lerp(_rangePoint.x, _rangePoint.y, val);

            _pointBar.eulerAngles = rot;
        }
    }
}