using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{    
    #region Parameters
        [SerializeField] 
        private float _speed = 8f;

        [SerializeField] 
        private bool _isEnemyLaser;
        
        private Player _player;
        private float _horizontalBounds = 9.5f;
        private float _verticalBounds = 5.5f;
    #endregion
    
    private void Start() {
        if (!_isEnemyLaser) {
            _player = FindObjectOfType<Player>().GetComponent<Player>();
        }
    }

    private void Update() {
        if (_isEnemyLaser) {
            transform.Translate(Time.deltaTime * _speed * Vector3.down);
        }
        else {
            transform.Translate(Time.deltaTime * _speed * Vector3.up);
        }

        RemoveLaserBeyondGameScreen();
    }

    private void RemoveLaserBeyondGameScreen() {
        if (transform.position.x > _horizontalBounds || transform.position.x < -_horizontalBounds ||
            transform.position.y > _verticalBounds || transform.position.y < -_verticalBounds) {
            if (!_isEnemyLaser) {
                ReturnToPool();
            }
            else { 
                Destroy(this.GameObject());
            }
        }
    }

    public void ReturnToPool() {
        _player.ReturnLaserPrefabToPool(this.GameObject());
    }
}
