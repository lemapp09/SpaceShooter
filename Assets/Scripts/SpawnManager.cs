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

    [Header("Power-Ups")] 
    [SerializeField] 
    [Tooltip("Spawn Manager has a cycle, this count is how many cycles " +
             "between PowerUp drops.")]
    private int _powerupCycleCount = 1;

    [Header("Spawn Cycle Length")]
    [Tooltip("Length of time for each spawn cycle in seconds")]
    [SerializeField] 
    private float _lengthOfSpawnCycle = 5f;
    
    private float _horizontalBounds = 9.5f;
    private float _verticalBounds = 5.5f;
    private bool _stopSpawning = false;

   #endregion

   IEnumerator SpawnRoutine()
    {
        int powerUpCycleCounter = 0;
        while (!_stopSpawning) {
            Instantiate(_enemyPrefab, new Vector3(Random.Range(-_horizontalBounds, _horizontalBounds),
                _verticalBounds, 0f), Quaternion.identity);
            yield return new WaitForSeconds(_lengthOfSpawnCycle / 2f);

            powerUpCycleCounter++;
            if (powerUpCycleCounter >= _powerupCycleCount && !_stopSpawning )
            {
                Instantiate(_powerups[Random.Range(0, _powerups.Length)], new Vector3(
                    Random.Range(-_horizontalBounds, _horizontalBounds),
                    _verticalBounds, 0f), Quaternion.identity);
                powerUpCycleCounter = 0;
            }
            
            yield return new WaitForSeconds(_lengthOfSpawnCycle / 2f);
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }

    public void StartSpawning() {
        StartCoroutine(SpawnRoutine());
    }
}
