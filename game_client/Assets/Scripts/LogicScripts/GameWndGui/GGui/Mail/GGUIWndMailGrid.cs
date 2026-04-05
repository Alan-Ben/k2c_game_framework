using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using System.Text;


namespace GOE
{
    public class GGUIWndMailGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoMailItem, GGUIMonoMailGrid, GGUIWndMailItem>
    {
        //邮件列表
        private List<GMailDataInfo> _m_mailList = new List<GMailDataInfo>();

        public GGUIWndMailGrid(GGUIMonoMailGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndMailItem _createItemWnd(GGUIMonoMailItem _itemMono)
        {
            GGUIWndMailItem item = new GGUIWndMailItem(_itemMono);
            return item;
        }

        protected override void _onDiscard()
        {
            NPGGUIMailItemCacheMgr.instance.discard();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_MAIL_ITEM, _simulateClickMailItem);
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_MAIL_ITEM, _simulateClickMailItem);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndMailItem _itemWnd, int _itemIdx)
        {
            if (null == _m_mailList || _m_mailList.Count <= _itemIdx)
                return;

            GMailDataInfo info = _m_mailList[_itemIdx];
            if (null == info)
                return;

            //设置信息
            _itemWnd.setMailItem(info);
            //显示窗口
            _itemWnd.showWnd();
        }

        /// <summary>
        /// 根据邮件Id获取对应的下标
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        private int _getIdx(long _id)
        {
            if (null == _m_mailList)
            {
                return -1;
            }

            for (int i = 0; i < _m_mailList.Count; i++)
            {
                if (_m_mailList[i].id == _id)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 根据邮件Id刷新item
        /// </summary>
        /// <param name="_mailUid"></param>
        public void refreshItemBy(long _mailUid)
        {
            int idx = _getIdx(_mailUid);
            if (idx < 0)
                return;

            //强制刷新指定mail item
            forceRefreshItem(idx);
        }

        /// <summary>
        /// 刷新列表显示
        /// </summary>
        /// <param name="_isShowLock"></param>
        public void refreshGridList(List<GMailDataInfo> _mailList)
        {
            _m_mailList = _mailList;

            GMailDataInfo.sortMailList(_m_mailList);
            //设置列表数量
            setItemCount(_m_mailList.Count);
            
            moveToTop();
        }
        
        private void _simulateClickMailItem(object[] _params)
        {
            if (_params == null || _params.Length < 1 || !(_params[0] is long mailItemIndex))
            {
                return;
            }
            
            if(_m_mailList == null)
                return;

            GMailDataInfo mailDataInfo = _m_mailList.SafeGet((int)mailItemIndex);
            if(mailDataInfo != null)
                mailDataInfo.showDetailWnd();
        }
    }
}