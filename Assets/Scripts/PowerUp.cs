using System.Collections;
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
       [SerializeField]
       private GameObject _explosion;

       private bool _isDead;
       private float _horizontalBounds = 9.5f;
       private float _verticalBounds = 5.5f;
       private Player _player;

    #endregion

    private void Start() {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) {
            Debug.Log("Player reference in Power-Up "+ _powerupId+" is null.");
        }
        if (_explosion == null) {
            Debug.Log("PowerUp# "+ _powerupId + " Explosion is null");
        }
    }

    void Update() {
        if (!_isDead) {
            transform.position += (Time.deltaTime * _speed * Vector3.down);
            RemovePowerUpBeyondGameScreen();
        }
    }
    
    private void RemovePowerUpBeyondGameScreen() {
        if (transform.position.x > _horizontalBounds || transform.position.x < -_horizontalBounds ||
            transform.position.y > _verticalBounds || transform.position.y < -_verticalBounds) {
            Destroy(this.GameObject());
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isDead) {
            if (other.transform.CompareTag("Player")) {
                Player player = other.transform.GetComponent<Player>();
                if (player != null) {
                    other.transform.GetComponent<Player>().Damage();
                    // 0 - Triple Shot, 1 - Speed, 2 - Shields
                    player.Awards(_powerupId);
                }
                StartCoroutine(DeathSequence());
            }

            if (other.transform.CompareTag("Laser")) {
                Laser laser = other.transform.GetComponent<Laser>();
                if (laser != null) {

                    // 0 - Triple Shot, 1 - Speed, 2 - Shields
                    if (_player != null) {
                        _player.Awards(_powerupId);
                    }
                    other.transform.GetComponent<Laser>().ReturnToPool();
                }
                StartCoroutine(DeathSequence());
            }
        }
    }
    
    IEnumerator DeathSequence() {
        _isDead = true;
        this.GetComponent<SpriteRenderer>().enabled = false;
        _explosion.SetActive(true);
        yield return new WaitForSeconds(2.633f);
        Destroy(this.GameObject());
    }
}
