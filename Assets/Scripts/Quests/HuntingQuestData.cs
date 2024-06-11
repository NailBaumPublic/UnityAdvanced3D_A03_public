using System;
using UnityEngine;

[Serializable]
public class HuntingTargets
{
	public int NPCID;
	public int TargetAmount;
}

// ��� ����Ʈ ������
// ����� ��ǥ ���� ��ǥ �������� �����Ѵ�.
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/HuntingQuestData")]
public class HuntingQuestData : QuestData
{
	[Header("QuestObjectives")]
	public HuntingTargets[] questObjectives;
}
