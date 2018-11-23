using UnityEngine;

namespace Interaction
{
	public sealed class InteractableSwitch : InteractableBase
	{
		[SerializeField] private bool _state = false;

		private Animator _animator;
		private readonly int _hashActivated = Animator.StringToHash("Activated");
		
		private void Awake()
		{
			_animator = GetComponent<Animator>();
			if (_animator != null)
				_animator.SetBool(_hashActivated, _state);
		}
		
		public override void Interact()
		{
			_state = !_state;
			if (_animator != null)
				_animator.SetBool(_hashActivated, _state);
		}
	}
}
