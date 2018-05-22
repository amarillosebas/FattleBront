#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace WhiteCat.Editor
{
	/// <summary>
	/// Hot Control 事件
	/// </summary>
	public enum HotControlEvent
	{
		None,
		MouseDown,
		MouseUp,
	}


    /// <summary>
    /// 编辑器 UI 状态控制工具 (推荐使用 EditorScope 代替)
    /// 注意: Begin 和 End 方法不支持嵌套调用
    /// </summary>
    public partial struct EditorUI
	{
		static int _lastRecordedHotControl;
		static float _lastRecordedLabelWidth;
		static bool _lastRecordedWideMode;
		static Color _lastRecordedContentColor;
		static Color _lastRecordedBackgroundColor;
		static Color _lastRecordedColor;
		static Color _lastRecordedHandlesColor;
		static Matrix4x4 _lastHandlesMatrix;


		/// <summary>
		/// 在绘制控件之前调用, 用以检查控件是否被鼠标选中
		/// </summary>
		public static void BeginHotControlChangeCheck()
		{
			_lastRecordedHotControl = GUIUtility.hotControl;
		}


		/// <summary>
		/// 在绘制控件之后调用, 返回该控件被鼠标选中的事件
		/// </summary>
		public static HotControlEvent EndHotControlChangeCheck()
		{
			if (_lastRecordedHotControl == GUIUtility.hotControl)
			{
				return HotControlEvent.None;
			}

			return GUIUtility.hotControl == 0 ? HotControlEvent.MouseUp : HotControlEvent.MouseDown;
		}


		/// <summary>
		/// 记录并设置 LabelWidth
		/// </summary>
		public static void BeginLabelWidth(float newWidth)
		{
			_lastRecordedLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = newWidth;
		}


		/// <summary>
		/// 恢复 LabelWidth
		/// </summary>
		public static void EndLabelWidth()
		{
			EditorGUIUtility.labelWidth = _lastRecordedLabelWidth;
		}


		/// <summary>
		/// 记录并设置 WideMode
		/// </summary>
		public static void BeginWideMode(bool newWideMode)
		{
			_lastRecordedWideMode = EditorGUIUtility.wideMode;
			EditorGUIUtility.wideMode = newWideMode;
		}


		/// <summary>
		/// 恢复 WideMode
		/// </summary>
		public static void EndWideMode()
		{
			EditorGUIUtility.wideMode = _lastRecordedWideMode;
		}


		/// <summary>
		/// 记录并设置 ContentColor
		/// </summary>
		public static void BeginGUIContentColor(Color newColor)
		{
			_lastRecordedContentColor = GUI.contentColor;
			GUI.contentColor = newColor;
		}


		/// <summary>
		/// 恢复 BackgroundColor
		/// </summary>
		public static void EndGUIContentColor()
		{
			GUI.contentColor = _lastRecordedContentColor;
		}


		/// <summary>
		/// 记录并设置 BackgroundColor
		/// </summary>
		public static void BeginGUIBackgroundColor(Color newColor)
		{
			_lastRecordedBackgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = newColor;
		}


		/// <summary>
		/// 恢复 BackgroundColor
		/// </summary>
		public static void EndGUIBackgroundColor()
		{
			GUI.backgroundColor = _lastRecordedBackgroundColor;
		}


		/// <summary>
		/// 记录并设置 Color
		/// </summary>
		public static void BeginGUIColor(Color newColor)
		{
			_lastRecordedColor = GUI.color;
			GUI.color = newColor;
		}


		/// <summary>
		/// 恢复 Color
		/// </summary>
		public static void EndGUIColor()
		{
			GUI.color = _lastRecordedColor;
		}


		/// <summary>
		/// 记录并设置 Handles.color
		/// </summary>
		public static void BeginHandlesColor(Color newColor)
		{
			_lastRecordedHandlesColor = Handles.color;
			Handles.color = newColor;
		}


		/// <summary>
		/// 恢复 Handles.color
		/// </summary>
		public static void EndHandlesColor()
		{
			Handles.color = _lastRecordedHandlesColor;
		}


		/// <summary>
		/// 记录并设置 Handles.matrix
		/// </summary>
		public static void BeginHandlesMatrix(ref Matrix4x4 newMatrix)
		{
			_lastHandlesMatrix = Handles.matrix;
			Handles.matrix = newMatrix;
		}


		/// <summary>
		/// 记录并设置 Handles.matrix
		/// </summary>
		public static void BeginHandlesMatrix(Matrix4x4 newMatrix)
		{
			_lastHandlesMatrix = Handles.matrix;
			Handles.matrix = newMatrix;
		}


		/// <summary>
		/// 恢复 Handles.matrix
		/// </summary>
		public static void EndHandlesMatrix()
		{
			Handles.matrix = _lastHandlesMatrix;
		}

	} // struct EditorUI

} // namespace WhiteCat.Editor

#endif // UNITY_EDITOR