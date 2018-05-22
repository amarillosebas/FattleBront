using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using WhiteCat.Editor;
#endif

namespace WhiteCat
{
	/// <summary>
	/// 混合器
	/// </summary>
	public interface IBlender<T>
	{
		bool Equals(T x, T y);
		T Blend(T x, T y);
		T Scale(T x, float y);
	}


	/// <summary>
	/// 一维混合事件
	/// </summary>
	[Serializable]
	public struct BlendFloatEvent
	{
		[Min(0)]
		public float startDelay;

		[Min(0.001)]
		public float duration;

		public float value;

		[Tooltip("Input and output are normalized.")]
		public AnimationCurve attenuation;
	}


	/// <summary>
	/// 二维混合事件
	/// </summary>
	[Serializable]
	public struct BlendVector2Event
	{
		[Min(0)]
		public float startDelay;

		[Min(0.001)]
		public float duration;

		public Vector2 value;

		[Tooltip("Input and output are normalized.")]
		public AnimationCurve attenuation;
	}


	/// <summary>
	/// 混合效果控制器
	/// 用于控制多种输入单一输出的效果，比如控制器震动、屏幕特效等
	/// </summary>
	public abstract class BlendEffectController<T> : ScriptableComponentWithEditor, IBlender<T>
	{
		[SerializeField, GetSet("baseChannelValue")]
		T _baseChannelValue;

		[SerializeField, GetSet("outputScale")]
		float _outputScale = 1f;

		List<Channel> _channels = new List<Channel>(4);
		bool _anyChannelChanged = true;
		T _channelsOutput;

		List<Event> _events = new List<Event>(4);

		bool _outputChanged;
		T _finalOutput;


		/// <summary>
		/// 混合通道
		/// </summary>
		public class Channel : IDisposable
		{
			T _value;
			BlendEffectController<T> _controller;

			/// <summary>
			/// 通道输出值
			/// </summary>
			public T value
			{
				get { return _value; }
				set
				{
					if (_controller != null && !_controller.Equals(_value, value))
					{
						_value = value;
						_controller._anyChannelChanged = true;
					}
				}
			}

			public Channel(T value, BlendEffectController<T> controller)
			{
				_value = value;
				_controller = controller;
				_controller._anyChannelChanged = true;
			}

			public void Dispose()
			{
				if (_controller != null)
				{
					_controller._channels.Remove(this);
					_controller._anyChannelChanged = true;
					_controller = null;
				}
			}
		}


		// 混合事件
		private struct Event
		{
			public float time01;
			public T value;
			public float startDelay;
			public float duration;
			public AnimationCurve attenuation;

			public float scale
			{
				get { return Mathf.Max(attenuation.Evaluate(time01), 0f); }
			}
		}


		/// <summary>
		/// 判断两个值是否相等
		/// </summary>
		public abstract bool Equals(T a, T b);


		/// <summary>
		/// 混合值
		/// </summary>
		public abstract T Blend(T a, T b);


		/// <summary>
		/// 缩放值
		/// </summary>
		public abstract T Scale(T a, float b);


		/// <summary>
		/// 基本通道值
		/// </summary>
		public T baseChannelValue
		{
			get { return _baseChannelValue; }
			set
			{
				if (!Equals(_baseChannelValue, value))
				{
					_baseChannelValue = value;
					_anyChannelChanged = true;
				}
			}
		}


		/// <summary>
		/// 输出缩放
		/// </summary>
		public float outputScale
		{
			get { return _outputScale; }
			set
			{
				value = Mathf.Max(value, 0f);
				if (_outputScale != value)
				{
					_outputScale = value;
					_outputChanged = true;
				}
			}
		}


		public T lastOutput
		{
			get { return _finalOutput; }
		}


		/// <summary>
		/// 创建通道 
		/// 当通道不再使用时，必须使用 Dispose 移除
		/// </summary>
		public Channel CreateChannel(T value)
		{
			var channel = new Channel(value, this);
			_channels.Add(channel);
			return channel;
		}


		/// <summary>
		/// 创建事件
		/// 事件在超过作用时间后自动移除
		/// </summary>
		public void CreateEvent(float startDelay, float duration, T value, AnimationCurve attenuation)
		{
			_events.Add(new Event
			{
				time01 = 0,
				value = value,
				startDelay = startDelay,
				duration = duration,
				attenuation = attenuation,
			});
		}


		private void UpdateEvents(float deltaTime)
		{
			Event e;
			float dt;

			for (int i = 0; i < _events.Count; i++)
			{
				e = _events[i];
				dt = deltaTime;

				if (e.startDelay > 0f)
				{
					e.startDelay -= deltaTime;
					if (e.startDelay <= 0f)
					{
						dt = -e.startDelay;
						e.startDelay = 0f;
					}
					_events[i] = e;
				}

				if (e.startDelay == 0f)
				{
					e.time01 += dt / e.duration;

					if (e.time01 >= 1f)
					{
						_events.RemoveAt(i--);
						e.time01 = 1f;
					}
					else _events[i] = e;

					_finalOutput = Blend(_finalOutput, Scale(e.value, e.scale));
				}
			}
		}


		/// <summary>
		/// 在必要的时候执行更新（比如子类实现时以一定频率调用）
		/// </summary>
		/// <returns> 如果输出发生变化返回 true, 否则返回 false </returns>
		public bool Refresh(float deltaTime)
		{
			if (_anyChannelChanged)
			{
				_anyChannelChanged = false;
				_channelsOutput = _baseChannelValue;

				for (int i = 0; i < _channels.Count; i++)
				{
					_channelsOutput = Blend(_channelsOutput, _channels[i].value);
				}

				_outputChanged = true;
			}

			if (_outputChanged || _events.Count > 0)
			{
				_outputChanged = false;
				_finalOutput = _channelsOutput;

				if (_events.Count > 0)
				{
					UpdateEvents(deltaTime);
				
					// 即使事件完全移除仍需再刷新一次
					_outputChanged = true;
				}

				_finalOutput = Scale(_finalOutput, _outputScale);
				return true;
			}

			return false;
		}


#if UNITY_EDITOR

		private void Reset()
		{
			_channels.Clear();
			_events.Clear();
			_anyChannelChanged = true;
		}


		void OnValidate()
		{
			_anyChannelChanged = true;
		}


		protected abstract void Editor_DrawOutputLayout();


		protected void Editor_DrawDebugInfo()
		{
			using (new DisabledScope(true))
			{
				Editor_DrawOutputLayout();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.HelpBox("Channels: " + (_channels.Count + 1), MessageType.None, true);
				EditorGUILayout.HelpBox("Events: " + _events.Count, MessageType.None, true);
				EditorGUILayout.EndHorizontal();
			}
		}


		protected override void Editor_OnInspectorGUI()
		{
			base.Editor_OnInspectorGUI();
			EditorGUILayout.Space();
			Editor_DrawDebugInfo();
		}

#endif

	} // class BlendEffectController<T>


	/// <summary>
	/// 混合效果控制器 (继承此类实现抽象方法)
	/// 用于控制多种输入单一输出的效果，比如控制器震动、屏幕特效等
	/// </summary>
	public abstract class BlendFloatEffectController : BlendEffectController<float>
	{
		public sealed override float Scale(float a, float b)
		{
			return a * b;
		}


		public sealed override bool Equals(float a, float b)
		{
			return a == b;
		}


		/// <summary>
		/// 创建事件
		/// 事件在超过作用时间后自动移除
		/// </summary>
		public void CreateEvent(BlendFloatEvent @event)
		{
			CreateEvent(@event.startDelay, @event.duration, @event.value, @event.attenuation);
		}

#if UNITY_EDITOR

		protected override void Editor_DrawOutputLayout()
		{
			EditorGUILayout.FloatField("Last Output", lastOutput);
		}

#endif

	} // BlendFloatEffectController


	/// <summary>
	/// 混合效果控制器 (继承此类实现抽象方法)
	/// 用于控制多种输入单一输出的效果，比如控制器震动、屏幕特效等
	/// </summary>
	public abstract class BlendVector2EffectController : BlendEffectController<Vector2>
	{
		public sealed override Vector2 Scale(Vector2 a, float b)
		{
			return a * b;
		}


		public sealed override bool Equals(Vector2 a, Vector2 b)
		{
			return a.x == b.x && a.y == b.y;
		}


		/// <summary>
		/// 创建事件
		/// 事件在超过作用时间后自动移除
		/// </summary>
		public void CreateEvent(BlendVector2Event @event)
		{
			CreateEvent(@event.startDelay, @event.duration, @event.value, @event.attenuation);
		}

#if UNITY_EDITOR

		protected override void Editor_DrawOutputLayout()
		{
			EditorGUILayout.Vector2Field("Last Output", lastOutput);
		}

#endif

	} // BlendVector2EffectController


	public class AdditiveFloatEffectController : BlendFloatEffectController
	{
		public sealed override float Blend(float a, float b)
		{
			return a + b;
		}
	}


	public class MultiplyFloatEffectController : BlendFloatEffectController
	{
		public sealed override float Blend(float a, float b)
		{
			return a * b;
		}
	}


	public class MaximumFloatEffectController : BlendFloatEffectController
	{
		public sealed override float Blend(float a, float b)
		{
			return Mathf.Max(a, b);
		}
	}


	public class AdditiveVector2EffectController : BlendVector2EffectController
	{
		public sealed override Vector2 Blend(Vector2 a, Vector2 b)
		{
			return a + b;
		}
	}


	public class MultiplyVector2EffectController : BlendVector2EffectController
	{
		public sealed override Vector2 Blend(Vector2 a, Vector2 b)
		{
			return Vector2.Scale(a, b);
		}
	}


	public class MaximumVector2EffectController : BlendVector2EffectController
	{
		public sealed override Vector2 Blend(Vector2 a, Vector2 b)
		{
			return Vector2.Max(a, b);
		}
	}

} // namespace WhiteCat