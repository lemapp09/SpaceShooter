using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  #region Parameters
    [SerializeField]
    private RectTransform _galaxyShooter;

    [SerializeField]
    private GameObject _inputs;

    [SerializeField] 
    private GameObject _viewInputs;

    [SerializeField]
    private GameObject _newGame;

    [SerializeField]
    private GameObject _gamedevHQLogo;

    private bool _toggleInputsScreen;
  #endregion
  
    private void Start() {
        StartCoroutine(OpeningRoutine());
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            ToggleViewInputs();
        }
    }

    public void LoadGame() {
        SceneManager.LoadScene(1);
    }

    public void ToggleViewInputs() {
        _toggleInputsScreen = !_toggleInputsScreen;
        _inputs.SetActive(_toggleInputsScreen);
    }

    IEnumerator OpeningRoutine()
    {
        float GalaxyShooterPosX = 558;
        float GalaxyShooterScale = 0;
        _inputs.SetActive(false);
        _viewInputs.SetActive(false);
        _newGame.SetActive(false);
        _gamedevHQLogo.SetActive(false);
        while (GalaxyShooterPosX > -217)
        {
            _galaxyShooter.anchoredPosition = new Vector2(GalaxyShooterPosX, 41f); // -216, 41
            _galaxyShooter.localScale = new Vector3(GalaxyShooterScale, GalaxyShooterScale, GalaxyShooterScale);
            yield return new WaitForSeconds(0.05f);
            GalaxyShooterPosX -= 7.74f;
            GalaxyShooterScale += 0.01f;
        }
        _gamedevHQLogo.SetActive(true);
        _galaxyShooter.anchoredPosition = new Vector2(-216f, 41f);
        _galaxyShooter.localScale = Vector3.one;
        _viewInputs.SetActive(true);
        _newGame.SetActive(true);
    }
}
