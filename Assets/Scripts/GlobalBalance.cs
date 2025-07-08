using System;

public class GlobalBalance
{
	public const int GOLD_PACK_1_AMOUNT = 160000;

	public const int GOLD_PACK_2_AMOUNT = 400000;

	public const int GOLD_PACK_3_AMOUNT = 1000000;

	public const int GOLD_PACK_4_AMOUNT = 2400000;

	public const int GOLD_PACK_5_AMOUNT = 6000000;

	public static int[] goldPacksAmount = new int[5]
	{
		160000,
		400000,
		1000000,
		2400000,
		6000000
	};

	private static int[] PrizeIntervalsSeconds = new int[7]
	{
		0,
		1800,
		3600,
		7200,
		10800,
		14400,
		18000
	};

	public const float ARENA_ADDITIONAL_INCOME_COEFFICIENT = 1.25f;

	public const int LEVELS_COMPLETED_NEEDED_FOR_OFFERS = 4;

	public const int LEVELS_COMPLETED_NEEDED_FOR_PRIZE = 2;

	public const int ARENA_LEVEL_REQUIREMENT = 10;

	public const int SURVIVAL_LEVEL_REQUIREMENT = 20;

	internal static float turretHPCoeff = 2.5f;

	public const int COINVALUE_BRONZE = 5;

	public const int COINVALUE_GOLDEN = 20;

	public const int COINVALUE_PLATINUM = 50;

	public const float GLOBAL_PRICES_COEFFICIENT = 0.3251953f;

	public const float GLOBAL_INCOME_COEFFICIENT = 0.12195f;

	public const float DISABLED_AUTOAIM_INCOME_COEFFICIENT = 1.25f;

	public const float DEFAULT_CONTROLS_SENSITIVITY_COEFFICIENT = 1f;

	public const float CONTROLS_SENSITIVITY_MAX = 1.3f;

	public const float CONTROLS_SENSITIVITY_MIN = 0.4f;

	public const float CONTROLS_SENSITIVITY_STEP = 0.1f;

	public const float DEFAULT_ANALOG_MAX_INCHES = 0.38f;

	public const float ANALOG_MAX_MIN = 40f;

	public const float ANALOG_MAX_MAX = 350f;

	public static TimeSpan GetTimeLeftForPrize(bool initialCheck = true)
	{
		int num = (GlobalCommons.Instance.globalGameStats.PrizeLevel < PrizeIntervalsSeconds.Length) ? GlobalCommons.Instance.globalGameStats.PrizeLevel : (PrizeIntervalsSeconds.Length - 1);
		TimeSpan timeSpan = TimeSpan.FromSeconds(PrizeIntervalsSeconds[num]);
		DateTime d = GlobalCommons.Instance.globalGameStats.LastTimeGotPrize + timeSpan;
		TimeSpan timeSpan2 = d - DateTime.Now;
		if (initialCheck && timeSpan2 > timeSpan + TimeSpan.FromMinutes(1.0))
		{
			GlobalCommons.Instance.globalGameStats.LastTimeGotPrize = DateTime.Now + timeSpan;
			return GetTimeLeftForPrize(initialCheck: false);
		}
		return timeSpan2;
	}

	internal static float GetExplBarrelHP()
	{
		int num = (GlobalCommons.Instance.SelectedLevelBalanceFactor <= 15) ? GlobalCommons.Instance.SelectedLevelBalanceFactor : 15;
		return 7 + (num - 1);
	}

	internal static float GetExplBarrelDamage()
	{
		return GameplayCommons.Instance.playersTankController.MaxHP / 4f;
	}

	internal static float GetSpawnerHP()
	{
		return 20 + (GlobalCommons.Instance.SelectedLevelBalanceFactor - 1) * 2;
	}

	internal static float GetCrateHP()
	{
		return 6f;
	}

	internal static float GetWallHP()
	{
		return 10 + (GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
	}

	internal static float GetSpecWallHP()
	{
		return 10f + (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1) * 0.75f;
	}

	internal static float GetHarderWallHP()
	{
		return 20 + (GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
	}

	internal static float GetCrackedIndestrWallHP()
	{
		return 20 + (GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
	}
}
