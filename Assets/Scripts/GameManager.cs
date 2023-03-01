using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   [SerializeField]
   private bool _isGameOver;

   public bool _isThruisterActive { get; private set; }
   private float _thrusterDuration = 2.5f;
   
   [SerializeField]
   public  bool _cameraCanShakeAgain { get; private set; }
   public static GameManager Instance;

   private void Awake() {
      Instance = this;
      _isThruisterActive = false;
      _cameraCanShakeAgain = true;
   }
   private void Update(){
      if (Input.GetKeyDown(KeyCode.R) && _isGameOver) {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
      if (Input.GetKeyDown(KeyCode.Escape)) {
         #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #endif
         Application.Quit();
      }
      if (Input.GetKeyDown(KeyCode.LeftShift)) {
         _isThruisterActive = true;
         if (_cameraCanShakeAgain) {
            StartCoroutine(CountDownToNextCameraShake());
         }
      }
      
      if (Input.GetKeyUp(KeyCode.LeftShift)) {
         _isThruisterActive = false;
      }
   }

   private IEnumerator CountDownToNextCameraShake()
   {
      CameraShake.Instance.ShakeCamera();
      _cameraCanShakeAgain = false;
      yield return new WaitForSeconds(_thrusterDuration);
      _cameraCanShakeAgain = true;
   }

   public void GameOver() {
      _isGameOver = true;
   }
}
