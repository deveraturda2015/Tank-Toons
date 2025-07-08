public class ShowSimpleMessageRequest
{
	public string MessageText;

	public float Scale;

	public ShowSimpleMessageRequest(string messageText, float scale)
	{
		Scale = scale;
		MessageText = messageText;
	}
}
