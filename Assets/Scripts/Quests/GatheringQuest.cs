using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ä�� ����Ʈ
[System.Serializable]
public class GatheringQuest : Quest
{
	public GatheringQuestData GatheringData;
	public int[] CurrentAmounts;

	public GatheringQuest(GatheringQuestData gatheringData) : base(gatheringData)
	{
		GatheringData = gatheringData;
		CurrentAmounts = new int[gatheringData.questObjectives.Length];
	}

	//�Ҹ�ǰ �������� �����ϸ� �� �������� �Ҹ�ǰ Ÿ���� ������ Ȯ�� �� ���� ������� ������ �߰�����
	public void GatherItem(ConsumableType type)
	{
		for (int i = 0; i < GatheringData.questObjectives.Length; i++)
		{
			GatheringObjectives objective = GatheringData.questObjectives[i];
			if (objective.consumableType == type)
			{
				CurrentAmounts[i]++;
			}
		}
	}

	//��� ���� ����� ��� �޼��ƴ��� Ȯ��
	public override bool QuestClearCheck()
	{
		bool cleared = true;
		for (int i = 0; i < GatheringData.questObjectives.Length; i++)
		{
			GatheringObjectives objective = GatheringData.questObjectives[i];
			if (CurrentAmounts[i] < objective.TargetAmount)
			{
				cleared = false;
				break;
			}
		}
		return cleared;
	}
}
