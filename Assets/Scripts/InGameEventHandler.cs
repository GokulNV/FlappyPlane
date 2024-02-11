public class InGameEventHandler
{
    public delegate void InputEvent();
    public static InputEvent PointerDown;
    public static InputEvent PointerUp;

    public delegate void FuelUpdate(float value);
    public static FuelUpdate OnFuelUpdate;

    public delegate void GameEvent();
    public static GameEvent StartGame;
    public static GameEvent OnGameOver;

    public delegate void CollisionEvent();
    public static CollisionEvent CollisionWithGround;

    public delegate void PlaneAction();
    public static PlaneAction Accelerate;

    public delegate void CollectibleEvent(Collectible collectible);
    public static CollectibleEvent CollectibleAcquired;

    public delegate void ScoreEvent(int score);
    public static ScoreEvent ScoreUpdate;
}