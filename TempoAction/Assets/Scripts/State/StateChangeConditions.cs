using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
	Enemy,
	Player
}

[Serializable]
public struct BooleanArray
{
	public bool[] array;
}

[CreateAssetMenu(fileName = "StateChangeConditions", menuName = "ScriptatbleObject/State/ChangeConditions")]
public class StateChangeConditions : ScriptableObject
{
	public StateType stateType;
	[SerializeField] public BooleanArray[] conditions;

	public bool GetChangable(int current, int next)
	{
		int length = conditions.Length - 1;

		if(current == next) { return false; }					// 같은 State는 비허용
		if(current < 0 || current > length) { return false; }	// current가 범위를 벗어난 경우 비허용(잘못된 입력)
		if(next < 0 || next > length) { return false; }        // next가 범위를 벗어난 경우 비허용(잘못된 입력)

		return conditions[current].array[next];
	}
}