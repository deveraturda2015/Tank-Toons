namespace UnityEngine.UI.Extensions
{
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/Extensions/SoftMaskScript")]
	public class SoftMaskScript : MonoBehaviour
	{
		private Material mat;

		private Canvas canvas;

		[Tooltip("The area that is to be used as the container.")]
		public RectTransform MaskArea;

		private RectTransform myRect;

		[Tooltip("A Rect Transform that can be used to scale and move the mask - Does not apply to Text UI Components being masked")]
		public RectTransform maskScalingRect;

		[Tooltip("Texture to be used to do the soft alpha")]
		public Texture AlphaMask;

		[Tooltip("At what point to apply the alpha min range 0-1")]
		[Range(0f, 1f)]
		public float CutOff;

		[Tooltip("Implement a hard blend based on the Cutoff")]
		public bool HardBlend;

		[Tooltip("Flip the masks alpha value")]
		public bool FlipAlphaMask;

		private Vector3[] worldCorners;

		private Vector2 AlphaUV;

		private Vector2 min;

		private Vector2 max = Vector2.one;

		private Vector2 p;

		private Vector2 siz;

		private Rect maskRect;

		private Rect contentRect;

		private Vector2 centre;

		private bool isText;

		private void Start()
		{
			myRect = GetComponent<RectTransform>();
			if (!MaskArea)
			{
				MaskArea = myRect;
			}
			if (GetComponent<Graphic>() != null)
			{
				mat = new Material(Shader.Find("UI Extensions/SoftMaskShader"));
				GetComponent<Graphic>().material = mat;
			}
			if ((bool)GetComponent<Text>())
			{
				isText = true;
				mat = new Material(Shader.Find("UI Extensions/SoftMaskShaderText"));
				GetComponent<Text>().material = mat;
				GetCanvas();
				if (base.transform.parent.GetComponent<Mask>() == null)
				{
					base.transform.parent.gameObject.AddComponent<Mask>();
				}
				base.transform.parent.GetComponent<Mask>().enabled = false;
			}
		}

		private void GetCanvas()
		{
			Transform transform = base.transform;
			int num = 100;
			int num2 = 0;
			while (canvas == null && num2 < num)
			{
				canvas = transform.gameObject.GetComponent<Canvas>();
				if (canvas == null)
				{
					transform = GetParentTranform(transform);
				}
				num2++;
			}
		}

		private Transform GetParentTranform(Transform t)
		{
			return t.parent;
		}

		private void Update()
		{
			SetMask();
		}

		private void SetMask()
		{
			maskRect = MaskArea.rect;
			contentRect = myRect.rect;
			if (isText)
			{
				maskScalingRect = null;
				if (canvas.renderMode == RenderMode.ScreenSpaceOverlay && Application.isPlaying)
				{
					p = canvas.transform.InverseTransformPoint(MaskArea.transform.position);
					siz = new Vector2(maskRect.width, maskRect.height);
				}
				else
				{
					worldCorners = new Vector3[4];
					MaskArea.GetWorldCorners(worldCorners);
					siz = worldCorners[2] - worldCorners[0];
					p = MaskArea.transform.position;
				}
				min = p - new Vector2(siz.x, siz.y) * 0.5f;
				max = p + new Vector2(siz.x, siz.y) * 0.5f;
			}
			else
			{
				if (maskScalingRect != null)
				{
					maskRect = maskScalingRect.rect;
				}
				centre = myRect.transform.InverseTransformPoint(MaskArea.transform.position);
				if (maskScalingRect != null)
				{
					centre = myRect.transform.InverseTransformPoint(maskScalingRect.transform.position);
				}
				AlphaUV = new Vector2(maskRect.width / contentRect.width, maskRect.height / contentRect.height);
				min = centre;
				max = min;
				siz = new Vector2(maskRect.width, maskRect.height) * 0.5f;
				min -= siz;
				max += siz;
				min = new Vector2(min.x / contentRect.width, min.y / contentRect.height) + new Vector2(0.5f, 0.5f);
				max = new Vector2(max.x / contentRect.width, max.y / contentRect.height) + new Vector2(0.5f, 0.5f);
			}
			mat.SetFloat("_HardBlend", HardBlend ? 1 : 0);
			mat.SetVector("_Min", min);
			mat.SetVector("_Max", max);
			mat.SetTexture("_AlphaMask", AlphaMask);
			mat.SetInt("_FlipAlphaMask", FlipAlphaMask ? 1 : 0);
			if (!isText)
			{
				mat.SetVector("_AlphaUV", AlphaUV);
			}
			mat.SetFloat("_CutOff", CutOff);
		}
	}
}
