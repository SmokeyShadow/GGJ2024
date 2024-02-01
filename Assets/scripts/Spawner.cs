using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GGJ2024_GiggleTeddy
{
    public class Spawner : MonoBehaviour
    {
        #region SERIALIZED FIELDS
        [SerializeField]
        private GameObject prefabToSpawn;
        [SerializeField]
        private Transform whereToSpawn;
        #endregion

        #region PUBLIC METHODS
        public void Spawn()
        {
            if (isActiveAndEnabled && gameObject.activeSelf)
            {
                Instantiate(prefabToSpawn, whereToSpawn.position, Quaternion.identity);
            }
        }
        #endregion
    }
}
