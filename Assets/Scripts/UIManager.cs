using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    private TextMeshProUGUI _gameOverText;
    [SerializeField] 
    private TextMeshProUGUI _restartGameText;
    [SerializeField] 
    private TextMeshProUGUI _gameWon;
    
    private GameManager _gameManager;
  #endregion
    private void Start() {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null) {
            Debug.Log("UIManger :: Start : GameManager is NULL ");
        }
    }

    public void UpdateScore(int score) {
        _scoreText.text = "Score: " + score.ToString();
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

    public void ToggleInputScreen(bool OnOff)
    {
        _inputsScreen.SetActive(OnOff);
    }
    

}
