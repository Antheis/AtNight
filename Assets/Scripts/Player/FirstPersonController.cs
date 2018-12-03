using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof (CharacterController), typeof (AudioSource), typeof(Animator))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement parameters")]
        [SerializeField] private float _walkSpeed = 7.5f;
        [SerializeField] private float _runSpeed = 10f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _gravityMultiplier = 1f;
        
        [SerializeField] private MouseLook _mouseLook;
        [SerializeField] private bool _dead;

        [HideInInspector]
        public float SpeedMultiplier = 1f;
        
        private bool _jump;
        private bool _isSprinting;
        private float _yRotation;
        private Vector2 _input;
        private Vector3 _moveDir = Vector3.zero;
        private CollisionFlags _collisionFlags;
        private bool _previouslyGrounded;
        private bool _jumping;
        private float _timeBeforeNextEventCheck;
        
        private readonly float _cooldownIfEvent = 10f;
        private readonly float _cooldownIfNoEvent = 1f;
        
        private AudioSource _audioSource;
        private Animator _animator;
        private Camera _camera;
        private CharacterController _characterController;
        private PlayerInfo _playerInfo;
        
        private readonly int _hashSpeed = Animator.StringToHash("Speed");
        private readonly int _hashAirbone = Animator.StringToHash("Airbone");
        private readonly int _hashDead = Animator.StringToHash("Dead");
        private readonly int _hashStress = Animator.StringToHash("StressLevel");
        private readonly int _hashRandom = Animator.StringToHash("RandomSeed");

        private readonly int _hashFlashLightFlickering = Animator.StringToHash("FlashLightFlickering");
        private readonly int _hashTrip = Animator.StringToHash("Tripping");

        [Header("Sound list")]
        #region Audio part
        [SerializeField] private AudioClip[] _footstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip _jumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip _landSound;           // the sound played when character touches back on ground.

        private void PlayLandingSound()
        {
            _audioSource.clip = _landSound;
            _audioSource.Play();
        }
        private void PlayJumpSound()
        {
            _audioSource.clip = _jumpSound;
            _audioSource.Play();
        }
        
        private void PlayFootStepAudio()
        {
            if (!_characterController.isGrounded || _footstepSounds.Length == 0)
                return;
            
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, _footstepSounds.Length);
            _audioSource.clip = _footstepSounds[n];
            _audioSource.PlayOneShot(_audioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            _footstepSounds[n] = _footstepSounds[0];
            _footstepSounds[0] = _audioSource.clip;
        }
        #endregion

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _playerInfo = GetComponent<PlayerInfo>();
            
            _mouseLook.Init(transform , _camera.transform);
            _jumping = false;
        }
        
        private void Update()
        {
            if (_dead)
                return;

            _mouseLook.LookRotation (transform, _camera.transform);

            // the jump state needs to read here to make sure it is not missed
            if (!_jump)
                _jump = Input.GetButtonDown("Jump");

            if (!_previouslyGrounded && _characterController.isGrounded)
            {
                PlayLandingSound();
                _moveDir.y = 0f;
                _jumping = false;
            }
            
            if (!_characterController.isGrounded && !_jumping && _previouslyGrounded)
                _moveDir.y = 0f;

            _previouslyGrounded = _characterController.isGrounded;
        }
        
        private void FixedUpdate()
        {
            if (_dead)
                return;
            
            float speed;
            GetInput();
            RandomEvent();
            CalculateSpeed(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*_input.y + transform.right*_input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, _characterController.radius, Vector3.down, out hitInfo,
                               _characterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            _moveDir.x = desiredMove.x*speed;
            _moveDir.z = desiredMove.z*speed;

            if (_characterController.isGrounded)
            {

                _moveDir.y = -9.81f * _gravityMultiplier;

                if (_jump)
                {
                    _moveDir.y = _jumpForce;
                    PlayJumpSound();
                    _jump = false;
                    _jumping = true;
                }
            }
            else
                _moveDir += Physics.gravity * _gravityMultiplier * Time.fixedDeltaTime;

            _collisionFlags = _characterController.Move(_moveDir*Time.fixedDeltaTime);

            _mouseLook.UpdateCursorLock();
            
            _animator.SetFloat(_hashSpeed, desiredMove.magnitude);
            _animator.SetBool(_hashAirbone, !_previouslyGrounded);
            _animator.SetFloat(_hashStress, _playerInfo.stressBar);
            _animator.SetInteger(_hashRandom, Random.Range(0, 10));
        }

        private void GetInput()
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            _input = new Vector2(horizontal, vertical);
            _isSprinting = Input.GetKey(KeyCode.LeftShift);
        }

        private void CalculateSpeed(out float speed)
        {
            speed = _isSprinting ? _runSpeed : _walkSpeed;
            speed *= SpeedMultiplier;

            
            // normalize input if it exceeds 1 in combined length:
            if (_input.sqrMagnitude > 1)
                _input.Normalize();
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (_collisionFlags == CollisionFlags.Below)
                return;

            if (body == null || body.isKinematic)
                return;
            
            body.AddForceAtPosition(_characterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        private bool _stressEvent;
        
        private void RandomEvent()
        {
            if (_timeBeforeNextEventCheck < 0f)
            {
                int randomValue = (int)(Random.Range(0, 100 - _playerInfo.stressBar));

                if (_input.magnitude > 0.1f && randomValue % 5 == 1 && 
                    ((randomValue > 25f && _isSprinting) ||
                    (randomValue > 75f && !_isSprinting)))
                {
                    _animator.SetTrigger(_hashTrip);
                    _timeBeforeNextEventCheck = _cooldownIfEvent;
                }
                else if (randomValue > 25f)
                {
                    _animator.SetTrigger(_hashFlashLightFlickering);
                    _timeBeforeNextEventCheck = _cooldownIfEvent;
                }
                else
                    _timeBeforeNextEventCheck = _cooldownIfNoEvent;
            }
            else
                _timeBeforeNextEventCheck -= Time.deltaTime;
        }

        #region Death
        public void Death(Transform from)
        {
            if (!_dead)
            {
                _dead = true;
                StartCoroutine(DeathCoroutine(from));
            }
        }
        private readonly float DeathTurnDegreesPerSec = 10f;
        private IEnumerator DeathCoroutine(Transform from)
        {
            _animator.SetTrigger(_hashDead);
            
            var dest = from.position;
            dest.y = transform.position.y;
            var desiredDirection = dest - transform.position;
            while (desiredDirection != transform.forward)
            {
                Vector3 newDir = Vector3.RotateTowards(transform.forward, desiredDirection, DeathTurnDegreesPerSec * Time.deltaTime, 0.0f);
                // Move our forward a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);
                yield return null;
            }
        }
        #endregion
    }
}
