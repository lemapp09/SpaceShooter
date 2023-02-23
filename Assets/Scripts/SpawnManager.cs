using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
   #region Paramters
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;


    private List<GameObject> _enemyPool = new List<GameObject>();
    private float _horizontalBounds = 9.5f;
    private float _verticalBounds = 5.5f;
    private bool _stopSpawning = false;

   #endregion

    void Start() {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine() {
        while (!_stopSpawning) {
            Instantiate(_enemyPrefab, new Vector3(Random.Range(-_horizontalBounds, _horizontalBounds),
                _verticalBounds, 0f), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
            
            Instantiate(_tripleShotPrefab, new Vector3(Random.Range(-_horizontalBounds, _horizontalBounds),
                _verticalBounds, 0f), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }
}
