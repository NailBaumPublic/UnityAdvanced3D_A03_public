using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Archer : MonoBehaviour, IDamagable, DropItem
{
	[Header("Stats")]
	public int NPCID = 2;
	public int MaxHealth;
	int _currentHealth;

	[Header("AI")]
	private NavMeshAgent agent;
	public float detectDistance;
	private AIState aiState;
	private Vector3 _startPosition;
	private float playerDistance;
	public float fieldOfView = 120f;
	private int IsMoving = Animator.StringToHash("IsMoving");
	private int IsAttacking = Animator.StringToHash("IsAttacking");

	[Header("Combat")]
	public int damage;
	public float attackRate;
	private float lastAttackTime;
	public float attackDistance;
	public GameObject Arrow;
	private Quaternion rotationOrigin;
	
	private Animator animator;
	private SkinnedMeshRenderer[] meshRenderer;
	public ItemData[] itemToDrop;

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		meshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
		_currentHealth = MaxHealth;
		_startPosition = transform.position;
	}

	private void Start()
	{
		SetState(AIState.Idle);
	}

	// 플레이어와의 거리를 계산해서 저장
	private void Update()
	{
		playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

		animator.SetBool(IsMoving, aiState != AIState.Idle);

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

	private void LateUpdate()
	{
		transform.rotation = rotationOrigin;
	}

	// 상태는
	// Idle = 원래 자리에서 대기
	// Wandering = 플레이어가 범위에서 벗어나면 제자리로 돌아감
	// Attaking = 플레이어가 감지 범위에 들어오면 쫓아가서 공격
	public void SetState(AIState state)
	{
		aiState = state;

		switch (aiState)
		{
			case AIState.Idle:
				agent.isStopped = true;
				break;
			case AIState.Wandering:
				agent.isStopped = false;
				break;
			case AIState.Attacking:
				agent.isStopped = false;
				break;
		}
	}

	// 대기 하다가 플레이어가 가까우면 추격
	// 멀어지면 원래 자리로 복귀, 원래 자리로 돌아오면 idle로 대기
	void PassiveUpdate()
	{
		if ((aiState == AIState.Wandering && agent.remainingDistance < 1.5f))
		{
			SetState(AIState.Idle);
		}

		if (playerDistance < detectDistance)
		{
			SetState(AIState.Attacking);
		}
	}

	// 적이 공격 범위면 공격
	// 범위에서 벗어나면 추격
	// 추격 불가능하거나 추격 범위 밖이면 원래 자리로 복귀
	void AttackingUpdate()
	{
		if (playerDistance < attackDistance && IsPlayerInFieldOfView())
		{
			agent.isStopped = true;
			if (Time.time - lastAttackTime > attackRate)
			{
				rotationOrigin = transform.rotation;
				lastAttackTime = Time.time;
				animator.SetBool(IsAttacking, true);
				Vector3 dir = (CharacterManager.Instance.Player.transform.position - transform.position + (Vector3.down * 1f)).normalized;

				GameObject arrow = Instantiate(Arrow, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
				arrow.GetComponent<Arrow>().direction = dir;
				arrow.GetComponent<Arrow>().damage = damage;
				
			}
		}
		else
		{
			animator.SetBool(IsAttacking, false);
			if (playerDistance < detectDistance)
			{
				agent.isStopped = false;
				NavMeshPath path = new NavMeshPath();
				agent.SetDestination(CharacterManager.Instance.Player.transform.position);
				//if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
				//{

				//}
				//else
				//{
				//	agent.SetDestination(_startPosition);
				//	SetState(AIState.Wandering);
				//}
			}
			else
			{
				agent.SetDestination(_startPosition);
				SetState(AIState.Wandering);
			}
		}
	}

	// 적이 앞에 있으면 공격
	bool IsPlayerInFieldOfView()
	{
		Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
		float angle = Vector3.Angle(transform.forward, directionToPlayer);
		return angle < fieldOfView * 0.5f;
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

	public void TakePhysicalDamage(int damage)
	{
		_currentHealth -= damage;
		if (_currentHealth <= 0)
		{
			Die();
		}

		StartCoroutine(DamageFlash());
	}

	public float GetHealthRatio()
	{
		return (float)_currentHealth / MaxHealth;
	}

	public void DropItem()
	{
		foreach (ItemData item in itemToDrop)
		{
			Instantiate(item.dropPrefab, transform.position, Quaternion.identity);
		}
	}
}
