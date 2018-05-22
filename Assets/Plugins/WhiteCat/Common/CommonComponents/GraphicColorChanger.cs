using UnityEngine;
using UnityEngine.UI;

namespace WhiteCat
{
    /// <summary>
    /// 用以在编辑器中序列化颜色改变事件
    /// </summary>
	[AddComponentMenu("White Cat/UI/Graphic Color Changer")]
	public class GraphicColorChanger : ScriptableComponentWithEditor
	{
		public Graphic target;
		public float fadeDuration;
		public TimeMode timeMode;
		public Color[] colors;


		public void SetColor(int index)
		{
			if (target && index >= 0 && index < colors.Length)
			{
				target.CrossFadeColor(colors[index], Mathf.Max(fadeDuration, 0f), timeMode == TimeMode.Unscaled, true);
			}
		}


		void OnEnable()
		{
			if (target && 0 < colors.Length)
			{
				target.color = colors[0];
			}
		}


#if UNITY_EDITOR

		void Reset()
		{
			target = GetComponent<Graphic>();
		}


		protected override void Editor_OnInspectorGUI()
		{
			base.Editor_OnInspectorGUI();
			enabled = WhiteCat.Editor.EditorUI.IndentedToggleButton("First as Default", enabled);
		}

#endif
	}
}