using Game1;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots;

    public List<Bot> Bots => _bots;

    public void Init(Game1Control controller)
    {
        foreach (var bot in _bots)
        {
            bot.Init(controller, this);
        }
    }

    public void RemoveBot(Bot bot)
    {
        _bots.Remove(bot);
    }

    public void Stop()
    {
        for (int i = 0; i < _bots.Count; i++)
        {
            _bots[i].StopGame();
        }
    }

    public List<Bot> PickRandomeAliveBots(int count)
    {
        var aliveBots = _bots.Where(x => !x.IsDie && !x.IsWin).ToList();
        if(aliveBots.Count < count)
        {
            return aliveBots;
        }
        return aliveBots.Take(count).ToList();
    }

    public void ShootSingle(Transform headPos)
    {
        
    }
}
