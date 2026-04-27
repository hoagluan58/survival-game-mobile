using NFramework;
using Old;
using SquidGame.Gameplay;
using UnityEngine;

public class GameMaster : SingletonMono<GameMaster>
{
    [SerializeField] private int[] _gameSceneIDs;

    public TypeStateGame TypeStateGame { get; set; }

    public long COIN { get { return DataManager.I.GameData.Coin; } set { DataManager.I.GameData.Coin = value; DataManager.I.SaveData(); EventManager.ChangeCoin(); } }
    public int ROUND { get { return DataManager.I.GameData.Round; } set { DataManager.I.GameData.Round = value; DataManager.I.SaveData(); } }
    public int LEVEL { get { return DataManager.I.GameData.Level; } set { DataManager.I.GameData.Level = value; DataManager.I.SaveData(); } }

    private BaseMinigameController _gamePlayControl;

    protected override void Awake()
    {
        base.Awake();
        _gamePlayControl = FindObjectOfType<BaseMinigameController>();
    }

    private void Start() => InitGame();

    public void InitGame()
    {
        TypeStateGame = TypeStateGame.Lobby;
        _gamePlayControl?.OnLoadMinigame();
        Log.Info("Init Game");
    }

    public void StartGame()
    {
        TypeStateGame = TypeStateGame.Playing;
        _gamePlayControl?.OnStart();
        Log.Info("Start Game");
    }

    public void GameWin()
    {
        if (TypeStateGame != TypeStateGame.Playing)
            return;
        TypeStateGame = TypeStateGame.GameWin;

        int level = PlayerPrefs.GetInt("level", 0);

        if (level == 0)
        {
            // Debug.LogErrorFormat("=====round: {0}", ROUND);
            level += 1;
            PlayerPrefs.SetInt("level", level); // lv1
        }
        else
        {
            level += 1;
            PlayerPrefs.SetInt("level", level);
        }

        _gamePlayControl?.OnWin();
        Log.Info("Game Win");

        NextRound();
        Static.TypeGameResult = TypeGameResult.Win;
    }

    public void GameOver()
    {
        if (TypeStateGame != TypeStateGame.Playing)
            return;
        TypeStateGame = TypeStateGame.GameOver;

        _gamePlayControl?.OnLose();
        Log.Info("Game Over");

        Static.TypeGameResult = TypeGameResult.Lose;
    }

    public void Revive()
    {
        TypeStateGame = TypeStateGame.Playing;
        _gamePlayControl?.OnRevive();
        Log.Info("Revive");
    }

    public void NextRound(bool isTrack = false)
    {
        ROUND++;

        if (isTrack)
        {
            int level = PlayerPrefs.GetInt("level", 0);

            if (level == 0)
            {
                level += 1;
                PlayerPrefs.SetInt("level", level); // lv1
            }
            else
            {
                level += 1;
                if (level < 11)
                {
                }
                PlayerPrefs.SetInt("level", level);
            }
        }

        if (ROUND == _gameSceneIDs.Length)
        {
            ROUND = 0;
            LEVEL++;
        }
    }

    public void LoadResult()
    {
        MyLib.SoundManager.I?.StopMusic();
        UIAllGame.I?.DisableTouchUI();
        //UIAllGame.I?.LoadScene(Define.SceneName.RESULT);
    }

    public void LoadGame(float delay = 0f)
    {
        UIAllGame.I?.DisableTouchUI();
        Invoke(nameof(Reload), delay);
    }

    private void Reload()
    {
        EffectManager.I?.HideEffectAll();
        MyLib.SoundManager.I?.StopMusic();
        MyLib.SoundManager.I?.StopFX();
        //_gamePlayControl?.OnRestart();
        UIAllGame.I?.LoadScene(_gameSceneIDs[ROUND]);
    }
}

public enum TypeGameResult
{
    Win,
    Lose,
    Reward,
    Revive
}
