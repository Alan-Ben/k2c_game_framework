using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 宴会赴宴消耗item
    /// </summary>
    public class GGUIWndDinnerCostItem : _ATALBasicUISubWnd<GGUIMonoDinnerCostItem>
    {
        private NPGGUIWndCommonItem _m_costItem;// 消耗
        private GDinnerJoinCostRefObj _m_costRef;//消耗配置
        private bool _m_isQuickJoin;//是否是快速赴宴

        public event Action<GDinnerJoinCostRefObj> onClickJoin;
        public event Action<GDinnerJoinCostRefObj> onClickSave;

        public GGUIWndDinnerCostItem(GGUIMonoDinnerCostItem _wnd) : base(_wnd)
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
            _m_costItem?.discard();
            _m_costItem = null;
            onClickJoin = null;
            onClickSave = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnJoin, _clickJoin);
            ALUGUICommon.combineBtnClick(wnd.btnSave, _clickSave);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _clickDetail);
            if (null != wnd.costItem)
            {
                _m_costItem = new NPGGUIWndCommonItem(wnd.costItem);
            }
        }

        /// <summary>
        /// 点击加入
        /// </summary>
        /// <param name="obj"></param>
        private void _clickJoin(GameObject obj)
        {
            if(!GCommon.isItemEnough(_m_costRef.cost_item,true))
                return;

            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_costRef.fixed_cd_id);
            if (null != fixedCdInfo && fixedCdInfo.getCount() ==0)//有限制且次数用完
            {
                //次数已经用完
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_cost_count_out_none));
                return;
            }
            
            onClickJoin?.Invoke(_m_costRef);
        }

        /// <summary>
        /// 点击保存按钮
        /// </summary>
        /// <param name="obj"></param>
        private void _clickSave(GameObject obj)
        {
            if(!GCommon.isItemEnough(_m_costRef.cost_item,true))
                return;
            
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_costRef.fixed_cd_id);
            if (null != fixedCdInfo && fixedCdInfo.getCount() ==0)//有限制且次数用完
            {
                //次数已经用完
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_cost_count_out_none));
                return;
            }
            onClickSave?.Invoke(_m_costRef);
        }

        /// <summary>
        /// 点击加成详情
        /// </summary>
        /// <param name="obj"></param>
        private void _clickDetail(GameObject obj)
        {
            if(wnd == null)
                return;
            DinnerJoinWayDetail joinWayDetail = new DinnerJoinWayDetail(_m_costRef);
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_DinnerJoinWay(joinWayDetail, 2910, wnd.toolTipsRoot, 0, 0));
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_costRef"></param>
        /// <param name="_isCanUse">是否可以使用</param>
        public void setInfo(GDinnerJoinCostRefObj _costRef, bool _isQuickJoin)
        {
            _m_isQuickJoin = _isQuickJoin;
            _m_costRef = _costRef;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            if(null == _m_costRef)
                return;
            
            _m_costItem?.showWnd();
            _m_costItem?.setItem(_m_costRef.cost_item);

            long costItemMyCount = GCommon.getItemCount(_m_costRef.cost_item.item);
            long costItemCount = _m_costRef.cost_item.count;
            bool isEnough = costItemMyCount >= costItemCount;
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_costRef.fixed_cd_id);
            int count = null != fixedCdInfo ? fixedCdInfo.getCount() : 0;
            int maxCount = null != fixedCdInfo ? fixedCdInfo.getMaxCount() : 0;

            if (fixedCdInfo != null)
                ALUGUICommon.setLabelTxt(wnd.txtFixedCd, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum2_num_num, count, maxCount));

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_costRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtJoinCoin, TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_coin ,_m_costRef.join_gain_coin));
            ALUGUICommon.setLabelTxt(wnd.txtJoinScore, TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_score ,_m_costRef.join_gain_score));
            ALUGUICommon.setLabelTxt(wnd.txtCostCount, TextTranslate.instance.getLanguage(TransKeyConst.dinner_join_cost_count , costItemMyCount.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT), costItemCount));

            ALUGUICommon.setGameObjEnable(wnd.goListHideCountNoLimit, maxCount > 0);

            bool hasJoinTime = (maxCount == 0) || count > 0;//是否还有加入次数，有限制次数或者无限制次数
            ALUGUICommon.setGameObjEnable(wnd.emptyListHide, hasJoinTime);
            ALUGUICommon.setGameObjEnable(wnd.emptyListShow, !hasJoinTime);
            
            ALUGUICommon.setGameObjEnable(wnd.quickJoinShow, _m_isQuickJoin);
            ALUGUICommon.setGameObjEnable(wnd.quickJoinHide, !_m_isQuickJoin);
            if (hasJoinTime && isEnough)
            {
                GGameCommonInfo.disgrayImage(wnd.goListGray);
            }
            else
            {
                GGameCommonInfo.grayImage(wnd.goListGray);
            }
        }
    }
}
