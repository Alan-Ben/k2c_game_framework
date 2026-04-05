using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 属性item容器
    /// </summary>
    public class GGUIWndConsortSkillAttrItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoConsortSkillAttrItem, GGUIMonoConsortSkillAttrItemContainer, GGUIWndConsortSkillAttrItem>
    {
        //窗口容器
        protected List<GGUIWndConsortSkillAttrItem> _m_lItemList;


        public GGUIWndConsortSkillAttrItemContainer(GGUIMonoConsortSkillAttrItemContainer _mono) : base(_mono)
        {
            initWnd();
        }


        protected override GGUIWndConsortSkillAttrItem _createItemWnd(GGUIMonoConsortSkillAttrItem _itemMono)
        {
            return new GGUIWndConsortSkillAttrItem(_itemMono);
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<GGUIWndConsortSkillAttrItem>();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_list"></param>
        public void showItemList(List<CommonAttrItemInfo> _list)
        {
            if (_list == null || _m_lItemList == null)
                return;

            GGUIWndConsortSkillAttrItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _list.Count; i++)
            {
                CommonAttrItemInfo item = _list[i];
                if (null == item)
                    continue;
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
                itemWnd.setInfo(item);
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

        public void setItemGray(bool _isGray)
        {
            if(null == wnd)
                return;
            if (null == _m_lItemList)
                return;
            foreach (GGUIWndConsortSkillAttrItem attrItem in _m_lItemList)
            {
                attrItem.setItemGray(_isGray);
            }
        }
    }
}
