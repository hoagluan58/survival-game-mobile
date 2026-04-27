using UnityEngine;

namespace SquidGame.LandScape.MinigameMingle
{
    public class RamdomColorMesh : MonoBehaviour
    {
        [SerializeField] private bool _isRandomColor = true;

        private void OnEnable()
        {
            if (_isRandomColor)
            {
                var rend = GetComponent<Renderer>();
                rend.material.color = GetRandomRainbowColor();
            }
        }

        private Color GetRandomRainbowColor()
        {
            float hue = UnityEngine.Random.Range(0f, 1f);
            return Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}
