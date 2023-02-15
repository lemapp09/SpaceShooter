using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Parameters
        [SerializeField]
        private float _speed = 5.0f;
        [SerializeField]
        private GameObject _laserPrefab;
        [SerializeField]
        private Transform _laserPoolContainer;
        [SerializeField]
        private Vector2 _laserLaunchOffset = new Vector3(0f, 0.8f, 0f);
        [SerializeField] 
        private float _laserFireRate = 0.15f;
        [SerializeField]
        private int _lives = 3;

        private Vector3 _inputDirection;
        private float _horizontalBounds = 9.5f;
        private float _verticalBounds = 5.5f;
        private List<GameObject> _laserPool = new List<GameObject>();
        private float _laserNextFire = -1f;
        private SpawnManager _spawnManager;
    #endregion
    
    void Start() {            
         _spawnManager = FindObjectOfType<SpawnManager>();
         if (_spawnManager == null) {
             Debug.LogError("the Spawn Manager is NUll.");
         }
         transform.position = new Vector3( 0, 0, 0 );
         PrePopulateLaserPool();
    }

    void Update() {
        CaptureInputs();
        PlayerInBounds();
    }

    private void FixedUpdate() {
        transform.Translate(_speed * Time.deltaTime * _inputDirection );
    }

    private void CaptureInputs() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(horizontalInput, verticalInput, 0);

        // If both a vertical and a horizontal key are pressed together, space is ignored
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _laserNextFire )
        {
            _laserNextFire = Time.time + _laserFireRate;
            FireLaser();
        }
    }

    private void FireLaser() {
        GameObject laser = RetrieveLaserFromPool();
        laser.transform.position = transform.position + new Vector3(_laserLaunchOffset.x, _laserLaunchOffset.y, 0);
        laser.SetActive(true);
    }

    private void PlayerInBounds() {
        // player game object will wrap around when it reaches edge of screen
        float newPositionX = transform.position.x;
        float newPositionY = transform.position.y;
        if (newPositionX <= -_horizontalBounds) {
            newPositionX = _horizontalBounds;
        }
        else if (newPositionX >= _horizontalBounds) {
            newPositionX = -_horizontalBounds;
        }
        
        if (newPositionY <= -_verticalBounds) {
            newPositionY = _verticalBounds;
        }
        else if (newPositionY >= _verticalBounds) {
            newPositionY = -_verticalBounds;
        }

        transform.position = new Vector3(newPositionX, newPositionY, 0);
    }

    public void Damage() {
        _lives--;
        if (_lives < 1) {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    
    #region LaserPool
    private void PrePopulateLaserPool()  {
        for (int i = 0; i < 40; i++)
        {
            GameObject tempObject = Instantiate(_laserPrefab, Vector3.zero , Quaternion.identity) as GameObject;
            tempObject.transform.parent = _laserPoolContainer;
            tempObject.SetActive(false);
            tempObject.name = "Laser" + i.ToString("000");
            _laserPool.Add(tempObject);
        }
    }

    private GameObject RetrieveLaserFromPool() {
        GameObject laser;
        if (_laserPool.Count > 0)
        {
            laser = _laserPool[_laserPool.Count - 1];
            _laserPool.Remove(laser);
        }
        else {
            laser = Instantiate(_laserPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            laser.transform.parent = _laserPoolContainer;
            laser.name = "laser" + _laserPoolContainer.childCount.ToString("000");
            _laserPool.Add(laser);
        }
        laser.SetActive(true);
        return laser;
    }

    public void ReturnLaserPrefabToPool(GameObject laser) {
        laser.transform.position = Vector3.zero;
        laser.SetActive(false);
        _laserPool.Add(laser);
    }
    #endregion
}
