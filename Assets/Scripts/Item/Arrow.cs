using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	public Vector3 direction;
	public int damage;

	private void Update()
	{
		transform.position += (direction * Time.deltaTime) * 10f;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Player>())
		{
			CharacterManager.Instance.Player.Controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
		}
		Destroy(gameObject);
	}
}
