using System.Collections.Generic;
using UnityEngine;

internal class GameObjectPools
{
	private List<SimpleBullet> simpleBullets;

	private int simpleBulletsAmount = 125;

	private int currentSimpleBulletsIndex;

	private List<TripleBullet> tripleBullets;

	private int tripleBulletsAmount = 125;

	private int currentTripleBulletsIndex;

	private List<BonusController> coinBonuses;

	private int coinBonusControllersAmount = 50;

	private int currentCoinBonusControllersIndex;

	public GameObjectPools()
	{
		simpleBullets = new List<SimpleBullet>();
		for (int i = 0; i < simpleBulletsAmount; i++)
		{
			simpleBullets.Add(GetNewInactiveSimpleBullet());
		}
		tripleBullets = new List<TripleBullet>();
		for (int j = 0; j < tripleBulletsAmount; j++)
		{
			tripleBullets.Add(GetNewInactiveTripleBullet());
		}
		coinBonuses = new List<BonusController>();
		for (int k = 0; k < coinBonusControllersAmount; k++)
		{
			coinBonuses.Add(GetNewInactiveCoinBonusController());
		}
	}

	private BonusController GetNewInactiveCoinBonusController()
	{
		BonusController component = Object.Instantiate(Prefabs.coinBonusPrefab).GetComponent<BonusController>();
		component.doDestroyGameobject = false;
		component.gameObject.SetActive(value: false);
		return component;
	}

	private SimpleBullet GetNewInactiveSimpleBullet()
	{
		SimpleBullet component = Object.Instantiate(Prefabs.simpleBulletPrefab).GetComponent<SimpleBullet>();
		component.gameObject.SetActive(value: false);
		return component;
	}

	private TripleBullet GetNewInactiveTripleBullet()
	{
		TripleBullet component = Object.Instantiate(Prefabs.tripleBulletPrefab).GetComponent<TripleBullet>();
		component.gameObject.SetActive(value: false);
		return component;
	}

	public void InitializeSimpleBullet(Vector3 position, float angleFix, float forceFix, EnemyTankController etc, float bulletDamage)
	{
		SimpleBullet simpleBullet = simpleBullets[currentSimpleBulletsIndex];
		if (simpleBullet.gameObject.activeInHierarchy)
		{
			simpleBullet = GetNewInactiveSimpleBullet();
			simpleBullets.Insert(currentSimpleBulletsIndex, simpleBullet);
			currentSimpleBulletsIndex++;
		}
		simpleBullet.gameObject.SetActive(value: true);
		simpleBullet.transform.position = position;
		simpleBullet.Initialize(angleFix, forceFix, etc, bulletDamage);
		currentSimpleBulletsIndex++;
		if (currentSimpleBulletsIndex >= simpleBullets.Count)
		{
			currentSimpleBulletsIndex = 0;
		}
	}

	public void InitializeTripleBullet(Vector3 position, float angleFix, float forceFix, EnemyTankController etc, float bulletDamage)
	{
		TripleBullet tripleBullet = tripleBullets[currentTripleBulletsIndex];
		if (tripleBullet.gameObject.activeInHierarchy)
		{
			tripleBullet = GetNewInactiveTripleBullet();
			tripleBullets.Insert(currentTripleBulletsIndex, tripleBullet);
			currentTripleBulletsIndex++;
		}
		tripleBullet.gameObject.SetActive(value: true);
		tripleBullet.transform.position = position;
		tripleBullet.Initialize(angleFix, forceFix, etc, bulletDamage);
		currentTripleBulletsIndex++;
		if (currentTripleBulletsIndex >= tripleBullets.Count)
		{
			currentTripleBulletsIndex = 0;
		}
	}

	public void InitializeCoin(Vector3 position, BonusController.BonusType coinType)
	{
		BonusController bonusController = coinBonuses[currentCoinBonusControllersIndex];
		if (bonusController.gameObject.activeInHierarchy)
		{
			bonusController = GetNewInactiveCoinBonusController();
			coinBonuses.Insert(currentCoinBonusControllersIndex, bonusController);
			currentCoinBonusControllersIndex++;
		}
		bonusController.gameObject.SetActive(value: true);
		bonusController.transform.position = position;
		bonusController.SetupCoin(coinType);
		currentCoinBonusControllersIndex++;
		if (currentCoinBonusControllersIndex >= coinBonuses.Count)
		{
			currentCoinBonusControllersIndex = 0;
		}
	}
}
