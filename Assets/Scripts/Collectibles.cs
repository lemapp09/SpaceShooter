using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Collectibles : MonoBehaviour
{

    [SerializeField] 
    [Tooltip("4 - More Ammo, 5 - More Life, 6 - Less Ammo, 7 = Less Life")]
    [Range(4,7)]
    private int _collectibleId = 3;
    [SerializeField]
    private GameObject _explosion;

    private bool _isDead;
    private float _horizontalBounds = 9f;
    private float _verticalBounds = 5f;
    private Player _player;
       
    [Header("Audio Clips")]
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private AudioClip _powerupSoundClip;

    private void Start() {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (_player == null) {
            Debug.Log("Player reference in Collectible "+ _collectibleId+" is null.");
        }
        if (_explosion == null) {
            Debug.Log("Collectible# "+ _collectibleId + " Explosion is null");
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (!_isDead) {
            if (other.transform.CompareTag("Player")) {
                Player player = other.transform.GetComponent<Player>();
                if (player != null) {
                    // 4 - More Ammo, 5 - More Life, 6 - Less Ammo, 7 = Less Life
                    player.Awards(_collectibleId);
                }
                StartCoroutine(DeathSequence());
            } else if (other.transform.CompareTag("Laser")) {
                Laser laser = other.transform.GetComponent<Laser>();
                if (laser != null) {
                    // 4 - More Ammo, 5 - More Life, 5 - Less Ammo, 7 = Less Life
                    if (_player != null) {
                        _player.Awards(_collectibleId);
                    }
                    other.transform.GetComponent<Laser>().ReturnToPool();
                } else Destroy(other);
            }
            StartCoroutine(DeathSequence());
        } else if (other.transform.CompareTag("EnemyLaser")) {
            StartCoroutine(DeathSequence());
        }  
    }
    
    
    IEnumerator DeathSequence() {
        _isDead = true;
        this.GetComponent<SpriteRenderer>().enabled = false;
        _explosion.SetActive(true);
        _audioSource.clip = _powerupSoundClip;
        _audioSource.Play();
        yield return new WaitForSeconds(0.8f);
        _audioSource.clip = _explosionSoundClip;
        _audioSource.Play();
        yield return new WaitForSeconds(1.833f);
        _explosion.SetActive(false);
        transform.position = new Vector3(_horizontalBounds = 5f,0f, 0f);
        StartCoroutine(HideForOneSec());
    }

    private IEnumerator HideForOneSec()
    {
        yield return new WaitForSeconds(1f);
        _isDead = false;
        this.GetComponent<SpriteRenderer>().enabled = true;
        transform.position = new Vector3(Random.Range(-_horizontalBounds,_horizontalBounds),Random.Range(-_verticalBounds, _verticalBounds), 0f);
    }
}
