using UnityEngine;

public class App : MonoBehaviour
{
    public static App Instance { get; private set; }

    public ScenesManager ScenesManager { get; private set; }
    public GameSession GameSession { get; private set; }

    public int lifesCount = 3;
    public int bombsCount = 3;
    public float bombExplodeDelaySec = 0.5f;
    public float roundTime = 60f;
    public Vector2 charactersSpawnTimeRange = new Vector2(0.2f, 3f);

    void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(Instance);

        ScenesManager = new ScenesManager();

        GameSession = new GameSession(lifesCount, bombsCount, bombExplodeDelaySec, roundTime, charactersSpawnTimeRange);
        GameSession.MakeNewGame();
    }
}
