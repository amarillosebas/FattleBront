using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace WhiteCat
{
	/// <summary>
	/// Unity 扩展方法
	/// </summary>
	public static partial class Extension
	{
		/// <summary>
		/// 安全获取组件. 如果物体上没有组件则自动添加
		/// </summary>
		public static T SafeGetComponent<T>(this GameObject target) where T : Component
		{
			T component = target.GetComponent<T>();
			if (!component) component = target.AddComponent<T>();
			return component;
		}


		/// <summary>
		/// 安全获取组件. 如果物体上没有组件则自动添加
		/// </summary>
		public static T SafeGetComponent<T>(this Component target) where T : Component
		{
			T component = target.GetComponent<T>();
			if (!component) component = target.gameObject.AddComponent<T>();
			return component;
		}


		/// <summary>
		/// 重置 Transform 的 localPosition, localRotation 和 localScale
		/// </summary>
		public static void ResetLocal(this Transform transform)
		{
			transform.localPosition = Vector3.zero;
			transform.localRotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}


		/// <summary>
		/// 复制 Gradient 实例
		/// </summary>
		public static Gradient Clone(this Gradient target)
		{
			var newGradient = new Gradient();
			newGradient.alphaKeys = target.alphaKeys;
			newGradient.colorKeys = target.colorKeys;

#if UNITY_5_5 || UNITY_5_5_OR_NEWER
			newGradient.mode = target.mode;
#endif

			return newGradient;
		}


		/// <summary>
		/// 复制 AnimationCurve 实例
		/// </summary>
		public static AnimationCurve Clone(this AnimationCurve target)
		{
			var newCurve = new AnimationCurve(target.keys);
			newCurve.postWrapMode = target.postWrapMode;
			newCurve.preWrapMode = target.preWrapMode;

			return newCurve;
		}


		/// <summary>
		/// 计算颜色的感观亮度, 经实验比 Color.grayscale 更准确 (但也更慢)
		/// 参考: http://alienryderflex.com/hsp.html, 参数有改动
		/// </summary>
		/// <param name="color"> 要计算感观亮度的颜色, alpha 通道被忽略 </param>
		/// <returns> 颜色的感观亮度, [0..1] </returns>
		public static float GetPerceivedBrightness(this Color color)
		{
			return Mathf.Sqrt(
				0.3f * color.r * color.r +
				0.6f * color.g * color.g +
				0.1f * color.b * color.b);
		}


		/// <summary>
		/// 灰度调节. 保持颜色的色调并调节到指定灰度. alpha 通道保持不变.
		/// </summary>
		public static Color AdjustGrayscaleLDR(this Color color, float ldrGrayscale)
		{
			float cg = color.grayscale;

			if (cg > Math.OneMillionth)
			{
				float cs = ldrGrayscale / cg;
				if (cs * color.maxColorComponent > 1f)
				{
					cs = 1f / color.maxColorComponent;

					color.r *= cs;
					color.g *= cs;
					color.b *= cs;

					cg = ldrGrayscale - color.grayscale;

					float max = color.maxColorComponent;

					if (color.r == max)
					{
						cs = Mathf.Min(1f - Mathf.Max(color.g, color.b), cg / (1f - 0.299f));
						color.g += cs;
						color.b += cs;
					}
					else if (color.g == max)
					{
						cs = Mathf.Min(1f - Mathf.Max(color.r, color.b), cg / (1f - 0.587f));
						color.r += cs;
						color.b += cs;
					}
					else
					{
						cs = Mathf.Min(1f - Mathf.Max(color.r, color.g), cg / (1f - 0.114f));
						color.r += cs;
						color.g += cs;
					}

					cg = ldrGrayscale - color.grayscale;

					float min = Mathf.Min(Mathf.Min(color.r, color.g), color.b);

					if (color.r == min)
					{
						color.r += Mathf.Min(1f - color.r, cg / 0.299f);
					}
					else if (color.g == min)
					{
						color.g += Mathf.Min(1f - color.g, cg / 0.587f);
					}
					else
					{
						color.b += Mathf.Min(1f - color.b, cg / 0.114f);
					}
				}
				else
				{
					color.r *= cs;
					color.g *= cs;
					color.b *= cs;
				}
			}
			else
			{
				color.r = ldrGrayscale;
				color.g = ldrGrayscale;
				color.b = ldrGrayscale;
			}

			return color;
		}


		/// <summary>
		/// 将屏幕尺寸转化为世界尺寸
		/// </summary>
		public static float ScreenToWorldSize(this Camera camera, float pixelSize, float clipPlane)
		{
			if (camera.orthographic)
			{
				return pixelSize * camera.orthographicSize * 2f / camera.pixelHeight;
			}
			else
			{
				return pixelSize * clipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 2f / camera.pixelHeight;
			}
		}


		/// <summary>
		/// 将世界尺寸转化为屏幕尺寸
		/// </summary>
		public static float WorldToScreenSize(this Camera camera, float worldSize, float clipPlane)
		{
			if (camera.orthographic)
			{
				return worldSize * camera.pixelHeight * 0.5f / camera.orthographicSize;
			}
			else
			{
				return worldSize * camera.pixelHeight * 0.5f / (clipPlane * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
			}
		}


        /// <summary>
        /// 计算 ZBufferParams, 可用于 compute shader 
        /// </summary>
        public static Vector4 GetZBufferParams(this Camera camera)
        {
            float fpn = camera.farClipPlane / camera.nearClipPlane;
            return SystemInfo.usesReversedZBuffer ? new Vector4(0f, 0f, fpn - 1f, 1f) : new Vector4(0f, 0f, 1f - fpn, fpn);
        }


        /// <summary>
        /// (深度优先)遍历 Transform 层级, 对每一个节点执行一个自定义的操作
        /// </summary>
        /// <param name="root"> 遍历开始的根部 Transform 对象 </param>
        /// <param name="operate"> 遍历到每一个节点时将调用此方法 </param>
        /// <param name="depthLimit"> 访问深度限制, 负值表示不限制, 0 表示只访问 root 本身而不访问其子级, 正值表示最多访问的子级层数 </param>
        public static void TraverseHierarchy(this Transform root, Action<Transform> operate, int depthLimit = -1)
		{
			operate(root);
			if (depthLimit == 0) return;

			int count = root.childCount;

			for (int i = 0; i < count; i++)
			{
				TraverseHierarchy(root.GetChild(i), operate, depthLimit - 1);
			}
		}


		/// <summary>
		/// (深度优先)遍历 Transform 层级, 判断每一个节点是否为查找目标, 发现查找目标则立即终止查找
		/// </summary>
		/// <param name="root"> 遍历开始的根部 Transform 对象 </param>
		/// <param name="match"> 判断当前节点是否为查找目标 </param>
		/// <param name="depthLimit"> 遍历深度限制, 负值表示不限制, 0 表示只访问 root 本身而不访问其子级, 正值表示最多访问的子级层数 </param>
		/// <returns> 如果查找到目标则返回此目标; 否则返回 null </returns>
		public static Transform FindInHierarchy(this Transform root, Predicate<Transform> match, int depthLimit = -1)
		{
			if (match(root)) return root;
			if (depthLimit == 0) return null;

			int count = root.childCount;
			Transform result = null;

			for (int i = 0; i < count; i++)
			{
				result = FindInHierarchy(root.GetChild(i), match, depthLimit - 1);
				if (result) break;
			}

			return result;
		}


		/// <summary>
		/// 为 EventTrigger 添加事件
		/// </summary>
		public static void AddListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<BaseEventData> callback)
		{
			var triggers = eventTrigger.triggers;
			var index = triggers.FindIndex(entry => entry.eventID == type);
			if (index < 0)
			{
				var entry = new EventTrigger.Entry();
				entry.eventID = type;
				entry.callback.AddListener(callback);
				triggers.Add(entry);
			}
			else
			{
				triggers[index].callback.AddListener(callback);
			}
		}


		/// <summary>
		/// 为 EventTrigger 移除事件
		/// </summary>
		public static void RemoveListener(this EventTrigger eventTrigger, EventTriggerType type, UnityAction<BaseEventData> callback)
		{
			var triggers = eventTrigger.triggers;
			var index = triggers.FindIndex(entry => entry.eventID == type);
			if (index >= 0)
			{
				triggers[index].callback.RemoveListener(callback);
			}
		}


		/// <summary>
		/// 延时调用指定的方法
		/// </summary>
		/// <param name="behaviour"> 协程附着的脚本对象 </param>
		/// <param name="delay"> 延迟时间(秒) </param>
		/// <param name="action"> 延时结束调用的方法 </param>
		public static void DelayedInvoke(this MonoBehaviour behaviour, float delay, Action action)
		{
			behaviour.StartCoroutine(DelayedCoroutine(delay, action));
		}


		/// <summary>
		/// 延迟协程
		/// </summary>
		/// <param name="delay"> 延迟时间(秒) </param>
		/// <param name="action"> 延时结束调用的方法 </param>
		/// <returns> 协程迭代器 </returns>
		public static IEnumerator DelayedCoroutine(float delay, Action action)
		{
			yield return new WaitForSeconds(delay);
			action();
		}

	} // class Extension

} // namespace WhiteCat