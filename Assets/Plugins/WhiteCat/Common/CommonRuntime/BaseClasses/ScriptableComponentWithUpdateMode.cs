using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WhiteCat
{
    /// <summary>
    /// ScriptableComponentWithUpdateMode
    /// </summary>
    public abstract class ScriptableComponentWithUpdateMode : ScriptableComponentWithEditor
    {
        [SerializeField, GetSet("updateMode")]
        UpdateMode _updateMode = UpdateMode.Update;


        [SerializeField]
        TimeMode _timeMode = TimeMode.Normal;


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


        /// <summary>
        /// 时间模式
        /// </summary>
        public TimeMode timeMode
        {
            get { return _timeMode; }
            set { _timeMode = value; }
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
            UpdateCall(timeMode == TimeMode.Normal ? Time.deltaTime : Time.unscaledDeltaTime);
        }


        protected abstract void UpdateCall(float deltaTime);


        protected virtual void OnEnable()
        {
            RegisterUpdateSafely();
        }


        protected virtual void OnDisable()
        {
            UnregisterUpdateSafely();
        }


#if UNITY_EDITOR

        UpdateMode _lastRegisteredUpdateMode;


        protected virtual void OnValidate()
        {
            if (_updateRegistered && _lastRegisteredUpdateMode != _updateMode)
            {
                UnregisterUpdateSafely();
                RegisterUpdateSafely();
            }
        }


        SerializedProperty _updateModeProp;
        SerializedProperty _timeModeProp;


        protected void Editor_FindUpdateModeProperties()
        {
            _updateModeProp = editor.serializedObject.FindProperty("_updateMode");
            _timeModeProp = editor.serializedObject.FindProperty("_timeMode");
        }


        protected void Editor_ReleaseUpdateModeProperties()
        {
            _updateModeProp = null;
            _timeModeProp = null;
        }


        protected void Editor_DrawUpdateModeProperties()
        {
            EditorGUILayout.PropertyField(_updateModeProp);
            EditorGUILayout.PropertyField(_timeModeProp);
        }

#endif

    } // class ScriptableComponentWithUpdateMode

} // namespace WhiteCat