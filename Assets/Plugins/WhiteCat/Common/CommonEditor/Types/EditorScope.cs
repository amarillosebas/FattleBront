#if UNITY_EDITOR

using System;
using UnityEngine;
using UnityEditor;

namespace WhiteCat.Editor
{
    public struct LabelWidthScope : IDisposable
    {
        float _orginal;

        public LabelWidthScope(float value)
        {
            _orginal = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = value;
        }

        void IDisposable.Dispose()
        {
            EditorGUIUtility.labelWidth = _orginal;
        }
    }


    public struct WideModeScope : IDisposable
    {
        bool _orginal;

        public WideModeScope(bool value)
        {
            _orginal = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = value;
        }

        void IDisposable.Dispose()
        {
            EditorGUIUtility.wideMode = _orginal;
        }
    }


    public struct IndentScope : IDisposable
    {
        int _increment;

        public IndentScope(int increment)
        {
            _increment = increment;
            EditorGUI.indentLevel += increment;
        }

        void IDisposable.Dispose()
        {
            EditorGUI.indentLevel -= _increment;
        }
    }


    public struct GUIContentColorScope : IDisposable
    {
        Color _orginal;

        public GUIContentColorScope(Color value)
        {
            _orginal = GUI.contentColor;
            GUI.contentColor = value;
        }

        void IDisposable.Dispose()
        {
            GUI.contentColor = _orginal;
        }
    }


    public struct GUIBackgroundColorScope : IDisposable
    {
        Color _orginal;

        public GUIBackgroundColorScope(Color value)
        {
            _orginal = GUI.backgroundColor;
            GUI.backgroundColor = value;
        }

        void IDisposable.Dispose()
        {
            GUI.backgroundColor = _orginal;
        }
    }


    public struct GUIColorScope : IDisposable
    {
        Color _orginal;

        public GUIColorScope(Color value)
        {
            _orginal = GUI.color;
            GUI.color = value;
        }

        void IDisposable.Dispose()
        {
            GUI.color = _orginal;
        }
    }


    public struct HandlesColorScope : IDisposable
    {
        Color _orginal;

        public HandlesColorScope(Color value)
        {
            _orginal = Handles.color;
            Handles.color = value;
        }

        void IDisposable.Dispose()
        {
            Handles.color = _orginal;
        }
    }


    public struct HandlesMatrixScope : IDisposable
    {
        Matrix4x4 _orginal;

        public HandlesMatrixScope(Matrix4x4 value)
        {
            _orginal = Handles.matrix;
            Handles.matrix = value;
        }

        public HandlesMatrixScope(ref Matrix4x4 value)
        {
            _orginal = Handles.matrix;
            Handles.matrix = value;
        }

        void IDisposable.Dispose()
        {
            Handles.matrix = _orginal;
        }
    }


    public struct GizmosColorScope : IDisposable
    {
        Color _orginal;

        public GizmosColorScope(Color value)
        {
            _orginal = Gizmos.color;
            Gizmos.color = value;
        }

        void IDisposable.Dispose()
        {
            Gizmos.color = _orginal;
        }
    }


    public struct GizmosMatrixScope : IDisposable
    {
        Matrix4x4 _orginal;

        public GizmosMatrixScope(Matrix4x4 value)
        {
            _orginal = Gizmos.matrix;
            Gizmos.matrix = value;
        }

        public GizmosMatrixScope(ref Matrix4x4 value)
        {
            _orginal = Gizmos.matrix;
            Gizmos.matrix = value;
        }

        void IDisposable.Dispose()
        {
            Gizmos.matrix = _orginal;
        }
    }


    public struct DisabledScope : IDisposable
    {
        public DisabledScope(bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
        }

        public void Dispose()
        {
            EditorGUI.EndDisabledGroup();
        }
    }


    /// <summary>
    /// 检查 IMGUI 是否发生输入变化. 必须使用 static New() 来产生对象
    /// </summary>
    public struct ChangeCheckScope : IDisposable
    {
        bool _end;
        bool _changed;

        public bool changed
        {
            get
            {
                if (!_end)
                {
                    _end = true;
                    _changed = EditorGUI.EndChangeCheck();
                }
                return _changed;
            }
        }

        public static ChangeCheckScope New()
        {
            EditorGUI.BeginChangeCheck();
            return new ChangeCheckScope();
        }

        void IDisposable.Dispose()
        {
            if (!_end)
            {
                _end = true;
                _changed = EditorGUI.EndChangeCheck();
            }
        }
    }

} // namespace WhiteCat.Editor

#endif // UNITY_EDITOR