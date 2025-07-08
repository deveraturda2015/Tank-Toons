using UnityEngine;

public class MainMenuGuidedRocket : MonoBehaviour
{
	private float lastTimeEmittedSmoke;

	private Vector3 smokeEmitCoord;

	private float smokeEmitTimeout = 0.08f;

	private Rigidbody2D rocketRigidBody;

	private float linearMoveFactor = 0.3f;

	private float torqueFactor = 0.006f;

	private Vector3 targetVector;

	private Vector2 resetCoordsMultiplierRange = new Vector2(1.5f, 2f);

	private MainMenuTanksController mmtc;

	public bool IsExploding;

	private void Start()
	{
		smokeEmitCoord = base.transform.position;
		rocketRigidBody = GetComponent<Rigidbody2D>();
		targetVector = base.transform.position * -1f;
	}

	public void InitRocket(MainMenuTanksController mmtc)
	{
		this.mmtc = mmtc;
		ResetRocket();
	}

	public void ResetRocket()
	{
		Vector3 position = RandomPoints.GetRandomOffScreenPoint() * UnityEngine.Random.Range(resetCoordsMultiplierRange.x, resetCoordsMultiplierRange.y);
		base.transform.position = position;
		targetVector = base.transform.position * -1f;
	}

	private void Update()
	{
		if (Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			lastTimeEmittedSmoke = Time.fixedTime;
			mmtc.effectsSpawner.SpawnSmoke(smokeEmitCoord, 1, 0f);
			smokeEmitCoord = base.transform.position;
		}
		Vector3 position = base.transform.position;
		if (!(Mathf.Abs(position.x) > GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell * resetCoordsMultiplierRange.y))
		{
			Vector3 position2 = base.transform.position;
			if (!(Mathf.Abs(position2.y) > GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell * resetCoordsMultiplierRange.y))
			{
				return;
			}
		}
		ResetRocket();
	}

	private void FixedUpdate()
	{
		FaceTarget();
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector = rotation * base.transform.right;
		rocketRigidBody.AddForce(vector.normalized * linearMoveFactor);
	}

	private void FaceTarget()
	{
		float num = Mathf.Abs(Vector2.Angle(base.transform.up, targetVector)) / 90f;
		Vector3 vector = Vector3.Cross(rocketRigidBody.transform.up, targetVector);
		vector.Normalize();
		float z = vector.z;
		rocketRigidBody.AddTorque(torqueFactor * z * num);
	}

	public void ExplodeRocket()
	{
		IsExploding = true;
		mmtc.effectsSpawner.CreateExplosionEffect(base.transform.position);
		mmtc.effectsSpawner.SpawnExplosionDecal(base.transform.position);
		for (int i = 0; i < mmtc.tanks.Count; i++)
		{
			if (Vector2.Distance(mmtc.tanks[i].tankBase.transform.position, base.transform.position) <= 1.5f)
			{
				mmtc.tanks[i].ResetTank();
			}
		}
		for (int j = 0; j < mmtc.rockets.Count; j++)
		{
			if (!mmtc.rockets[j].IsExploding && Vector2.Distance(mmtc.rockets[j].transform.position, base.transform.position) <= 1.5f)
			{
				mmtc.rockets[j].ExplodeRocket();
			}
		}
		ResetRocket();
		IsExploding = false;
		mmtc.ShakeCamera();
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		ExplodeRocket();
	}
}
