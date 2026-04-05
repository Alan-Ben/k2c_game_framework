using ALPackage;
using Common.GuildEnum;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟建设列表Item
    /// </summary>
    public class GGUIWndGuildConstructContainerItem : _ATALBasicUISubWnd<GGUIMonoGuildConstructContainerItem>
    {
        //建设配置
        private GuildConstructRefObj _m_constructRef;
        //图标
        private NPGGuiWndTexture _m_wIcon;
        //品质框
        private GGuiWndSprite _m_wQualityIcon;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;

        public GGUIWndGuildConstructContainerItem(GGUIMonoGuildConstructContainerItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wQualityIcon?.hideWnd();
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wQualityIcon?.discardTexture();
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wQualityIcon?.discard();
            _m_wQualityIcon = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickConstruct);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgQuality != null)
                _m_wQualityIcon = new GGuiWndSprite(wnd.imgQuality);

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickConstruct);
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
            ALUGUICommon.setLabelTxt(wnd.txtExp, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_constructRef.add_guild_exp));
            ALUGUICommon.setLabelTxt(wnd.txtWealth, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_constructRef.add_guild_wealth));
            ALUGUICommon.setLabelTxt(wnd.txtCoin, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_constructRef.add_personal_guild_coin));

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

            //设置品质框
            if (_m_wQualityIcon != null)
            {
                if (_m_constructRef.quality != EQuality.NONE)
                {
                    NPGSpriteIndex qualityIcon = GCommon.getItemQualityIcon(
                        _m_constructRef.type == EGuildConstructType.ITEM ? ENPItemType.BAG_ITEM : ENPItemType.CURRENCY,
                        _m_constructRef.quality);

                    _m_wQualityIcon.showWnd();
                    _m_wQualityIcon.setTexture(qualityIcon);
                }
                else if (_m_constructRef.cost != null)
                {
                    _m_wQualityIcon.showWnd();
                    _m_wQualityIcon.setTexture(_m_constructRef.cost.getQualityIcon());
                }
            }
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
            ALUGUICommon.setGameObjEnable(wnd.goFreeShowList, isFree);
            ALUGUICommon.setGameObjEnable(wnd.goFreeHideList, !isFree);

            //剩余捐赠次数
            long leftCount = 0;
            if (_m_constructRef.fix_cd_id > 0)
                leftCount = NPPlayer.instance.fixedCdComp.getCount(_m_constructRef.fix_cd_id);
            else
                leftCount = 1;

            if (leftCount <= 0)
            {
                //没有次数
                ALUGUICommon.setGameObjEnable(wnd.goCurConstructShowList,NPPlayer.instance.guildComp.getIsConstructById(_m_constructRef.id));
                ALUGUICommon.setGameObjEnable(wnd.goDoneShowList,true);
                ALUGUICommon.setGameObjEnable(wnd.goDoneHideList,false);
            }
            else
            {
                //有次数
                ALUGUICommon.setGameObjEnable(wnd.goCurConstructShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goDoneShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goDoneHideList, true);
            }
        }

        //建设是否免费
        private bool _getIsConstructFree()
        {
            return _m_constructRef != null &&
                   _m_constructRef.free_condition != null &&
                   !_m_constructRef.free_condition.isEmpty &&
                   _m_constructRef.free_condition.IsEnable(null);
        }

        //点击建设按钮
        private void _onClickConstruct(GameObject _go)
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
                });
            }
        }
    }
}
