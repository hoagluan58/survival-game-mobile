using NFramework;
using UnityEngine;

namespace Game11
{
    public class StickTarget : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerCheck;
        [SerializeField] private GameObject _xObject;
        [SerializeField] private GameObject _oObject;


        private bool _isActive;
        private bool _isCanStick;

        public void SetActive(bool b)
        {
            _isActive = b;
            gameObject.SetActive(b);
        }

        public bool IsCanStick()
        {
            return _isCanStick;
        }


        private void Update()
        {
            if (!_isActive) return;

            RaycastHit hit;
            if (Physics.Raycast(transform.position - Vector3.forward, Vector3.forward, out hit, 5f, _layerCheck))
            {
                GameObject hitObject = hit.transform.gameObject;
                Vector3 wantedPos = transform.position;
                wantedPos.z = hit.point.z - 0.1f;
                transform.position = wantedPos;

                if (hitObject.CompareTag(Tag.Stone))
                {
                    _xObject.gameObject.SetActive(true);
                    _oObject.gameObject.SetActive(false);
                    _isCanStick = false;
                }
                else if (hitObject.CompareTag(Tag.Wall))
                {
                    _xObject.gameObject.SetActive(false);
                    _oObject.gameObject.SetActive(true);
                    _isCanStick = true;
                }
            }
        }
    }
}