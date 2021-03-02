using System;

public static class GameEvents
{
    public static event Action ONLevelComplete;
    public static event Action ONCollidedWithDestructibles;
    public static event Action ONLevelContinue;
    public static event Action ONGameOver;
    public static event Action ONStartGame;
    public static event Action ONMorePoints;
    public static event Action ONPowerUpBuy;
    public static event Action<int> ONBuyItem;

    public static void LevelComplete()
    {
        ONLevelComplete?.Invoke();
    }
    public static void CollidedWithDestructibles()
    {
        ONCollidedWithDestructibles?.Invoke();
    }
    
    public static void LevelContinue()
    {
        ONLevelContinue?.Invoke();
    }

    public static void GameOver()
    {
        ONGameOver?.Invoke();
    }

    public static void StartGame()
    {
        ONStartGame?.Invoke();
    }

    public static void GiveMorePoints()
    {
        ONMorePoints?.Invoke();
    }
    
    public static void PowerUpBuy()
    {
        ONPowerUpBuy?.Invoke();
    }
    
    public static void BuyItem(int amount)
    {
        ONBuyItem?.Invoke(amount);
    }
}
