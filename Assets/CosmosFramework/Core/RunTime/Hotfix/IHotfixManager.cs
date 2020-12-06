﻿using Cosmos.Hotfix;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Cosmos
{
    public interface IHotfixManager
    {
        /// <summary>
        /// 热更新DLL
        /// </summary>
        TextAsset HotfixDll { get; }
        /// <summary>
        /// 热更新程序集
        /// </summary>
        Assembly HotfixAssembly { get; }
        /// <summary>
        /// 热更新环境
        /// </summary>
        object HotfixEnvironment { get; }
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        Dictionary<HotfixMethodType, Dictionary<string, MethodInfo>> FixedMethods { get; }
        /// <summary>
        /// 热修复后的方法
        /// </summary>
        Dictionary<HotfixMethodType, Dictionary<string, Delegate>> FixedDelegates { get; }
        Action FixMethod(Action action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Action<T> FixMethod<T>(Action<T> action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Action<T1, T2> FixMethod<T1, T2>(Action<T1, T2> action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Action<T1, T2, T3> FixMethod<T1, T2, T3>(Action<T1, T2, T3> action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Func<TResult> FixMethod<TResult>(Func<TResult> action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Func<T, TResult> FixMethod<T, TResult>(Func<T, TResult> action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Func<T1, T2, TResult> FixMethod<T1, T2, TResult>(Func<T1, T2, TResult> action);
        /// <summary>
        /// 热修复目标方法
        /// </summary>
        /// <param name="action">目标方法</param>
        /// <returns>修复后的方法</returns>
        Func<T1, T2, T3, TResult> FixMethod<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> action);
    }
}
