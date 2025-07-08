using UnityEngine;

public class GuidedRocket : MonoBehaviour
{
	private Rigidbody2D rocketRigidBody;

	private float torqueFactor = 0.006f;

	private float linearMoveFactor = 0.3f;

	private Vector3 prevPosition;

	private float explosionDamage = 6f;

	private float explosionImpulse = 5f;

	private float explosionRadius = 1f;

	private float launchedTimestamp;

	private float destroyTimeout = 10f;

	private float lastTimeEmittedSmoke;

	private Vector3 smokeEmitCoord;

	private float smokeEmitTimeout = 0.08f;

	private Vector2 lastMovementVector = Vector2.zero;

	private void Start()
	{
		smokeEmitCoord = base.transform.position;
		lastTimeEmittedSmoke = Time.fixedTime;
		launchedTimestamp = Time.fixedTime;
		rocketRigidBody = GetComponent<Rigidbody2D>();
		base.transform.rotation = GameplayCommons.Instance.playersTankController.TankTurret.transform.rotation;
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector = rotation * base.transform.right;
		rocketRigidBody.AddForce(vector.normalized * 0.1f, ForceMode2D.Impulse);
		explosionDamage = PlayerBalance.guidedRocketExplosionDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[6]];
	}

	private void Update()
	{
		if (Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			lastTimeEmittedSmoke = Time.fixedTime;
			Quaternion rotation = Quaternion.Euler(0f, 0f, -90f);
			Vector3 a = rotation * base.transform.right;
			a.Normalize();
			a *= 0.2f;
			Vector3 position = base.transform.position;
			float x = position.x + a.x;
			Vector3 position2 = base.transform.position;
			float y = position2.y + a.y;
			Vector3 position3 = base.transform.position;
			Vector3 coords = new Vector3(x, y, position3.z);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(coords, 1, 0f);
		}
	}

	private void FixedUpdate()
	{
		if (Time.fixedTime - launchedTimestamp >= destroyTimeout || GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			ProcessDestruction();
			return;
		}
		FaceTarget();
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector = rotation * base.transform.right;
		rocketRigidBody.AddForce(vector.normalized * linearMoveFactor);
		prevPosition = base.transform.position;
	}

	private void FaceTarget()
	{
		Vector3 vector = (!(GameplayCommons.Instance.touchesController.MovementVector == Vector2.zero)) ? GameplayCommons.Instance.touchesController.MovementVector : lastMovementVector;
		lastMovementVector = vector;
		float num = Mathf.Abs(Vector2.Angle(base.transform.up, vector)) / 90f;
		Vector3 vector2 = Vector3.Cross(rocketRigidBody.transform.up, vector);
		vector2.Normalize();
		float z = vector2.z;
		rocketRigidBody.AddTorque(torqueFactor * z * num);
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		ProcessDestruction();
	}

	public void DestroyRocket()
	{
		ProcessDestruction();
	}

	private void ProcessDestruction()
	{
		GameplayCommons.Instance.cameraController.ShakeCamera();
		GameplayCommons.Instance.weaponsController.ResetGuidedRocket(this);
		ExplosionResult explosionResult = ExplosionProcessor.ProcessExplosion(ExplosionProcessor.ExplosionTrigger.player, base.transform.position, explosionDamage, explosionImpulse, prevPosition, explosionRadius);
		WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(base.transform.position));
		if (explosionResult.EnemyOrSpawnerOrDestObstWasHit)
		{
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
		}
		else
		{
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
		}
		if (explosionResult.EnemyOrSpawnerWasHit)
		{
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHitEnemyWithGuidedMissile);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
