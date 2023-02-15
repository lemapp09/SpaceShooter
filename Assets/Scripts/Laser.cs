using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{    
    #region Parameters
        [SerializeField] 
        private float _speed = 8f;
    
        private Player _player;
        private float _horizontalBounds = 9.5f;
        private float _verticalBounds = 5.5f;
    #endregion
    
    private void Start() {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
    }

    private void FixedUpdate() {
        transform.Translate(Time.deltaTime * _speed * Vector3.up);
    }

    private void Update() {
        RemoveLaserBeyondGameScreen();
    }

    private void RemoveLaserBeyondGameScreen() {
        if (transform.position.x > _horizontalBounds || transform.position.x < -_horizontalBounds ||
            transform.position.y > _verticalBounds || transform.position.y < -_verticalBounds) {
            ReturnToPool();
        }
    }

    public void ReturnToPool() {
        _player.ReturnLaserPrefabToPool(this.GameObject());
    }
}
