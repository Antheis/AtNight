using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SeeEnnemyBehavior : MonoBehaviour {

  public bool on = true;
  public float walkSpeed = 10.0f;
  public float runSpeed = 15.0f;
	public float rotationSpeed = 20.0f; //Rotation during movement modifier. If AI starts spinning at random, increase this value. (First check to make sure it's not due to visual radius limitations)
	public float visualRadius = 100.0f; //How close does the player need to be to be seen by the enemy? Set to 0 to remove this limitation.
	public float attackRange = 10.0f;

	private Vector3 lastVisTargetPos; //Monitor target position if we lose sight of target. provides semi-intelligent AI.
	private bool playerHasBeenSeen = false; //An enhancement to how the AI functions prior to visibly seeing the target. Brings AI to life when target is close, but not visible.
  private bool initialGo = false;
	private bool go = false;
	private Renderer render;
	private FirstPersonController player; //The target, or whatever the AI is looking for.
	private bool isRunning = false;
	public float teleportDistance = 30.0f; // distance for the AI to teleport around the player
  private float teleportTimer;
	public float reduceRangeTime = 2.0f;
  private float lostPlayerTimer;
  public float lostPlayerTime = 2.0f;


	CharacterController characterController; //CC used for enemy movement and etc.
	private Animator _animator;
	private EnnemySoundManager _soundManager;

	private readonly int _hashSpeed = Animator.StringToHash("Speed");
	private readonly int _hashAttack = Animator.StringToHash("Attack");

	void Start() {
			StartCoroutine(Initialize()); //co-routine is used incase you need to interupt initiialization until something else is done.
	}

	IEnumerator Initialize() {

			player = FindObjectOfType<FirstPersonController>();
			_animator = GetComponent<Animator>();
			render = gameObject.GetComponent<Renderer>();
			_soundManager = GetComponent<EnnemySoundManager>();
			characterController = GetComponent<CharacterController>();
      lostPlayerTimer = Time.time;
      teleportTimer = Time.time;
			initialGo = true;
			yield return null;
	}

	// Update is called once per frame
	void Update () {
			if (on && initialGo)
					AIFunctionality();
	}

	void AIFunctionality() {
		if (!player.transform)
			return;

		lastVisTargetPos = player.transform.position; //Target tracking method for semi-intelligent AI
		Vector3 moveToward = lastVisTargetPos - transform.position; //Used to face the AI in the direction of the target
		Vector3 moveAway = transform.position - lastVisTargetPos; //Used to face the AI away from the target when running away
		float distance = Vector3.Distance(transform.position, player.transform.position);

    if (TargetIsInSight ()) {
      if (!go) return;

      if (render.isVisible) {
        isRunning = false;
        // MoveTowards(moveAway);
        Stop();
      } else {
        isRunning = true;
        MoveTowards(moveToward);
      }
      lostPlayerTimer = Time.time;
    } else {
      if (Time.time - lostPlayerTimer > lostPlayerTime) {
        TeleportAroundPlayer(moveToward);
      }
    }
		//start attacking if close enough
		if (distance < attackRange) {
			Attack();
		}
	}

	bool TargetIsInSight ()
	{
			//determine if the enemy should be doing anything other than standing still
			go = true;

			//then lets make sure the target is within the vision radius we allowed our enemy
			//remember, 0 radius means to ignore this check

			if ((visualRadius > 0) && (Vector3.Distance(transform.position, player.transform.position) > visualRadius))
					return false;

			//Now check to make sure nothing is blocking the line of sight
			RaycastHit sight;
			if (Physics.Linecast(transform.position, player.transform.position, out sight))
			{
					if (!playerHasBeenSeen && sight.transform == player.transform)
							playerHasBeenSeen = true;
					return sight.transform == player.transform;
			}
			return false;
	}

  void TeleportAroundPlayer (Vector3 direction) {
    // Rotate monster to look to player
    direction.y = 0;
    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

    // change position of monster in a random position in a certain radius around player.
    float randAngle = Random.Range(0.0f, 360.0f);
    Vector2 newPosition = new Vector2(Mathf.Cos(randAngle) * teleportDistance, Mathf.Sin(randAngle) * teleportDistance);
    newPosition.x += player.transform.position.x;
    newPosition.y += player.transform.position.z;
    transform.position = new Vector3(newPosition.x, 0, newPosition.y);


    // decrease radius when passsed a certain amount of time;
    if ((Time.time - teleportTimer) > reduceRangeTime) {
      lostPlayerTime -= 1.0f;
      if (lostPlayerTime < 1.0f)
        lostPlayerTime  = 1.0f;
      teleportTimer = Time.time;
    }
    _soundManager.Teleport();
  }

	void MoveTowards (Vector3 direction) {
      _animator.speed = 1;
			direction.y = 0;

			float speed = walkSpeed;

			if (isRunning)
					speed = runSpeed;

			//rotate toward or away from the target

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);


			//slow down when we are not facing the target

			Vector3 forward = transform.TransformDirection(Vector3.forward);

			float speedModifier = Vector3.Dot(forward, direction.normalized);

			speedModifier = Mathf.Clamp01(speedModifier);



			//actually move toward or away from the target

			direction = forward * speed * speedModifier;

			characterController.Move(direction * Time.deltaTime);

			_animator.SetFloat(_hashSpeed, direction.magnitude);
			_soundManager.Walk();
	}

	private void Attack()
	{
			_animator.SetTrigger(_hashAttack);
			_soundManager.Attack();
			enabled = false;
			player.Death(transform);
	}

  private void Stop() {
    _animator.speed = 0;
  }
}
