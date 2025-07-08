using System.Collections.Generic;
using UnityEngine;

public class PlayerBalance
{
	internal static List<int> PlayerUnavailableWeaponIndexesList = new List<int>
	{
		9,
		10,
		11
	};

	internal static float freezeTimeout = 5f;

	internal static float invisibilityTimeout = 6f;

	internal static float doubleDamageTimeout = 5f;

	private static List<int[]> weaponsUpgradesCost;

	private static List<int[]> ammoMax;

	private static float[] minigunDamageValues;

	private static float[] laserDamageValues;

	private static float[] tripleDamageValues;

	private static float[] shockerDamageValues;

	internal static List<WeaponTypes> LockedWeaponTypes = new List<WeaponTypes>
	{
		WeaponTypes.triple,
		WeaponTypes.shocker
	};

	internal static float[] MachinegunShootIntervals = new float[7]
	{
		0f,
		0.11f,
		0.088f,
		0.073333f,
		0.062857f,
		0.055f,
		0.04888888f
	};

	internal static float[] shotguDamageValues = new float[7]
	{
		0f,
		1.5f,
		1.66f,
		1.825f,
		2f,
		2.16f,
		2.325f
	};

	internal static float[] cannonExplosionDamageValues = new float[7]
	{
		0f,
		45.08f,
		53.4f,
		61.8f,
		70.2f,
		78.6f,
		87f
	};

	internal static float[] homingRocketExplosionDamageValues = new float[7]
	{
		0f,
		23f,
		25.3f,
		27.6f,
		29.9f,
		32.2f,
		34.5f
	};

	internal static float[] guidedRocketExplosionDamageValues = new float[7]
	{
		0f,
		155f,
		170.4f,
		185.8f,
		201.2f,
		216.6f,
		232f
	};

	internal static float[] railgunDamageValues = new float[7]
	{
		0f,
		155f,
		170.4f,
		185.8f,
		201.2f,
		216.6f,
		232f
	};

	internal static float[] mineExplosionDamageValues = new float[7]
	{
		0f,
		103f,
		113.4f,
		123.8f,
		134.2f,
		144.6f,
		155f
	};

	internal static float[] ricochetExplosionDamageValues = new float[7]
	{
		0f,
		155f,
		170.4f,
		185.8f,
		201.2f,
		216.6f,
		232f
	};

	internal static int[] ricochetBounceCountValues = new int[7]
	{
		2,
		2,
		3,
		3,
		3,
		4,
		4
	};

	internal static List<int[]> WeaponsUpgradesCost
	{
		get
		{
			if (weaponsUpgradesCost != null)
			{
				return weaponsUpgradesCost;
			}
			List<int[]> list = new List<int[]>();
			list.Add(new int[6]
			{
				10,
				200,
				400,
				1000,
				2500,
				4000
			});
			list.Add(new int[6]
			{
				11500,
				4000,
				4500,
				5000,
				5500,
				6000
			});
			list.Add(new int[6]
			{
				20250,
				6000,
				6500,
				7000,
				8000,
				9000
			});
			list.Add(new int[6]
			{
				30375,
				9000,
				9500,
				10000,
				11000,
				12000
			});
			list.Add(new int[6]
			{
				45562,
				13000,
				14000,
				15000,
				17000,
				19000
			});
			list.Add(new int[6]
			{
				68343,
				20000,
				21000,
				23000,
				25000,
				27000
			});
			list.Add(new int[6]
			{
				102515,
				32000,
				33000,
				35000,
				37000,
				40000
			});
			list.Add(new int[6]
			{
				204518,
				62000,
				64000,
				68000,
				72000,
				75000
			});
			list.Add(new int[6]
			{
				306776,
				100000,
				105000,
				110000,
				115000,
				120000
			});
			list.Add(new int[6]
			{
				306776,
				100000,
				105000,
				110000,
				115000,
				120000
			});
			list.Add(new int[6]
			{
				306776,
				100000,
				105000,
				110000,
				115000,
				120000
			});
			list.Add(new int[6]
			{
				306776,
				100000,
				105000,
				110000,
				115000,
				120000
			});
			list.Add(new int[6]
			{
				410060,
				136686,
				146686,
				156686,
				166686,
				176686
			});
			list.Add(new int[6]
			{
				512575,
				170858,
				180858,
				190858,
				200858,
				210858
			});
			list.Add(new int[6]
			{
				615090,
				205030,
				215030,
				225030,
				235030,
				245030
			});
			weaponsUpgradesCost = list;
			for (int i = 0; i < weaponsUpgradesCost[0].Length; i++)
			{
				weaponsUpgradesCost[0][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[0][i] * 0.7f);
				weaponsUpgradesCost[1][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[1][i] * 0.47f);
				weaponsUpgradesCost[2][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[2][i] * 1.2f);
				weaponsUpgradesCost[3][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[3][i] * 1.8f);
				weaponsUpgradesCost[4][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[4][i] * 1.8f);
				weaponsUpgradesCost[5][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[5][i] * 1.8f);
				weaponsUpgradesCost[6][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[6][i] * 2.6f);
				weaponsUpgradesCost[7][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[7][i] * 3f);
				weaponsUpgradesCost[8][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[8][i] * 3.5f);
				weaponsUpgradesCost[12][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[12][i] * 3.5f);
				weaponsUpgradesCost[13][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[13][i] * 3.5f);
				weaponsUpgradesCost[14][i] = Mathf.FloorToInt((float)weaponsUpgradesCost[14][i] * 3.5f);
			}
			int num = 0;
			for (int j = 0; j < weaponsUpgradesCost.Count; j++)
			{
				for (int k = 0; k < weaponsUpgradesCost[0].Length; k++)
				{
					weaponsUpgradesCost[j][k] = Mathf.FloorToInt((float)weaponsUpgradesCost[j][k] * 0.3251953f);
					weaponsUpgradesCost[j][k] = GetRoundedPrice(weaponsUpgradesCost[j][k]);
					num += weaponsUpgradesCost[j][k];
				}
			}
			DebugHelper.Log("total weapons upgrades cost: " + num.ToString());
			return weaponsUpgradesCost;
		}
	}

	internal static int[] speedUpgradeCost
	{
		get
		{
			int[] array = new int[10]
			{
				300,
				800,
				1600,
				4000,
				8000,
				16000,
				32000,
				40000,
				48000,
				64000
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Mathf.FloorToInt((float)array[i] * 0.3251953f);
				array[i] = GetRoundedPrice(array[i]);
			}
			return array;
		}
	}

	internal static float[] speedUpgradeValues
	{
		get
		{
			float[] array = new float[11];
			float num = 5f;
			float num2 = 5f / 18f;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = num;
				num += num2;
			}
			return array;
		}
	}

	internal static int[] armorUpgradeCost
	{
		get
		{
			int[] array = new int[15]
			{
				3000,
				6000,
				12000,
				24000,
				48000,
				96000,
				192000,
				384000,
				768000,
				1152000,
				1536000,
				1920000,
				2304000,
				2688000,
				3072000
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Mathf.FloorToInt((float)array[i] * 0.3251953f);
				array[i] = GetRoundedPrice(array[i]);
			}
			return array;
		}
	}

	internal static int[] armorUpgradeValues
	{
		get
		{
			int[] array = new int[16]
			{
				80,
				120,
				170,
				230,
				300,
				380,
				470,
				570,
				680,
				800,
				930,
				1070,
				1220,
				1380,
				1550,
				1730
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i] *= 2;
			}
			return array;
		}
	}

	internal static List<int[]> AmmoMax
	{
		get
		{
			if (ammoMax != null)
			{
				return ammoMax;
			}
			List<int[]> list = new List<int[]>();
			list.Add(new int[7]
			{
				2147483647,
				2147483647,
				2147483647,
				2147483647,
				2147483647,
				2147483647,
				2147483647
			});
			list.Add(new int[9]
			{
				0,
				25,
				27,
				29,
				31,
				33,
				35,
				37,
				39
			});
			list.Add(new int[9]
			{
				0,
				150,
				160,
				170,
				180,
				190,
				200,
				210,
				220
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				23,
				26,
				29,
				32,
				35,
				38,
				41,
				44
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				5,
				6,
				7,
				8,
				9,
				10,
				11,
				12
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			list.Add(new int[9]
			{
				0,
				150,
				160,
				170,
				180,
				190,
				200,
				210,
				220
			});
			list.Add(new int[9]
			{
				0,
				8,
				9,
				10,
				11,
				12,
				13,
				14,
				15
			});
			ammoMax = list;
			for (int i = 1; i < ammoMax.Count; i++)
			{
				for (int j = 0; j < ammoMax[0].Length; j++)
				{
					float num = 1.2f;
					if (i == 5)
					{
						num = 1.3f;
					}
					ammoMax[i][j] = Mathf.CeilToInt((float)ammoMax[i][j] * num);
				}
			}
			for (int k = 1; k < ammoMax.Count; k++)
			{
				for (int l = 0; l < ammoMax[0].Length; l++)
				{
					ammoMax[k][l] *= 2;
				}
			}
			return ammoMax;
		}
	}

	internal static float[] MinigunDamageValues
	{
		get
		{
			if (minigunDamageValues != null)
			{
				return minigunDamageValues;
			}
			minigunDamageValues = new float[7]
			{
				0f,
				2.3f,
				2.55f,
				2.77f,
				3f,
				3.225f,
				3.45f
			};
			for (int i = 0; i < minigunDamageValues.Length; i++)
			{
				minigunDamageValues[i] *= 1.1f;
			}
			return minigunDamageValues;
		}
	}

	internal static float[] LaserDamageValues
	{
		get
		{
			if (laserDamageValues != null)
			{
				return laserDamageValues;
			}
			laserDamageValues = new float[7]
			{
				0f,
				103f,
				113.4f,
				123.8f,
				134.2f,
				144.6f,
				155f
			};
			for (int i = 0; i < laserDamageValues.Length; i++)
			{
				laserDamageValues[i] *= 1.1f;
			}
			return laserDamageValues;
		}
	}

	internal static float[] TripleDamageValues
	{
		get
		{
			if (tripleDamageValues != null)
			{
				return tripleDamageValues;
			}
			tripleDamageValues = new float[7]
			{
				0f,
				2.3f,
				2.55f,
				2.77f,
				3f,
				3.225f,
				3.45f
			};
			for (int i = 0; i < tripleDamageValues.Length; i++)
			{
				tripleDamageValues[i] *= 1.1f;
			}
			return tripleDamageValues;
		}
	}

	internal static float[] ShockerDamageValues
	{
		get
		{
			if (shockerDamageValues != null)
			{
				return shockerDamageValues;
			}
			shockerDamageValues = new float[7]
			{
				0f,
				103f,
				113.4f,
				123.8f,
				134.2f,
				144.6f,
				155f
			};
			for (int i = 0; i < shockerDamageValues.Length; i++)
			{
				shockerDamageValues[i] *= 1.1f;
			}
			return shockerDamageValues;
		}
	}

	public static int GetRoundedPrice(int initialPrice)
	{
		if (initialPrice < 100)
		{
			return Mathf.FloorToInt((float)initialPrice / 10f) * 10;
		}
		if (initialPrice < 1000)
		{
			return Mathf.FloorToInt((float)initialPrice / 50f) * 50;
		}
		if (initialPrice < 15000)
		{
			return Mathf.FloorToInt((float)initialPrice / 100f) * 100;
		}
		return Mathf.FloorToInt((float)initialPrice / 500f) * 500;
	}
}
