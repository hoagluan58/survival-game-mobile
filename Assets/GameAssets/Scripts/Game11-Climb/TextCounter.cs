using NFramework;
using TMPro;
using UnityEngine;

namespace Game11
{
    public class TextCounter : MonoBehaviour
    {
        [SerializeField] private GameObject _sample;
        [SerializeField] private float _disPer100m;
        [SerializeField] private int _startDis;

        [ButtonMethod]
        [ContextMenu("Create")]
        public void Create()
        {
            while (transform.childCount > 1)
                DestroyImmediate(transform.GetChild(1).gameObject);

            Vector3 posCreate = Vector3.zero;
            int _contentTmp = _startDis;

            for (int i = 0; i <= _startDis / 100; i++)
            {
                TextMeshPro tmp = Instantiate(_sample, transform).GetComponent<TextMeshPro>();
                tmp.gameObject.SetActive(true);
                tmp.transform.localPosition = posCreate;
                tmp.text = _contentTmp + "-";

                _contentTmp -= 100;
                posCreate.y += _disPer100m;
            }
        }
    }
}
