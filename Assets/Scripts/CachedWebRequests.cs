using System;
using System.Collections.Generic;
using UnityEngine;

internal class CachedWebRequests
{
	internal List<ListLevelsRequest> CachedListLevelsRequests = new List<ListLevelsRequest>();

	internal void UpdateCachedRequests()
	{
		int num = CachedListLevelsRequests.Count;
		while (num-- > 0)
		{
			ListLevelsRequest listLevelsRequest = CachedListLevelsRequests[num];
			if (listLevelsRequest.ResultTimestamp.HasValue)
			{
				DateTime? resultTimestamp = listLevelsRequest.ResultTimestamp;
				DateTime? dateTime = (!resultTimestamp.HasValue) ? null : new DateTime?(resultTimestamp.GetValueOrDefault() + TimeSpan.FromMinutes(5.0));
				if (dateTime.HasValue && dateTime.GetValueOrDefault() < DateTime.Now)
				{
					goto IL_00f7;
				}
			}
			DateTime? resultTimestamp2 = listLevelsRequest.ResultTimestamp;
			DateTime? dateTime2 = (!resultTimestamp2.HasValue) ? null : new DateTime?(resultTimestamp2.GetValueOrDefault() + TimeSpan.FromMinutes(6.0));
			if (!dateTime2.HasValue || !(dateTime2.GetValueOrDefault() < DateTime.Now))
			{
				continue;
			}
			goto IL_00f7;
			IL_00f7:
			UnityEngine.Debug.Log("Cache result expired...");
			CachedListLevelsRequests.RemoveAt(num);
		}
	}
}
