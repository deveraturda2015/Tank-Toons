public interface CloudSaveManager
{
	void Initialize();

	SavegameData GetCloudSaveData();

	bool LoadGame();

	bool SaveGame();
}
