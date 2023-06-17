using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement.Components
{
    [RequireComponent(typeof(Rigidbody2D)),
     RequireComponent(typeof(Animator)),
     RequireComponent(typeof(NetworkObject))]
    public sealed class FighterMovement : NetworkBehaviour, IMoveableReceiver, IJumperReceiver, IFighterReceiver
    {
        public float speed = 1.0f;
        public float jumpAmount = 1.0f;

        [SerializeField] public NetworkVariable<float> currentLife = new NetworkVariable<float>();

        private Rigidbody2D _rigidbody2D;
        private Animator _animator;
        private NetworkAnimator _networkAnimator;
        private Transform _feet;
        private LayerMask _floor;

        private Vector3 _direction = Vector3.zero;
        private bool _grounded = true;

        private static readonly int AnimatorSpeed = Animator.StringToHash("speed");
        private static readonly int AnimatorVSpeed = Animator.StringToHash("vspeed");
        private static readonly int AnimatorGrounded = Animator.StringToHash("grounded");
        private static readonly int AnimatorAttack1 = Animator.StringToHash("attack1");
        private static readonly int AnimatorAttack2 = Animator.StringToHash("attack2");
        private static readonly int AnimatorHit = Animator.StringToHash("hit");
        private static readonly int AnimatorDie = Animator.StringToHash("die");

        //Countdown 
        public bool countDownActive = false;

        //Vida
        public Vida vidaUI;

        void Start()
        {

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _networkAnimator = GetComponent<NetworkAnimator>();
            
            _feet = transform.Find("Feet");
            _floor = LayerMask.GetMask("Floor");

            vidaUI = GameObject.FindObjectOfType<Vida>();
            InitCharacterServerRpc();

        }


        [ServerRpc(RequireOwnership = false)]
        public void InitCharacterServerRpc()
        {
            currentLife.Value = vidaUI.maxHP;
        }


        [ServerRpc]
        void AnimacionesServerRpc()
        {
            _grounded = Physics2D.OverlapCircle(_feet.position, 0.1f, _floor);
            _animator.SetFloat(AnimatorSpeed, this._direction.magnitude);
            _animator.SetFloat(AnimatorVSpeed, this._rigidbody2D.velocity.y);
            _animator.SetBool(AnimatorGrounded, this._grounded);
        }
        void Update()
        {
            if (!IsOwner) return;            
            AnimacionesServerRpc();
        }

        void FixedUpdate()
        {
            _rigidbody2D.velocity = new Vector2(_direction.x, _rigidbody2D.velocity.y);
        }

        [ServerRpc]
        public void MoveServerRpc(IMoveableReceiver.Direction direction)
        {
            if (countDownActive) return;
            if (direction == IMoveableReceiver.Direction.None)
            {
                this._direction = Vector3.zero;
                return;
            }

            bool lookingRight = direction == IMoveableReceiver.Direction.Right;
            _direction = (lookingRight ? 1f : -1f) * speed * Vector3.right;
            transform.localScale = new Vector3(lookingRight ? 1 : -1, 1, 1);
        }

        [ServerRpc]
        public void JumpServerRpc(IJumperReceiver.JumpStage stage)
        {
            if (countDownActive) return;
            switch (stage)
            {
                case IJumperReceiver.JumpStage.Jumping:
                    if (_grounded)
                    {
                        float jumpForce = Mathf.Sqrt(jumpAmount * -2.0f * (Physics2D.gravity.y * _rigidbody2D.gravityScale));
                        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    }
                    break;
                case IJumperReceiver.JumpStage.Landing:
                    break;
            }
        }

        [ServerRpc]
        public void Attack1ServerRpc()
        {
            _networkAnimator.SetTrigger(AnimatorAttack1);
        }

        [ServerRpc]
        public void Attack2ServerRpc()
        {
            _networkAnimator.SetTrigger(AnimatorAttack2);
        }

        [ServerRpc(RequireOwnership = false)]
        public void TakeHitServerRpc(int damage)
        {
            _networkAnimator.SetTrigger(AnimatorHit);
            
            TakeHitClientRpc(damage);
        }

        [ClientRpc]
        public void TakeHitClientRpc(int damage)
        {
            vidaUI.currentHP -= damage;
        }
        

        [ServerRpc(RequireOwnership = false)]
        public void DieServerRpc()
        {
            _networkAnimator.SetTrigger(AnimatorDie);
            GameManager.instance.PlayerDead();
            Invoke("DieClientRpc", 2);
            
        }

        [ClientRpc]
        public void DieClientRpc()
        {
            //Cambiar cámara?
        }

        public float GetLife()
        {
            return vidaUI.currentHP;
        }
    }
}