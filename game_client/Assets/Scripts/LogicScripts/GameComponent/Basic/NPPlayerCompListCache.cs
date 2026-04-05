using System;
using System.Collections.Generic;

using ALPackage;

namespace GOE
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public class NPPlayerCompListCache : _AALUnsafeThreadCacheController<List<_ANPBasicPlayerComponent>, List<_ANPBasicPlayerComponent>>
    {
        public NPPlayerCompListCache() : base(1, 20)
        {
            init(new List<_ANPBasicPlayerComponent>());
        }

        protected override List<_ANPBasicPlayerComponent> _createItem(List<_ANPBasicPlayerComponent> _template)
        {
            return new List<_ANPBasicPlayerComponent>();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "PlayerCompListCache"; } }

        protected override void _discardItem(List<_ANPBasicPlayerComponent> _item)
        {
            _item.Clear();
            return;
        }

        protected override void _onInit(List<_ANPBasicPlayerComponent> _template)
        {
        }

        protected override void _resetItem(List<_ANPBasicPlayerComponent> _item)
        {
            _item.Clear();
        }
    }
}
