using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
   #region Paramters
   [Header("Objects to Spawn")]
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerups;

    [Header("Enemy")]
    [Tooltip("Length of time for each Enemy spawn cycle in seconds")]
    [SerializeField] 
    private float _lengthOfEnemySpawnCycle = 5f;
    private int _enemyNumber = 0;
    
    [Header("Power-Ups")] 
    [Tooltip("Length of time for each PowerUp spawn cycle in seconds")]
    [SerializeField] 
    private float _lengthOfPowerupSpawnCycle = 5f;

    [Header("Collectibles")] [SerializeField]
    private GameObject[] _collectibles;
    
    private float _horizontalBounds = 9.5f;
    private float _verticalBounds = 5.5f;
    private bool _stopSpawning = false;

   #endregion

   public void StartSpawning() {
       StartCoroutine(SpawnEnemyRoutine());
       StartCoroutine(DelayStartOfPowerUps());
       foreach (var collectible in _collectibles) {
           GameObject go = Instantiate(collectible);
           go.name = "Enemy" + _enemyNumber.ToString("D3");
           go.transform.position = new Vector3(Random.Range(-_horizontalBounds + 1f,_horizontalBounds - 1f),
               Random.Range(-_verticalBounds + 1f, _verticalBounds - 1f), 0f);
       }
   }
   
   IEnumerator SpawnEnemyRoutine() {
        while (!_stopSpawning) {
            GameObject go = Instantiate(_enemyPrefab, new Vector3(Random.Range(-_horizontalBounds, _horizontalBounds),
                _verticalBounds, 0f), Quaternion.identity);
            go.name = "Enemy" + _enemyNumber;
            _enemyNumber++;
            yield return new WaitForSeconds(_lengthOfEnemySpawnCycle);
        }
   }
   
   IEnumerator SpawnPowerUpRoutine() {
       while ( !_stopSpawning ) {
               Instantiate(_powerups[Random.Range(0, _powerups.Length)], 
                   new Vector3(Random.Range(-_horizontalBounds, _horizontalBounds),
                   _verticalBounds, 0f), Quaternion.identity);
               yield return new WaitForSeconds(_lengthOfPowerupSpawnCycle); 
       }
   }

   IEnumerator DelayStartOfPowerUps()
   {
       yield return new WaitForSeconds(1f);
       StartCoroutine(SpawnPowerUpRoutine());
   }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }
}
