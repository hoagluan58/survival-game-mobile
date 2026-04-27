using System;

public enum TypePool
{
    
}

public enum TypeSwipe
{
    Up,
    Right,
    Left,
    DoubleUp,
    None
}
public enum TypePanelUI
{
    PanelMain,
    PanelGameOver,
    PanelGameWin,
    PanelInGame,
    PanelPause,
    PanelSetting,
    PanelShop,
    PanelTutorial,
    PanelSession,
}

public enum TypeStateGame
{
    None,
    Lobby,
    Playing,
    GameOver,
    GameWin,
    Pause
}

public class EnumManager
{
    public static TypePool ConvertToPool(string type)
    {
        return (TypePool)Enum.Parse(typeof(TypePool), type, false);
    }
}