using UnityEngine;

public class GameSession
{
    public readonly int _lifesLeft;
    public readonly int _bombsCount;
    public readonly float _bombExplodeDelaySec;
    public readonly float _timeLeft;
    private readonly Vector2 _charactersSpawnRange;

    public GameSession(int lifesCount, int bombsCount, float bombExplodeDelaySec, float roundTime, Vector2 charactersSpawnRange)
    {
        _lifesLeft = lifesCount;
        _bombsCount = bombsCount;
        _bombExplodeDelaySec = bombExplodeDelaySec;
        _timeLeft = roundTime;
        _charactersSpawnRange = charactersSpawnRange;
    }

    public void MakeNewGame()
    {
        App.Instance.ScenesManager.LoadScene(SceneName.Game, null, OnGameSceneLoaded);
    }

    public void RestartCurrentGame()
    {
        MakeNewGame();
    }

    private void OnGameSceneLoaded()
    {
        Game game = Object.FindObjectOfType<Game>();
        game.Init(_lifesLeft, _bombsCount, _bombExplodeDelaySec, _timeLeft, _charactersSpawnRange);
        game.BeginGame();
    }
}