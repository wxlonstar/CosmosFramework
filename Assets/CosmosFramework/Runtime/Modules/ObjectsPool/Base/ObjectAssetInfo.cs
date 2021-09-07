﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos
{
    public class ObjectAssetInfo : AssetInfo
    {
        public TypeStringPair ObjectKey { get; private set; }
        public ObjectAssetInfo(string objectName,string assetPath) :base(assetPath)
        {
            this.ObjectKey = new TypeStringPair(typeof(object), objectName);
        }
        public ObjectAssetInfo(Type objectType, string objectName, string assetPath) : base(assetPath)
        {
            this.ObjectKey = new TypeStringPair(objectType, objectName);
        }
        public ObjectAssetInfo(string objectName, string assetBundleName, string assetPath)
            : base(assetBundleName, assetPath)
        {
            this.ObjectKey = new TypeStringPair(typeof(object), objectName);
        }
        public ObjectAssetInfo(Type objectType, string objectName, string assetBundleName, string assetPath)
             : base(assetBundleName, assetPath)
        {
            this.ObjectKey = new TypeStringPair(objectType, objectName);
        }
    }
}
