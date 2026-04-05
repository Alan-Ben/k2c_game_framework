using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 本地推送设置列表
    /// </summary>
    public class GGUIWndGameSettingLocalPushContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoGameSettingLocalPushContainerItem, GGUIMonoGameSettingLocalPushContainer, GGUIWndGameSettingLocalPushContainerItem>
    {
        private List<GGUIWndGameSettingLocalPushContainerItem> _m_lItemList;//item列表

        public GGUIWndGameSettingLocalPushContainer(GGUIMonoGameSettingLocalPushContainer _mono) : base(_mono)
        {
            _m_lItemList = new List<GGUIWndGameSettingLocalPushContainerItem>();
            initWnd();
        }

        protected override GGUIWndGameSettingLocalPushContainerItem _createItemWnd(GGUIMonoGameSettingLocalPushContainerItem _itemMono)
        {
            return new GGUIWndGameSettingLocalPushContainerItem(_itemMono);
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if(_m_lItemList != null)
                _m_lItemList.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
        }


        public void showItemList()
        {
            if (wnd == null || _m_lItemList == null)
                return;

            List<LocalPushRefObj> localPushRefList = new List<LocalPushRefObj>();
            GRefdataCoreMgr.instance.localPushCore.dealAllRef(_refObj =>
            {
                if(_refObj != null && GCommon.isFuncUnlock(_refObj.func_type) && GCommon.isSimpleUnlock(_refObj.simple_unlock_id))
                    localPushRefList.Add(_refObj);
            });

            GGUIWndGameSettingLocalPushContainerItem itemWnd = null;
            int itemIdx = 0;
            //遍历玩家数据
            for (int i = 0; i < localPushRefList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (itemIdx >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[itemIdx];

                if (itemWnd != null)
                {
                    itemWnd.showWnd();
                    itemWnd.setInfo(localPushRefList[i]);
                }

                itemIdx++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= itemIdx; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }

        /// <summary>
        /// 刷新所有item开关状态
        /// </summary>
        public void refreshAllState()
        {
            if (_m_lItemList == null)
                return;

            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if(_m_lItemList[i] != null)
                    _m_lItemList[i].refreshWnd();
            }
        }
    }
}
