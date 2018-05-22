using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using WhiteCat.Editor;
#endif

namespace WhiteCat
{
	/// <summary>
	/// 标记在一个字段上, 可以指定其显示的 Label, Label 内容由一个属性指定
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class DynamicLabelAttribute : PropertyAttributeWithEditor
	{

#if UNITY_EDITOR

		UnityEngine.Object _target;
		string _propertyName;
		PropertyInfo _propertyInfo;


		public DynamicLabelAttribute(string propertyName)
		{
			_propertyName = propertyName;
		}


		protected override void Editor_OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (_target == null)
			{
				_target = property.serializedObject.targetObject;
				_propertyInfo = Reflection.GetPropertyInfo(_target, _propertyName);

				if (_propertyInfo == null)
				{
					EditorGUI.LabelField(rect, label.text, "Can not find property.");
				}
				else if (typeof(string) != _propertyInfo.PropertyType)
				{
					_propertyInfo = null;
					EditorGUI.LabelField(rect, label.text, "Property type is not string.");
				}
				else if (!_propertyInfo.CanRead)
				{
					_propertyInfo = null;
					EditorGUI.LabelField(rect, label.text, "Property can not read.");
				}
			}

			if (_propertyInfo == null) return;

			EditorGUI.PropertyField(rect, property, EditorUI.TempContent(_propertyInfo.GetValue(_target, null) as string), true);
		}

#else

		public DynamicLabelAttribute(string label)
		{
		}

#endif // UNITY_EDITOR

	} // class DynamicLabelAttribute

} // namespace WhiteCat