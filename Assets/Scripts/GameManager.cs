using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   [SerializeField]
   private bool _isGameOver;
   
   private void Update(){
      if (Input.GetKeyDown(KeyCode.R) && _isGameOver) {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      }
      if (Input.GetKeyDown(KeyCode.Escape))
      {
         #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #endif
         Application.Quit();
      }
   }

   public void GameOver() {
      _isGameOver = true;
   }
}
