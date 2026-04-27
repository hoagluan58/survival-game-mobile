using DG.Tweening;
using NFramework;
using System.Collections;
using SquidGame.Core;
using UnityEngine;
using SquidGame.LandScape;

namespace Game8
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private GameObject _glassMain;
        [SerializeField] private GameObject _glassPink;
        [SerializeField] private GameObject _glassGreen;
        [SerializeField] private GameObject _glassBreakPf;
        [SerializeField] private SerializableDictionary<TypeShape, GameObject> _shapeMapper;

        private GameObject _curGlassBreak;
        private bool _isTrue;
        private Vector3 _originalScale = new Vector3(3f, 1f, 4f);

        public bool IsTrue { get { return _isTrue; } }

        private bool _isActive = false;

        public bool IsBroken;

        public void Init(TypeShape typeShape, bool isTrue)
        {
            _isTrue = isTrue;
            _shapeMapper[typeShape].SetActive(true);
            _curGlassBreak = null;
            IsBroken = false;
            if (_isTrue)
                _shapeMapper[typeShape].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", Color.green);
        }

        public void StartAnim(int id)
        {
            StartCoroutine(IE_Anim(id));
        }

        private IEnumerator IE_Anim(int id)
        {
            while (!_isActive)
            {
                if (id % 2 == 0)
                {
                    _glassPink.SetActive(true);
                    _glassGreen.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                    _glassPink.SetActive(false);
                    _glassGreen.SetActive(true);
                }
                else
                {
                    _glassPink.SetActive(false);
                    _glassGreen.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    _glassPink.SetActive(true);
                    _glassGreen.SetActive(false);
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        public void Active()
        {
            _isActive = true;
            _glassPink.SetActive(false);
            _glassGreen.SetActive(false);
            _glassMain.SetActive(true);
            transform.DOScale(Vector3.zero, 0.25f);
        }

        public void Show()
        {
            DOTween.Kill(transform);
            transform.localScale = Vector3.zero;
            transform.DOScale(_originalScale, 0.25f);
        }

        public void Break()
        {
            GameSound.I.PlaySFX(Define.SoundPath.SFX_MG02_GLASS_BREAK);
            HideAllShape();
            if (_curGlassBreak == null)
            {
                _curGlassBreak = Instantiate(_glassBreakPf, transform);
                _curGlassBreak.transform.localPosition = Vector3.zero;
                _curGlassBreak.SetActive(true);
                this.InvokeDelay(3f, () => Destroy(_curGlassBreak));
            }
            _glassMain.SetActive(false);
            IsBroken = true;
        }

        private void HideAllShape()
        {
            _glassPink.SetActive(false);
            _glassGreen.SetActive(false);
            foreach (var item in _shapeMapper)
            {
                item.Value.SetActive(false);
            }
        }
    }
}
