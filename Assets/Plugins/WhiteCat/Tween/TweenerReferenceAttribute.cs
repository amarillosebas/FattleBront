using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using WhiteCat.Editor;
#endif

namespace WhiteCat.Tween
{
	// Tweener
	public partial class Tweener
	{
		/// <summary>
		/// TweenerReferenceAttribute
		/// </summary>
		[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
		sealed class TweenerReferenceAttribute : PropertyAttributeWithEditor
		{

#if UNITY_EDITOR

			protected override void Editor_OnGUI(Rect rect, SerializedProperty property, GUIContent label)
			{
				var target = property.serializedObject.targetObject as TweenAnimation;

				// 绘制 Tweener 引用

				rect.width -= rect.height + 4;

				EditorGUI.BeginChangeCheck();

				var tweener = EditorGUI.ObjectField(rect, "Tweener", target.tweener,
					typeof(Tweener), !EditorUtility.IsPersistent(target)) as Tweener;

				if (EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(target, "Set Tweener");
					target.tweener = tweener;
					EditorUtility.SetDirty(target);
				}

				rect.x = rect.xMax + 4f;
				rect.width = rect.height;

				if (target.tweener)
				{
					// 绘制个性化颜色块

					EditorUI.BeginGUIColor(EditorUI.defaultContentColor);
					GUI.DrawTexture(rect, EditorAssets.bigDiamondTexture);
					EditorUI.EndGUIColor();

					EditorUI.BeginGUIColor(target.tweener._personalizedColor);
					GUI.DrawTexture(rect, EditorAssets.smallDiamondTexture);
					EditorUI.EndGUIColor();
				}
				else
				{
					// 绘制 + 按钮

					EditorUI.BeginGUIContentColor(EditorUI.defaultContentColor);

					if (GUI.Button(rect, EditorAssets.addTexture, GUIStyle.none))
					{
						tweener = Undo.AddComponent<Tweener>(target.gameObject);
						Undo.IncrementCurrentGroup();
						Undo.RecordObject(target, "Set Tweener");
						target.tweener = tweener;
						EditorUtility.SetDirty(target);
					}

					EditorUI.EndGUIContentColor();
				}
			}

#endif // UNITY_EDITOR

		} // class TweenerReferenceAttribute

	} // class Tweener

} // namespace WhiteCat.Tween