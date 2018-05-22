using System;
using System.Collections;
using UnityEngine;

namespace WhiteCat
{
	/// <summary>
	/// 通用工具
	/// </summary>
	public struct Utilities
	{
		static GameObject _globalGameObject;
		static GlobalComponent _globalComponent;
        static Texture2D _whiteTexture;
        static Texture2D _blackTexture;
		static Mesh _quadMesh;


        /// <summary>
        /// 全局游戏对象. 不可见, 不会保存, 不可编辑, 不可卸载. 不要尝试 Destroy 它, 否则世界会坏掉
        /// </summary>
        static GameObject globalGameObject
		{
			get
			{
				if (!_globalGameObject)
				{
					_globalGameObject = new GameObject("GlobalGameObject");
#if UNITY_EDITOR
                    if (Application.isPlaying)
#endif
                    GameObject.DontDestroyOnLoad(_globalGameObject);

					_globalGameObject.hideFlags =
                        HideFlags.HideInHierarchy
						| HideFlags.HideInInspector
						| HideFlags.DontSaveInEditor
						| HideFlags.DontSaveInBuild
						| HideFlags.DontUnloadUnusedAsset;
				}
				return _globalGameObject;
			}
		}


		/// <summary>
		/// 全局组件，用以方便的添加/移除更新事件
		/// </summary>
		static GlobalComponent globalComponent
		{
			get
			{
				if (!_globalComponent)
				{
					_globalComponent = globalGameObject.AddComponent<GlobalComponent>();
				}
				return _globalComponent;
			}
		}


        public static Texture2D whiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }


        public static Texture2D blackTexture
        {
            get
            {
                if (_blackTexture == null)
                {
                    _blackTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    _blackTexture.SetPixel(0, 0, Color.black);
                    _blackTexture.Apply();
                }

                return _blackTexture;
            }
        }


        /// <summary>
        /// 添加全局组件
        /// </summary>
        public static T AddGlobalComponent<T>() where T : Component
        {
            return globalGameObject.AddComponent<T>();
        }


        /// <summary>
        /// 安全获取全局组件，如果不存在会自动添加
        /// </summary>
        public static T SafeGetGlobalComponent<T>() where T : Component
        {
            return globalGameObject.SafeGetComponent<T>();
        }


        /// <summary>
        /// 添加全局更新
        /// </summary>
        public static void AddGlobalUpdate(UpdateMode updateMode, Action update)
        {
            switch (updateMode)
            {
                case UpdateMode.Update:
                    globalComponent.update += update;
                    return;
                case UpdateMode.LateUpdate:
                    globalComponent.lateUpdate += update;
                    return;
                case UpdateMode.FixedUpdate:
                    globalComponent.fixedUpdate += update;
                    return;
                case UpdateMode.WaitForFixedUpdate:
                    globalComponent.waitForFixedUpdate += update;
                    return;
                case UpdateMode.WaitForEndOfFrame:
                    globalComponent.waitForEndOfFrame += update;
                    return;
            }
        }


        /// <summary>
        /// 移除全局更新
        /// </summary>
        public static void RemoveGlobalUpdate(UpdateMode updateMode, Action update)
        {
            switch (updateMode)
            {
                case UpdateMode.Update:
                    globalComponent.update -= update;
                    return;
                case UpdateMode.LateUpdate:
                    globalComponent.lateUpdate -= update;
                    return;
                case UpdateMode.FixedUpdate:
                    globalComponent.fixedUpdate -= update;
                    return;
                case UpdateMode.WaitForFixedUpdate:
                    globalComponent.waitForFixedUpdate -= update;
                    return;
                case UpdateMode.WaitForEndOfFrame:
                    globalComponent.waitForEndOfFrame -= update;
                    return;
            }
        }


        /// <summary>
        /// 四边形 mesh
        /// </summary>
        public static Mesh quadMesh
		{
			get
			{
				if (!_quadMesh)
				{
					var vertices = new[]
					{
						new Vector3(-0.5f, -0.5f, 0f),
						new Vector3(0.5f,  0.5f, 0f),
						new Vector3(0.5f, -0.5f, 0f),
						new Vector3(-0.5f,  0.5f, 0f)
					};

					var uvs = new[]
					{
						new Vector2(0f, 0f),
						new Vector2(1f, 1f),
						new Vector2(1f, 0f),
						new Vector2(0f, 1f)
					};

					var indices = new[] { 0, 1, 2, 1, 0, 3 };

					_quadMesh = new Mesh
					{
						vertices = vertices,
						uv = uvs,
						triangles = indices
					};

					_quadMesh.RecalculateNormals();
					_quadMesh.RecalculateBounds();
				}

				return _quadMesh;
			}
		}


		/// <summary>
		/// 同时设置 Unity 时间缩放和 FixedUpdate 频率
		/// </summary>
		/// <param name="timeScale"> 要设置的时间缩放 </param>
		/// <param name="fixedFrequency"> 要设置的 FixedUpdate 频率 </param>
		public static void SetTimeScaleAndFixedFrequency(float timeScale, float fixedFrequency = 50f)
		{
			Time.timeScale = timeScale;
			Time.fixedDeltaTime = timeScale / fixedFrequency;
		}


		/// <summary>
		///颜色轮. 不同 hue 对应的颜色为:
		/// 0-red; 0.167-yellow; 0.333-green; 0.5-cyan; 0.667-blue; 0.833-magenta; 1-red
		/// </summary>
		public static Color ColorWheel(float hue)
		{
			return new Color(
				GreenColorWheel(hue + 1f / 3f),
				GreenColorWheel(hue),
				GreenColorWheel(hue - 1f / 3f));
		}


		static float GreenColorWheel(float hue)
		{
			hue = ((hue % 1f + 1f) % 1f) * 6f;

			if (hue < 1f) return hue;
			if (hue < 3f) return 1f;
			if (hue < 4f) return (4f - hue);
			return 0f;
		}


		/// <summary>
		/// 交换两个变量的值
		/// </summary>
		public static void Swap<T>(ref T a, ref T b)
		{
			T c = a;
			a = b;
			b = c;
		}


		/// <summary>
		/// 判断集合是否为 null 或元素个数是否为 0
		/// </summary>
		public static bool IsNullOrEmpty(ICollection collection)
		{
			return collection == null || collection.Count == 0;
		}

	} // struct Utilities


	[ExecuteInEditMode]
	public class GlobalComponent : ScriptableComponent
	{
		WaitForFixedUpdate _waitForFixedUpdate;
		WaitForEndOfFrame _waitForEndOfFrame;


		public event Action fixedUpdate;
		public event Action waitForFixedUpdate;
		public event Action update;
		public event Action lateUpdate;
		public event Action waitForEndOfFrame;


		void Start()
		{
			_waitForFixedUpdate = new WaitForFixedUpdate();
			StartCoroutine(WaitForFixedUpdate());

			_waitForEndOfFrame = new WaitForEndOfFrame();
			StartCoroutine(WaitForEndOfFrame());
		}


		void FixedUpdate()
		{
			if (fixedUpdate != null) fixedUpdate();
		}


		IEnumerator WaitForFixedUpdate()
		{
			while (true)
			{
				yield return _waitForFixedUpdate;
				if (waitForFixedUpdate != null) waitForFixedUpdate();
			}
		}


		void Update()
		{
			if (update != null) update();
		}


		void LateUpdate()
		{
			if (lateUpdate != null) lateUpdate();
		}


		IEnumerator WaitForEndOfFrame()
		{
			while (true)
			{
				yield return _waitForEndOfFrame;
				if (waitForEndOfFrame != null) waitForEndOfFrame();
			}
		}
	}

} // namespace WhiteCat