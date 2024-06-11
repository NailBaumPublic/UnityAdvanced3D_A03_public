using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//채집 퀘스트
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

	//소모품 아이템을 수집하면 그 아이템의 소모품 타입이 같은지 확인 후 같은 수집대상 갯수에 추가해줌
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

	//모든 수집 대상이 모두 달성됐는지 확인
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
