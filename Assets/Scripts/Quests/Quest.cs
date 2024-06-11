using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����Ʈ Ŭ������ IsActive�� ���������� �ƴ��� �Ǵ��ϰ� ����Ʈ���� ����������� �޼ҵ尡 �ִ�.
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