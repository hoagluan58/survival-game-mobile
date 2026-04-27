using NFramework;
using UnityEngine;

namespace SquidGame
{
    [System.Serializable]
    public class RewardData
    {
        public ERewardType Type;

        [ConditionalField(nameof(Type), compareValues: ERewardType.Currency)]
        public ECurrencyType CurrencyType;
        [ConditionalField(nameof(Type), compareValues: ERewardType.Hat)]
        public string HatId;

        public Sprite Icon;
        public int Amount;

        public RewardData(ERewardType type, ECurrencyType currencyType, int amount)
        {
            Type = type;
            CurrencyType = currencyType;
            Icon = GetIconSprite(currencyType);
            Amount = amount;
        }

        public RewardData(ERewardType type, int amount)
        {
            Type = type;
            Icon = GetIconSprite();
            Amount = amount;
        }

        public RewardData(ERewardType type, string hatId)
        {
            Type = type;
            HatId = hatId;
            Icon = GetIconSprite();
            Amount = 0;
        }

        private Sprite GetIconSprite(ECurrencyType currencyType = ECurrencyType.Money)
        {
            switch (Type)
            {
                case ERewardType.Currency:
#if UNITY_EDITOR
                    return FileUtils.LoadFirstAssetWithName<Sprite>($"currency_{currencyType}".ToLower());
#else
                    return null;
#endif
                case ERewardType.Hat:
#if UNITY_EDITOR
                    return FileUtils.LoadFirstAssetWithName<Sprite>($"hat_{HatId}".ToLower());
#else
                    return null;
#endif
                default:
                    return null;
            }
        }
    }

    public enum ERewardType
    {
        Currency,
        Hat
    }

    public enum ECurrencyType
    {
        Money
    }
}
