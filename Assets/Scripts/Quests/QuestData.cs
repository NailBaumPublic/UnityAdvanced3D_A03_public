using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestType
{
	Gathering,
	Hunting
}

[Serializable]
public class Reward
{
	public ItemData Item;
	public int RewardAmount;
}

//퀘스트 데이터로
//퀘스트 이름, 설명, 타입, 그리고 보상 아이템 목록 존재
public class QuestData : ScriptableObject
{
	[Header("Info")]
	public string QuestName;
	public string Description;
	public QuestType type;

	[Header("Reward")]
	public Reward[] rewards;
}
