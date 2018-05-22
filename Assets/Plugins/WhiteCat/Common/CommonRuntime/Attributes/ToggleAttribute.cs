using System;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WhiteCat
{
	/// <summary>
	/// 标记在一个字段上, 由另一个字段或属性控制可编辑性
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
	public sealed class ToggleAttribute : PropertyAttributeWithEditor
	{

#if UNITY_EDITOR

		UnityEngine.Object _target;
		string _undoString;
		bool _isField;
		bool _autoDisable;
		string _fieldOrPropertyName;
		FieldInfo _fieldInfo;
		PropertyInfo _propertyInfo;


		public ToggleAttribute(string fieldOrPropertyName, bool isField, bool autoDisable = true)
		{
			_isField = isField;
			_autoDisable = autoDisable;
			_fieldOrPropertyName = fieldOrPropertyName;
		}


		protected override void Editor_OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (_target == null)
			{
				_target = property.serializedObject.targetObject;
				_undoString = _target.ToString();

				if (_isField)
				{
					_fieldInfo = Reflection.GetFieldInfo(_target, _fieldOrPropertyName);

					if (_fieldInfo == null)
					{
						EditorGUI.LabelField(rect, label.text, "Can not find field.");
					}
					else if (typeof(bool) != _fieldInfo.FieldType)
					{
						_fieldInfo = null;
						EditorGUI.LabelField(rect, label.text, "Field type is not bool.");
					}
					else if (_fieldInfo.IsInitOnly)
					{
						_fieldInfo = null;
						EditorGUI.LabelField(rect, label.text, "Field can not write.");
					}

					if (_fieldInfo == null) return;
				}
				else
				{
					_propertyInfo = Reflection.GetPropertyInfo(_target, _fieldOrPropertyName);

					if (_propertyInfo == null)
					{
						EditorGUI.LabelField(rect, label.text, "Can not find property.");
					}
					else if (typeof(bool) != _propertyInfo.PropertyType)
					{
						_propertyInfo = null;
						EditorGUI.LabelField(rect, label.text, "Property type is not bool.");
					}
					else if (!_propertyInfo.CanRead || !_propertyInfo.CanWrite)
					{
						_propertyInfo = null;
						EditorGUI.LabelField(rect, label.text, "Property can not read.");
					}

					if (_propertyInfo == null) return;
				}
			}

			EditorGUI.BeginChangeCheck();
			float width = rect.width;
			rect.width = 16f;
			bool toggle = (bool)(_isField ? _fieldInfo.GetValue(_target) : _propertyInfo.GetValue(_target, null));
			toggle = EditorGUI.ToggleLeft(rect, GUIContent.none, toggle);
			rect.width = width;
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(_target, _undoString);
				if (_isField) _fieldInfo.SetValue(_target, toggle);
				else _propertyInfo.SetValue(_target, toggle, null);
				EditorUtility.SetDirty(_target);
			}

			EditorGUI.BeginDisabledGroup(_autoDisable && !toggle);
			rect.xMin += 16f;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = labelWidth - 16f;
			EditorGUI.PropertyField(rect, property, label, true);
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.EndDisabledGroup();
		}

#else

		public ToggleAttribute(string fieldOrPropertyName, bool isField)
		{
		}

#endif // UNITY_EDITOR

	} // class ToggleAttribute

} // namespace WhiteCat