using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GGJ2024_GiggleTeddy
{
    public class Enemy : MonoBehaviour
    {
        #region SERIALIZED FIELDS
        [SerializeField]
        private GameObject boxRightPosition;
        [SerializeField]
        private GameObject boxLeftPosition;
        [SerializeField]
        private GameObject crown;
        #endregion

        #region PRIVATE FIELDS
        GameObject box;
        SpriteRenderer spRenderer;
        Animator animator;
        Vector3 target;
        bool hasCrown;
        bool hitBox;
        bool destroyed;
        float speed = 0;
        float hitTimer = 0;
        float hitDuration = 5;
        #endregion

        #region MONO BEHAVIOURS
        private void Start()
        {
            animator = GetComponent<Animator>();
            spRenderer = GetComponent<SpriteRenderer>();
            animator.SetBool("IsWalking", true);
        }

        void Update()
        {
            if (destroyed)
                return;

            if (hitBox)
            {
                if (box != null)
                {
                    hitTimer += Time.deltaTime / hitDuration;
                    if (hitTimer >= 1)
                    {
                        box.GetComponent<Shatterable>().HitReceived();
                        StartMove();
                        hasCrown = true;
                        crown.SetActive(true);
                        hitBox = false;
                    }
                }
                return;
            }

            if (Utility.AlmostEqual(transform.position, target, 0.2f))
            {
                GameControl.Instance.BlowBalloon();
                GameControl.Instance.RemoveEnemyToList(gameObject);
                Destroy(gameObject);
            }

            transform.Translate(Time.deltaTime * speed * (target - transform.position).normalized);

        }
        #endregion

        #region PUBLIC METHODS
        public void Init(float speed, Vector3 target)
        {
            this.speed = speed;
            this.target = target;
        }

        public void AttachBox(GameObject go)
        {
            box = go;
        }

        public void Laugh()
        {
            animator.SetBool("IsLaughing", true);
        }

        public SpriteRenderer GetSpriteRenderer()
        {
            return spRenderer;
        }

        public bool HasCrown()
        {
            return hasCrown;
        }

        public GameObject GetBoxPosition(bool rightSidePosition)
        {
            return rightSidePosition ? boxRightPosition : boxLeftPosition;
        }

        public void StopMove()
        {
            hitBox = true;
            animator.SetBool("IsHitting", true);
        }

        public void StartMove()
        {
            hitBox = false;
            animator.SetBool("IsHitting", false);
        }

        public bool IsHitting()
        {
            return hitBox;
        }

        public void OnDestroyedStatus()
        {
            destroyed = true;
        }
        #endregion
    }
}
