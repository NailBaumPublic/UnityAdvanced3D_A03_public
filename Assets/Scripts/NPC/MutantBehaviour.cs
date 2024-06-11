using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 뮤턴트 행동방식
//컷씬에 순서에 따라 애니메이션이 작동하고
//전투가 시작되면 주인공을 끝까지 따라온다.
public class MutantBehaviour : MonoBehaviour, IDamagable
{
	[Header("Stats")]
	public int NPCID = 1;
	public int MaxHealth;
	int currentHealth;
	public float walkSpeed;
	public float runSpeed;
	public ItemData[] dropOnDeath;

	[Header("AI")]
	private NavMeshAgent agent;
	public float detectDistance;
	private AIState aiState;

	[Header("Combat")]
	public int damage;
	public float attackRate;
	public float attackDistance;
	private float lastAttackTime;

	private float playerDistance;

	public float fieldOfView = 120f;

	Animator animator;
	private SkinnedMeshRenderer[] meshRenderer;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
		currentHealth = MaxHealth;
	}

	private void Start()
	{
		SetState(AIState.Idle);
	}

	private void Update()
	{
		playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

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
				break;
			case AIState.Attacking:
				agent.speed = runSpeed;
				agent.isStopped = false;
				break;
		}
	}

	void PassiveUpdate()
	{
		if (playerDistance < detectDistance)
		{
			SetState(AIState.Attacking);
		}
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
			}
		}
		else
		{
			agent.isStopped = false;
			agent.SetDestination(CharacterManager.Instance.Player.transform.position);
		}
	}

	bool IsPlayerInFieldOfView()
	{
		Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
		float angle = Vector3.Angle(transform.forward, directionToPlayer);
		return angle < fieldOfView * 0.5f;
	}

	public void Wakeup()
	{
		animator.SetTrigger("WakeUp");
	}

	public void StartFight()
	{
        SoundManager.Instance.MutantMusicOn();
        animator.SetTrigger("StartFight");
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

	public float GetHealthRatio()
	{
		return (float)currentHealth / MaxHealth;
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

	void Die()
	{
		for (int i = 0; i < dropOnDeath.Length; i++)
		{
			Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
		}
		QuestManager.Instance.HuntMonster(NPCID);
		SoundManager.Instance.BackgroundMusicMute();
		Destroy(gameObject);
	}

}
