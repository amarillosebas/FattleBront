#if !UNITY_5_3_OR_NEWER && UNITY_5_3
#define UNITY_5_3_OR_NEWER
#endif

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using WhiteCat.Editor;
#endif

namespace WhiteCat.Tween
{
	/// <summary>
	/// Color 类型的插值动画
	/// </summary>
	public abstract class TweenColor : TweenFromTo<Color>
	{
		[SerializeField]
		Gradient _gradient = new Gradient();


		/// <summary> 使用哪种插值模式 </summary>
		public ColorMode colorMode = ColorMode.FromTo;

		/// <summary> 是否启用 HDR. 仅用于非渐变 </summary>
		public bool hdr = false;

		/// <summary> RGB 动画开关. 仅用于非渐变 </summary>
		public bool toggleRGB = true;

		/// <summary> Alpha 动画开关. 仅用于非渐变 </summary>
		public bool toggleAlpha = true;


		/// <summary> 渐变 </summary>
		public Gradient gradient
		{
			get { return _gradient; }
		}


		// 根据插值系数更改插值状态
		public override void OnTween(float factor)
		{
			if (colorMode == ColorMode.Gradient)
			{
				current = _gradient.Evaluate(factor);
			}
			else
			{
                current = Color.LerpUnclamped(_from, _to, factor);
			}
		}


        public abstract void SetRawValue(Color value);


        public override Color current
        {
            set
            {
                if (colorMode == ColorMode.FromTo)
                {
                    var c = current;

                    if (!toggleRGB)
                    {
                        value.r = c.r;
                        value.g = c.g;
                        value.b = c.b;
                    }

                    if (!toggleAlpha)
                    {
                        value.a = c.a;
                    }
                }

                SetRawValue(value);
            }
        }

#if UNITY_EDITOR

        SerializedProperty _colorModeProperty;
		SerializedProperty _hdrProperty;
		SerializedProperty _toggleRGBProperty;
		SerializedProperty _toggleAlphaProperty;
		SerializedProperty _fromAlphaProperty;
		SerializedProperty _toAlphaProperty;
		SerializedProperty _gradientProperty;


		protected override void Editor_OnEnable()
		{
			base.Editor_OnEnable();

			_colorModeProperty = editor.serializedObject.FindProperty("colorMode");
			_hdrProperty = editor.serializedObject.FindProperty("hdr");
			_toggleRGBProperty = editor.serializedObject.FindProperty("toggleRGB");
			_toggleAlphaProperty = editor.serializedObject.FindProperty("toggleAlpha");
			_fromAlphaProperty = _fromProperty.FindPropertyRelative("a");
			_toAlphaProperty = _toProperty.FindPropertyRelative("a");
			_gradientProperty = editor.serializedObject.FindProperty("_gradient");
		}


		protected override void Editor_OnDisable()
		{
			base.Editor_OnDisable();

			_colorModeProperty = null;
			_hdrProperty = null;
			_toggleRGBProperty = null;
			_toggleAlphaProperty = null;
			_fromAlphaProperty = null;
			_toAlphaProperty = null;
			_gradientProperty = null;
		}


		protected override void DrawExtraFields()
		{
			EditorGUILayout.PropertyField(_colorModeProperty);

			if (colorMode == ColorMode.Gradient)
			{
				EditorGUILayout.PropertyField(_gradientProperty);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_hdrProperty, EditorUI.TempContent("HDR"));
				if (EditorGUI.EndChangeCheck() && !_hdrProperty.boolValue)
				{
					if (_from.maxColorComponent > 1f)
					{
						var max = _from.maxColorComponent;
						var c = new Color(_from.r / max, _from.g / max, _from.b / max, _from.a);
						_fromProperty.colorValue = c;
					}
					if (_to.maxColorComponent > 1f)
					{
						var max = _to.maxColorComponent;
						var c = new Color(_to.r / max, _to.g / max, _to.b / max, _to.a);
						_toProperty.colorValue = c;
					}
				}

#if !UNITY_5_3_OR_NEWER
				if (hdr)
				{
					EditorGUILayout.HelpBox("HDR color picker requires Unity 5.3 or higher.", MessageType.Info, false);
				}
#endif

				ColorRGBField(_toggleRGBProperty, _fromProperty, _toProperty, hdr);
				ClampedFloatChannelField(_toggleAlphaProperty, "A", _fromAlphaProperty, _toAlphaProperty, 0f, 1f);
			}
		}

#endif // UNITY_EDITOR

			} // class TweenColor

		} // namespace WhiteCat.Tween