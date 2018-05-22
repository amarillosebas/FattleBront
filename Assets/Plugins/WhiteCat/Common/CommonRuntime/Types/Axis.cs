using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using WhiteCat.Editor;
#endif

namespace WhiteCat
{
    /// <summary>
    /// 轴
    /// </summary>
    public enum Axis
    {
        None = 0,

        PositiveX = 1,      // 000 001
        PositiveY = 2,      // 000 010
        PositiveZ = 4,      // 000 100
        NegativeX = 8,      // 001 000
        NegativeY = 16,     // 010 000
        NegativeZ = 32,     // 100 000

        X = 9,
        Y = 18,
        Z = 36,

        All = 63,

    } // enum Axis


    /// <summary>
    /// 轴之间关系
    /// </summary>
    public enum AxisRelation
    {
        Same = 0,
        Vertical = 1,
        Opposite = 2,
    }


    /// <summary>
    /// Axis 的使用方式
    /// </summary>
    public enum AxisUsage
    {
        Direction6 = 0,
        Axis3 = 1
    }


    /// <summary>
    /// 标记在一个 Axis 字段上, 可以指定其编辑器风格
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class AxisUsageAttribute : PropertyAttributeWithEditor
    {

#if UNITY_EDITOR

        AxisUsage _usage;


        public AxisUsageAttribute(AxisUsage usage)
        {
            _usage = usage;
        }


        protected override void Editor_OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            property.intValue = (int)EditorUI.AxisField(rect, label.text, (Axis)property.intValue, _usage);
        }

#else

        public AxisUsageAttribute(AxisUsage usage)
        {
        }

#endif // UNITY_EDITOR

    } // class AxisUsageAttribute

} // namespace WhiteCat


#if UNITY_EDITOR

namespace WhiteCat.Editor
{
    public partial struct EditorUI
    {
        static string[][] _axisNames =
        {
            new string[] { "+X", "-X", "+Y", "-Y", "+Z", "-Z" },
            new string[] { "X", "Y", "Z" }
        };


        static int[][] _axisValues =
        {
            new int[] {(int)Axis.PositiveX, (int)Axis.NegativeX, (int)Axis.PositiveY, (int)Axis.NegativeY, (int)Axis.PositiveZ, (int)Axis.NegativeZ },
            new int[] {(int)Axis.X, (int)Axis.Y, (int)Axis.Z }
        };


        public static Axis AxisField(Rect rect, string label, Axis value, AxisUsage usage)
        {
            return (Axis)EditorGUI.IntPopup(rect, label, (int)value, _axisNames[(int)usage], _axisValues[(int)usage]);
        }


        public static Axis AxisFieldLayout(string label, Axis value, AxisUsage usage)
        {
            return AxisField(EditorGUILayout.GetControlRect(true), label, value, usage);
        }
    }
}

#endif