using System;
using System.Collections.Generic;

namespace ALPackage
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public class ALMsgActionListCache : _AALUnsafeThreadCacheController<List<Action>, List<Action>>
    {
        private static ALMsgActionListCache _g_instance = new ALMsgActionListCache();
        public static ALMsgActionListCache instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new ALMsgActionListCache();
                return _g_instance;
            }
        }

        public ALMsgActionListCache() : base(1, 20)
        {
            init(new List<Action>());
        }

        protected override List<Action> _createItem(List<Action> _template)
        {
            return new List<Action>();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "BroadcastActionLIstCache"; } }

        protected override void _discardItem(List<Action> _item)
        {
            _item.Clear();
            return;
        }

        protected override void _onInit(List<Action> _template)
        {
        }

        protected override void _resetItem(List<Action> _item)
        {
            _item.Clear();
        }
    }
}
