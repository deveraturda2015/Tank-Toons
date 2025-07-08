using System;
using System.Globalization;
using UnityEngine;

public class PromotionController
{
	private DateTime PromoStartTime = new DateTime(2000, 1, 1, 10, 10, 10);

	private DateTime PromoEndTime = new DateTime(2010, 5, 1, 19, 43, 20);

	public TimeSpan? PromoTimeLeft
	{
		get
		{
			if (DateTime.Now > PromoStartTime && DateTime.Now < PromoEndTime)
			{
				return PromoEndTime - DateTime.Now;
			}
			return null;
		}
	}

	public PromotionController()
	{
		RemoteSettings.Updated += HandleRemoteSettingsUpdate;
	}

	private void HandleRemoteSettingsUpdate()
	{
		string @string = RemoteSettings.GetString("PROMO_START_DATETIME", "2000-05-08 14:40:52,531");
		string string2 = RemoteSettings.GetString("PROMO_END_DATETIME", "2001-05-08 14:40:52,531");
		bool @bool = RemoteSettings.GetBool("PROMO_SHOW_PAYING", defaultValue: false);
		bool bool2 = RemoteSettings.GetBool("PROMO_SHOW_NONPAYING", defaultValue: false);
		int @int = RemoteSettings.GetInt("PROMO_LEVELS_REQUIRED", 100500);
		DateTime result = new DateTime(2000, 1, 1, 10, 10, 10);
		DateTime result2 = new DateTime(2001, 1, 1, 10, 10, 10);
		if (((GlobalCommons.Instance.globalGameStats.isPayingPlayer && @bool) || (!GlobalCommons.Instance.globalGameStats.isPayingPlayer && bool2)) && @int <= GlobalCommons.Instance.globalGameStats.GameStatistics.GetStat(GameStatistics.Stat.LevelsCompleted) && DateTime.TryParseExact(@string, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out result) && DateTime.TryParseExact(string2, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out result2))
		{
			PromoStartTime = result;
			PromoEndTime = result2;
		}
	}
}
