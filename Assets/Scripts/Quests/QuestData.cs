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

//����Ʈ �����ͷ�
//����Ʈ �̸�, ����, Ÿ��, �׸��� ���� ������ ��� ����
public class QuestData : ScriptableObject
{
	[Header("Info")]
	public string QuestName;
	public string Description;
	public QuestType type;

	[Header("Reward")]
	public Reward[] rewards;
}
