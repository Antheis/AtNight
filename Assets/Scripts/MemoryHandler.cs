using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryHandler : MonoBehaviour {
	public enum Memory : byte {
		LETTER1 = 0,
		TEDDY,
		LETTER2,
		BALL,
		LETTER3,
		RADIO
	}

	public RawImage[] inventoryImg;
	private bool[] memories;

	public delegate void ObjectPickupDelegate(int nbPickedUp);
	public event ObjectPickupDelegate OnNewMemoryPickedUp;
	
	// Use this for initialization
	void Start () {
		memories = new bool[Enum.GetValues(typeof(Memory)).Length];
		for (int i = 0; i < memories.Length; ++i)
			memories[i] = false;
	}

	public void MemoryPicked(Memory mem)
	{
		memories[(int)mem] = true;
		inventoryImg[(int)mem].color = new Color(0.8f, 0.8f, 0.8f);

		if (OnNewMemoryPickedUp != null)
		{
			int nb = 0;
			foreach(bool b in memories)
				if (b)
					nb++;
			OnNewMemoryPickedUp(nb);
		}
	}

	public bool GetAllMemories() {
		foreach (bool b in memories)
			if (!b)
				return false;
		return true;
	}
}
