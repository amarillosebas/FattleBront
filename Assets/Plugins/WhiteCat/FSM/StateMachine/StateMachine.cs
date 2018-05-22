using System;
using UnityEngine;
using UnityEngine.Events;

namespace WhiteCat.FSM
{
	/// <summary>
	/// 通用状态机组件
	/// </summary>
	[AddComponentMenu("White Cat/FSM/State Machine")]
	public class StateMachine : BaseStateMachine
	{
        [SerializeField, GetSet("updateMode")]
        UpdateMode _updateMode = UpdateMode.Update;


        /// <summary>
        /// 时间模式
        /// </summary>
        public TimeMode timeMode = TimeMode.Normal;


        /// <summary>
        /// 初始状态
        /// </summary>
        public BaseState startState;


		[Header("Sub-state Machine Events")]
		[SerializeField]
		UnityEvent _onEnter;

		[SerializeField]
		UnityEvent _onExit;


		// 是否作为子状态机使用
		bool _isSubStateMachine = false;


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
            if (!_isSubStateMachine)
            {
                OnUpdate(timeMode == TimeMode.Normal ? Time.deltaTime : Time.unscaledDeltaTime);
            }
        }


        void OnEnable()
        {
            RegisterUpdateSafely();
        }


        void OnDisable()
        {
            UnregisterUpdateSafely();
        }


        // 设置初始状态
        void Start()
		{
			if (startState)
			{
				currentStateComponent = startState;
			}
		}


		/// <summary>
		/// 添加或移除更新状态触发的事件
		/// </summary>
		public event Action<float> onUpdate;


		/// <summary>
		/// 添加或移除进入状态触发的事件 (作为子状态机)
		/// </summary>
		public event UnityAction onEnter
		{
			add
			{
				if (_onEnter == null)
				{
					_onEnter = new UnityEvent();
				}
				_onEnter.AddListener(value);
			}
			remove
			{
				if (_onEnter != null)
				{
					_onEnter.RemoveListener(value);
				}
			}
		}


		/// <summary>
		/// 添加或移除离开状态触发的事件 (作为子状态机)
		/// </summary>
		public event UnityAction onExit
		{
			add
			{
				if (_onExit == null)
				{
					_onExit = new UnityEvent();
				}
				_onExit.AddListener(value);
			}
			remove
			{
				if (_onExit != null)
				{
					_onExit.RemoveListener(value);
				}
			}
		}


		// 当作为子状态机使用时, 需要停止主动调用 OnUpdate
		public override void OnEnter()
		{
			_isSubStateMachine = true;

			if (_onEnter != null)
			{
				_onEnter.Invoke();
			}
		}


		// 当不再作为子状态机使用时, 才允许主动调用 OnUpdate
		public override void OnExit()
		{
			_isSubStateMachine = false;

			if (_onExit != null)
			{
				_onExit.Invoke();
			}
		}


		public override void OnUpdate(float deltaTime)
		{
			base.OnUpdate(deltaTime);

			if (onUpdate != null)
			{
				onUpdate(deltaTime);
			}
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

#endif

    } // class StateMachine

} // namespace WhiteCat.FSM