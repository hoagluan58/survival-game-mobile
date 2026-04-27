using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SquidGame.LandScape.Minigame4
{

    [CreateAssetMenu(fileName = "CameraConfig" , menuName = "Game/Minigame4/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private CameraData _default;
        [SerializeField] private CameraData _playGame;
        [SerializeField] private CameraData _playerCameraData;
        public CameraData Default => _default;
        public CameraData PlayGame => _playGame;
        public CameraData PlayerCameraData => _playerCameraData;
    }


    [System.Serializable]
    public class CameraData
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }
}
