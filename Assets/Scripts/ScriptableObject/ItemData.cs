using System;
using UnityEngine;

public enum ItemType
{
    Equipable,  // 장착 가능
    Consumable, // 소모품
    Resource,   // 자원
    Buildable   // 건설 가능
}

public enum ConsumableType
{
    Health, // 체력
    Hunger, // 허기
    Thirst, // 갈증
    Warmth  // 온기
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; // 소비 유형
    public float value; // 소비 값
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // 인게임에 표시될 이름
    public string description; // 설명
    public ItemType type;      // 아이템 유형
    public Sprite icon;        // 아이템 아이콘
    public GameObject dropPrefab; // 드랍 시 나타날 프리팹

    [Header("Stacking")]
    public bool canStack;      // 중복 가능 여부
    public int maxStackAmount; // 최대 중첩 수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 소모품 속성 배열

    [Header("Equip")]
    public GameObject equipPrefab; // 장착 프리팹

    [Header("Buildable")]
    public GameObject buildPrefab; // 건설 프리팹
    public GameObject previewPrefab; // 프리뷰 프리팹
    public ObjectSort sort; // 객체 정렬 유형
    public int cost; // 비용
}
