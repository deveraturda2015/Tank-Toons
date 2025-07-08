using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTanksController : MonoBehaviour
{
	public EffectsSpawner effectsSpawner;

	internal List<MainMenuTankController> tanks = new List<MainMenuTankController>();

	internal List<MainMenuGuidedRocket> rockets = new List<MainMenuGuidedRocket>();

	private TileMap tileMap;

	private float shakeFactor;

	private float initialCameraOrtoSize;

	private float targetCameraOrtoSize;

	private float zoomInOutSpeed = 0.3f;

	private bool initialZoomCompleted;

	public bool FireRockets;

	public TileMap.TilesType TilesType;

	public bool DoInitialZoom;

	public bool ReduceTanksCount;

	public bool SlowTanksDown;

	public bool TouchKillsTanks;

	public bool dragCameraAround;

	public bool DisableTanks;

	private bool initialCameraDrag = true;

	private bool shakeXdirection;

	private bool shakeYdirection;

	private bool shakeOrtoDirection;

	private bool shakeRotationDirection;

	private void Start()
	{
		Camera.main.orthographicSize = CameraController.GetOrthoSize();
		targetCameraOrtoSize = (initialCameraOrtoSize = Camera.main.orthographicSize);
		effectsSpawner = new EffectsSpawner(EffectsSpawner.EffectsSpawnerPreset.MainMenu);
		WeaponTypes[] array = DisableTanks ? new WeaponTypes[0] : ((!ReduceTanksCount) ? new WeaponTypes[9]
		{
			WeaponTypes.machinegun,
			WeaponTypes.shotgun,
			WeaponTypes.minigun,
			WeaponTypes.cannon,
			WeaponTypes.gold,
			WeaponTypes.homingRocket,
			WeaponTypes.laser,
			WeaponTypes.railgun,
			WeaponTypes.suicide
		} : new WeaponTypes[3]
		{
			WeaponTypes.machinegun,
			WeaponTypes.shotgun,
			WeaponTypes.cannon
		});
		for (int i = 0; i < array.Length; i++)
		{
			MainMenuTankController component = UnityEngine.Object.Instantiate(Prefabs.mainMenuTank, RandomPoints.GetRandomMovePoint(), Quaternion.identity).GetComponent<MainMenuTankController>();
			component.InitTank(this, array[i], SlowTanksDown);
			tanks.Add(component);
		}
		if (FireRockets)
		{
			for (int j = 0; j < 2; j++)
			{
				MainMenuGuidedRocket component2 = UnityEngine.Object.Instantiate(Prefabs.mainMenuRocket, RandomPoints.GetRandomOffScreenPoint(), Quaternion.identity).GetComponent<MainMenuGuidedRocket>();
				component2.InitRocket(this);
				rockets.Add(component2);
			}
		}
		InitTileMap();
		float orthographicSize = Camera.main.orthographicSize;
		if (DoInitialZoom)
		{
			Camera.main.orthographicSize = orthographicSize * 0.5f;
			Camera.main.DOOrthoSize(orthographicSize, 0.5f).OnCompleteWithCoroutine(ProcessInitialSizeCompleted);
		}
		else
		{
			initialZoomCompleted = true;
		}
		if (dragCameraAround)
		{
			PanTilemapCamera();
		}
	}

	private void PanTilemapCamera()
	{
		float num = 2f;
		Vector3 vector = default(Vector3);
		Vector3 vector2;
		do
		{
			float x = UnityEngine.Random.Range(0f - num, num);
			float y = UnityEngine.Random.Range(0f - num, num);
			Vector3 position = Camera.main.transform.position;
			vector = new Vector3(x, y, position.z);
			vector2 = vector - Camera.main.transform.position;
		}
		while (vector2.magnitude < num / 2f);
		float duration = vector2.magnitude * 6f;
		float delay = (!initialCameraDrag) ? UnityEngine.Random.Range(0.5f, 1f) : UnityEngine.Random.Range(0.1f, 0.5f);
		Camera.main.transform.DOMove(vector, duration).OnCompleteWithCoroutine(PanTilemapCamera).SetEase(Ease.InOutQuint)
			.SetDelay(delay);
	}

	private void ProcessInitialSizeCompleted()
	{
		initialZoomCompleted = true;
	}

	private void InitTileMap()
	{
		tileMap = UnityEngine.Object.FindObjectOfType<TileMap>();
		tileMap.InitTileMap(GlobalCommons.Instance.GetTilesTypeForMenu());
		Transform transform = tileMap.transform;
		Vector3 position = tileMap.transform.position;
		float x = position.x - 17f;
		Vector3 position2 = tileMap.transform.position;
		float y = position2.y - 9f;
		Vector3 position3 = tileMap.transform.position;
		transform.position = new Vector3(x, y, position3.z);
	}

	private void ProcessTap(Vector3 position)
	{
		if (!TouchKillsTanks)
		{
			return;
		}
		for (int i = 0; i < tanks.Count; i++)
		{
			if (Vector2.Distance(tanks[i].tankBase.transform.position, position) <= 1f)
			{
				tanks[i].ResetTank(showExplosion: true);
			}
		}
		for (int j = 0; j < rockets.Count; j++)
		{
			if (Vector2.Distance(rockets[j].transform.position, position) <= 1f)
			{
				rockets[j].ExplodeRocket();
			}
		}
	}

	private void Update()
	{
		ProcessTouches();
		effectsSpawner.Update();
		ProcessShake();
	}

	private void ProcessShake()
	{
		if (!initialZoomCompleted)
		{
			return;
		}
		if (targetCameraOrtoSize != initialCameraOrtoSize)
		{
			if (targetCameraOrtoSize > initialCameraOrtoSize)
			{
				targetCameraOrtoSize -= zoomInOutSpeed;
				if (targetCameraOrtoSize < initialCameraOrtoSize)
				{
					targetCameraOrtoSize = initialCameraOrtoSize;
				}
			}
			else
			{
				targetCameraOrtoSize += zoomInOutSpeed;
				if (targetCameraOrtoSize > initialCameraOrtoSize)
				{
					targetCameraOrtoSize = initialCameraOrtoSize;
				}
			}
		}
		if (shakeFactor > 0f && Time.timeScale > 0f)
		{
			shakeFactor -= Time.deltaTime * CameraController.SHAKE_FACTOR_DEPLETIONRATE;
			if (shakeFactor < 0f)
			{
				shakeFactor = 0f;
			}
			float num = shakeFactor * CameraController.SHAKE_FACTOR_MOD;
			float num2 = shakeFactor * CameraController.SHAKE_ORTO_MOD;
			float num3 = shakeFactor * CameraController.SHAKE_ROTATION_MOD;
			if (shakeOrtoDirection)
			{
				num2 *= -1f;
			}
			if (shakeRotationDirection)
			{
				num3 *= -1f;
			}
			Camera.main.orthographicSize = targetCameraOrtoSize + num2;
			Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, num3);
		}
		else
		{
			Camera.main.orthographicSize = targetCameraOrtoSize;
		}
	}

	private void ProcessTouches()
	{
		int touchCount = UnityEngine.Input.touchCount;
		for (int i = 0; i < touchCount; i++)
		{
			Touch touch = UnityEngine.Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began)
			{
				Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
				ProcessTap(position);
			}
		}
	}

	public void ShakeCamera(float amount = 5f)
	{
		if (initialZoomCompleted && GlobalCommons.Instance.globalGameStats.ScreenShakeEnabled)
		{
			if (shakeFactor < amount)
			{
				shakeFactor = amount;
			}
			shakeXdirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
			shakeYdirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
			shakeOrtoDirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
			shakeRotationDirection = ((UnityEngine.Random.value > 0.5f) ? true : false);
		}
	}
}
