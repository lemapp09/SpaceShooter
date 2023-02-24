using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    #region Parameters

       [SerializeField] 
       private float _speed = 3;
       [SerializeField] 
       [Tooltip("0 - Triple Shot, 1 - Speed, 2 - Shields")]
       private int _powerupId = 1;
    
       private float _horizontalBounds = 9.5f;
       private float _verticalBounds = 5.5f;
       private Player _player;

    #endregion

    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("Player reference in Power-Up is null.");
        }
    }

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
                other.transform.GetComponent<Player>().Damage();
                // 0 - Triple Shot, 1 - Speed, 2 - Shields
                player.Awards(_powerupId);
            }
            Destroy(this.GameObject());
        }

        if (other.transform.CompareTag("Laser")) {
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser != null) {
                
                // 0 - Triple Shot, 1 - Speed, 2 - Shields
                if (_player != null) {
                    _player.Awards(_powerupId);
                }
                other.transform.GetComponent<Laser>().ReturnToPool();
                Destroy(this.GameObject());
            }
            Destroy(this.GameObject());
        }
    }
}
