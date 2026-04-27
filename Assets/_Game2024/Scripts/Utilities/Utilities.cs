using SquidGame.SaveData;
using System;
using System.Collections.Generic;

namespace SquidGame
{
    public static class Utilities
    {
        public static List<RewardData> ParseStringToRewardDatas(string dataString)
        {
            var result = new List<RewardData>();

            if (string.IsNullOrEmpty(dataString))
                return result;

            var datas = dataString.Split(";");
            foreach (var data in datas)
            {
                var splitDatas = data.Split("-");

                if (!Enum.TryParse<ERewardType>(splitDatas[0], out var rewardType))
                    return result;

                switch (rewardType)
                {
                    case ERewardType.Currency:
                        if (Enum.TryParse<ECurrencyType>(splitDatas[1], out var currencyType))
                        {
                            result.Add(new RewardData(ERewardType.Currency, currencyType, int.Parse(splitDatas[2])));
                        }
                        break;
                    case ERewardType.Hat:
                        result.Add(new RewardData(ERewardType.Hat, splitDatas[1]));
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static void HandleRewards(List<RewardData> rewards)
        {
            foreach (var reward in rewards)
            {
                switch (reward.Type)
                {
                    case ERewardType.Currency:
                        HandleCurrency(reward.CurrencyType, reward.Amount);
                        break;
                    case ERewardType.Hat:
                        HandleHat(reward.HatId);
                        break;
                }
            }

            void HandleCurrency(ECurrencyType currencyType, int amount)
            {
                switch (currencyType)
                {
                    case ECurrencyType.Money:
                        UserData.I.Coin += amount;
                        break;
                }
            }

            void HandleHat(string hatId)
            {
                UserData.I.UnlockSkin(int.Parse(hatId));
            }
        }

        public static List<T> ParseStringToList<T>(string dataString, string delimeter)
        {
            var result = new List<T>();
            if (string.IsNullOrEmpty(dataString))
                return result;

            var splitData = dataString.Split(delimeter);
            foreach (var data in splitData)
            {
                result.Add((T)Convert.ChangeType(data, typeof(T)));
            }
            return result;
        }
    }
}
