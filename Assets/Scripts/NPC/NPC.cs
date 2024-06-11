using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
	Idle,
	Wandering,
	Attacking
}

public class NPC : MonoBehaviour, IDamagable, DropItem
{
	[Header("Stats")]
	public int NPCID = 0;
	public int MaxHealth;
	int currentHealth;
	public float walkSpeed;
	public float runSpeed;

	[Header("AI")]
	private NavMeshAgent agent;
	public float detectDistance;
	private AIState aiState;

	[Header("Wandering")]
	public float minWanderDistance;
	public float maxWanderDistance;
	public float minWanderWaitTime;
	public float maxWanderWaitTime;
	public float maxWanderingMoveTime;
	private float wanderingStartTime;

	[Header("Combat")]
	public int damage;
	public float attackRate;
	private float lastAttackTime;
	public float attackDistance;

	private float playerDistance;

	public float fieldOfView = 120f;

	private Animator animator;
	private SkinnedMeshRenderer[] meshRenderer;

	public ItemData[] itemToDrop;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
		currentHealth = MaxHealth;
	}

	private void Start()
	{
		SetState(AIState.Wandering);
	}

	private void Update()
	{
		playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

		animator.SetBool("Moving", aiState != AIState.Idle);

		switch (aiState)
		{
			case AIState.Idle:
			case AIState.Wandering:
				PassiveUpdate();
				break;
			case AIState.Attacking:
				AttackingUpdate();
				break;
		}
	}

	public void SetState(AIState state)
	{
		aiState = state;

		switch (aiState)
		{
			case AIState.Idle:
				agent.speed = walkSpeed;
				agent.isStopped = true;
				break;
			case AIState.Wandering:
				agent.speed = walkSpeed;
				agent.isStopped = false;
				break;
			case AIState.Attacking:
				agent.speed = runSpeed;
				agent.isStopped = false;
				break;
		}

		animator.speed = agent.speed / walkSpeed;
	}

	void PassiveUpdate()
	{
		if ((aiState == AIState.Wandering && agent.remainingDistance < 0.1f) || (Time.time - wanderingStartTime >= maxWanderingMoveTime))
		{
			SetState(AIState.Idle);
			Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
		}

		if (playerDistance < detectDistance)
		{
			SetState(AIState.Attacking);
		}
	}

	void WanderToNewLocation()
	{
		if (aiState != AIState.Idle) { return; }
		SetState(AIState.Wandering);
		agent.SetDestination(GetWanderLocation());
		wanderingStartTime = Time.time;
	}

	Vector3 GetWanderLocation()
	{
		NavMeshHit hit;

		NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

		int i = 0;

		while (Vector3.Distance(transform.position, hit.position) < detectDistance)
		{
			NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
			i++;
			if (i == 30) { break; }
		}

		return hit.position;
	}

	void AttackingUpdate()
	{
		if (playerDistance < attackDistance && IsPlayerInFieldOfView())
		{
			agent.isStopped = true;
			if (Time.time - lastAttackTime > attackRate)
			{
				lastAttackTime = Time.time;
				CharacterManager.Instance.Player.Controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
				animator.speed = 1;
				animator.SetTrigger("Attack");
			}
		}
		else
		{
			if (playerDistance < detectDistance)
			{
				agent.isStopped = false;
				NavMeshPath path = new NavMeshPath();
				agent.SetDestination(CharacterManager.Instance.Player.transform.position);
			}
			else
			{
				agent.SetDestination(transform.position);
				agent.isStopped = true;
				SetState(AIState.Wandering);
			}
		}
	}

	bool IsPlayerInFieldOfView()
	{
		Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
		float angle = Vector3.Angle(transform.forward, directionToPlayer);
		return angle < fieldOfView * 0.5f;
	}

	public void TakePhysicalDamage(int damage)
	{
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			Die();
		}

		StartCoroutine(DamageFlash());
	}

	void Die()
	{
		DropItem();
		QuestManager.Instance.HuntMonster(NPCID);

		Destroy(gameObject);
	}

	IEnumerator DamageFlash()
	{
		for (int i = 0; i < meshRenderer.Length; i++)
		{
			meshRenderer[i].material.color = new Color(1.0f, 0.6f, 0.6f);
		}

		yield return new WaitForSeconds(0.1f);

		for (int i = 0; i < meshRenderer.Length; i++)
		{
			meshRenderer[i].material.color = Color.white;
		}
	}

	public float GetHealthRatio()
	{
		return (float)currentHealth / MaxHealth;
	}

	public void DropItem()
	{
		foreach (ItemData item in itemToDrop)
		{
			Instantiate(item.dropPrefab, transform.position + new Vector3(1, 1), Quaternion.identity);
		}
	}
}
