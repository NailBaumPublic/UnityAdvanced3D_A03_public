using System;
using UnityEngine;

[Serializable]
public class HuntingTargets
{
	public int NPCID;
	public int TargetAmount;
}

// 사냥 퀘스트 데이터
// 현재는 목표 적과 목표 마릿수를 저장한다.
[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/HuntingQuestData")]
public class HuntingQuestData : QuestData
{
	[Header("QuestObjectives")]
	public HuntingTargets[] questObjectives;
}
