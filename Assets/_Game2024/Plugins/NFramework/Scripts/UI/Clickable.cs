using UnityEngine;
using UnityEngine.UI;

namespace NFramework
{
    public class Clickable : MonoBehaviour
    {
        [SerializeField] private float _alphaThreshold = 0.1f;

        private void Start() => GetComponent<Image>().alphaHitTestMinimumThreshold = _alphaThreshold;
    }
}