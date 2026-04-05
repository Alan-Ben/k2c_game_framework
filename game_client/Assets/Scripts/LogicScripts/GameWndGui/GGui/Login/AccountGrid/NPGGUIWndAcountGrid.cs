using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


namespace GOE
{
    /// <summary>
    /// 账号选择下拉窗口
    /// </summary>
    public class NPGGUIWndAcountGrid : _AALUGUIBasicGridSubWnd<NPGGUIMonoAcountGridItem, NPGGUIMonoAcountGrid, NPGGUIWndAcountGridItem>
    {
        /** 帐号数据列表 */
        public List<InternalAccountInfo> _m_lAcountList;

        public NPGGUIWndAcountGrid(NPGGUIMonoAcountGrid _containerWnd)
            : base(_containerWnd)
        {
            _m_lAcountList = null;
            initWnd();
        }

        protected override NPGGUIWndAcountGridItem _createItemWnd(NPGGUIMonoAcountGridItem _itemMono)
        {
            return new NPGGUIWndAcountGridItem(_itemMono);
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        protected override void _refreshItemwnd(NPGGUIWndAcountGridItem _itemMono, int _itemIdx)
        {
            if(_itemIdx >= _m_lAcountList.Count)
            {
                _itemMono.setAccount("--");
                return;
            }

            string acount = _m_lAcountList[_itemIdx].userName;
            _itemMono.setAccount(acount);
        }

        //开启下拉窗口
        public void setAccountList(List<InternalAccountInfo> _acountList)
        {
            _m_lAcountList = _acountList;

            //设置列表数量
            if(null == _m_lAcountList)
                setItemCount(0);
            else
                setItemCount(_m_lAcountList.Count);
        }

        /// <summary>
        /// 删除账号
        /// </summary>
        /// <param name="_account"></param>
        public void removeAccount(string _account)
        {
            for(int i = 0; i < _m_lAcountList.Count; i++)
            {
                if(_m_lAcountList[i].userName.Equals(_account))
                {
                    _m_lAcountList.RemoveAt(i);
                    break;
                }
            }

            //设置列表数量
            if(null == _m_lAcountList)
                setItemCount(0);
            else
                setItemCount(_m_lAcountList.Count);
        }
    }
}
