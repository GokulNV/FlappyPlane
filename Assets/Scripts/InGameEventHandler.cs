public class InGameEventHandler
{
    public delegate void InputEvent();

    /// <summary>
    /// Event invoked when the player touches the input device.
    /// </summary>
    public static InputEvent PointerDown;

    /// <summary>
    /// Event invoked when the player releases the input device.
    /// </summary>
    public static InputEvent PointerUp;

    public delegate void FuelUpdate(float value);
    /// <summary>
    /// Event invoked when the fuel level is updated.
    /// </summary>
    public static FuelUpdate OnFuelUpdate;

    public delegate void GameEvent();
    /// <summary>
    /// Event invoked when the game starts.
    /// </summary>
    public static GameEvent StartGame;
    /// <summary>
    /// Event invoked when the game is over.
    /// </summary>
    public static GameEvent OnGameOver;

    public delegate void CollisionEvent();
    /// <summary>
    /// Event invoked when the player's object collides with the ground.
    /// </summary>
    public static CollisionEvent CollisionWithGround;

    public delegate void PlaneAction();
    /// <summary>
    /// Event invoked when the player accelerates the plane.
    /// </summary>
    public static PlaneAction Accelerate;

    public delegate void CollectibleEvent(Collectible collectible);
    /// <summary>
    /// Event invoked when the player acquires a collectible object.
    /// </summary>
    public static CollectibleEvent CollectibleAcquired;

    public delegate void ScoreEvent(int score);
    /// <summary>
    /// Event invoked when the player's score is updated.
    /// </summary>
    public static ScoreEvent ScoreUpdate;
}
