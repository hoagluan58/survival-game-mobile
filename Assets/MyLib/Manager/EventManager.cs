using System;

public static class EventManager
{
    public static Action OnGameInitDone;
    public static Action OnChangeCoin;
    public static Action OnCloseShop;

    public static Action<int> OnChangeHat;
    public static Action<bool> OnLoadingLoadScene;

    public static void ChangeCoin()
    {
        OnChangeCoin?.Invoke();
    }

    public static void PlayerChangeHat(int id)
    {
        OnChangeHat?.Invoke(id);
    }

    public static void GameInitDone()
    {
        OnGameInitDone?.Invoke();
    }

    public static void CloseShop()
    {
        OnCloseShop?.Invoke();
    }

     public static void LoadingLoadScene(bool isLoad)
    {
        OnLoadingLoadScene?.Invoke(isLoad);
    }
}
