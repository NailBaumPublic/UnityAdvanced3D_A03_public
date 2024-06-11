using System;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
	public Equip curEquip;
	public Transform equipParent;

	private PlayerController playerController;
	private PlayerCondition PlayerCondition;

    public Action handInven;
    public int selectInvenNum;

    private void Start()
	{
		playerController = GetComponent<PlayerController>();
		PlayerCondition = GetComponent<PlayerCondition>();
	}

	public void EquipNew(ItemData data)
	{
		UnEquip();
		bool IsHaveScript = Instantiate(data.equipPrefab, equipParent).TryGetComponent<Equip>(out curEquip);
        Destroy(curEquip.gameObject.GetComponent<Rigidbody>());

        curEquip.gameObject.transform.localPosition = new Vector3(0.4f, -0.06f, 0.5f);
        curEquip.gameObject.transform.localRotation = Quaternion.Euler(0, 85, 0);
    }

	public void UnEquip()
	{
		if(curEquip != null) { 
			Destroy(curEquip.gameObject);
			curEquip = null;
		}
	}

	public void OnAttackInput(InputAction.CallbackContext context)
	{
		if(context.phase == InputActionPhase.Performed && curEquip != null && playerController.canLook)
		{
			curEquip.OnAttackInput();
		}
	}

    public void OnHandInventory(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started && Input.GetKey(KeyCode.Keypad1))
        {
            selectInvenNum = 14;
            handInven?.Invoke();
        }

        if (context.phase != InputActionPhase.Started && Input.GetKey(KeyCode.Keypad2))
        {
            selectInvenNum = 15;
            handInven?.Invoke();
        }

        if (context.phase != InputActionPhase.Started && Input.GetKey(KeyCode.Keypad3))
        {
            selectInvenNum = 16;
            handInven?.Invoke();
        }

        if (context.phase != InputActionPhase.Started && Input.GetKey(KeyCode.Keypad4))
        {
            selectInvenNum = 17;
            handInven?.Invoke();
        }

        if (context.phase != InputActionPhase.Started && Input.GetKey(KeyCode.Keypad5))
        {
            selectInvenNum = 18;
            handInven?.Invoke();
        }
    }

}
