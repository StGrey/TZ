using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Game _game = null;
    [SerializeField]
    private GameObject _restartBtnObj = null;
    [SerializeField]
    private Text _timeLeftLabel = null;
    [SerializeField]
    private Text _bombLeftLabel = null;
    [SerializeField]
    private Text _lifesLeftLabel = null;
    [SerializeField]
    private Button _useBombBtn = null;
    [SerializeField]
    private GameObject _winWindow = null, _loseWindow = null;

    void Awake()
    {
        SetRestartBtnObjState(false);
    }

    void Start()
    {
        _game.OnGameEnded += OnGameEnded;
        _game.OnLifesCountChanged += OnLifesCountChanged;
        _game.OnBombsCountChanged += OnBombsCountChanged;
    }

    private void OnBombsCountChanged()
    {
        _bombLeftLabel.text = string.Format("{0} Bombs", _game.BombsLeft);
        _useBombBtn.enabled = _game.BombsLeft > 0;
    }

    private void OnLifesCountChanged()
    {
        _lifesLeftLabel.text = string.Format("{0} Lifes", _game.LifesLeft);
    }

    private void OnGameEnded(bool win)
    {
        SetRestartBtnObjState(true);

        if (win)
        {
            _winWindow.SetActive(true);
        }
        else
        {
            _loseWindow.SetActive(true);
        }
    }

    void Update()
    {
        _timeLeftLabel.text = string.Format("{0} Time", (int)_game.TimeLeft);
    }

    public void OnResetBtnClicked()
    {
        App.Instance.GameSession.RestartCurrentGame();
    }

    public void UseBombBtnClicked()
    {
        _game.UseBomb();
    }

    private void SetRestartBtnObjState(bool isActive)
    {
        _restartBtnObj.SetActive(isActive);
    }
}