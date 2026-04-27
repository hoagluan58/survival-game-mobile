using System.Collections;
using System.Collections.Generic;
using NFramework;
using SquidGame.LandScape.Core;
using SquidGame.LandScape.Data;
using UnityEngine;

namespace SquidGame.LandScape.Lobby
{
    public class BotManager : MonoBehaviour
    {
        [SerializeField] private Transform _spawnTransform, _gateTransform, _avatarParentTransform;
        [SerializeField] private BotController _botController;
        [SerializeField] private BotAvatar _botAvatar;

        [SerializeField] private Sprite[] avatarSprites;

        [SerializeField] private float _spawnRadius;
        [SerializeField] private int _baseAmount;

        float[] multipliers = { 1f, 0.82f, 0.66f, 0.33f, 0.165f };
        List<int> botNames = new();
        List<BotAvatar> botAvatars = new();
        int botName;

        public void Initialize(int seasonProgress)
        {
            int seasonProgressIndex = Mathf.Clamp(seasonProgress, 0, multipliers.Length - 1);

            SpawnBot(seasonProgressIndex);
            SetBotAvatarsPanel(seasonProgressIndex);
        }

        void SpawnBot(int seasonProgress)
        {
            System.Random random = new();
            HashSet<int> uniqueNumbers = new();

            while (uniqueNumbers.Count < (multipliers[seasonProgress] * 100))
            {
                uniqueNumbers.Add(random.Next(1, 456));
            }

            botNames.AddRange(uniqueNumbers);
            StartCoroutine(SpawningBots(seasonProgress, new List<int>(botNames)));
        }

        void SetBotAvatarsPanel(int seasonProgress)
        {
            int playerAvatarIndex = 54;
            BotAvatar botAvatar;

            for (int i = 0; i < 100; i++)
            {
                botAvatar = Instantiate(_botAvatar, _avatarParentTransform);
                botAvatars.Add(botAvatar);

                if (i > (multipliers[seasonProgress] * 100))
                {
                    botAvatar.Disable();
                }
                else
                {
                    botName = botNames[Random.Range(0, botNames.Count)];
                    botNames.Remove(botName);
                    botAvatar.SetInfo(botName, avatarSprites.RandomItem());
                }
            }

            botAvatars.Shuffle();
            for (int i = 0; i < botAvatars.Count; i++)
            {
                // if (i == playerAvatarIndex) continue;
                botAvatars[i].transform.SetSiblingIndex(i);
            }

            botAvatars[playerAvatarIndex].HightlightPlayerAvatar();
            botAvatars[playerAvatarIndex].SetInfo(456, avatarSprites[GameData.I.GetData<UserData>(Define.SaveKey.USER_DATA).UserHair - 1]);
        }

        IEnumerator SpawningBots(int seasonProgress, List<int> botNames)
        {
            for (int i = 0; i < GetBotSpawnAmount(seasonProgress); i++)
            {
                Vector3 spawnPos = _spawnTransform.position + new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 0, Random.Range(-_spawnRadius, _spawnRadius));
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);

                botName = botNames[Random.Range(0, botNames.Count)];
                botNames.Remove(botName);

                Instantiate(_botController, spawnPos, randomRotation, transform).Init(_gateTransform.position, botName);
                yield return new WaitForSeconds(0.1f);
            }

            int GetBotSpawnAmount(int seasonProgress)
            {
                return (int)(_baseAmount * multipliers[Mathf.Clamp(seasonProgress, 0, multipliers.Length - 1)]);
            }
        }
    }
}















