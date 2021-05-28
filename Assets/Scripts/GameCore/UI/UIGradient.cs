using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
	[AddComponentMenu("UI/Effects/Gradient")]
	public class UIGradient : BaseMeshEffect
	{
		public Color Color1 = Color.red;
		public Color Color2 = Color.green;

		public override void ModifyMesh(VertexHelper vh)
		{
			if (!enabled) return;

			var vertex = default(UIVertex);
			for (int i = 0; i < vh.currentVertCount; i++) {
				vh.PopulateUIVertex (ref vertex, i);
				vertex.color = i < 2 ? Color1 : Color2;
				vh.SetUIVertex (vertex, i);
			}
		}
	}
}
