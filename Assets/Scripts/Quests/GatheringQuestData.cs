using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GatheringObjectives
{
	public ConsumableType consumableType;
	public int TargetAmount;
}

// ���� ����Ʈ ������
//����� ��ǥ �����۰� �����ۺ� ��ǥ ������ �����Ѵ�.
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/GatheringQuestData")]
public class GatheringQuestData : QuestData
{
	[Header("QuestObjectives")]
	public GatheringObjectives[] questObjectives;
}
