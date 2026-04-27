using NFramework;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using static SquidGame.LandScape.Game.CharacterBodySkin;

namespace SquidGame.LandScape.Game
{
    public class CharacterAnimationConfigSO : ScriptableObject
    {
        public List<AnimationConfig> Configs;

        public AnimationConfig GetConfig(EAnimStyle animStyle) => Configs.Find(x => x.Style == animStyle);

#if UNITY_EDITOR
        [Header("EDITOR")]
        [SerializeField] private string _animFolderPath;

        private readonly List<EAnimStyle> _happyAnims = new List<EAnimStyle>()
        {
            EAnimStyle.Victory_1,
            EAnimStyle.Victory_2,
            EAnimStyle.Victory_3,
        };

        private readonly List<EAnimStyle> _sadAnims = new List<EAnimStyle>()
        {
            EAnimStyle.Die,
        };

        //disable b/c : xai mixamo nen bam sync cai mat het anim , ai ranh dau keo tay lai
        //[Button]
        //public void Sync()
        //{
        //    Configs = new List<AnimationConfig>();
        //    foreach (EAnimStyle animStyle in Enum.GetValues(typeof(EAnimStyle)))
        //    {
        //        var clip = FileUtils.LoadFirstAssetWithName<AnimationClip>($"A_{animStyle}", folderPath: _animFolderPath);
        //        var config = new AnimationConfig(animStyle, clip, GetFace(animStyle));
        //        Configs.Add(config);
        //    }

        //    EFaceName GetFace(EAnimStyle style)
        //    {
        //        if (_happyAnims.Contains(style)) return EFaceName.Happy;
        //        if (_sadAnims.Contains(style)) return EFaceName.Sad;

        //        return EFaceName.Normal;
        //    }
        //}
#endif
    }

    [System.Serializable]
    public class AnimationConfig
    {
        public EAnimStyle Style;
        public AnimationClip Clip;
        public EFaceName Face;

        public AnimationConfig(EAnimStyle style, AnimationClip clip, EFaceName faceName)
        {
            Style = style;
            Clip = clip;
            Face = faceName;
        }
    }

    public enum EAnimStyle
    {
        Running,
        Jump,
        Idle,
        Victory_1,
        Victory_2,
        Victory_3,
        Die,
        Stand_Still_Pose_1,
        Stand_Still_Pose_2,
        Stand_Still_Pose_3,
        Falling,
        Throw,
        Pull_Left,
        Pull_Right,
        Throw_1,
        Victory_4,
        Head_Shake,
        Rock_Paper_Scissor1,
        Rock_Paper_Scissor2,
        Win_Round
    }
}
