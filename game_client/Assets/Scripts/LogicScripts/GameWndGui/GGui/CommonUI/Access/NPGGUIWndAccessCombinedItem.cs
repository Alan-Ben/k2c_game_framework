using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取途径中默认的item
    /// </summary>
    public class NPGGUIWndAccessCombinedItem : _ATALBasicUISubWnd<NPGGUIMonoAccessCombinedItem>
    {
        private NPAccessCombinedItemInfo _m_info;



        public NPGGUIWndAccessCombinedItem(NPGGUIMonoAccessCombinedItem _mono) : base(_mono)
        {
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
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickBtnGoTo);
        }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickBtnGoTo);
        }


        #region 点击事件

        /// <summary>
        /// 点击前往
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnGoTo(GameObject _go)
        {
            if (_m_info == null || _m_info.oriItemId == 0)
                return;

            //打开合成窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemBeCombined.instance, () =>
            {
                GGUIWndBagItemBeCombined.instance.showWnd();
                GGUIWndBagItemBeCombined.instance.init(_m_info.itemType, _m_info.itemId);
            }, UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
        }

        #endregion


        #region 窗体事件

    

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_accessInfo"></param>
        public void setInfo(NPAccessCombinedItemInfo _info)
        {
            _m_info = _info;
        }
        #endregion
    }
}
