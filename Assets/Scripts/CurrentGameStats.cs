public class CurrentGameStats
{
	private string moneyCollected = EncryptString.Encrypt("0");

	private GameStatistics gameStatistics;

	public GameStatistics GameStatistics => gameStatistics;

	public int MoneyCollected => int.Parse(EncryptString.Decrypt(moneyCollected.ToString()));

	public CurrentGameStats()
	{
		gameStatistics = new GameStatistics(isGlobal: false);
	}

	public void PickupMoney(int amount, bool showUIText = true)
	{
		int num = int.Parse(EncryptString.Decrypt(moneyCollected)) + amount;
		moneyCollected = EncryptString.Encrypt(num.ToString());
		if (showUIText)
		{
			GameplayCommons.Instance.gameplayUIController.ShowProfitText(num);
		}
		GlobalCommons.Instance.globalGameStats.IncreaseMoney(amount);
	}
}
