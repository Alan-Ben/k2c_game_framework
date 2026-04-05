using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 规则二级列表item容器
    /// </summary>
    public class NPGGUIWndRuleListSubItemContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoRuleListSubItem,NPGGUIMonoRuleListSubItemContainer,NPGGUIWndRuleListSubItem>
    {
        
        //窗口容器
        private List<NPGGUIWndRuleListSubItem> _m_lItemList;

        public NPGGUIWndRuleListSubItemContainer(NPGGUIMonoRuleListSubItemContainer _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<NPGGUIWndRuleListSubItem>();
            initWnd();
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
            if (_m_lItemList != null)
            {
                NPGGUIWndRuleListSubItem temp = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    temp = _m_lItemList[i];
                    if (temp == null)
                        continue;
                    temp.discard();
                }
                _m_lItemList.Clear();
            }
        }

        protected override void _onWndInitDone()
        {
            
        }

        protected override NPGGUIWndRuleListSubItem _createItemWnd(NPGGUIMonoRuleListSubItem _itemMono)
        {
            return new NPGGUIWndRuleListSubItem(_itemMono);
        }

        public void showItemList(List<NPRuleSubRefObj> _itemList)
        {
            NPRuleSubRefObj tempData = null;
            NPGGUIWndRuleListSubItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemList.Count; ++i)
            {
                tempData = _itemList[i];
                if (tempData == null || !tempData.show_cond.IsEnable(null))
                    continue;

                if (i >= _m_lItemList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemList[i];
                }

                tempItemWnd.setInfo(tempData);
                tempItemWnd.showWnd();
                count++;
            }

            for (int i = _m_lItemList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemList[i - 1]);
                _m_lItemList.RemoveAt(i - 1);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
        }
    }
}
