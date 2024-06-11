using UnityEngine;

public class EquipTool : Equip
{
	public float attackRate;
	private bool attacking;
	public float useStamina;

	[Header("Combat")]
	public int onCreatureDamage;
	public int onWoodDamage;
	public int onMineralDamage;
	public int AttackDistance;

	[Header("TargetTypes")]
	private int _creatureLayer;
	private int _woodLayer;
	private int _mineralLayer;

	[Header("Audio")]
	public AudioClip AttackClip;
	public AudioSource AttackSoundSource;

	private Animator animator;
	private new Camera camera;

	private void Start()
	{
		animator = GetComponent<Animator>();
		camera = Camera.main;
		_creatureLayer = LayerMask.NameToLayer("Creature");
		_woodLayer = LayerMask.NameToLayer("Wood");
		_mineralLayer = LayerMask.NameToLayer("Mineral");
	}

	public override void OnAttackInput()
	{
		base.OnAttackInput();
		if (!attacking)
		{
			if (CharacterManager.Instance.Player.Condition.UseStamina(useStamina))
			{
				AttackSoundSource.PlayOneShot(AttackClip);
				attacking = true;
				animator.SetTrigger("Attack");
				OnHit();
				Invoke("OnCanAttack", attackRate);
			}
		}
	}

	void OnCanAttack()
	{
		attacking = false;
	}

	public void OnHit()
	{
		Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, AttackDistance))
		{
			IDamagable target = hit.collider.GetComponent<IDamagable>();
			int targetLayer = hit.collider.gameObject.layer;

			if (targetLayer == _creatureLayer)
			{
				target.TakePhysicalDamage(onCreatureDamage);
			}
			else if (targetLayer == _woodLayer)
			{
				target.TakePhysicalDamage(onWoodDamage);
			}
			else if (targetLayer == _mineralLayer)
			{
				target.TakePhysicalDamage(onMineralDamage);
			}

			//적 체력 UI에 표시
			CharacterManager.Instance.Player.Condition.EnemyHealthUIUpdate(target.GetHealthRatio());
		}
	}

}
