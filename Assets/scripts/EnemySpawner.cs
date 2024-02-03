using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ2024_GiggleTeddy
{
    public class EnemySpawner : MonoBehaviour
    {
        #region SERIALZED FIELDS
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private Transform spawnerRightPosition;
        [SerializeField]
        private Transform spawnerLeftPosition;
        [SerializeField]
        private Transform enemyTargetRight;
        [SerializeField]
        private Transform enemyTargetLeft;
        #endregion

        #region PRIVATE FIELDS
        float spawnTimer = 0;
        float movingSpeedTimer = 0;
        float spawnTime = 8;
        float movingSpeed = 0.2f;
        float maxMovingSpeed = 5f;
        #endregion

        #region MONO BEHAVIOURS
        void Update()
        {
            #region ChangeDifficulty 
            movingSpeed = Mathf.Lerp(movingSpeed, maxMovingSpeed, movingSpeedTimer);
            spawnTimer += Time.deltaTime / spawnTime;
            movingSpeedTimer += Time.deltaTime / GameControl.Instance.GetLevelTime();
            if (movingSpeedTimer >= 1)
            {
                movingSpeedTimer = 0;
                movingSpeed = 0;
            }
            #endregion

            #region SWITCH LEVELS
            switch (GameControl.Instance.GetLevel())
            {
                case 1:
                    if (spawnTimer >= 1)
                    {
                        spawnTimer = 0;
                        BuildEnemy();
                    }
                    break;
                case 2:
                    if (spawnTimer >= 1)
                    {
                        spawnTime = 6;
                        spawnTimer = 0;
                        maxMovingSpeed = 5.5f;
                        BuildEnemy();
                    }
                    break;
                case 3:
                    if (spawnTimer >= 1)
                    {
                        spawnTime = 8;
                        spawnTimer = 0;
                        maxMovingSpeed = 6;
                        BuildEnemy();
                    }
                    break;
                case 4:
                    UIManager.Instance.SetText("You Win!");
                    Time.timeScale = 0;
                    break;
                default:
                    break;
            }
            #endregion
        }
        #endregion

        #region PRIVATE METHODS
        void BuildEnemy()
        {
            if (GameControl.Instance.GetLevel() == 3)
            {
                int bothSide = Random.Range(0, 2);
                if (bothSide == 0)
                {
                    InstantiateEnemy(spawnerRightPosition.position, spawnerRightPosition.rotation, movingSpeed, enemyTargetRight.position, true);
                    InstantiateEnemy(spawnerLeftPosition.position, spawnerLeftPosition.rotation, movingSpeed, enemyTargetLeft.position, false);
                }
                else
                {
                    int rd = Random.Range(0, 2);
                    if (rd == 0)
                    {
                        InstantiateEnemy(spawnerRightPosition.position, spawnerRightPosition.rotation, movingSpeed, enemyTargetRight.position, true);
                    }
                    else
                    {
                        InstantiateEnemy(spawnerLeftPosition.position, spawnerLeftPosition.rotation, movingSpeed, enemyTargetLeft.position, false);
                    }
                }
            }
            else
            {
                int rd = Random.Range(0, 2);
                if (rd == 0)
                {
                    InstantiateEnemy(spawnerRightPosition.position, spawnerRightPosition.rotation, movingSpeed, enemyTargetRight.position, true);
                }
                else
                {
                    InstantiateEnemy(spawnerLeftPosition.position, spawnerLeftPosition.rotation, movingSpeed, enemyTargetLeft.position, false);
                }
            }
        }

        void InstantiateEnemy(Vector3 position, Quaternion rotation, float speed, Vector3 target, bool flip)
        {
            GameObject enemy;
            enemy = Instantiate(enemyPrefab, position, rotation);
            enemy.GetComponent<SpriteRenderer>().flipX = flip;
            enemy.GetComponent<Enemy>().Init(speed, target);
            GameControl.Instance.AddEnemyToList(enemy);
        }
        #endregion
    }
}
