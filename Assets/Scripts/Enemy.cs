using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
  #region Parameters

    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _shield;
    private bool _isDead;
    private float _horizontalBounds = 9f;
    private float _verticalBounds = 5.5f;
    private float _speed = 4f;
    private Player _player;
  #endregion
    
    private void Start() {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) {
            Debug.Log("Player reference in Enemy is null.");
        }
        if (_explosion == null) {
            Debug.Log("Explosion is null in enemy");
        }
        if (_shield == null) {
            Debug.Log("Shield is null in enemy");
        }
    }
    
    private void Update()
    {
        MoveEnemyBeyondGameScreen();
        EnemyMovement();
    }

    private void EnemyMovement() {
        if (!_isDead) {
            transform.Translate(_speed * Time.deltaTime * Vector3.down);
        }
    }

    private void MoveEnemyBeyondGameScreen()
    {
        if (!_isDead) {
            if (transform.position.y < -_verticalBounds) {
                float randomX = Random.Range(-_horizontalBounds, _horizontalBounds);
                transform.position = new Vector3(randomX, _verticalBounds, 0f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isDead) {
            if (other.transform.CompareTag("Player")) {
                Player player = other.transform.GetComponent<Player>();
                if (player != null) {
                    other.transform.GetComponent<Player>().Damage();
                }
                StartCoroutine(DeathSequence());
            }

            if (other.transform.CompareTag("Laser")) {
                Laser laser = other.transform.GetComponent<Laser>();
                if (laser != null) {
                    other.transform.GetComponent<Laser>().ReturnToPool();
                }
                _player.EnemyKill(10);
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