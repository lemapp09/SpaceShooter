using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region Parameters

       [SerializeField] 
       private float _speed = 3;
       [SerializeField] 
       [Tooltip("1 - Triple Shot")]
       private int _awardNumber = 1;
    
       private float _horizontalBounds = 9.5f;
       private float _verticalBounds = 5.5f;

    #endregion

    void Update() { 
        transform.position += (Time.deltaTime * _speed * Vector3.down);
        RemovePowerUpBeyondGameScreen();
    }
    
    private void RemovePowerUpBeyondGameScreen() {
        if (transform.position.x > _horizontalBounds || transform.position.x < -_horizontalBounds ||
            transform.position.y > _verticalBounds || transform.position.y < -_verticalBounds) {
            Destroy(this.GameObject());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) {
                // 1 - Triple Shot, 
                player.Awards(_awardNumber);
            }
            Destroy(this.GameObject());
        }
    }
}
