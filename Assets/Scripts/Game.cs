using UnityEngine;
using System;
using System.Collections;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameField _gameField = null;

    public event Action OnGameBegun;
    public event Action<bool> OnGameEnded;

    public event Action OnLifesCountChanged;
    public event Action OnBombsCountChanged;

    public bool IsGameActive { get; private set; }

    private bool _isBombMode = false;

    public int LifesLeft
    {
        get { return _lifsLeft; }
        set
        {
            _lifsLeft = value;
            if (OnLifesCountChanged != null)
            {
                OnLifesCountChanged();
            }
        }
    }
    private int _lifsLeft;

    public int BombsLeft
    {
        get { return _bombsLeft; }
        set
        {
            _bombsLeft = value;
            if (OnBombsCountChanged != null)
            {
                OnBombsCountChanged();
            }
        }
    }
    private int _bombsLeft;

    private float _bombExplodeDelaySec;

    public float TimeLeft { get; private set; }

    private Vector2 _charactersSpawnRange;

    private GameScreen _screen;
    private UnitsController _unitsCtrl = null;
    private BombsController _bombsCtrl = null;

    private Coroutine _spawnCoroutine = null;

    public void Init(int lifesCount, int bombsCount, float bombExplodeDelaySec, float roundTime, Vector2 charactersSpawnRange)
    {
        _lifsLeft = lifesCount;
        _bombsLeft = bombsCount;
        _bombExplodeDelaySec = bombExplodeDelaySec;
        TimeLeft = roundTime;

        _charactersSpawnRange = charactersSpawnRange;

        _screen = new GameScreen(Camera.main);

        _unitsCtrl = new UnitsController(Camera.main, _screen.Bounds);
        _unitsCtrl.OnUnitPassedBottomLine += OnUnitPassedBottomLine;
        _unitsCtrl.OnUnitKilled += OnUnitKilled;

        _bombsCtrl = new BombsController(Camera.main);

        _gameField.Fit(_screen.Bounds);
    }

    private void OnUnitKilled(UnitBase unit)
    {
        if (unit.Type == UnitType.ForwardDiagonalFriend)
        {
            EndGame(false);
        }
    }

    private void OnUnitPassedBottomLine(UnitBase unit)
    {
        if (unit.Type != UnitType.ForwardDiagonalFriend)
        {
            LifesLeft--;

            if (LifesLeft <= 0)
            {
                EndGame(false);
            }
        }
    }

    public void BeginGame()
    {
        IsGameActive = true;
        _spawnCoroutine = App.Instance.StartCoroutine(SpawnCoroutine());

        if (OnGameBegun != null)
        {
            OnGameBegun();
        }
    }

    public void EndGame(bool win)
    {
        IsGameActive = false;

        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }

        _unitsCtrl.ReturnAllToPool(true);

        if (OnGameEnded != null)
        {
            OnGameEnded(win);
        }
    }

    void Update()
    {
        if (IsGameActive)
        {
            DetectTouches();
            UpdateTime();
        }
    }

    private void DetectTouches()
    {
        Vector2 clickPos = Vector2.zero;
        if (Input.touchSupported)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    clickPos = Input.GetTouch(i).position;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                clickPos = Input.mousePosition;
            }
        }

        if (!_isBombMode)
        {
            _unitsCtrl.OnTouchReleased(clickPos);
        }
        else
        {
            if (_bombsCtrl.Spawn(clickPos, _bombExplodeDelaySec, OnBombExplode))
            {
                BombsLeft--;
                _isBombMode = false;
            }
        }
    }

    private void OnBombExplode(UnitBase[] hits)
    {
        _unitsCtrl.KillUnits(hits);
    }

    private void UpdateTime()
    {
        if (TimeLeft > 0)
        {
            TimeLeft -= Time.deltaTime;
        }
        else
        {
            TimeLeft = 0f;
            EndGame(true);
        }
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_charactersSpawnRange.x, _charactersSpawnRange.y));
            if (IsGameActive)
            {
                _unitsCtrl.Spawn();
            }
        }
    }

    public void UseBomb()
    {
        if (BombsLeft > 0)
        {
            _isBombMode = true;
        }
    }

    void OnDestroy()
    {
        _unitsCtrl.OnUnitPassedBottomLine -= OnUnitPassedBottomLine;
    }
}