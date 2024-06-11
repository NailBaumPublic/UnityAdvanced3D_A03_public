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

// 수집 퀘스트 데이터
//현재는 목표 아이템과 아이템별 목표 갯수를 저장한다.
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/GatheringQuestData")]
public class GatheringQuestData : QuestData
{
	[Header("QuestObjectives")]
	public GatheringObjectives[] questObjectives;
}
