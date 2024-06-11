using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfoUI : MonoBehaviour
{

    public static ItemInfoUI Instance;
    private Vector3 UIMovePoint;

    [SerializeField]
    private TextMeshProUGUI ItemName;
    [SerializeField]
    private TextMeshProUGUI ItemInfo;

    public string sendItemName;
    public string sendItemInfo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UIOff();
        ClearUI();
    }

    private void Update()
    {
        FollowingUI();
    }

    public void UpdateUI()
    {
        ItemName.text = sendItemName;
        ItemInfo.text = sendItemInfo;
    }

    public void ClearUI()
    {
        ItemName.text = string.Empty;
        ItemInfo.text = string.Empty;

        sendItemName = null;
        sendItemInfo = null;
    }

    public void FollowingUI()
    {
        UIMovePoint = Input.mousePosition;
        transform.position = UIMovePoint + new Vector3(130, 80, 0);
    }
    public void UIOn()
    {
        this.gameObject.SetActive(true);
    }
    public void UIOff()
    {
        this.gameObject.SetActive(false);
    }
}
