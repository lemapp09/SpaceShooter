using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    #region Parameters
    
    private bool _startingSequence;
    private bool _isDead;
    private Player _player;
    [SerializeField]
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _explosion;
    
    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    
    #endregion
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) {
            Debug.Log("Player reference in Asteroid is null.");
        }
        if (_spawnManager == null) {
            Debug.Log("Spawn Manager reference in Asteroid is null.");
        }
        _startingSequence = true;
        StartCoroutine(BringAsteroidInToScene());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isDead && !_startingSequence) {
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

    private IEnumerator BringAsteroidInToScene()
        {
         float AsteroidPosY = this.transform.position.y;
         float AsteroidRotZ = this.transform.rotation.z;
          while (this.transform.position.y > 0)
         {
            yield return new WaitForSeconds(0.01f);
            this.transform.position = new Vector3(0f, AsteroidPosY + 0.13f, 0f);
            this.transform.rotation = Quaternion.Euler(0, 0, AsteroidRotZ);
            AsteroidPosY -= 0.013f;
            AsteroidRotZ -= 0.72f;
            
         }
         _startingSequence = false;
    }
        
    IEnumerator DeathSequence() {
        _isDead = true;
        this.GetComponent<SpriteRenderer>().enabled = false;
        _audioSource.clip = _explosionSoundClip;
        _audioSource.Play();
        _explosion.SetActive(true);
        yield return new WaitForSeconds(2.633f);
        _spawnManager.StartSpawning();
        DestroyChildrenGameObjects();
        Destroy(this.GameObject());
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
}
