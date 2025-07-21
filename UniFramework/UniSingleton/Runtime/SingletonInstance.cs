
using System;

namespace Uni.Singleton
{
    public abstract class SingletonInstance<T> where T : class
    {
        private static T _instance;
        public static T Inst
        {
            get
            {
                if (_instance == null)
                    _instance = Activator.CreateInstance<T>();
                return _instance;
            }
        }

        protected SingletonInstance()
        {
            if (_instance != null)
                throw new System.Exception($"{typeof(T)} instance already created.");
            _instance = this as T;
        }
        protected void DestroyInstance()
        {
            _instance = null;
        }
    }
}
