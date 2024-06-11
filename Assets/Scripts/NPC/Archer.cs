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

	// �÷��̾���� �Ÿ��� ����ؼ� ����
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

	// ���´�
	// Idle = ���� �ڸ����� ���
	// Wandering = �÷��̾ �������� ����� ���ڸ��� ���ư�
	// Attaking = �÷��̾ ���� ������ ������ �Ѿư��� ����
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

	// ��� �ϴٰ� �÷��̾ ������ �߰�
	// �־����� ���� �ڸ��� ����, ���� �ڸ��� ���ƿ��� idle�� ���
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

	// ���� ���� ������ ����
	// �������� ����� �߰�
	// �߰� �Ұ����ϰų� �߰� ���� ���̸� ���� �ڸ��� ����
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

	// ���� �տ� ������ ����
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
