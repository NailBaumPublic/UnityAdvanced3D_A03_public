using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//퀘스트 매니저에서 퀘스트 목록을 관리한다
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

	//아이템 채집시 소모품의 종류에 따라 소모품 채집 퀘스트에 갯수를 늘려준다.
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

	//몬스터가 죽을 때 사냥 카운트에 추가
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

	//보상을 지급하는 함수로 받은 퀘스트의 보상들을 주인공의 인벤토리에 넣어준다.
	//그리고 보상 알림판을 띄워준다.
	private void GiveReward(Quest quest)
	{
		string text = $"{quest.Data.QuestName} 완료!\n보상지급 :\n";

		foreach(Reward reward in quest.Data.rewards)
		{
			text += $"{reward.Item.displayName}을 {reward.RewardAmount}개 획득!\n";
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

	//퀘스트 보상 창 끄기
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

	//퀘스트 정보 창 업데이트
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
								text += "음식:\t";
								break;
							case ConsumableType.Thirst:
								text += "음료:\t";
								break;
						}
						text += $"{gathering.CurrentAmounts[i]}/{gatheringObjective.TargetAmount}\n";
					}
				}
			}
		}
		if (notActiveQuests) { text += "현재 진행 중인 퀘스트 없음"; }
		QuestInfoText.SetText(text);
	}
}
