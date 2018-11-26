using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCollider : MonoBehaviour {
	public MemoryHandler boss;
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Player") && boss.GetAllMemories())
			Destroy(transform.gameObject);
	}
}
