using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ2024_GiggleTeddy
{
    public class Shatterable : MonoBehaviour, IHittable
    {
        #region SERIALIZED FIELDS
        [SerializeField]
        private List<Spawner> spawnPoints;
        #endregion

        #region PRIVATE FIELDS
        private SpriteRenderer render;
        #endregion

        #region MONO BEHAVIOURS
        void Start()
        {
            render = GetComponent<SpriteRenderer>();
        }
        #endregion

        #region PUBLIC METHODS
        public void HitReceived()
        {
            Die();
        }

        public void Die()
        {
            render.enabled = false;

            foreach (Spawner spawn in spawnPoints)
            {
                spawn.Spawn();
            }

            Destroy(gameObject);
        }
        #endregion
    }
}
