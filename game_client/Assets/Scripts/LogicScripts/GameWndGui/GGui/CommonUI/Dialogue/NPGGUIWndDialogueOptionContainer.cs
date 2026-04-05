using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 对话回应选项列表
    /// </summary>
    public class NPGGUIWndDialogueOptionContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoDialogueOptionItem, NPGGUIMonoDialogueOptionContainer, NPGGUIWndDialogueOptionItem>
    {
        private List<NPGGUIWndDialogueOptionItem> _m_lItemList;

        public NPGGUIWndDialogueOptionContainer(NPGGUIMonoDialogueOptionContainer _mono) : base(_mono)
        {
            initWnd();
        }

        public event Action<NPGGUIWndDialogueOptionItem> onSelectItem;

        protected override NPGGUIWndDialogueOptionItem _createItemWnd(NPGGUIMonoDialogueOptionItem _itemMono)
        {
            return new NPGGUIWndDialogueOptionItem(_itemMono);
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
            _m_lItemList = new List<NPGGUIWndDialogueOptionItem>();
        }

        public void showItemList(List<NPDialogueResponseOptionRefObj> _list)
        {
            if (_m_lItemList == null || _list == null)
                return;

            NPGGUIWndDialogueOptionItem itemWnd = null;
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
                itemWnd.setInfo(i, _onSelectItem, _list[i]);
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

        private void _onSelectItem(NPGGUIWndDialogueOptionItem _itemWnd)
        {
            onSelectItem?.Invoke(_itemWnd);
        }
    }
}
