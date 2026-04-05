using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /******************
     * Transform的存储cache对象，避免创建过多对象
     **/
    public class TransformListCache : _AALUnsafeThreadCacheController<List<Transform>, List<Transform>>
    {
        private static TransformListCache _g_instance = new TransformListCache();
        public static TransformListCache instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new TransformListCache();
                return _g_instance;
            }
        }

        public TransformListCache() : base(1, 20)
        {
            init(new List<Transform>());
        }

        protected override List<Transform> _createItem(List<Transform> _template)
        {
            return new List<Transform>();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "TransformListCache"; } }

        protected override void _discardItem(List<Transform> _item)
        {
            if (_item != null) 
                _item.Clear();
        }

        protected override void _onInit(List<Transform> _template)
        {
        }

        protected override void _resetItem(List<Transform> _item)
        {
            if (_item != null) 
                _item.Clear();
        }
    }
}
