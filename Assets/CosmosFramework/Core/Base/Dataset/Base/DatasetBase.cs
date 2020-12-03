﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cosmos
{
    /// <summary>
    /// 所有 ScriptableObject DatasetBase对象的基类
    /// </summary>
    public abstract class DatasetBase : ScriptableObject
    {
        /// <summary>
        /// 所有对象共有的名称
        /// </summary>
        [SerializeField]
        protected string objectName = "Newobject";
        public string ObjectName { get { return objectName; }set { objectName = value; } }
        /// <summary>
        /// 重置清空内容
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// 仅仅在Editor模式下使用
        /// </summary>
        public virtual void Preview() { }

        /// <summary>
        /// 执行对象中的函数
        /// </summary>
        public virtual void Execute() { }
    }
}