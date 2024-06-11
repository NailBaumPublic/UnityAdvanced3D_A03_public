//사냥 퀘스트
//채집 퀘스트랑 비슷하게 사냥 목표와 마릿수를 다채우면 깨짐
public class HuntingQuest : Quest
{
	public HuntingQuestData HuntingQuestData { get; set; }
	public int[] CurrentAmounts;

	public HuntingQuest(HuntingQuestData data) : base(data)
	{
		HuntingQuestData = data;
		CurrentAmounts = new int[HuntingQuestData.questObjectives.Length];
	}

	//몬스터를 처치하면 그 몬스터의 이 같은지 확인 후 같은 수집대상 갯수에 추가해줌
	public void HuntMonster(int targetID)
	{
		for (int i = 0; i < HuntingQuestData.questObjectives.Length; i++)
		{
			HuntingTargets objective = HuntingQuestData.questObjectives[i];
			if (objective.NPCID == targetID)
			{
				CurrentAmounts[i]++;
			}
		}
	}

	//퀘스트 종료 조건인 모든 몬스터가 할당 마릿수를 채웠는지 확인
	public override bool QuestClearCheck()
	{
		bool cleared = true;
		for (int i = 0; i < HuntingQuestData.questObjectives.Length; i++)
		{
			HuntingTargets objective = HuntingQuestData.questObjectives[i];
			if (CurrentAmounts[i] < objective.TargetAmount)
			{
				cleared = false;
				break;
			}
		}
		return cleared;
	}
}
