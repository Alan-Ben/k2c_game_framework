using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 文本item容器
    /// </summary>
    public class GGUIWndCommonTextContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoCommonTextContainerItem, GGUIMonoCommonTextContainer, GGUIWndCommonTextContainerItem>
    {
        //item列表
        protected List<GGUIWndCommonTextContainerItem> _m_lItemList;
        public GGUIWndCommonTextContainer(GGUIMonoCommonTextContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndCommonTextContainerItem _createItemWnd(GGUIMonoCommonTextContainerItem _itemMono)
        {
            GGUIWndCommonTextContainerItem item = new GGUIWndCommonTextContainerItem(_itemMono);
            return item;
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _m_lItemList[i]?.discard();
                }
                _m_lItemList.Clear();
                _m_lItemList = null;
            }
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndCommonTextContainerItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        public void setInfo(List<string> _list)
        {
            if (_list == null || _m_lItemList == null)
                return;

            GGUIWndCommonTextContainerItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _list.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];
                itemWnd.showWnd();
                itemWnd.setInfo(_list[i]);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }

    }
}
