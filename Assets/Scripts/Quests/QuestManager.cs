using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//����Ʈ �Ŵ������� ����Ʈ ����� �����Ѵ�
public class QuestManager : MonoBehaviour
{
	private static QuestManager instance;
	public static QuestManager Instance { get { return instance; } }

	[SerializeReference]
	public List<QuestData> QuestDataList;

	private List<Quest> QuestList;
	public TextMeshProUGUI QuestInfoText;
	public GameObject QuestRewardPanel;
	public TextMeshProUGUI QuestRewardText;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		QuestList = new List<Quest>();
		foreach (QuestData data in QuestDataList)
		{
			switch (data.type)
			{
				case QuestType.Gathering:
					QuestList.Add(new GatheringQuest(data as GatheringQuestData));
					break;
				case QuestType.Hunting:
					QuestList.Add(new HuntingQuest(data as HuntingQuestData));
					break;
			}
		}
	}

	private void Start()
	{
		StartQuest(0);
	}

	//������ ä���� �Ҹ�ǰ�� ������ ���� �Ҹ�ǰ ä�� ����Ʈ�� ������ �÷��ش�.
	public void GatherItem(ItemDataConsumable[] consumables)
	{
		if(QuestList.Count == 0)
		{
			return;
		}
		foreach (Quest quest in QuestList)
		{
			if (quest is GatheringQuest && quest.IsActive)
			{
				GatheringQuest gatheringQ = (GatheringQuest)quest;
				foreach (ItemDataConsumable consumable in consumables)
				{
					gatheringQ.GatherItem(consumable.type);
				}
				if (quest.QuestClearCheck())
				{
					GiveReward(quest);
				}
			}
		}
		UpdateQuestInfoText();
	}

	//���Ͱ� ���� �� ��� ī��Ʈ�� �߰�
	public void HuntMonster(int Id)
	{
		if (QuestList.Count == 0)
		{
			return;
		}
		foreach (Quest quest in QuestList)
		{
			if (quest is HuntingQuest && quest.IsActive)
			{
				HuntingQuest huntingQ = (HuntingQuest)quest;
				huntingQ.HuntMonster(Id);
				if (quest.QuestClearCheck())
				{
					GiveReward(quest);
				}
			}
		}
		UpdateQuestInfoText();
	}

	//������ �����ϴ� �Լ��� ���� ����Ʈ�� ������� ���ΰ��� �κ��丮�� �־��ش�.
	//�׸��� ���� �˸����� ����ش�.
	private void GiveReward(Quest quest)
	{
		string text = $"{quest.Data.QuestName} �Ϸ�!\n�������� :\n";

		foreach(Reward reward in quest.Data.rewards)
		{
			text += $"{reward.Item.displayName}�� {reward.RewardAmount}�� ȹ��!\n";
		}

		QuestRewardText.text = text;
		QuestRewardPanel.SetActive(true);

		foreach (Reward reward in quest.Data.rewards)
		{
			for (int i = 0; i < reward.RewardAmount; i++)
			{
				CharacterManager.Instance.Player.itemData = reward.Item;
				CharacterManager.Instance.Player.addItem.Invoke();
			}
		}
		quest.IsActive = false;
		StartCoroutine(QuestRewardTurnOff());
		if(quest == QuestList[0])
		{
			StartQuest(1);
		}
	}

	//����Ʈ ���� â ����
	IEnumerator QuestRewardTurnOff()
	{
		yield return new WaitForSeconds(3f);
		QuestRewardPanel.SetActive(false);
	}

	void StartQuest(int i)
	{
		QuestList[i].IsActive = true;
		UpdateQuestInfoText();
	}

	//����Ʈ ���� â ������Ʈ
	void UpdateQuestInfoText()
	{
		string text = "";
		bool notActiveQuests = true;

		foreach (Quest quest in QuestList)
		{
			if(quest.IsActive)
			{
				notActiveQuests = false;
				text += $"{quest.Data.QuestName}\n{quest.Data.Description}\n";
				if (quest is GatheringQuest)
				{
					GatheringQuest gathering = (GatheringQuest)quest;
					int length = gathering.GatheringData.questObjectives.Length;

					for (int i = 0; i < length; i++)
					{
						GatheringObjectives gatheringObjective = gathering.GatheringData.questObjectives[i];
						switch (gatheringObjective.consumableType)
						{
							case ConsumableType.Hunger:
								text += "����:\t";
								break;
							case ConsumableType.Thirst:
								text += "����:\t";
								break;
						}
						text += $"{gathering.CurrentAmounts[i]}/{gatheringObjective.TargetAmount}\n";
					}
				}
			}
		}
		if (notActiveQuests) { text += "���� ���� ���� ����Ʈ ����"; }
		QuestInfoText.SetText(text);
	}
}
