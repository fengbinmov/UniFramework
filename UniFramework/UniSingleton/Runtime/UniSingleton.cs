using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UniFramework.Singleton
{
    public static class UniSingleton
    {
        private class Wrapper
        {
            private const string ONCREATE = "OnCreate";
            private const string ONUPDATE = "OnUpdate";
            private const string ONDESTROY = "OnDestroy";
            private const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            private readonly Type[] types1 = new Type[] { typeof(Action<object>) };
            private readonly Type[] types2 = new Type[] {  };

            public Action<object> onCreate { private set; get; }
            public Action OnUpdate { private set; get; }
            public Action onDestroy { private set; get; }

            public int Priority { private set; get; }
            public object Singleton { private set; get; }

            public Wrapper(object module,Type type, int priority)
            {
                Singleton = module;
                Priority = priority;
                
                MethodInfo method1 = type.GetMethod(ONCREATE, bindingFlags, Type.DefaultBinder, types1, null);
                MethodInfo method2 = type.GetMethod(ONUPDATE, 0,bindingFlags, Type.DefaultBinder, types2, null);
                MethodInfo method3 = type.GetMethod(ONDESTROY, 0,bindingFlags, Type.DefaultBinder, types2, null);

                if (method1 != null) onCreate = (Action<object>)method1.CreateDelegate(types1[0], module);
                if (method2 != null) OnUpdate = (Action)method2.CreateDelegate(typeof(Action), module);
                if (method3 != null) onDestroy = (Action)method3.CreateDelegate(typeof(Action), module);
            }
        }

        private static bool _isInitialize = false;
        private static GameObject _driver = null;
        private static int onUpdateCount;
        private static readonly List<Wrapper> _wrappers = new List<Wrapper>(100);
        private static MonoBehaviour _behaviour;
        private static bool _isDirty = false;

        /// <summary>
        /// 初始化单例系统
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialize)
                throw new Exception($"{nameof(UniSingleton)} is initialized !");

            if (_isInitialize == false)
            {
                // 创建驱动器
                _isInitialize = true;
                _driver = new GameObject($"[{nameof(UniSingleton)}]");
                _behaviour = _driver.AddComponent<UniSingletonDriver>();
                UnityEngine.Object.DontDestroyOnLoad(_driver);
                UniLogger.Log($"{nameof(UniSingleton)} initalize !");
            }
        }

        /// <summary>
        /// 销毁单例系统
        /// </summary>
        public static void Destroy()
        {
            if (_isInitialize)
            {
                DestroyAll();

                _isInitialize = false;
                if (_driver != null)
                    GameObject.Destroy(_driver);
                UniLogger.Log($"{nameof(UniSingleton)} destroy all !");
            }
        }

        /// <summary>
        /// 更新单例系统
        /// </summary>
        internal static void Update()
        {
            // 如果需要重新排序
            if (_isDirty)
            {
                _isDirty = false;
                int flag = 0;
                for (int i = 0; i < onUpdateCount - 1; i++)
                {
                    for (int j = 0; j < onUpdateCount -1- i; j++)
                    {
                        if (_wrappers[i].Priority > _wrappers[j + 1].Priority) {
                            var temp = _wrappers[i];
                            _wrappers[i] = _wrappers[j + 1];
                            _wrappers[j + 1] = temp;
                            flag++;
                        }
                    }
                    if (flag == 0) break;
                }
            }

            // 轮询所有模块
            for (int i = 0; i < onUpdateCount; i++)
            {
                _wrappers[i].OnUpdate();
            }
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        public static T GetSingleton<T>() where T : class, ISingleton
        {
            System.Type type = typeof(T);
            for (int i = 0; i < _wrappers.Count; i++)
            {
                if (_wrappers[i].Singleton.GetType() == type)
                    return _wrappers[i].Singleton as T;
            }

            UniLogger.Error($"Not found manager : {type}");
            return null;
        }

        /// <summary>
        /// 查询单例是否存在
        /// </summary>
        public static bool Contains<T>() where T : class, ISingleton
        {
            System.Type type = typeof(T);
            for (int i = 0; i < _wrappers.Count; i++)
            {
                if (_wrappers[i].Singleton.GetType() == type)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 创建单例
        /// </summary>
        /// <param name="priority">运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行</param>
        public static T CreateSingleton<T>(int priority = 0) where T : class, ISingleton
        {
            return CreateSingleton<T>(null, priority);
        }

        /// <summary>
        /// 创建单例
        /// </summary>
        /// <param name="createParam">附加参数</param>
        /// <param name="priority">运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行</param>
        public static T CreateSingleton<T>(System.Object createParam, int priority = 0) where T : class, ISingleton
        {
            if (priority < 0)
                throw new Exception("The priority can not be negative");

            if (Contains<T>())
                throw new Exception($"Module is already existed : {typeof(T)}");

            // 如果没有设置优先级
            if (priority == 0)
            {
                int minPriority = GetMinPriority();
                priority = --minPriority;
            }

            T module = Activator.CreateInstance<T>();
            Wrapper wrapper = new Wrapper(module,typeof(T), priority);

            wrapper?.onCreate(createParam);

            if (wrapper.OnUpdate != null)
            {
                _wrappers.Insert(0,wrapper);
                onUpdateCount++;
            }
            else
            {
                _wrappers.Add(wrapper);
            }

            _isDirty = true;
            return module;
        }

        /// <summary>
        /// 销毁单例
        /// </summary>
        public static bool DestroySingleton<T>() where T : class, ISingleton
        {
            var type = typeof(T);
            for (int i = 0; i < _wrappers.Count; i++)
            {
                if (_wrappers[i].Singleton.GetType() == type)
                {
                    _wrappers[i]?.onDestroy();

                    if (_wrappers[i].OnUpdate != null) onUpdateCount--;

                    _wrappers.RemoveAt(i);
                    _isDirty = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 开启一个协程
        /// </summary>
        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _behaviour.StartCoroutine(coroutine);
        }
        public static Coroutine StartCoroutine(string methodName)
        {
            return _behaviour.StartCoroutine(methodName);
        }

        /// <summary>
        /// 停止一个协程
        /// </summary>
        public static void StopCoroutine(Coroutine coroutine)
        {
            _behaviour.StopCoroutine(coroutine);
        }
        public static void StopCoroutine(string methodName)
        {
            _behaviour.StopCoroutine(methodName);
        }

        /// <summary>
        /// 停止所有协程
        /// </summary>
        public static void StopAllCoroutines()
        {
            _behaviour.StopAllCoroutines();
        }

        private static int GetMinPriority()
        {
            int minPriority = 0;
            for (int i = 0; i < _wrappers.Count; i++)
            {
                if (_wrappers[i].Priority < minPriority)
                    minPriority = _wrappers[i].Priority;
            }
            return minPriority; //小于等于零
        }
        private static void DestroyAll()
        {
            for (int i = 0; i < _wrappers.Count; i++)
            {
                _wrappers[i]?.onDestroy();
            }
            _wrappers.Clear();
            onUpdateCount = 0;
        }
    }
}