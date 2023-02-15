using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region Parameters
        private float _horizontalBounds = 9.5f;
        private float _verticalBounds = 5.5f;
        private float _speed = 4f;
    #endregion

    void Update()
    {
        EnemyMovement();
        MoveEnemyBeyondGameScreen();
    }

    private void EnemyMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
    }

    private void MoveEnemyBeyondGameScreen()
    {
        if (transform.position.y < -_verticalBounds)
        {
            float randomX = Random.Range(-_horizontalBounds, _horizontalBounds);
            transform.position = new Vector3( randomX, _verticalBounds, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();
            if (player != null) {
                other.transform.GetComponent<Player>().Damage();
            }
            ReturnEnemyToPool();
        }

        if (other.transform.CompareTag("Laser")) {
            Laser laser = other.transform.GetComponent<Laser>();
            if (laser != null) {
                other.transform.GetComponent<Laser>().ReturnToPool();
            }
            ReturnEnemyToPool();
        }
    }

    private void ReturnEnemyToPool()
    {
        SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager != null) {
            spawnManager.ReturnEnemyPrefabToPool(this.gameObject);
        }
    }
}
