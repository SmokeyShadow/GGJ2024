using System.Collections;
using UnityEngine;
namespace GGJ2024_GiggleTeddy
{
    public class Clown : MonoBehaviour
    {
        #region SERIALIZED FIELDS
        [SerializeField]
        private Transform throwStartRightPosition;
        [SerializeField]
        private Transform throwStartLeftPosition;
        [SerializeField]
        private GameObject box;
        #endregion

        #region PUBLIC FIELDS
        public float maxSpeed = 100f;
        public float Speed = 50f;
        #endregion

        #region PRIVATE FIELDS
        private Rigidbody2D rigidBody;
        private Animator animator;
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool isMoving = false;
        private bool hitBox;
        private bool afterHit;
        #endregion

        #region MONO BEhAVIOURS
        void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (hitBox)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject nearestEnemy = GameControl.Instance.GetNearestEnemy();
                if (nearestEnemy != null)
                {
                    bool enemyFlipx = nearestEnemy.GetComponent<SpriteRenderer>().flipX;

                    GameObject boxGO = Instantiate(box, MoveLeft() ? throwStartLeftPosition.position : throwStartRightPosition.position,
                                                    MoveLeft() ? throwStartLeftPosition.rotation : throwStartRightPosition.rotation);

                    if (nearestEnemy != null)
                    {
                        boxGO.GetComponent<Box>().Init(nearestEnemy.GetComponent<Enemy>().GetBoxPosition(!enemyFlipx).transform.position, !enemyFlipx);
                        nearestEnemy.GetComponent<Enemy>().AttachBox(boxGO);
                        boxGO.GetComponent<Box>().AttachEnemy(nearestEnemy);
                    }
                }
            }

            if (Input.GetKey(KeyCode.A))
            {
                afterHit = false;
                moveLeft = true;
                moveRight = false;

                if (!isMoving)
                {
                    isMoving = true;
                    animator.SetFloat("Direction", -1);
                    animator.SetTrigger("Move");
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                afterHit = false;
                moveRight = true;
                moveLeft = false;
                if (!isMoving)
                {
                    isMoving = true;
                    animator.SetFloat("Direction", 1);
                    animator.SetTrigger("Move");
                }
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                if (afterHit)
                {
                    afterHit = false;
                    return;
                }
                Stand();
            }
        }

        void FixedUpdate()
        {
            float move = 0;

            if (moveLeft)
                move = -1;
            else if (moveRight)
                move = 1;


            if (move != 0)
            {
                if (Mathf.Abs(rigidBody.velocity.x) < maxSpeed)
                {
                    Vector2 toMove = new Vector2(move * Speed, rigidBody.velocity.y);
                    rigidBody.AddForce(toMove);
                }

            }
            else if (move == 0)
            {
                rigidBody.velocity = new Vector2(0, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Hittable")
            {
                if (!collision.gameObject.GetComponent<Box>().IsHitted())
                {
                    StartCoroutine(HitBoxRoutine(collision.gameObject));
                }
            }
        }
        #endregion

        #region PUBLIC METHODS
        public bool MoveRight()
        {
            return moveRight || !isMoving;
        }

        public bool MoveLeft()
        {
            return moveLeft;
        }
        #endregion

        #region PRIVATE METHODS
        void Stand()
        {
            StopMove();
            animator.SetTrigger("Stand");
        }

        void StopMove()
        {
            isMoving = false;
            moveRight = false;
            moveLeft = false;
        }
        #endregion

        #region COROUTINES
        IEnumerator HitBoxRoutine(GameObject go)
        {
            afterHit = true;
            hitBox = true;
            StopMove();
            go.GetComponent<Box>().OnHit();
            animator.SetTrigger("Attack");
            go.GetComponent<Box>().GetEnemy().GetComponent<Enemy>().OnDestroyedStatus();
            yield return new WaitForSeconds(1.5f);
            go.GetComponent<Shatterable>().HitReceived();
            go.GetComponent<Box>().GetEnemy().GetComponent<Animator>().enabled = false;
            StartCoroutine(TellJokeRoutine(go.GetComponent<Box>().GetEnemy().GetComponent<Enemy>()));
        }

        IEnumerator TellJokeRoutine(Enemy go)
        {
            yield return new WaitForSeconds(1);
            animator.SetTrigger("Press Nose");
            SoundPlayer.Instance.PlaySound(SoundPlayer.SoundClip.PressingNose);
            yield return new WaitForSeconds(1);
            hitBox = false;
            afterHit = false;
            go.Laugh();
            SoundPlayer.Instance.PlaySound(SoundPlayer.SoundClip.Laugh);
            go.GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(1);
            GameControl.Instance.RemoveEnemyToList(go.gameObject);
            Destroy(go.gameObject);
        }
        #endregion
    }
}