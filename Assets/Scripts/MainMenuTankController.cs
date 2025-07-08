using DG.Tweening;
using UnityEngine;

public class MainMenuTankController : MonoBehaviour
{
	private enum TankState
	{
		idling,
		moving
	}

	public GameObject tankBase;

	private GameObject tankTurret;

	private float idleStartTimestamp;

	private Rigidbody2D baseRB;

	private int dirtSpewTicker;

	private TankState currentState;

	public MainMenuTanksController tanksController;

	private Vector3 currentMovePoint;

	private SpriteRenderer turretSR;

	private SpriteRenderer baseSR;

	private int baseSpriteIndex;

	public Sprite[] EnemyTurretsSprites;

	public Sprite[] EnemyBasesSprites1;

	public Sprite[] EnemyBasesSprites2;

	private float currentSpeed = 22f;

	private float tracksAnimationTicker;

	private float tracksAnimationTickerMax = 0.05f;

	private float changeTurretTurnDirectionTimestamp;

	private bool slowdownTanks;

	private bool ReinitTurretTurnDirectionInitial = true;

	private bool ReinitIdleStartTimestampInitial = true;

	private void Start()
	{
	}

	public void InitTank(MainMenuTanksController tanksController, WeaponTypes weaponType, bool slowdownTanks)
	{
		this.slowdownTanks = slowdownTanks;
		this.tanksController = tanksController;
		baseSpriteIndex = (int)weaponType;
		tankBase = UnityEngine.Object.Instantiate(Prefabs.enemyTankBasePrefab, base.transform.position, Quaternion.identity);
		tankTurret = UnityEngine.Object.Instantiate(Prefabs.enemyTurretPrefab, base.transform.position, Quaternion.identity);
		tankBase.transform.parent = base.transform;
		tankTurret.transform.parent = base.transform;
		tankBase.AddComponent<MainMenuTankBaseController>();
		baseRB = tankBase.GetComponent<Rigidbody2D>();
		ReinitIdleStartTimestamp();
		turretSR = tankTurret.GetComponent<SpriteRenderer>();
		turretSR.sprite = EnemyTurretsSprites[(int)weaponType];
		baseSR = tankBase.GetComponent<SpriteRenderer>();
		baseSR.sprite = EnemyBasesSprites1[baseSpriteIndex];
		tankBase.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		tankTurret.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		TurnTurret();
	}

	private void TurnTurret()
	{
		tankTurret.transform.DORotate(new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f)), UnityEngine.Random.Range(0.5f, 1f)).SetEase(Ease.InOutCubic).SetDelay(UnityEngine.Random.Range(0.5f, 1.5f))
			.OnCompleteWithCoroutine(TurnTurret);
	}

	public void ResetTank(bool showExplosion = false)
	{
		tanksController.effectsSpawner.CreateTankExplodedEffect(tankBase.transform.position);
		tanksController.effectsSpawner.CreateTankExplodedEffect(tankBase.transform.position);
		tanksController.effectsSpawner.SpawnExplosionDecal(tankBase.transform.position);
		if (showExplosion)
		{
			tanksController.effectsSpawner.CreateExplosionEffect(tankBase.transform.position);
			tanksController.ShakeCamera();
		}
		currentState = TankState.idling;
		ReinitIdleStartTimestamp();
		Vector3 position = RandomPoints.GetRandomOffScreenPoint() * UnityEngine.Random.Range(1f, 1.5f);
		tankBase.transform.position = position;
	}

	private void ReinitIdleStartTimestamp()
	{
		if (ReinitIdleStartTimestampInitial)
		{
			ReinitIdleStartTimestampInitial = false;
			if (UnityEngine.Random.value > 0.5f)
			{
				idleStartTimestamp = Time.fixedTime;
			}
			idleStartTimestamp = Time.fixedTime + UnityEngine.Random.Range(0.5f, 2f);
		}
		else if (slowdownTanks)
		{
			idleStartTimestamp = Time.fixedTime + UnityEngine.Random.Range(3f, 5f);
		}
		else
		{
			idleStartTimestamp = Time.fixedTime + UnityEngine.Random.Range(1f, 3f);
		}
	}

	public void ProcessOtherTankCollision(MainMenuTankController otherTank)
	{
		currentState = TankState.moving;
		ReinitCurrentSpeed();
		currentMovePoint = tankBase.transform.position + (tankBase.transform.position - otherTank.tankBase.transform.position) * UnityEngine.Random.Range(1f, 3f);
	}

	private void ReinitCurrentSpeed()
	{
		if (slowdownTanks)
		{
			currentSpeed = UnityEngine.Random.Range(9f, 13f);
		}
		else
		{
			currentSpeed = UnityEngine.Random.Range(10f, 25f);
		}
	}

	private void Update()
	{
		switch (currentState)
		{
		case TankState.idling:
			if (Time.fixedTime > idleStartTimestamp)
			{
				currentState = TankState.moving;
				currentMovePoint = RandomPoints.GetRandomMovePoint();
				ReinitCurrentSpeed();
			}
			break;
		case TankState.moving:
			AnimateTracks();
			if (Vector2.Distance(tankBase.transform.position, currentMovePoint) < 1f)
			{
				currentState = TankState.idling;
				ReinitIdleStartTimestamp();
			}
			break;
		}
		tankTurret.transform.position = tankBase.transform.position;
	}

	private void FixedUpdate()
	{
		if (currentState == TankState.moving)
		{
			MoveTo(currentMovePoint);
		}
	}

	private void MoveTo(Vector3 moveToPoint)
	{
		Vector2 targetVector = moveToPoint - tankBase.transform.position;
		float magnitude = targetVector.magnitude;
		float num = currentSpeed;
		baseRB.AddForce(targetVector.normalized * num, ForceMode2D.Force);
		RotateBodyToDirection(targetVector, baseRB, num / 2f);
	}

	private void RotateBodyToDirection(Vector2 targetVector, Rigidbody2D rb, float torqueFactor)
	{
		float num = Mathf.Abs(Vector2.Angle(tankBase.transform.up, targetVector));
		Vector3 vector = Vector3.Cross(rb.transform.up, targetVector);
		vector.Normalize();
		float z = vector.z;
		rb.AddTorque(torqueFactor * z * num / 90f);
		if (z != 0f)
		{
			SpawnTracksDirt(z, num);
			SpawnTracksDirt(z, num);
		}
	}

	private void SpawnTracksDirt(float turnDirection, float angleDif)
	{
		dirtSpewTicker++;
		float num = float.MaxValue;
		int num2 = dirtSpewTicker % 2;
		if (angleDif > 90f)
		{
			switch (num2)
			{
			case 0:
				num = ((!(turnDirection < 0f)) ? 135f : (-135f));
				break;
			case 1:
				num = ((!(turnDirection < 0f)) ? (-45f) : 45f);
				break;
			}
		}
		else
		{
			switch (num2)
			{
			case 0:
				num = -135f;
				break;
			case 1:
				num = -45f;
				break;
			}
		}
		if (num != float.MaxValue)
		{
			Quaternion rotation = Quaternion.Euler(0f, 0f, num);
			Vector3 a = rotation * tankBase.transform.right;
			a.Normalize();
			a *= 0.5f;
			Vector3 position = tankBase.transform.position;
			float x = position.x + a.x;
			Vector3 position2 = tankBase.transform.position;
			float y = position2.y + a.y;
			Vector3 position3 = tankBase.transform.position;
			Vector3 coords = new Vector3(x, y, position3.z);
			tanksController.effectsSpawner.CreateTracksDirtEffect(coords);
		}
	}

	private void AnimateTracks()
	{
		Vector2 velocity = baseRB.velocity;
		if (Mathf.Abs(velocity.x) > 0.1f || Mathf.Abs(velocity.y) > 0.1f)
		{
			tracksAnimationTicker += Time.deltaTime;
		}
		if (tracksAnimationTicker > tracksAnimationTickerMax)
		{
			do
			{
				tracksAnimationTicker -= tracksAnimationTickerMax;
			}
			while (tracksAnimationTicker > tracksAnimationTickerMax);
			if (baseSR.sprite == EnemyBasesSprites1[baseSpriteIndex])
			{
				baseSR.sprite = EnemyBasesSprites2[baseSpriteIndex];
			}
			else
			{
				baseSR.sprite = EnemyBasesSprites1[baseSpriteIndex];
			}
		}
	}
}
