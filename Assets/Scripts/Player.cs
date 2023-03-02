using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Parameters
        [Header("Speed")]
        [SerializeField]
        private float _speed = 5.0f;
        [SerializeField]
        [Tooltip("The magnitude that speed increases when" +
                 "player is awarded speed boost")]
        // Initial requested speed was 8.5 (later 10)
        private float _speedBoostFactor = 1.7f;
        private float _speedUpFactor = 1.0f;
        
        [Header("Lasers")]
        [SerializeField]
        private GameObject _laserPrefab;
        [SerializeField]
        private Transform _laserPoolContainer;
        [SerializeField]
        private Vector3 _laserLaunchOffset = new Vector3(0f, 0.8f, 0f);
        [SerializeField]
        private Vector3 _leftTripleOffset = new Vector3(-0.622f, -0.156f, 0f);
        [SerializeField]
        private Vector3 _rightTripleOffset = new Vector3(0.622f, -0.156f, 0f);
        [SerializeField] 
        private float _laserFireRate = 0.15f;
        [SerializeField] 
        private int _ammoCount = 0;
        [SerializeField]
        [Tooltip("Set to the starting amount of ammo and the most allowed in game")]
        private int _maxAmmoCount = 15;

        [Header("Shield")] 
        [SerializeField] 
        private GameObject _shieldPrefab;
        [SerializeField] 
        private int _shieldHits;

        [Header("Thrusters")] 
        [SerializeField] 
        private GameObject _thrustersPreFab;
        private bool _isThrusterActive;
        
        [Header("Lives")]
        [SerializeField]
        private int _lives = 3;
        private bool _isPlayerDead;
        
        [Header("Prizes")]
        [SerializeField]
        private bool _isTripleShotActive = true;
        [SerializeField]
        private bool _isSpeedBoostActive;
        [SerializeField]
        private bool _isShieldActive;
        [SerializeField]
        private bool _isMOAGActive;
        [SerializeField] 
        [Range(3, 10)] 
        private int _numberOfMOAGLasers = 9;
        [SerializeField]
        private float _timePrizesLast = 5f;

        private Vector3 _inputDirection;
        private float _horizontalBounds = 9.5f;
        private float _verticalBounds = 5.5f;
        private List<GameObject> _laserPool = new List<GameObject>();
        private float _laserNextFire = -1f;

        [Header("Player")] 
        [SerializeField]
        private GameObject _platerHurtLeft;
        [SerializeField]
        private GameObject _platerHurtRight;
        private bool _isPlayerHurtActive;

        [Header("UI Manager")] [SerializeField]
        private UIManager _uiManager;
        private int _score;
        private bool _toggleInputsScreen;

        [Header("Spawn Manager")] 
        [SerializeField]
        private SpawnManager _spawnManager;

        [Header("Audio Clips")]
        [SerializeField]
        private AudioClip _explosionSoundClip;
        [SerializeField]
        private AudioClip _laserSoundClip;
        private AudioSource _audioSource;

        [Header("Explosion")] 
        [SerializeField] 
        private GameObject _explosion;
        #endregion
    
    void Start()
    {
         if ( _spawnManager == null) {
             Debug.LogError("The Spawn Manager on Player  is NUll.");
         }

         _audioSource = this.GetComponent<AudioSource>();
         if (_audioSource == null){
             Debug.Log("The Audio Source on Player is NULL.");
         }
         else {
             _audioSource.clip = _laserSoundClip;
         }
         transform.position = new Vector3( 0, -4, 0 );
         PrePopulateLaserPool();
         _shieldPrefab.SetActive(false);
         _platerHurtLeft.SetActive(false);
         _platerHurtRight.SetActive(false);
         _ammoCount = _maxAmmoCount;
         UIManager.Instance.UpdateAmmo(_ammoCount);
    }

    void Update() {
        CaptureInputs();
        PlayerInBounds();
        if (GameManager.Instance._isThruisterActive) {
            _speedUpFactor = _speedBoostFactor;
        } else {
            _speedUpFactor = 1f;}
        transform.Translate(_speed * _speedUpFactor * Time.deltaTime * _inputDirection );
    }

    private void CaptureInputs() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        _inputDirection = new Vector3(horizontalInput, verticalInput, 0);
        if ((horizontalInput != 0 || verticalInput != 0 ) && !_isThrusterActive && !_isPlayerDead) {
            StartCoroutine(AnimateThrusters());
        }

        // If both a vertical and a horizontal key are pressed together, space is ignored
        // ↑→ works, ↑← fails, ↓→ fails, ↓→ fails
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _laserNextFire )
        {
            _laserNextFire = Time.time + _laserFireRate;
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            _uiManager.ToggleInputScreen(_toggleInputsScreen);
            _toggleInputsScreen = !_toggleInputsScreen;
        }
        
        // MOAG LAser Count Selection
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) _numberOfMOAGLasers = 3;
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) _numberOfMOAGLasers = 4;
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) _numberOfMOAGLasers = 5;
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6)) _numberOfMOAGLasers = 6;
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7)) _numberOfMOAGLasers = 7;
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8)) _numberOfMOAGLasers = 8;
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9)) _numberOfMOAGLasers = 9;
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) _numberOfMOAGLasers = 0;
    }

    private void FireLaser() {
        if (_isMOAGActive && _ammoCount > 0) {
            _ammoCount--;
            if (_numberOfMOAGLasers > 2) {
                for (int i = 0; i < _numberOfMOAGLasers; i++) {
                    GameObject go = RetrieveLaserFromPool();
                    go.transform.position = transform.position + _laserLaunchOffset;
                    float angle = -85 + ((170 / (_numberOfMOAGLasers - 1) * i));
                    go.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
        }
        else if (_isTripleShotActive && _ammoCount > 0) {
            _ammoCount--;
            RetrieveLaserFromPool().transform.position = transform.position + _laserLaunchOffset;
            RetrieveLaserFromPool().transform.position = transform.position + _leftTripleOffset;
            RetrieveLaserFromPool().transform.position = transform.position + _rightTripleOffset;
        }
        else {if (_ammoCount > 0) {  
            _ammoCount--;
            RetrieveLaserFromPool().transform.position = transform.position + _laserLaunchOffset;
            } 
        }
        _audioSource.Play();
        if (_ammoCount < 1) _ammoCount = 0;
        UIManager.Instance.UpdateAmmo(_ammoCount);
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isPlayerDead) {
            if (other.transform.CompareTag("EnemyLaser")) {
                Destroy(other);
                Damage();
            }
        }
    }

    public void Damage() {
        if (!_isPlayerDead) {
            if (!_isShieldActive) {
                _lives--;
                _uiManager.UpdateLives(_lives);
            }
            else {
                _shieldHits++;
                if (_shieldHits > 3) {
                    _isShieldActive = false;
                    _shieldPrefab.SetActive(false);
                    _shieldHits = 0;
                }
            }
            if (_lives < 1) {
                _uiManager.UpdateLives(_lives);
                _spawnManager.OnPlayerDeath();
                _uiManager.GameOver();
                DestroyChildrenGameObjects();
                StartCoroutine(AnimateExplosion());
            }
            else {
                if (!_isPlayerHurtActive) {
                    StartCoroutine(AnimatePlayerHurt());
                }
            }
        }
    }

    public void Awards(int awardNumber)
    {
        // 0 - Triple Shot, 1 - Speed Power-Up, 2 - Shields, 3 - MOAG, 4 - More Ammo, 5 - More Life, 6 - Less Ammo, 7 = Less Life
        switch (awardNumber)  {
            case 0:
                if (!_isTripleShotActive) {
                    StartCoroutine(TripleShotPowerDownRoutine());
                }
                break;
            case 1:
                if (!_isSpeedBoostActive) {
                    StartCoroutine(SpeedBoostPowerDownRoutine());
                }
                break;
            case 2:
                if (!_isShieldActive) {
                    StartCoroutine(ShieldBoostPowerDownRoutine());
                }
                break;
            case 3 :
                if (!_isMOAGActive) {
                    UIManager.Instance.DisplayNumberOfMOAGLasersText();
                    StartCoroutine(MOAGPowerDownRoutine());
                }
                break;
            case 4:
                _ammoCount += 5;
                if (_ammoCount > _maxAmmoCount) _ammoCount = _maxAmmoCount;
                UIManager.Instance.UpdateAmmo(_ammoCount);
                break;
            case 5:
                _lives++;
                if (_lives > 2) _lives = 3;
                _uiManager.UpdateLives(_lives);
                break;
            case 6:
                _ammoCount -= 5;
                if (_ammoCount < 0) _ammoCount = 0;
                UIManager.Instance.UpdateAmmo(_ammoCount);
                break;
            case 7:
                _lives--;
                if(_lives < 1) {
                    _spawnManager.OnPlayerDeath();
                    _uiManager.GameOver();
                    DestroyChildrenGameObjects();
                    StartCoroutine(AnimateExplosion());
                } else {
                    _uiManager.UpdateLives(_lives);
                }
                break;
            default:
                Debug.Log("Default Award trigger in Player award");
                break;
        }       
    }
    
    public void EnemyKill(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    private IEnumerator MOAGPowerDownRoutine()
    {
        _isMOAGActive = true;
        _isTripleShotActive = false;
        yield return new WaitForSeconds(_timePrizesLast);
        _isMOAGActive = false;
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        _isTripleShotActive = true;
        yield return new WaitForSeconds(_timePrizesLast);
        _isTripleShotActive = false;
    }
    
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        _isSpeedBoostActive = true;
        UIManager.Instance.StartThrusterCoolDown(_timePrizesLast);
        _speedUpFactor = _speedBoostFactor;
        yield return new WaitForSeconds(_timePrizesLast);
        _speedUpFactor = 1.0f;
        _isSpeedBoostActive = false;
    }
    
    IEnumerator ShieldBoostPowerDownRoutine()
    {
        _shieldHits = 0;
        _isShieldActive = true;
        _shieldPrefab.SetActive(true);
        yield return new WaitForSeconds(_timePrizesLast);
        _shieldPrefab.SetActive(false);
        _isShieldActive = false;
        _shieldHits = 0;
    }
    
    IEnumerator AnimateThrusters()
    {
        _isThrusterActive = true;
        _thrustersPreFab.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (!_isPlayerDead) {
            _thrustersPreFab.SetActive(false);
            _isThrusterActive = false;
        }
    }

    IEnumerator AnimatePlayerHurt()
    {
        _isPlayerHurtActive = true;
        if (Random.Range(0, 1) == 1) {
            _platerHurtLeft.SetActive(true);
            yield return new WaitForSeconds(1.85f);
            if (!_isPlayerDead)  {
                _platerHurtLeft.SetActive(false);
            }
        }
        else {
            _platerHurtRight.SetActive(true);
            yield return new WaitForSeconds(1.85f);
            if (!_isPlayerDead) {
                _platerHurtRight.SetActive(false);
            }
        }
        _isPlayerHurtActive = false;
    }

    IEnumerator AnimateExplosion()
    {
        _isPlayerDead = true;
        this.GetComponent<SpriteRenderer>().enabled = false;
        _explosion.SetActive(true);
        _audioSource.clip = _explosionSoundClip;
        _audioSource.Play();
        yield return new WaitForSeconds(2.63f);
        Destroy(this.gameObject);
    }
        
    private void DestroyChildrenGameObjects()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child != transform) {
                Destroy(child.gameObject);
            }
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

    public void ReturnLaserPrefabToPool(GameObject laser)
    {
            laser.transform.position = Vector3.zero;
            laser.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            laser.SetActive(false);
            _laserPool.Add(laser);
    }
    #endregion
}
