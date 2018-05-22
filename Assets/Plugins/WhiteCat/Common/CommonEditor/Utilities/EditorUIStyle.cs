#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace WhiteCat.Editor
{
	/// <summary>
	/// 编辑器 GUIStyle 工具
	/// </summary>
	public partial struct EditorUI
	{
		static GUIStyle _buttonStyle;
		static GUIStyle _buttonLeftStyle;
		static GUIStyle _buttonMiddleStyle;
		static GUIStyle _buttonRightStyle;
		static GUIStyle _centeredBoldLabelStyle;

#if !UNITY_2018_1_OR_NEWER
		static ColorPickerHDRConfig _colorPickerHDRConfig;
#endif


        /// <summary>
        /// 按钮 GUIStyle
        /// </summary>
        public static GUIStyle buttonStyle
		{
			get
			{
				if (_buttonStyle == null) _buttonStyle = "Button";
				return _buttonStyle;
			}
		}


		/// <summary>
		/// 左侧按钮 GUIStyle
		/// </summary>
		public static GUIStyle buttonLeftStyle
		{
			get
			{
				if (_buttonLeftStyle == null) _buttonLeftStyle = "ButtonLeft";
				return _buttonLeftStyle;
			}
		}


		/// <summary>
		/// 中部按钮 GUIStyle
		/// </summary>
		public static GUIStyle buttonMiddleStyle
		{
			get
			{
				if (_buttonMiddleStyle == null) _buttonMiddleStyle = "ButtonMid";
				return _buttonMiddleStyle;
			}
		}


		/// <summary>
		/// 右侧按钮 GUIStyle
		/// </summary>
		public static GUIStyle buttonRightStyle
		{
			get
			{
				if (_buttonRightStyle == null) _buttonRightStyle = "ButtonRight";
				return _buttonRightStyle;
			}
		}


		/// <summary>
		/// 居中且加粗的 Label
		/// </summary>
		public static GUIStyle centeredBoldLabelStyle
		{
			get
			{
				if (_centeredBoldLabelStyle == null)
				{
					_centeredBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
					_centeredBoldLabelStyle.alignment = TextAnchor.MiddleCenter;
				}
				return _centeredBoldLabelStyle;
			}
		}


		/// <summary>
		/// 编辑器默认内容颜色 (文本, 按钮图片等)
		/// </summary>
		public static Color defaultContentColor
		{
			get { return EditorStyles.label.normal.textColor; }
		}


		/// <summary>
		/// 编辑器默认背景颜色
		/// </summary>
		public static Color defaultBackgroundColor
		{
			get
			{
				float rgb = EditorGUIUtility.isProSkin ? 56f : 194f;
				rgb /= 255f;
				return new Color(rgb, rgb, rgb, 1f);
			}
		}


#if !UNITY_2018_1_OR_NEWER
		/// <summary>
		/// HDR 拾色器设置
		/// </summary>
		public static ColorPickerHDRConfig colorPickerHDRConfig
		{
			get
			{
				if (_colorPickerHDRConfig == null)
				{
					_colorPickerHDRConfig = new ColorPickerHDRConfig(0f, 8f, 0.125f, 3f);
				}
				return _colorPickerHDRConfig;
			}
		}
#endif

	} // struct EditorUI

} // namespace WhiteCat.Editor

#endif // UNITY_EDITOR