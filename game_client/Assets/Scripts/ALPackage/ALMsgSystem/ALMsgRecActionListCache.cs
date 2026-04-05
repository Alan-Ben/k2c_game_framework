using System;
using System.Collections.Generic;

namespace ALPackage
{
    /******************
     * 节点存储cache对象，避免创建过多对象
     **/
    public class ALMsgRecActionListCache : _AALUnsafeThreadCacheController<List<MsgRecAction>, List<MsgRecAction>>
    {
        private static ALMsgRecActionListCache _g_instance = new ALMsgRecActionListCache();
        public static ALMsgRecActionListCache instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new ALMsgRecActionListCache();
                return _g_instance;
            }
        }

        public ALMsgRecActionListCache() : base(1, 20)
        {
            init(new List<MsgRecAction>());
        }

        protected override List<MsgRecAction> _createItem(List<MsgRecAction> _template)
        {
            return new List<MsgRecAction>();
        }

        //警告信息文字
        protected override string _warningTxt { get { return "BroadcastCallbackListCache"; } }

        protected override void _discardItem(List<MsgRecAction> _item)
        {
            _item.Clear();
            return;
        }

        protected override void _onInit(List<MsgRecAction> _template)
        {
        }

        protected override void _resetItem(List<MsgRecAction> _item)
        {
            _item.Clear();
        }
    }
}