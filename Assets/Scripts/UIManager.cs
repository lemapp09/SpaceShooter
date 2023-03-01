using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
  #region Parameters
    [Header("UI")] 
    [SerializeField] 
    private GameObject _inputsScreen;
    [SerializeField] 
    private GameObject _noLives;
    [SerializeField] 
    private GameObject _oneLife;
    [SerializeField] 
    private GameObject _twoLives;
    [SerializeField] 
    private GameObject _threeLives;
    [SerializeField] 
    private TextMeshProUGUI _scoreText;
    [SerializeField] 
    private TextMeshProUGUI _ammoText;
    [SerializeField] 
    private TextMeshProUGUI _gameOverText;
    [SerializeField] 
    private TextMeshProUGUI _restartGameText;
    [SerializeField] 
    private GameObject _numberMOAGText;
    [SerializeField]
    private int _moagDisplayCount = 0;
    [SerializeField] 
    private TextMeshProUGUI _gameWon;
    [SerializeField] 
    private Scrollbar _thrusterDurationDisplay;
    
    private GameManager _gameManager;
    public static UIManager Instance;
  #endregion

  private void Awake() {
      Instance = this;
  }
  
    private void Start() {
        _gameManager = GameManager.Instance;
        if (_gameManager == null) {
            Debug.Log("UIManger :: Start : GameManager is NULL ");
        }
    }

    public void UpdateScore(int score) {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateAmmo(int ammo) {
        _ammoText.text = "Ammo: " + ammo.ToString();
    }

    public void UpdateLives(int lives)
    {
        switch (lives)
        {
            case 0:
                _noLives.SetActive(true);
                _oneLife.SetActive(false);
                _twoLives.SetActive(false);
                _threeLives.SetActive(false);
                break;
            case 1:
                _noLives.SetActive(false);
                _oneLife.SetActive(true);
                _twoLives.SetActive(false);
                _threeLives.SetActive(false);
                break;
            case 2:
                _noLives.SetActive(false);
                _oneLife.SetActive(false);
                _twoLives.SetActive(true);
                _threeLives.SetActive(false);
                break;
            case 3:
                _noLives.SetActive(false);
                _oneLife.SetActive(false);
                _twoLives.SetActive(false);
                _threeLives.SetActive(true);
                break;
        }
    }

    public void GameOver() {
        _gameManager.GameOver();
        _gameOverText.enabled = true;
        _restartGameText.enabled = true;
        StartCoroutine(GameOverFlickerRoutine());
    }

    private IEnumerator GameOverFlickerRoutine() {
        while (true) {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = true;
        }
    }
    
    public void GameWon() {
        _gameWon.enabled = true;
    }

    public void ToggleInputScreen(bool OnOff) {
        _inputsScreen.SetActive(OnOff);
    }

    public void DisplayNumberOfMOAGLasersText() {
        _moagDisplayCount++;
        if (_moagDisplayCount < 6) {
            _numberMOAGText.SetActive(true);
            StartCoroutine(DisplayMOAGOneSec());
        }
    }

    public void StartThrusterCoolDown(float _timePrizesLast)
    {
        if (_timePrizesLast > 0) {
            StartCoroutine(ThrusterDisplayCoolDown(_timePrizesLast));
        }
    }

    private IEnumerator ThrusterDisplayCoolDown(float _timePrizesLast)
    {
        _thrusterDurationDisplay.gameObject.SetActive(true);
        int i = 0;
        float size = 1f;
        while (i < _timePrizesLast * 100) {
            yield return new WaitForSeconds(0.01f);
            size -= (1/ (_timePrizesLast * 100));
            _thrusterDurationDisplay.size = size;
            i++;
        }
        _thrusterDurationDisplay.gameObject.SetActive(false);
    }

    private IEnumerator DisplayMOAGOneSec()
    {
        yield return new WaitForSeconds(1f);
        _numberMOAGText.SetActive(false);
    }
}
