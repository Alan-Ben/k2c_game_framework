using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用奖励预览窗口--双item列表界面
    /// </summary>
    public class NPGGUIWndCommonRewardPreviewTowList : NPGGUIWndCommonRewardPreview<NPGGUIMonoCommonRewardPreviewTowList>
    {
        private NPGGUIWndCommonItemContainer _m_itemSecondContainer;
        private List<_IItem> _m_itemSecondList;
        private string _m_secondDescStr;
        
        public NPGGUIWndCommonRewardPreviewTowList(long _uiPathId)
            : base(_uiPathId)
        {
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();
            _refreshTowListWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            if (null != _m_itemSecondContainer)
            {
                _m_itemSecondContainer.discard();
                _m_itemSecondContainer = null;
            }
            _m_itemSecondList?.Clear();
            _m_itemSecondList = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if(null == wnd)
                return;
            
            if (null != wnd.itemSecondContainer)
            {
                _m_itemSecondContainer = new NPGGUIWndCommonItemContainer(wnd.itemSecondContainer);
            }
            _m_itemSecondList = new List<_IItem>();
        }
        
        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_rewardSecondId"></param>
        public void setTowListData(long _rewardSecondId, string _secondDescStr)
        {
            List<NPCommonCostItem> itemList = GCommon.getShowListByReawrdId(_rewardSecondId);
            if (null != itemList)
            {
                setTowListData(itemList.toItemDataList(), _secondDescStr);
            }
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemSecondList"></param>
        public void setTowListData(List<NPCommonCostItem> _itemSecondList, string _secondDescStr)
        {
            if (null != _itemSecondList)
            {
                setTowListData(_itemSecondList.toItemDataList(), _secondDescStr);   
            }
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemSecondList"></param>
        /// <param name="_uiPathId"></param>
        public void setTowListData(List<_IItem> _itemSecondList, string _secondDescStr)
        {
            if(null == wnd)
                return;
            _m_itemSecondList.Clear();
            _m_itemSecondList.AddRange(_itemSecondList);
            _m_itemSecondList.Sort(_sortByQuality);
            _m_secondDescStr = _secondDescStr;
            _refreshTowListWnd();
        }
        
        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshTowListWnd()
        {
            if(null == wnd)
                return;
            ///刷新列表
            if (null != _m_itemSecondContainer)
            {
                _m_itemSecondContainer.showItemList(_m_itemSecondList);
            }

            ALUGUICommon.setLabelTxt(wnd.txtSecondDesc, TextTranslate.instance.getLanguage(_m_secondDescStr));
            
            ALUGUICommon.setGameObjEnable(wnd.towListShowHide, null != _m_itemSecondList && _m_itemSecondList.Count > 0);
        }


        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickClose(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}