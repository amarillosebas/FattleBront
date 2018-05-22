using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WhiteCat.Paths
{
	/// <summary>
	/// 在路径上按指定速度移动
	/// </summary>
	[AddComponentMenu("White Cat/Path/Move Along Path/Move Along Path with Speed")]
	public class MoveAlongPathWithSpeed : MoveAlongPath
	{
		/// <summary>
		/// 移动速度
		/// </summary>
		public float speed = 1f;


		[SerializeField, GetSet("updateMode")]
        UpdateMode _updateMode = UpdateMode.Update;


		/// <summary>
		/// 时间模式
		/// </summary>
		public TimeMode timeMode = TimeMode.Normal;


        /// <summary>
        /// 更新模式
        /// </summary>
        public UpdateMode updateMode
        {
            get { return _updateMode; }
            set
            {
                if (_updateMode != value)
                {
                    UnregisterUpdateSafely();
                    _updateMode = value;
                    RegisterUpdateSafely();
                }
            }
        }


        bool _updateRegistered = false;


        void RegisterUpdateSafely()
        {
            if (!_updateRegistered && enabled
#if UNITY_EDITOR
                && Application.isPlaying
#endif
                )
            {
                _updateRegistered = true;
                Utilities.AddGlobalUpdate(_updateMode, UpdateCall);
#if UNITY_EDITOR
                _lastRegisteredUpdateMode = _updateMode;
#endif
            }
        }


        void UnregisterUpdateSafely()
        {
            if (_updateRegistered)
            {
                _updateRegistered = false;
                Utilities.RemoveGlobalUpdate(_updateMode, UpdateCall);
            }
        }


        void UpdateCall()
        {
            distance += speed * (timeMode == TimeMode.Normal ? Time.deltaTime : Time.unscaledDeltaTime);
        }


        void OnEnable()
        {
            RegisterUpdateSafely();
        }


        void OnDisable()
        {
            UnregisterUpdateSafely();
        }

#if UNITY_EDITOR

        UpdateMode _lastRegisteredUpdateMode;


        void OnValidate()
        {
            if (_updateRegistered && _lastRegisteredUpdateMode != _updateMode)
            {
                UnregisterUpdateSafely();
                RegisterUpdateSafely();
            }
        }


        SerializedProperty _speedProperty;
        SerializedProperty _updateModeProperty;
        SerializedProperty _timeModeProperty;


        protected override void Editor_OnEnable()
        {
            _speedProperty = editor.serializedObject.FindProperty("speed");
            _updateModeProperty = editor.serializedObject.FindProperty("_updateMode");
            _timeModeProperty = editor.serializedObject.FindProperty("timeMode");
        }


        protected override void Editor_OnDisable()
        {
            _speedProperty = null;
            _updateModeProperty = null;
            _timeModeProperty = null;
        }


        protected override void Editor_OnExtraInspectorGUI()
        {
            editor.serializedObject.Update();
            EditorGUILayout.PropertyField(_speedProperty);
            EditorGUILayout.PropertyField(_updateModeProperty);
            EditorGUILayout.PropertyField(_timeModeProperty);
            editor.serializedObject.ApplyModifiedProperties();
        }

#endif

    } // class MoveAlongPathWithSpeed

} // namespace WhiteCat.Paths