//��� ����Ʈ
//ä�� ����Ʈ�� ����ϰ� ��� ��ǥ�� �������� ��ä��� ����
public class HuntingQuest : Quest
{
	public HuntingQuestData HuntingQuestData { get; set; }
	public int[] CurrentAmounts;

	public HuntingQuest(HuntingQuestData data) : base(data)
	{
		HuntingQuestData = data;
		CurrentAmounts = new int[HuntingQuestData.questObjectives.Length];
	}

	//���͸� óġ�ϸ� �� ������ �� ������ Ȯ�� �� ���� ������� ������ �߰�����
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

	//����Ʈ ���� ������ ��� ���Ͱ� �Ҵ� �������� ä������ Ȯ��
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
