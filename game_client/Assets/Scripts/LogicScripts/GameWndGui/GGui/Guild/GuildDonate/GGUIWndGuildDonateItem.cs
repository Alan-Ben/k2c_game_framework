using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠item
    /// </summary>
    public class GGUIWndGuildDonateItem : _ATALBasicUISubWnd<GGUIMonoGuildDonateItem>
    {
        //建设配置
        private GuildConstructRefObj _m_constructRef;
        
        private NPGGuiWndTexture _m_wIcon;//icon
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;//奖励列表
        private GGUIWndCommonFixedCd _m_wFixedCd;//fixedCd Mono
        private NPGGUIWndCommonItem _m_wCostItem;//消耗道具
        
        public GGUIWndGuildDonateItem(GGUIMonoGuildDonateItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.icon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.icon);

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);

            if (wnd.monoFixedCd != null)
                _m_wFixedCd = new GGUIWndCommonFixedCd(wnd.monoFixedCd);

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnShowFreeDonateDescToolTip, _onShowFreeDonateDescBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnDonate, _onDonateBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnShowFreeDonateDescToolTip, _onShowFreeDonateDescBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnDonate, _onDonateBtnClick);
            }
            
            _m_wIcon?.discard();
            _m_wIcon = null;
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
            
            _m_wFixedCd?.discard();
            _m_wFixedCd = null;
            
            _m_wCostItem?.discard();
            _m_wCostItem = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wRewardContainer?.hideWnd();
            _m_wFixedCd?.hideWnd();
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wRewardContainer?.resetWnd();
            _m_wFixedCd?.resetWnd();
            _m_wCostItem?.resetWnd();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(GuildConstructRefObj _refObj)
        {
            if (wnd == null)
                return;

            _m_constructRef = _refObj;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshConstructInfo();
            _refreshConstructBtn();
        }

        //刷新建设信息
        private void _refreshConstructInfo()
        {
            if (wnd == null || _m_constructRef == null)
                return;

            //设置基础信息
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_constructRef.name));

            //设置图标
            if (_m_wIcon != null)
            {
                if (_m_constructRef.icon.isValid())
                {
                    _m_wIcon.showWnd();
                    _m_wIcon.setTexture(_m_constructRef.icon);
                }
                else if(_m_constructRef.cost != null)
                {
                    _m_wIcon.showWnd();
                    _m_wIcon.setTexture(_m_constructRef.cost.getIcon());
                }
            }

            // 捐献奖励
            List<NPCommonCostItem> rewardList = new List<NPCommonCostItem>();
            // 个人联盟币
            if (_m_constructRef.add_personal_guild_coin > 0)
                rewardList.Add(new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.personal_guild_coin_common_item, _m_constructRef.add_personal_guild_coin));
            // 联盟财富
            if(_m_constructRef.add_guild_wealth > 0)
                rewardList.Add(new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_wealth_common_item, _m_constructRef.add_guild_wealth));
            // 联盟经验
            if(_m_constructRef.add_guild_exp > 0)
                rewardList.Add(new NPCommonCostItem(GRefdataCoreMgr.instance.npGeneral.guild_exp_common_item, _m_constructRef.add_guild_exp));
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(rewardList);
            }
            
            if(_m_constructRef.free_condition != null && !_m_constructRef.free_condition.isEmpty)
                ALUGUICommon.setGameObjEnable(wnd.hasFreeDonateConditionShow, true);
            else
                ALUGUICommon.setGameObjEnable(wnd.hasFreeDonateConditionShow, false);
        }

        //刷新建设按钮
        private void _refreshConstructBtn()
        {
            if (wnd == null || _m_constructRef == null)
                return;

            //刷新消耗
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(_m_constructRef.cost);
            }

            //是否免费
            bool isFree = _getIsConstructFree();
            ALUGUICommon.setGameObjEnable(wnd.freeDonateShowGoList, isFree);
            ALUGUICommon.setGameObjEnable(wnd.freeDonateHideGoList, !isFree);

            if (_m_wFixedCd != null)
            {
                _m_wFixedCd.showWnd();
                _m_wFixedCd.setFixedCdId(_m_constructRef.fix_cd_id);
            }
        }

        //建设是否免费
        private bool _getIsConstructFree()
        {
            return _m_constructRef != null && (_m_constructRef.cost == null || !_m_constructRef.cost.IsValid ||
                                               (_m_constructRef.free_condition != null &&
                                                !_m_constructRef.free_condition.isEmpty &&
                                                _m_constructRef.free_condition.IsEnable(null)));
        }

        #region 点击按钮

        /// <summary>
        /// 显示免费捐赠描述按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onShowFreeDonateDescBtnClick(GameObject _go)
        {
            // 若不存在可免费条件
            if(wnd == null || _m_constructRef == null || _m_constructRef.free_condition == null || _m_constructRef.free_condition.isEmpty)
                return;
            
            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(wnd.freeDonateDescToolTipResId),
                UIResPathAssistant.getObjName(wnd.freeDonateDescToolTipResId),
                TextTranslate.instance.getLanguage(_m_constructRef.free_condition_desc),
                rectTransform, wnd.freeDonateDescToolTipOffset.x, wnd.freeDonateDescToolTipOffset.y));
        }

        /// <summary>
        /// 捐赠按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onDonateBtnClick(GameObject _go)
        {
            if (_m_constructRef == null)
                return;

            long leftCount = 0;
            if (_m_constructRef.fix_cd_id > 0)
                leftCount = NPPlayer.instance.fixedCdComp.getCount(_m_constructRef.fix_cd_id);
            else
                leftCount = 1;

            //已捐赠没有次数
            if (leftCount <= 0)
                return;

            //免费或者道具充足
            if(_getIsConstructFree() || GCommon.isItemEnough(_m_constructRef.cost, true))
            {
                NPPlayer.instance.guildComp.reqGuildConstruct(_m_constructRef.id, () =>
                {
                    //捐献成功
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_constructSucceed_none);
                    //刷新消耗
                    _refreshConstructBtn();
                });
            }
        }
        
        #endregion
    }
}