using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 对话历史回顾选项列表
    /// </summary>
    public class GGUIWndDialogueHistoryOptionContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoDialogueHistoryOptionContainerItem, GGUIMonoDialogueHistoryOptionContainer, GGUIWndDialogueHistoryOptionContainerItem>
    {
        private List<GGUIWndDialogueHistoryOptionContainerItem> _m_lItemList;

        public GGUIWndDialogueHistoryOptionContainer(GGUIMonoDialogueHistoryOptionContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override GGUIWndDialogueHistoryOptionContainerItem _createItemWnd(GGUIMonoDialogueHistoryOptionContainerItem _itemMono)
        {
            return new GGUIWndDialogueHistoryOptionContainerItem(_itemMono);
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
            _m_lItemList = new List<GGUIWndDialogueHistoryOptionContainerItem>();
        }

        public void showItemList(List<NPDialogueResponseOptionRefObj> _list, int _selectIndex)
        {
            if (_m_lItemList == null || _list == null)
                return;

            GGUIWndDialogueHistoryOptionContainerItem itemWnd = null;
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
                itemWnd.setInfo(_list[i], i == _selectIndex);
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
