using ALPackage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIWndCommonPageTagContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoCommonPageTagItem, NPGGUIMonoCommonPageTagContainer, NPGGUIWndCommonPageTagItem>
    {
        private List<NPGGUIWndCommonPageTagItem> _m_lItemList;//子窗体列表
        private NPGGUIWndCommonPageTagItem _m_wSelectedItemWnd;//当前选中的item

        public NPGGUIWndCommonPageTagContainer(NPGGUIMonoCommonPageTagContainer _mono) : base(_mono)
        {
            initWnd();
        }


        protected override NPGGUIWndCommonPageTagItem _createItemWnd(NPGGUIMonoCommonPageTagItem _itemMono)
        {
            return new NPGGUIWndCommonPageTagItem(_itemMono);
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
            _m_wSelectedItemWnd = null;
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;

            _m_wSelectedItemWnd = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<NPGGUIWndCommonPageTagItem>();
        }


        /// <summary>
        /// 设置总数量
        /// </summary>
        /// <param name="_totalCount"></param>
        public void setTotalCount(int _totalCount)
        {
            NPGGUIWndCommonPageTagItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _totalCount; ++i)
            {
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

                //设置未选中
                tempItemWnd.setSelected(false);

                count++;
            }

            //移除多余项
            for (int i = _m_lItemList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemList[i - 1]);
                _m_lItemList.RemoveAt(i - 1);
            }

            //刷新布局
            _refreshContentLayout();
        }

        /// <summary>
        /// 选中
        /// </summary>
        /// <param name="_index"></param>
        public void setSelected(int _index)
        {
            //取消上个选中
            _m_wSelectedItemWnd?.setSelected(false);

            //设置选中
            if(_m_lItemList != null)
            {
                _m_wSelectedItemWnd = _m_lItemList.SafeGet(_index);
                _m_wSelectedItemWnd?.setSelected(true);
            }

            //刷新布局
            _refreshContentLayout();
        }

        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addLaterMonoTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;

                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }
    }
}
