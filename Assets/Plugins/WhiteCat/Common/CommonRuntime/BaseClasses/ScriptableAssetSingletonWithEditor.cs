using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using WhiteCat.Editor;
#endif

namespace WhiteCat
{
	/// <summary>
	/// ScriptableAssetSingletonWithEditor
	/// 在编辑器和运行时提供访问实例的方法
	/// 如果未曾创建资源，或 Build 后不包含资源，运行时会创建一个临时对象
	/// 注意：没有被场景引用, 且 没有加入 Preloaded Assets 列表 且 不在 Resources 里的资源在 Build 时会被忽略
	/// </summary>
	public class ScriptableAssetSingletonWithEditor<T> : ScriptableAssetWithEditor
		where T : ScriptableAssetSingletonWithEditor<T>
	{
		static T _instance;


		/// <summary>
		/// 访问单例
		/// </summary>
		public static T instance
		{
			get
			{
				if (_instance == null)
				{
#if UNITY_EDITOR
					_instance = EditorAsset.FindAsset<T>();
					if (_instance == null)
					{
						_instance = CreateInstance<T>();
						Debug.LogWarning(string.Format("No asset of {0} loaded, a temporary instance was created. Use {0}.CreateSingletonAsset to create the asset.", typeof(T).Name));
					}
#else
					_instance = CreateInstance<T>();
					Debug.LogWarning(string.Format("No asset of {0} loaded, a temporary instance was created. Are you forget to add the asset to \"Preloaded Assets\" list?", typeof(T).Name));
#endif
				}
				return _instance;
			}
		}


		protected ScriptableAssetSingletonWithEditor()
		{
			_instance = this as T;
		}

#if UNITY_EDITOR

		/// <summary>
		/// 创建单例资源, 如果已经存在则选中该资源
		/// </summary>
		public static void CreateSingletonAsset()
		{
			bool needCreate = false;

			if (_instance == null)
			{
				_instance = EditorAsset.FindAsset<T>();
				if (_instance == null)
				{
					_instance = CreateInstance<T>();
					needCreate = true;
				}
			}
			else needCreate = !AssetDatabase.IsNativeAsset(_instance);

			if (needCreate)
			{
				EditorAsset.CreateAsset(_instance, EditorAsset.activeDirectory + '/' + typeof(T).Name + ".asset", true, false);
			}

			Selection.activeObject = instance;
		}

#endif

	} // class ScriptableAssetSingletonWithEditor

} // namespace WhiteCat