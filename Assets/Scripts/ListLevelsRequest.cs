using System;
using UnityEngine.Networking;

internal class ListLevelsRequest
{
	internal const int WITHRESULT_EXPIRATION_TIMEOUT_MIN = 5;

	internal const int NORESULT_EXPIRATION_TIMEOUT_MIN = 6;

	internal UserLevelsMenuController.LevelListType LevelListType;

	internal UserLevelsMenuController.LevelListPeriodType LevelListPeriodType;

	internal int PageSize;

	internal int PageIndex;

	internal UserLevelsMenuController.LoadLevelsListCallbackType LoadLevelsListCallbackType;

	internal DateTime? ResultTimestamp;

	internal UnityWebRequest Request;

	public ListLevelsRequest(UserLevelsMenuController.LevelListType levelListType, UserLevelsMenuController.LevelListPeriodType levelListPeriodType, int pageSize, int pageIndex, UserLevelsMenuController.LoadLevelsListCallbackType loadLevelsListCallbackType)
	{
		LevelListPeriodType = levelListPeriodType;
		LevelListType = levelListType;
		PageSize = pageSize;
		PageIndex = pageIndex;
		LoadLevelsListCallbackType = loadLevelsListCallbackType;
	}

	internal ListLevelsRequest AssociateWWW(UnityWebRequest request)
	{
		Request = request;
		return this;
	}
}
