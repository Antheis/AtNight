using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryInfo : MonoBehaviour {
	public MemoryHandler.Memory type;
	private MemoryHandler handler;

	void Start()
	{
		handler = transform.parent.gameObject.GetComponent<MemoryHandler>();
	}

	public void PickObject() {
		handler.MemoryPicked(type);
	}
}