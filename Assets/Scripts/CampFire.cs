using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
	public int Damage;
	public float DamageRate;
	public float Heat;

	List<IDamagable> things = new List<IDamagable>();
	float heatRadius = 1.5f;

	private void Start()
	{
		InvokeRepeating("DealDamage", 0, DamageRate);
	}

	private void Update()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, heatRadius);
		foreach(Collider collider in colliders)
		{
			if(collider.gameObject.TryGetComponent(out Player player))
			{
				player.Condition.Heated(Heat * Time.deltaTime);
			}
		}
	}

	void DealDamage()
	{
		for (int i = 0; i < things.Count; i++)
		{
			things[i].TakePhysicalDamage(Damage);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent(out IDamagable damagable))
		{
			things.Add(damagable);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.TryGetComponent(out IDamagable damagable))
		{
			things.Remove(damagable);
		}
	}
}
