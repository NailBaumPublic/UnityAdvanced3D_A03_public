using UnityEngine;
using static UnityEditor.Progress;

public interface DropItem
{
	public void DropItem();
}

public class Resource : MonoBehaviour, IDamagable, DropItem
{
	public int woodSpawnHealth;
	private int CurrentHealth;
	public int MaxHealth;
	public ItemData[] itemToDrop;

	private void Start()
	{
		CurrentHealth = MaxHealth;
	}

	public void TakePhysicalDamage(int damage)
	{
		int beforeHealth = CurrentHealth;
		CurrentHealth -= damage;
		if(beforeHealth/woodSpawnHealth - CurrentHealth/woodSpawnHealth > 0)
		{
			DropItem();
		}
		if(CurrentHealth <= 0)
		{
			ResourceSpawner.Instance.RespawnInSeconds(gameObject);
			gameObject.SetActive(false);
		}
	}

	public float GetHealthRatio()
	{
		return CurrentHealth / (float)MaxHealth;
	}

	public void DropItem()
	{
		foreach (ItemData item in itemToDrop)
		{
			Instantiate(item.dropPrefab, transform.position + new Vector3(1, 1), Quaternion.identity);
		}
	}

	public void RestoreHealth()
	{
		CurrentHealth = MaxHealth;
	}
}
