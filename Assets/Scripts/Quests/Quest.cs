using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//퀘스트 클래스로 IsActive로 진행중인지 아닌지 판단하고 퀘스트별로 구현해줘야할 메소드가 있다.
public class Quest
{
	public QuestData Data;
	public bool IsActive = false;

	public Quest(QuestData data)
	{
		Data = data;
	}
	public virtual bool QuestClearCheck()
	{
		return false;
	}
}