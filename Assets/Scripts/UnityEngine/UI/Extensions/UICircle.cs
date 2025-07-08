using System;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/Primitives/UI Circle")]
	public class UICircle : MaskableGraphic
	{
		[SerializeField]
		private Texture m_Texture;

		[Range(0f, 100f)]
		public int fillPercent = 100;

		public bool fill = true;

		public float thickness = 5f;

		[Range(0f, 360f)]
		public int segments = 360;

		public override Texture mainTexture => (!(m_Texture == null)) ? m_Texture : Graphic.s_WhiteTexture;

		public Texture texture
		{
			get
			{
				return m_Texture;
			}
			set
			{
				if (!(m_Texture == value))
				{
					m_Texture = value;
					SetVerticesDirty();
					SetMaterialDirty();
				}
			}
		}

		private void Update()
		{
			thickness = Mathf.Clamp(thickness, 0f, base.rectTransform.rect.width / 2f);
		}

		protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
		{
			UIVertex[] array = new UIVertex[4];
			for (int i = 0; i < vertices.Length; i++)
			{
				UIVertex simpleVert = UIVertex.simpleVert;
				simpleVert.color = color;
				simpleVert.position = vertices[i];
				simpleVert.uv0 = uvs[i];
				array[i] = simpleVert;
			}
			return array;
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			Vector2 pivot = base.rectTransform.pivot;
			float num = (0f - pivot.x) * base.rectTransform.rect.width;
			Vector2 pivot2 = base.rectTransform.pivot;
			float num2 = (0f - pivot2.x) * base.rectTransform.rect.width + thickness;
			vh.Clear();
			Vector2 vector = Vector2.zero;
			Vector2 vector2 = Vector2.zero;
			Vector2 vector3 = new Vector2(0f, 0f);
			Vector2 vector4 = new Vector2(0f, 1f);
			Vector2 vector5 = new Vector2(1f, 1f);
			Vector2 vector6 = new Vector2(1f, 0f);
			float num3 = (float)fillPercent / 100f;
			float num4 = 360f / (float)segments;
			int num5 = (int)((float)(segments + 1) * num3);
			for (int i = 0; i < num5; i++)
			{
				float f = (float)Math.PI / 180f * ((float)i * num4);
				float num6 = Mathf.Cos(f);
				float num7 = Mathf.Sin(f);
				vector3 = new Vector2(0f, 1f);
				vector4 = new Vector2(1f, 1f);
				vector5 = new Vector2(1f, 0f);
				vector6 = new Vector2(0f, 0f);
				Vector2 vector7 = vector;
				Vector2 vector8 = new Vector2(num * num6, num * num7);
				Vector2 vector9;
				Vector2 vector10;
				if (fill)
				{
					vector9 = Vector2.zero;
					vector10 = Vector2.zero;
				}
				else
				{
					vector9 = new Vector2(num2 * num6, num2 * num7);
					vector10 = vector2;
				}
				vector = vector8;
				vector2 = vector9;
				vh.AddUIVertexQuad(SetVbo(new Vector2[4]
				{
					vector7,
					vector8,
					vector9,
					vector10
				}, new Vector2[4]
				{
					vector3,
					vector4,
					vector5,
					vector6
				}));
			}
		}
	}
}
