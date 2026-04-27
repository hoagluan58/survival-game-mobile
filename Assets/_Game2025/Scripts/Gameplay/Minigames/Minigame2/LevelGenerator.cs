using Redcode.Extensions;
using UnityEngine;

namespace SquidGame.LandScape.Minigame2
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("REF")]
        [SerializeField] private GameObject _startMapObject;
        [SerializeField] private GlassPanel _glassPf;
        [SerializeField] private Transform _glassParentTf;
        [SerializeField] private Transform _cameraConfinerTf;
        [SerializeField] private GameObject _barrierTf;

        [Header("CONFIGS")]
        [Header("LEVEL")]
        [SerializeField] private LevelConfigSO _levelConfigSO;
        [SerializeField] private float _minSizeConfinerZ;

        [Header("GLASS PANEL")]
        [SerializeField] private Vector3 _maxGlassPos; // Z will be converted 
        [SerializeField] private Vector3 _distanceBetweenRow;
        [SerializeField] private Vector3 _distanceBetweenCol;
        [SerializeField] private float _offsetZStartMap;

        [Header("BARRIER")]
        [SerializeField] private Vector3 _maxBarrierPos;
        [SerializeField] private float _oppositeBarrierX;
        [SerializeField] private float _distanceBetweenBarrier;
        [SerializeField] private float _offsetZBarrier;


        private LevelData _levelData;
        private GlassPanel[,] _glassPanels;
        public LevelData LevelData => _levelData;
        public GlassPanel[,] GlassPanels => _glassPanels;

        public void SpawnLevel(int levelId)
        {
            _levelData = _levelConfigSO.GetLevelConfig(levelId);
            _levelData.GetFirstGlassPosition(_maxGlassPos, _distanceBetweenRow);
            _glassPanels = new GlassPanel[_levelData.Rows, _levelData.Columns]; // 4, 2

            // Generate glass panels
            for (var i = 0; i < _levelData.Rows; i++)
            {
                var rndTrueMove = Random.Range(0, _levelData.Columns);
                for (var j = 0; j < _levelData.Columns; j++)
                {
                    var glassPos = _levelData.FirstGlassPos + ((_distanceBetweenRow * i) + (_distanceBetweenCol * j));
                    var glass = Instantiate(_glassPf, _glassParentTf);
                    glass.transform.localPosition = glassPos;
                    glass.Init(new GlassPanelData(i, j, j == rndTrueMove));
                    _glassPanels[i, j] = glass;
                }
            }

            // Generate barriers
            var barrierSize = GetBarrierZSize();

            for (var i = 0; i < _levelData.Columns; i++)
            {
                var leftSidePos = _maxBarrierPos + new Vector3(_distanceBetweenBarrier * i, 0, 0);
                var rightSidePos = leftSidePos + new Vector3(_oppositeBarrierX, 0, 0);

                var leftBarrier = Instantiate(_barrierTf, leftSidePos, Quaternion.identity, _glassParentTf);
                var rightBarrier = Instantiate(_barrierTf, rightSidePos, Quaternion.identity, _glassParentTf);

                leftBarrier.transform.SetLocalScaleZ(barrierSize);
                rightBarrier.transform.SetLocalScaleZ(barrierSize);
            }

            // Move object to match glass bridge 
            _startMapObject.transform.SetPositionZ(GetStartMapZ(_levelData.FirstGlassPos.z));
            _cameraConfinerTf.SetLocalScaleZ(GetConfinerZ());

        }

        private float GetBarrierZSize() => _levelData.Rows * _distanceBetweenRow.z + _offsetZBarrier;

        private float GetStartMapZ(float firstGlassZPos) => firstGlassZPos + _offsetZStartMap;

        private float GetConfinerZ() => _minSizeConfinerZ + (_distanceBetweenRow.z * (_levelData.Rows - 1));
    }
}
