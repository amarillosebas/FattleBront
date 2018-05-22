#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace WhiteCat.Editor
{
    public class MeasuringWindow : EditorWindow
    {
        static MeasuringWindow _instance;

        bool _distanceFoldout;
        Transform _distanceStartTrans;
        Vector3 _distanceStartPos;
        Transform _distanceEndTrans;
        Vector3 _distanceEndPos;


        [MenuItem("Window/White Cat/Measuring")]
        static void ShowWindow()
        {
            if (!_instance)
            {
                _instance = GetWindow<MeasuringWindow>("Measuring");
                _instance.autoRepaintOnSceneChange = true;
            }
            _instance.ShowUtility();
        }


        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }


        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            Tools.hidden = false;
        }


        void OnGUI()
        {
            EditorGUILayout.Space();
            if (_distanceFoldout = EditorGUILayout.Foldout(_distanceFoldout, "Distance"))
            {
                using (new IndentScope(2)) DrawDistanceUI();
            }

            Tools.hidden = _distanceFoldout;
        }


        void OnSceneGUI(SceneView scene)
        {
            if (_distanceFoldout)
            {
                DrawDistanceHandles();
            }
        }


        void DrawDistanceUI()
        {
            EditorGUILayout.LabelField("Start");
            using (new IndentScope(2))
            {
                _distanceStartTrans = EditorGUILayout.ObjectField(GUIContent.none, _distanceStartTrans, typeof(Transform), true) as Transform;
                if (_distanceStartTrans)
                {
                    using (var scope = ChangeCheckScope.New())
                    {
                        _distanceStartPos = EditorGUILayout.Vector3Field(GUIContent.none, _distanceStartTrans.position);
                        if (scope.changed)
                        {
                            Undo.RecordObject(_distanceStartTrans, "Transform.position");
                            _distanceStartTrans.position = _distanceStartPos;
                            EditorUtility.SetDirty(_distanceStartTrans);
                        }
                    }
                }
                else
                {
                    _distanceStartPos = EditorGUILayout.Vector3Field(GUIContent.none, _distanceStartPos);
                }
            }

            EditorGUILayout.LabelField("End");
            using (new IndentScope(2))
            {
                _distanceEndTrans = EditorGUILayout.ObjectField(GUIContent.none, _distanceEndTrans, typeof(Transform), true) as Transform;
                if (_distanceEndTrans)
                {
                    using (var scope = ChangeCheckScope.New())
                    {
                        _distanceEndPos = EditorGUILayout.Vector3Field(GUIContent.none, _distanceEndTrans.position);
                        if (scope.changed)
                        {
                            Undo.RecordObject(_distanceEndTrans, "Transform.position");
                            _distanceEndTrans.position = _distanceEndPos;
                            EditorUtility.SetDirty(_distanceEndTrans);
                        }
                    }
                }
                else
                {
                    _distanceEndPos = EditorGUILayout.Vector3Field(GUIContent.none, _distanceEndPos);
                }
            }

            EditorGUILayout.LabelField("Distance");
            using (new IndentScope(2))
            {
                EditorGUILayout.Vector3Field(GUIContent.none, _distanceEndPos - _distanceStartPos);
                EditorGUILayout.FloatField(GUIContent.none, Vector3.Distance(_distanceStartPos, _distanceEndPos));
            }
        }


        void DrawDistanceHandles()
        {
            if (_distanceStartTrans)
            {
                using (var scope = ChangeCheckScope.New())
                {
                    _distanceStartPos = Handles.PositionHandle(_distanceStartTrans.position, Tools.pivotRotation == PivotRotation.Local ? _distanceStartTrans.rotation : Quaternion.identity);
                    if (scope.changed)
                    {
                        Undo.RecordObject(_distanceStartTrans, "Transform.position");
                        _distanceStartTrans.position = _distanceStartPos;
                        EditorUtility.SetDirty(_distanceStartTrans);
                    }
                }
            }
            else
            {
                using (var scope = ChangeCheckScope.New())
                {
                    _distanceStartPos = Handles.PositionHandle(_distanceStartPos, Quaternion.identity);
                    if (scope.changed) Repaint();
                }
            }

            if (_distanceEndTrans)
            {
                using (var scope = ChangeCheckScope.New())
                {
                    _distanceEndPos = Handles.PositionHandle(_distanceEndTrans.position, Tools.pivotRotation == PivotRotation.Local ? _distanceEndTrans.rotation : Quaternion.identity);
                    if (scope.changed)
                    {
                        Undo.RecordObject(_distanceEndTrans, "Transform.position");
                        _distanceEndTrans.position = _distanceEndPos;
                        EditorUtility.SetDirty(_distanceEndTrans);
                    }
                }
            }
            else
            {
                using (var scope = ChangeCheckScope.New())
                {
                    _distanceEndPos = Handles.PositionHandle(_distanceEndPos, Quaternion.identity);
                    if (scope.changed) Repaint();
                }
            }

            Handles.DrawLine(_distanceStartPos, _distanceEndPos);
            Handles.Label((_distanceStartPos + _distanceEndPos) * 0.5f, Vector3.Distance(_distanceStartPos, _distanceEndPos).ToString("0.00 m"), EditorStyles.whiteLabel);
        }
    }
}

#endif