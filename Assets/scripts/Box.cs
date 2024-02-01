using UnityEngine;
namespace GGJ2024_GiggleTeddy
{
    public class Box : MonoBehaviour
    {
        #region PRIVATE FIELDS
        GameObject enemy;
        Animator animator;
        Vector3 targetPosition;
        float speed = 5;
        bool rollRight;
        bool isHitted;
        #endregion

        #region MONO BEHAVIOURS
        void Update()
        {
            if (targetPosition != Vector3.zero)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            }
            if (Utility.AlmostEqual(transform.position, targetPosition, 0.2f))
            {
                if (rollRight)
                    animator.SetBool("Rolling_Right", false);
                else
                    animator.SetBool("Rolling_Left", false);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void Init(Vector3 targetPosition, bool rollRight)
        {
            animator = GetComponent<Animator>();
            this.targetPosition = targetPosition;
            this.rollRight = rollRight;
            RollAnimate();
        }

        public void AttachEnemy(GameObject go)
        {
            enemy = go;
        }

        public GameObject GetEnemy()
        {
            return enemy;
        }

        public bool IsHitted()
        {
            return isHitted;
        }

        public void OnHit()
        {
            isHitted = true;
        }
        #endregion

        #region PRIVATE METHODS
        void RollAnimate()
        {
            if (rollRight)
                animator.SetBool("Rolling_Right", true);
            else
                animator.SetBool("Rolling_Left", true);
            animator.enabled = true;
        }
        #endregion

    }
}
