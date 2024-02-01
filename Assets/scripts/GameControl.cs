using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundPlayer;
namespace GGJ2024_GiggleTeddy
{
    public class GameControl : MonoBehaviour
    {
        #region STATIC FIELDS
        private static GameControl instance;
        #endregion

        #region SERIALIZED FIELDS
        [SerializeField]
        private List<Animator> balloonsAnimators;
        [SerializeField]
        private List<GameObject> enemies;
        [SerializeField]
        private Clown clown;
        #endregion

        #region PRIVATE FIELDS
        float timer = 0f;
        int currentLevel = 1;
        float changeDifficultyTime = 40;
        #endregion

        #region PROPERTIES
        public static GameControl Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<GameControl>();
                return instance;
            }
        }
        #endregion

        #region MONO BEHAVIOURS
        void Start()
        {
            StartCoroutine(ShowLevel1Routine());
        }

        private void Update()
        {
            timer += Time.deltaTime / changeDifficultyTime;
            if (timer >= 1)
            {
                currentLevel++;
                UIManager.Instance.SetText("Level " + currentLevel.ToString(), 1, 0);
                timer = 0;
            }
        }
        #endregion

        #region PUBLIC METHODS
        public int GetLevel()
        {
            return currentLevel;
        }

        public float GetLevelTime()
        {
            return changeDifficultyTime;
        }

        public void BlowBalloon()
        {
            if (balloonsAnimators.Count == 0)
                return;
            int rand = Random.Range(0, balloonsAnimators.Count);
            balloonsAnimators[rand].enabled = true;
            StartCoroutine(BlowRoutine(balloonsAnimators[rand].gameObject));   
        }

        public void AddEnemyToList(GameObject go)
        {
            enemies.Add(go);
        }

        public void RemoveEnemyToList(GameObject go)
        {
            enemies.Remove(go);
        }

        public GameObject GetNearestEnemy()
        {
            GameObject go = null;

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].GetComponent<Enemy>().GetSpriteRenderer().isVisible && !enemies[i].GetComponent<Enemy>().HasCrown() && !enemies[i].GetComponent<Enemy>().IsHitting())
                {
                    if (clown.MoveLeft())
                    {
                        if (enemies[i].transform.position.x <= clown.gameObject.transform.position.x)
                        {
                            go = enemies[i];
                            break;
                        }
                    }
                    else if (clown.MoveRight())
                    {
                        if (enemies[i].transform.position.x >= clown.gameObject.transform.position.x)
                        {
                            go = enemies[i];
                            break;
                        }
                    }
                }
            }

            if (go != null)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (clown.MoveLeft())
                    {
                        if (enemies[i].transform.position.x > go.transform.position.x && enemies[i].transform.position.x <= clown.gameObject.transform.position.x)
                        {
                            if (enemies[i].GetComponent<Enemy>().GetSpriteRenderer().isVisible && !enemies[i].GetComponent<Enemy>().HasCrown() && !enemies[i].GetComponent<Enemy>().IsHitting())
                                go = enemies[i];
                        }
                    }
                    else if (clown.MoveRight())
                    {
                        if (enemies[i].transform.position.x < go.transform.position.x && enemies[i].transform.position.x >= clown.gameObject.transform.position.x)
                        {
                            if (enemies[i].GetComponent<Enemy>().GetSpriteRenderer().isVisible && !enemies[i].GetComponent<Enemy>().HasCrown() && !enemies[i].GetComponent<Enemy>().IsHitting())
                                go = enemies[i];
                        }
                    }
                }
            }
            if (go != null)
                go.GetComponent<Enemy>().StopMove();
            return go;
        }
        #endregion

        #region COROUTINES
        IEnumerator ShowLevel1Routine()
        {
            yield return new WaitForSeconds(2);
            UIManager.Instance.SetText("Level " + currentLevel.ToString(), 1, 3);
        }

        IEnumerator BlowRoutine(GameObject go)
        {
            yield return new WaitForSecondsRealtime(1f);
            Destroy(go.transform.parent.gameObject);
            balloonsAnimators.Remove(go.GetComponent<Animator>());
            if (balloonsAnimators.Count == 0)
            {
                UIManager.Instance.SetText("You Lose! ", 5, 4);
                Time.timeScale = 0;
            }
        }
        #endregion
    }
}