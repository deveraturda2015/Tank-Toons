namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/InputFocus")]
	public class InputFocus : MonoBehaviour
	{
		protected InputField _inputField;

		public bool _ignoreNextActivation;

		private void Start()
		{
			_inputField = GetComponent<InputField>();
		}

		private void Update()
		{
			if (UnityEngine.Input.GetKeyUp(KeyCode.Return) && !_inputField.isFocused)
			{
				if (_ignoreNextActivation)
				{
					_ignoreNextActivation = false;
					return;
				}
				_inputField.Select();
				_inputField.ActivateInputField();
			}
		}

		public void buttonPressed()
		{
			bool flag = _inputField.text == string.Empty;
			_inputField.text = string.Empty;
			if (!flag)
			{
				_inputField.Select();
				_inputField.ActivateInputField();
			}
		}

		public void OnEndEdit(string textString)
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
			{
				bool flag = _inputField.text == string.Empty;
				_inputField.text = string.Empty;
				if (flag)
				{
					_ignoreNextActivation = true;
				}
			}
		}
	}
}
