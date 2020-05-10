﻿using System;
namespace Cosmos
{
    /// <summary>
    /// 通用继承形单例
    /// </summary>
    /// <typeparam name="T">继承自此单例的可构造类型</typeparam>
    public abstract class Singleton<T> : IDisposable
        where T : Singleton<T>, new()
    {
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.OnInitialization();
                }
                return instance;
            }
        }
        /// <summary>
        /// 非空虚方法，IDispose接口
        /// </summary>
        public virtual void Dispose() {instance.OnTermination() ; instance = default(T); }
        /// <summary>
        //空的虚方法，在当前单例对象为空初始化时执行一次
        /// </summary>
        protected virtual void OnInitialization() { }
        /// <summary>
        //空的虚方法，在当前单例对象被销毁时执行一次
        /// </summary>
        protected virtual void OnTermination() { }
    }

}
