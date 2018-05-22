#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;

#if UNITY_2017_1_OR_NEWER
using UnityEditor.IMGUI.Controls;
#endif

namespace WhiteCat.Editor
{
	/// <summary>
	/// 菜单项状态
	/// </summary>
	public enum MenuItemState
	{
		Normal,
		Selected,
		Disabled,
	}


	/// <summary>
	/// 编辑器 UI 工具
	/// </summary>
	public partial struct EditorUI
	{
		static GUIContent _GUIContent;

		static Vector3[] _lineVertices = new Vector3[2];
		static Vector3[] _boundsVertices = new Vector3[10];


        /// <summary>
        /// 获取一个随机颜色, 这个颜色具有一定的饱和度和明度
        /// </summary>
        public static Color GetRandomColor()
		{
#if UNITY_5_3 || UNITY_5_3_OR_NEWER

			return Color.HSVToRGB(
				(float)Math.random.Range01(),
				Math.random.Range(0.5f, 1f),
				Math.random.Range(0.5f, 1f));

#else

				var color = Utilities.ColorWheel((float)Math.random.Range01());
				var scale = Math.random.Range(0.5f, 1f);
				var basic = (float)Math.random.Range01() * (1f - scale);

				color.r = color.r * scale + basic;
				color.g = color.g * scale + basic;
				color.b = color.b * scale + basic;

				return color;

#endif
		}


		/// <summary>
		/// 获取临时的 GUIContent
		/// </summary>
		public static GUIContent TempContent(string text = null, Texture image = null, string tooltip = null)
		{
			if (_GUIContent == null) _GUIContent = new GUIContent();

			_GUIContent.text = text;
			_GUIContent.image = image;
			_GUIContent.tooltip = tooltip;

			return _GUIContent;
		}


		/// <summary>
		/// 绘制矩形的边框
		/// </summary>
		/// <param name="rect"> 矩形 </param>
		/// <param name="color"> 边框颜色 </param>
		/// <param name="size"> 边框大小 </param>
		public static void DrawWireRect(Rect rect, Color color, float borderSize = 1f)
		{
			Rect draw = new Rect(rect.x, rect.y, rect.width, borderSize);
			EditorGUI.DrawRect(draw, color);
			draw.y = rect.yMax - borderSize;
			EditorGUI.DrawRect(draw, color);
			draw.yMax = draw.yMin;
			draw.yMin = rect.yMin + borderSize;
			draw.width = borderSize;
			EditorGUI.DrawRect(draw, color);
			draw.x = rect.xMax - borderSize;
			EditorGUI.DrawRect(draw, color);
		}


		/// <summary>
		/// 绘制抗锯齿线段
		/// </summary>
		public static void HandlesDrawAALine(Vector3 point1, Vector3 point2)
		{
			_lineVertices[0] = point1;
			_lineVertices[1] = point2;
			Handles.DrawAAPolyLine(_lineVertices);
		}


		/// <summary>
		/// 绘制抗锯齿线段
		/// </summary>
		public static void HandlesDrawAALine(Vector3 point1, Vector3 point2, float width)
		{
			_lineVertices[0] = point1;
			_lineVertices[1] = point2;
			Handles.DrawAAPolyLine(width, _lineVertices);
		}


		/// <summary>
		/// 绘制边界框的线框
		/// </summary>
		public static void HandlesDrawWireBounds(Bounds bounds)
		{
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;

			_boundsVertices[0] = min;
			_boundsVertices[1] = new Vector3(min.x, min.y, max.z);
			_boundsVertices[2] = new Vector3(max.x, min.y, max.z);
			_boundsVertices[3] = new Vector3(max.x, min.y, min.z);
			_boundsVertices[4] = min;

			_boundsVertices[5] = new Vector3(min.x, max.y, min.z);
			_boundsVertices[6] = new Vector3(min.x, max.y, max.z);
			_boundsVertices[7] = max;
			_boundsVertices[8] = new Vector3(max.x, max.y, min.z);
			_boundsVertices[9] = new Vector3(min.x, max.y, min.z);

			Handles.DrawAAPolyLine(_boundsVertices);

			HandlesDrawAALine(_boundsVertices[1], _boundsVertices[6]);
			HandlesDrawAALine(_boundsVertices[2], _boundsVertices[7]);
			HandlesDrawAALine(_boundsVertices[3], _boundsVertices[8]);
		}


		/// <summary>
		/// 绘制一个单行高且缩进的按钮
		/// </summary>
		public static bool IndentedButton(string text)
		{
			var rect = EditorGUILayout.GetControlRect(true);
			rect.xMin += EditorGUIUtility.labelWidth;
			return GUI.Button(rect, text, EditorStyles.miniButton);
		}


		/// <summary>
		/// 绘制一个单行高且缩进的开关按钮
		/// </summary>
		public static bool IndentedToggleButton(string text, bool value)
		{
			var rect = EditorGUILayout.GetControlRect(true);
			rect.xMin += EditorGUIUtility.labelWidth;
			return GUI.Toggle(rect, value, text, EditorStyles.miniButton);
		}


        public static Vector2 SingleLineVector2Field(Rect rect, Vector2 value, GUIContent label)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
            using (new LabelWidthScope(14))
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                rect.width = (rect.width - 4) * 0.5f;
                value.x = EditorGUI.FloatField(rect, "X", value.x);
                rect.x = rect.xMax + 4;
                value.y = EditorGUI.FloatField(rect, "Y", value.y);

                EditorGUI.indentLevel = indent;
            }
            return value;
        }


        public static Vector2 SingleLineVector2Field(Rect rect, Vector2 value, GUIContent label, float aspectRatio)
        {
            rect = EditorGUI.PrefixLabel(rect, label);
            using (new LabelWidthScope(14))
            {
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                rect.width = (rect.width - 4) * 0.5f;
                using (var scope = ChangeCheckScope.New())
                {
                    value.x = EditorGUI.FloatField(rect, "X", value.x);
                    if (scope.changed) value.y = value.x / aspectRatio;
                }

                rect.x = rect.xMax + 4;
                using (var scope = ChangeCheckScope.New())
                {
                    value.y = EditorGUI.FloatField(rect, "Y", value.y);
                    if (scope.changed) value.x = value.y * aspectRatio;
                }

                EditorGUI.indentLevel = indent;
            }
            return value;
        }


        public static void SingleLineVector2Field(Rect rect, SerializedProperty property, GUIContent label)
        {
            property.vector2Value = SingleLineVector2Field(rect, property.vector2Value, label);
        }


        public static void SingleLineVector2Field(Rect rect, SerializedProperty property, GUIContent label, float aspectRatio)
        {
            property.vector2Value = SingleLineVector2Field(rect, property.vector2Value, label, aspectRatio);
        }


        public static Vector2 SingleLineVector2FieldLayout(Vector2 value, GUIContent label)
        {
            return SingleLineVector2Field(EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight), value, label);
        }


        public static void SingleLineVector2FieldLayout(SerializedProperty property, GUIContent label)
        {
            property.vector2Value = SingleLineVector2Field(EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight), property.vector2Value, label);
        }


        /// <summary>
        /// 拖动鼠标以修改数值
        /// </summary>
        public static float DragValue(Rect rect, GUIContent content, float value, float step, GUIStyle style, bool horizontal = true)
		{
			int controlID = GUIUtility.GetControlID(FocusType.Passive);

			switch (Event.current.GetTypeForControl(controlID))
			{
				case EventType.Repaint:
					{
						GUI.Label(rect, content, style);
						break;
					}

				case EventType.MouseDown:
					{
						if (Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
						{
							GUIUtility.hotControl = controlID;
						}
						break;
					}

				case EventType.MouseUp:
					{
						if (GUIUtility.hotControl == controlID)
						{
							GUIUtility.hotControl = 0;
						}
						break;
					}
			}

			if (Event.current.isMouse && GUIUtility.hotControl == controlID)
			{
				if (Event.current.type == EventType.MouseDrag)
				{
					if (horizontal) value += Event.current.delta.x * step;
					else value -= Event.current.delta.y * step;
					value = Math.RoundToSignificantDigitsFloat(value);

					GUI.changed = true;
				}

				Event.current.Use();
			}

			return value;
		}


		/// <summary>
		/// 绘制可调节的进度条控件
		/// </summary>
		/// <param name="rect"> 绘制的矩形范围 </param>
		/// <param name="value"> [0, 1] 范围的进度 </param>
		/// <param name="backgroundColor"> 背景色 </param>
		/// <param name="foregroundColor"> 进度填充色 </param>
		/// <returns> 用户修改后的进度 </returns>
		public static float ProgressBar(
			Rect rect,
			float value,
			Color backgroundColor,
			Color foregroundColor)
		{
			int controlID = GUIUtility.GetControlID(FocusType.Passive);

			switch (Event.current.GetTypeForControl(controlID))
			{
				case EventType.Repaint:
					{
                        using (new GUIColorScope(backgroundColor))
                        {
                            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);

                            rect.width = Mathf.Round(rect.width * value);
                            GUI.color = foregroundColor;
                            GUI.DrawTexture(rect, EditorGUIUtility.whiteTexture);
                        }
						break;
					}

				case EventType.MouseDown:
					{
						if (Event.current.button == 0 && rect.Contains(Event.current.mousePosition))
						{
							GUIUtility.hotControl = controlID;
						}
						break;
					}

				case EventType.MouseUp:
					{
						if (GUIUtility.hotControl == controlID)
						{
							GUIUtility.hotControl = 0;
						}
						break;
					}
			}

			if (Event.current.isMouse && GUIUtility.hotControl == controlID)
			{
				float offset = Event.current.mousePosition.x - rect.x + 1f;
				value = Mathf.Clamp01(offset / rect.width);

				GUI.changed = true;
				Event.current.Use();
			}

			return value;
		}


		/// <summary>
		/// 绘制可调节的进度条控件
		/// </summary>
		/// <param name="rect"> 绘制的矩形范围 </param>
		/// <param name="value"> [0, 1] 范围的进度 </param>
		/// <param name="backgroundColor"> 背景色 </param>
		/// <param name="foregroundColor"> 进度填充色 </param>
		/// <param name="borderColor"> 绘制的边界框颜色 </param>
		/// <param name="drawForegroundBorder"> 是否绘制进度条右侧的边界线 </param>
		/// <returns> 用户修改后的进度 </returns>
		public static float ProgressBar(
			Rect rect,
			float value,
			Color backgroundColor,
			Color foregroundColor,
			Color borderColor,
			bool drawForegroundBorder = false)
		{
			float result = ProgressBar(rect, value, backgroundColor, foregroundColor);

			if (Event.current.type == EventType.Repaint)
			{
				DrawWireRect(rect, borderColor);

				if (drawForegroundBorder)
				{
					rect.width = Mathf.Round(rect.width * value);
					if (rect.width > 0f)
					{
						rect.xMin = rect.xMax - 1f;
						EditorGUI.DrawRect(rect, borderColor);
					}
				}
			}

			return result;
		}


		/// <summary>
		/// 创建菜单
		/// </summary>
		/// <param name="itemCount"> 菜单项总数, 包括所有级别的菜单项和分隔符 </param>
		/// <param name="getItemContent"> 获取菜单项内容, 分割符必须以 "/" 结尾 </param>
		/// <param name="getItemState"> 获取菜单项状态, 不会对分隔符获取状态 </param>
		/// <param name="onSelect"> 菜单项被选中的回调 </param>
		/// <returns> 创建好的菜单, 接下来可以通过调用 DropDown 或 ShowAsContext 来显示菜单 </returns>
		public static GenericMenu CreateMenu(
			int itemCount,
			Func<int, GUIContent> getItemContent,
			Func<int, MenuItemState> getItemState,
			Action<int> onSelect)
		{
			GenericMenu menu = new GenericMenu();
			GUIContent item;
			MenuItemState state;

			for(int i=0; i<itemCount; i++)
			{
				item = getItemContent(i);
				if(item.text.EndsWith("/"))
				{
					menu.AddSeparator(item.text);
				}
				else
				{
					state = getItemState(i);
					if(state == MenuItemState.Disabled)
					{
						menu.AddDisabledItem(item);
					}
					else
					{
						int index = i;
						menu.AddItem(item, state == MenuItemState.Selected, () => onSelect(index));
					}
				}
			}

			return menu;
		}


        public static void HandlesDrawSphereOutline(Vector3 position, float radius)
        {
            var cameraTrans = Camera.current.transform;
            var cam_obj = cameraTrans.position - position;
            float v2 = cam_obj.sqrMagnitude;

            if (v2 > Camera.current.nearClipPlane * Camera.current.nearClipPlane)
            {
                float r2 = radius * radius;
                float r2_d_v2 = r2 / v2;

                Handles.CircleHandleCap(0, r2_d_v2 * cam_obj + position, Quaternion.LookRotation(cam_obj), radius * Mathf.Sqrt(1f - r2_d_v2), EventType.Repaint);
            }
        }


#if UNITY_2017_1_OR_NEWER

		static BoxBoundsHandle _boxBoundsHandle;
		static BoxBoundsHandle boxBoundsHandle
		{
			get
			{
				if (_boxBoundsHandle == null) _boxBoundsHandle = new BoxBoundsHandle();
				return _boxBoundsHandle;
			}
		}


		static SphereBoundsHandle _sphereBoundsHandle;
		static SphereBoundsHandle sphereBoundsHandle
		{
			get
			{
				if (_sphereBoundsHandle == null) _sphereBoundsHandle = new SphereBoundsHandle();
				return _sphereBoundsHandle;
			}
		}


		static CapsuleBoundsHandle _capsuleBoundsHandle;
		static CapsuleBoundsHandle capsuleBoundsHandle
		{
			get
			{
				if (_capsuleBoundsHandle == null) _capsuleBoundsHandle = new CapsuleBoundsHandle();
				return _capsuleBoundsHandle;
			}
		}


		/// <summary>
		/// 绘制 Box 调节框
		/// </summary>
		public static void BoxHandle(ref Vector3 center, ref Vector3 size, Color color)
		{
			boxBoundsHandle.center = center;
			boxBoundsHandle.size = size;
			boxBoundsHandle.SetColor(color);
			boxBoundsHandle.DrawHandle();
			center = boxBoundsHandle.center;
			size = boxBoundsHandle.size;
		}


		/// <summary>
		/// 绘制 Box 调节框
		/// </summary>
		public static Bounds BoxHandle(Bounds bounds, Color color)
		{
			boxBoundsHandle.center = bounds.center;
			boxBoundsHandle.size = bounds.size;
			boxBoundsHandle.SetColor(color);
			boxBoundsHandle.DrawHandle();
			bounds.center = boxBoundsHandle.center;
			bounds.size = boxBoundsHandle.size;
			return bounds;
		}


		/// <summary>
		/// 绘制 Box 调节框
		/// </summary>
		public static Bounds BoxHandle(Transform transform, Bounds bounds, Color color)
		{
			using (new HandlesMatrixScope(transform.localToWorldMatrix))
			{
				return BoxHandle(bounds, color);
			}
		}


		/// <summary>
		/// 绘制 Box 调节框
		/// </summary>
		public static void BoxHandle(SerializedProperty boundsProperty, Color color)
		{
			boundsProperty.boundsValue = BoxHandle(boundsProperty.boundsValue, color);
		}


		/// <summary>
		/// 绘制 Box 调节框
		/// </summary>
		public static void BoxHandle(Transform transform, SerializedProperty boundsProperty, Color color)
		{
			using (new HandlesMatrixScope(transform.localToWorldMatrix))
			{
				boundsProperty.boundsValue = BoxHandle(boundsProperty.boundsValue, color);
			}
		}


		/// <summary>
		/// 绘制 Sphere 调节框
		/// </summary>
		public static void SphereHandle(ref Vector3 center, ref float radius, Color color)
		{
			sphereBoundsHandle.center = center;
			sphereBoundsHandle.radius = radius;
			sphereBoundsHandle.SetColor(color);
			sphereBoundsHandle.DrawHandle();
			center = sphereBoundsHandle.center;
			radius = sphereBoundsHandle.radius;
		}


		/// <summary>
		/// 绘制 Sphere 调节框
		/// </summary>
		public static void SphereHandle(Transform transform, ref Vector3 center, ref float radius, Color color)
		{
			using (new HandlesMatrixScope(transform.localToWorldMatrix))
			{
				SphereHandle(ref center, ref radius, color);
			}
		}


		/// <summary>
		/// 绘制 Sphere 调节框
		/// </summary>
		public static void SphereHandle(SerializedProperty center, SerializedProperty radius, Color color)
		{
			sphereBoundsHandle.center = center.vector3Value;
			sphereBoundsHandle.radius = radius.floatValue;
			sphereBoundsHandle.SetColor(color);
			sphereBoundsHandle.DrawHandle();
			center.vector3Value = sphereBoundsHandle.center;
			radius.floatValue = sphereBoundsHandle.radius;
		}


		/// <summary>
		/// 绘制 Sphere 调节框
		/// </summary>
		public static void SphereHandle(Transform transform, SerializedProperty center, SerializedProperty radius, Color color)
		{
			using (new HandlesMatrixScope(transform.localToWorldMatrix))
			{
				SphereHandle(center, radius, color);
			}
		}


		/// <summary>
		/// 绘制 Capsule 调节框
		/// </summary>
		public static void CapsuleHandle(ref Vector3 center, ref float radius, ref float height, Color color, CapsuleBoundsHandle.HeightAxis axis)
		{
			capsuleBoundsHandle.center = center;
			capsuleBoundsHandle.radius = radius;
			capsuleBoundsHandle.height = height;
			capsuleBoundsHandle.heightAxis = axis;
			capsuleBoundsHandle.SetColor(color);
			capsuleBoundsHandle.DrawHandle();
			center = capsuleBoundsHandle.center;
			radius = capsuleBoundsHandle.radius;
			height = capsuleBoundsHandle.height;
		}


		/// <summary>
		/// 绘制 Capsule 调节框
		/// </summary>
		public static void CapsuleHandle(Transform transform, ref Vector3 center, ref float radius, ref float height, Color color, CapsuleBoundsHandle.HeightAxis axis)
		{
			using (new HandlesMatrixScope(transform.localToWorldMatrix))
			{
				CapsuleHandle(ref center, ref radius, ref height, color, axis);
			}
		}

#endif

	} // struct EditorUI

} // namespace WhiteCat.Editor

#endif // UNITY_EDITOR