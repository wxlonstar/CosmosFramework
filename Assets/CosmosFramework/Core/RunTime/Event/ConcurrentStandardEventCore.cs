﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos
{
    /// <summary>
    /// 标准事件模型；
    /// 线程安全事件Core；
    /// 此类型的事件可以与EventManager并行使用；
    /// EventManager为全局事件，当前此类事件可以是区域事件；
    /// </summary>
    /// <typeparam name="TKey">key的类型</typeparam>
    /// <typeparam name="TValue">value的类型</typeparam>
    /// <typeparam name="TDerived">派生类的类型</typeparam>
    public class ConcurrentStandardEventCore<TKey, TValue, TDerived> : Singleton<TDerived>
                where TDerived : ConcurrentStandardEventCore<TKey, TValue, TDerived>, new()
        where TValue : class
    {
        protected ConcurrentDictionary<TKey, List<EventHandler<TValue>>> eventDict = new ConcurrentDictionary<TKey, List<EventHandler<TValue>>>();
        #region Sync
        public virtual void AddEventListener(TKey key, EventHandler<TValue> handler)
        {
            if (eventDict.ContainsKey(key))
                eventDict[key].Add(handler);
            else
            {
                List<EventHandler<TValue>> handlerSet = new List<EventHandler<TValue>>();
                handlerSet.Add(handler);
                eventDict.TryAdd(key, handlerSet);
            }
        }
        public virtual void RemoveEventListener(TKey key, EventHandler<TValue> handler)
        {
            if (eventDict.ContainsKey(key))
            {
                var handlerSet = eventDict[key];
                handlerSet.Remove(handler);
                if (handlerSet.Count <= 0)
                    eventDict.TryRemove(key, out _);
            }
        }
        public bool HasEventListened(TKey key)
        {
            return eventDict.ContainsKey(key);
        }
        public void Dispatch(TKey key, object sender, TValue value)
        {
            if (eventDict.ContainsKey(key))
            {
                var handlerSet = eventDict[key];
                int length = handlerSet.Count;
                for (int i = 0; i < length; i++)
                {
                    handlerSet[i]?.Invoke(sender, value);
                }
            }
        }
        public void Dispatch(TKey key, object sender)
        {
            Dispatch(key, sender, null);
        }
        #endregion
        #region Async
        public async virtual Task AddEventListenerAsync(TKey key, EventHandler<TValue> handler)
        {
            await Task.Run(() =>
            {
                if (eventDict.ContainsKey(key))
                    eventDict[key].Add(handler);
                else
                {
                    List<EventHandler<TValue>> handlerSet = new List<EventHandler<TValue>>();
                    handlerSet.Add(handler);
                    eventDict.TryAdd(key, handlerSet);
                }
            });
        }
        public async virtual Task RemoveEventListenerAsyncc(TKey key, EventHandler<TValue> handler)
        {
            await Task.Run(() =>
            {
                if (eventDict.ContainsKey(key))
                {
                    var handlerSet = eventDict[key];
                    handlerSet.Remove(handler);
                    if (handlerSet.Count <= 0)
                        eventDict.TryRemove(key, out _);
                }
            });
        }
        public async Task<bool> HasEventListenedAsync(TKey key)
        {
            return await Task.Run<bool>(() => { return eventDict.ContainsKey(key); });
        }
        public async Task DispatchAsync(TKey key, object sender, TValue value)
        {
            await Task.Run(() =>
            {
                if (eventDict.ContainsKey(key))
                {
                    var handlerSet = eventDict[key];
                    int length = handlerSet.Count;
                    for (int i = 0; i < length; i++)
                    {
                        handlerSet[i]?.Invoke(sender, value);
                    }
                }
            });
        }
        public async Task DispatchAsync(TKey key, object sender)
        {
            await DispatchAsync(key, sender, null);
        }
        #endregion
    }
}