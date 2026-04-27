using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Game8
{
    public class BoardControl : MonoBehaviour
    {
        private Line[] _lines;
        private int _nextLine;
        public bool IsCanJump { get; set; }

        private Game8Control _controller;

        private void Awake()
        {
            _lines = GetComponentsInChildren<Line>();
        }

        public void Init(Game8Control controller)
        {
            _controller = controller;
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].Init(i);
            }
        }

        public void Active()
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].Active();
            }

            _nextLine = -1;
            ShowNextLine(0.25f);
        }

        public Tile GetTileJump(TypeShape typeShape)
        {
            Tile tile = _lines[_nextLine].GetPos(typeShape);
            return tile;
        }

        public void ShowNextLine(float delay = 0f)
        {
            _nextLine++;
            if (_nextLine == _lines.Length)
            {
                _controller.PlayerControl.JumpToWin();
                return;
            }
            StartCoroutine(IE_ShowNextLine(delay));
        }

        private IEnumerator IE_ShowNextLine(float delay)
        {
            yield return new WaitForSeconds(delay);
            _lines[_nextLine].Show();

            yield return new WaitForSeconds(0.5f);
            IsCanJump = true;
        }

        public void BreakAll()
        {
            _lines[_nextLine - 1].BreakAll();
            _controller.PlayerControl.Fall();
        }

        public void Revive()
        {
            IsCanJump = true;
        }
    }
}
