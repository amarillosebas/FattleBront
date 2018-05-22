using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WhiteCat.Tween
{
	/// <summary>
	/// Vector3 类型的插值动画
	/// </summary>
	public abstract class TweenVector3 : TweenFromTo<Vector3>
	{
		/// <summary> X 通道开关 </summary>
		public bool toggleX = true;

		/// <summary> Y 通道开关 </summary>
		public bool toggleY = true;

		/// <summary> Z 通道开关 </summary>
		public bool toggleZ = true;


        // 根据插值系数更改插值状态
        public override void OnTween(float factor)
        {
            current = Vector3.LerpUnclamped(_from, _to, factor);
        }


        public abstract void SetRawValue(Vector3 value);


        public override Vector3 current
        {
            set
            {
                var c = current;

                if (!toggleX) value.x = c.x;
                if (!toggleY) value.y = c.y;
                if (!toggleZ) value.z = c.z;

                SetRawValue(value);
            }
        }

#if UNITY_EDITOR

        SerializedProperty[] _toggleProperties;
		SerializedProperty[] _fromProperties;
		SerializedProperty[] _toProperties;


		protected static SerializedProperty[] GetVector3Properties(SerializedProperty vector3Property)
		{
			SerializedProperty[] properties = new SerializedProperty[3];
			properties[0] = vector3Property.FindPropertyRelative("x");
			properties[1] = vector3Property.FindPropertyRelative("y");
			properties[2] = vector3Property.FindPropertyRelative("z");
			return properties;
		}


		protected override void Editor_OnEnable()
		{
			base.Editor_OnEnable();

			_toggleProperties = new SerializedProperty[3];
			_toggleProperties[0] = editor.serializedObject.FindProperty("toggleX");
			_toggleProperties[1] = editor.serializedObject.FindProperty("toggleY");
			_toggleProperties[2] = editor.serializedObject.FindProperty("toggleZ");

			_fromProperties = GetVector3Properties(_fromProperty);
			_toProperties = GetVector3Properties(_toProperty);
		}


		protected override void Editor_OnDisable()
		{
			base.Editor_OnDisable();
			_toggleProperties = null;
			_fromProperties = null;
			_toProperties = null;
		}


		protected override void DrawExtraFields()
		{
			DrawFromToChannels();
		}


		protected void DrawFromToChannels()
		{
			FloatChannelField(_toggleProperties[0], "X", _fromProperties[0], _toProperties[0]);
			FloatChannelField(_toggleProperties[1], "Y", _fromProperties[1], _toProperties[1]);
			FloatChannelField(_toggleProperties[2], "Z", _fromProperties[2], _toProperties[2]);
		}


		protected void DrawClampedFromToChannels(float min, float max)
		{
			ClampedFloatChannelField(_toggleProperties[0], "X", _fromProperties[0], _toProperties[0], min, max);
			ClampedFloatChannelField(_toggleProperties[1], "Y", _fromProperties[1], _toProperties[1], min, max);
			ClampedFloatChannelField(_toggleProperties[2], "Z", _fromProperties[2], _toProperties[2], min, max);
		}

#endif // UNITY_EDITOR

	} // class TweenVector3

} // namespace WhiteCat.Tween