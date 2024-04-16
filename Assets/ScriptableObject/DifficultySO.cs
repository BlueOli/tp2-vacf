using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultySO : ScriptableObject
{
	[SerializeField]
	private int _difficulty;

	public int Difficulty
	{
		get { return _difficulty; }
		set { _difficulty = value; }
	}

}
