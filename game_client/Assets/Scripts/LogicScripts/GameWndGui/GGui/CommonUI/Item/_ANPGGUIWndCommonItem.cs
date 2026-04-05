using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 新的物品UI样式，根据物品数据动态加载UI内容，支持显示{已有数量}/{物品数量}
    /// </summary>
    public abstract class _ANPGGUIWndCommonItem<T> : _ATALBasicUISubWnd<T> where T : NPGGUIMonoCommonItem
    {
        private _IItem _m_cidItemData;//数据对象
        private long _m_lCustomCount;//外部传参的数量，用于替代{背包数量}作为{已有数量}

        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private GGuiWndSprite _m_wQualityWnd;//品质图片
        private CommonUISfxObj _m_sfxObj;//品质特效

        public _ANPGGUIWndCommonItem(T _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        public long itemCount { get { return null == _m_cidItemData ? 0 : _m_cidItemData.getCount(); } }
        public _IItem itemData { get { return _m_cidItemData; } }

        public event Func<string, string> getShowItemNumStrFunc;
        
        /// <summary>
        /// show 方法
        /// </summary>
        /// <param name="_item"></param>
        public void showWnd(_IItem _item)
        {
            base.showWnd();
            setItem(_item);
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discardTexture();

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discardTexture();
            
            if (null != _m_sfxObj)
                _m_sfxObj.forceDiscard();
            _m_sfxObj = null;
        }

        protected override void _onDiscard()
        {
            _m_cidItemData = null;

            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discard();
            _m_wTexIconWnd = null;

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discard();
            _m_wQualityWnd = null;
            
            if (null != _m_sfxObj)
            {
                _m_sfxObj.forceDiscard();
                _m_sfxObj = null;
            }

            getShowItemNumStrFunc = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnItem, _onClickBtnItem);
            ALUGUICommon.uncombineBtnLongPress(wnd.longPressBtn, _onLongPressBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texIcon != null)
                _m_wTexIconWnd = new NPGGuiWndTexture(wnd.texIcon);

            if (wnd.quality != null)
                _m_wQualityWnd = new GGuiWndSprite(wnd.quality);
            
            ALUGUICommon.combineBtnClick(wnd.btnItem, _onClickBtnItem);
            ALUGUICommon.combineBtnLongPress(wnd.longPressBtn, _onLongPressBtn);

        }

        #region 窗体方法

        /// <summary>
        /// 播放品质动画
        /// </summary>
        /// <param name="_quality"></param>
        public void playQualityAnim(EQuality _quality)
        {
            if (wnd == null)
                return;

            playAnim(wnd.getQualityAnimName(_quality));
        }

        /// <summary>
        /// 播放窗体动画
        /// </summary>
        /// <param name="_animName"></param>
        public void playAnim(string _animName)
        {
            if (wnd == null || wnd.anim == null || string.IsNullOrEmpty(_animName))
                return;

            wnd.anim.Play(_animName, PlayMode.StopAll);
        }

        /// <summary>
        /// 刷新拥有的数量
        /// </summary>
        public void refreshHasCount()
        {
            _refreshCostItemNum();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            if (_m_cidItemData == null)
                return;

            _setName(_m_cidItemData.getItemName());
            _setDesc(_m_cidItemData.getItemDesc());
            _setIcon(_m_cidItemData.getIcon());
            _setQualityIcon(_m_cidItemData.getQualityIcon());
            //设置品质特效
            _setQualitySfx();
            _refreshCanBeCombined();
            _refreshCostItemNum();
            _refreshExpireShow();
#if UNITY_EDITOR
            //设置editor下预制体展示的名称
            GCommon.setItemGameObjectName_EditorOnly(go, $"{_m_cidItemData.getItemType().ToString()}_{_m_cidItemData.subId}");
#endif
        }

        /// <summary>
        /// 刷新显示可合成GoList
        /// </summary>
        private void _refreshCanBeCombined()
        {
            if (wnd == null)
                return;

            if (!wnd.isShowCanBeCombined)
                ALUGUICommon.setGameObjEnable(wnd.canBeCombinedShowGoList, false);
            else
            {
                bool isShow = GCommon.isItemCanOnceBeCombined(_m_cidItemData.getItemType(),_m_cidItemData.subId,_m_cidItemData.getCount());
                ALUGUICommon.setGameObjEnable(wnd.canBeCombinedShowGoList, isShow);
            }

        }

        /// <summary>
        /// 刷新消耗物品的数量显示
        /// </summary>
        private void _refreshCostItemNum()
        {
            if (wnd == null)
                return;

            if (_m_cidItemData == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.txtNum, true);
            ALUGUICommon.setGameObjEnable(wnd.txtNumImgGo, true);

            //物品信息数量
            long count = _m_cidItemData.getCount();

            switch (_m_cidItemData.getItemType())
            {
                //lazy_cd显示的是上限
                case ENPItemType.LAZY_CD:
                    //如果要是否显示已拥有的数量，cd的样式特殊处理（{当前值}/{最大值}）
                    if (wnd.showHasItemNum)
                    {
                        PlayerLazyCDInfo info = NPPlayer.instance.lazyCdComp.getLazyCDInfo(_m_cidItemData.subId);
                        count = info.MaxCount;
                    }
                    break;
                case ENPItemType.ITEM_DEF:
                    ItemDefRefObj itemDefRefObj = GRefdataCoreMgr.instance.itemDefCore.getRef(_m_cidItemData.subId);
                    if (null != itemDefRefObj)
                    {
                        count = count * itemDefRefObj.getCount();
                    }
                    break;
            }

            //数量为0时是否需要隐藏数量文本，隐藏文本就不用赋值显示了，直接return
            if (wnd.zeroHideTxtNum && count == 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.txtNum, false);
                ALUGUICommon.setGameObjEnable(wnd.txtNumImgGo, false);
                return;
            }

            //已有数量，默认取背包数量，有外部传参取外部传参
            long hasNum = _m_lCustomCount > 0 ? _m_lCustomCount : GCommon.getItemCount(_m_cidItemData.getItemType(), _m_cidItemData.subId);

            //已有数量文本
            string strHasNum = hasNum.ToString();
            //物品数量文本
            string strItemCount = count.ToString();

            //是否显示大数字
            if (wnd.showLargeNum)
            {
                strHasNum = hasNum.ToLargeString(_m_cidItemData.getLargeStringType());
                strItemCount = count.ToLargeString(_m_cidItemData.getLargeStringType());
            }

            //最终数量文本
            string strShowNum = string.Empty;

            //显示有效期
            if (_m_cidItemData.getItemType() == ENPItemType.TITLE
                ||_m_cidItemData.getItemType() == ENPItemType.ICON
                ||_m_cidItemData.getItemType() == ENPItemType.ICON_BGK
                ||_m_cidItemData.getItemType() == ENPItemType.BUBBLE
                ||_m_cidItemData.getItemType() == ENPItemType.CHAT_EMOTE_GROUP
                )
            {
                //如果有效期为0，说明是永久，不需要展示有效期时间
                if (count == 0)
                {
                    strHasNum = TimeUtil.millisecondsToTime_Max(hasNum * 1000);
                    strItemCount = TextTranslate.instance.getLanguage(TransKeyConst.time_forever);//永久
                }
                else
                {
                    strHasNum = TimeUtil.millisecondsToTime_Max(hasNum*1000);
                    strItemCount = TimeUtil.millisecondsToTime_Max(count*1000);
                }
            }

            //是否需要显示已拥有数量，显示规则{已有数量}/{物品数量}
            if (wnd.showHasItemNum)
            {
                //同时勾选时，只改变{已有数量}的文本颜色
                if (wnd.useNotEnoughTxtColor && hasNum < count)
                {
                    strHasNum = GCommon.addColorForRichText(strHasNum, wnd.notEnoughTxtColor);
                }
                strShowNum = $"{strHasNum}/{strItemCount}";
            }
            else
            {
                strShowNum = strItemCount;
                //数量不足改变文本颜色
                if (wnd.useNotEnoughTxtColor && hasNum < count)
                {
                    strShowNum = GCommon.addColorForRichText(strShowNum, wnd.notEnoughTxtColor);
                }
            }

            if (getShowItemNumStrFunc != null)
            {
                strShowNum = getShowItemNumStrFunc.Invoke(strShowNum);
            }
            
            if (string.IsNullOrEmpty(wnd.txtNumKey))
                ALUGUICommon.setLabelTxt(wnd.txtNum, strShowNum);
            else
                ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(wnd.txtNumKey, strShowNum));

        }

        /// <summary>
        /// 刷新过期展示
        /// </summary>
        private void _refreshExpireShow()
        {
            if(wnd == null || _m_cidItemData == null)
                return;

            bool isShowExpire = false;
            if (_m_cidItemData.getItemType() == ENPItemType.BAG_ITEM)
            {
                BagItemRefObj bagItemRefObj = GRefdataCoreMgr.instance.bagItemCore.getRef(_m_cidItemData.subId);
                isShowExpire = bagItemRefObj != null && bagItemRefObj.is_expire_item;
            }

            ALUGUICommon.setGameObjEnable(wnd.goExpireShowList, isShowExpire);
        }

        /// <summary>
        /// 设置物品名字文本
        /// </summary>
        /// <param name="_name"></param>
        private void _setName(string _name)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _name);
        }

        /// <summary>
        /// 设置物品描述
        /// </summary>
        /// <param name="_desc"></param>
        private void _setDesc(string _desc)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.itemDescTxt, _desc);
        }
        /// <summary>
        /// 设置物品图片
        /// </summary>
        /// <param name="_ovTexIdx"></param>
        private void _setIcon(NPGTextureIndex _ovTexIdx)
        {
            //判断该物品 icon 配置的是 RawImage 还是 Image
            if (_m_wTexIconWnd != null && _ovTexIdx != null && _ovTexIdx.enable())
            {
                _m_wTexIconWnd.setTexture(_ovTexIdx);
                _m_wTexIconWnd.showWnd();
            }
        }

        /// <summary>
        /// 设置品质图片
        /// </summary>
        /// <param name="_qualityIdx"></param>
        private void _setQualityIcon(NPGSpriteIndex _qualityIdx)
        {
            if (_m_wQualityWnd != null && _qualityIdx != null && _qualityIdx.enable())
            {
                _m_wQualityWnd.setTexture(_qualityIdx);
                _m_wQualityWnd.showWnd();
            }
        }

        //设置品质特效
        private void _setQualitySfx()
        {
            if(null == wnd || null == _m_cidItemData || null == wnd.qualitySfxParent)
                return;
            
            //不需要特效不处理
            if(!wnd.needQualitySfx)
                return;

            if (null != _m_sfxObj)
            {
                _m_sfxObj.forceDiscard();
                _m_sfxObj = null;
            }

            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) _m_cidItemData.getQuality());
            if(null == qualityExtRefObj)
                return;
            
            if(qualityExtRefObj.item_sfx_id > 0)
                _m_sfxObj = PlaySfxMgr.instance.playUISfx(qualityExtRefObj.item_sfx_id, wnd.qualitySfxParent);
        }

        #endregion


        #region 设置数据

        /// <summary>
        /// 设置物品信息
        /// </summary>
        /// <param name="_itemData"></param>
        /// <param name="_customCount">已有数量替换值</param>
        public void setItem(NPCommonCostItem _itemData, long _customCount = -1)
        {
            if (_itemData == null || _itemData.getItemType() == ENPItemType.NONE)
                return;

            setItem(_itemData.toCommonItemData(), _customCount);
        }

        /// <summary>
        /// 设置物品信息
        /// </summary>
        /// <param name="_itemData"></param>
        /// <param name="_customCount">已有数量替换值</param>
        public void setItem(_IItem _itemData, long _customCount = -1)
        {
            if (_itemData == null || _itemData.getItemType() == ENPItemType.NONE)
                return;

            _m_cidItemData = _itemData;
            _m_lCustomCount = _customCount;

            //刷新显示
            _refreshWnd();
        }

        /// <summary>
        /// 设置品质背景图
        /// </summary>
        /// <param name="_index"></param>
        public void setQualityIcon(NPGSpriteIndex _index)
        {
            _setQualityIcon(_index);
        }

        #endregion

        /// <summary>
        /// 展示详情
        /// </summary>
        public void showItemDetail(GameObject _go)
        {
            if (null == _m_cidItemData || null == wnd)
                return;

            if (wnd.isDetailJumpTo)
            {
                switch (_m_cidItemData.getItemType())
                {
                    case ENPItemType.HERO:
                        HeroCardShowInfo showInfo = new HeroCardShowInfo(
                            NPPlayer.instance.heroComponent.getHeroInfo(_m_cidItemData.subId),
                            GRefdataCoreMgr.instance.heroRefCore.getRef(_m_cidItemData.subId));
                        if (showInfo.heroInfo != null)
                            QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null));
                        else
                            QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
                        return;
                    case ENPItemType.CONSORT:
                        GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_cidItemData.subId);
                        if (consortInfo != null)
                            GNodeUnLockConsortDetail.addConsortNode(NPPlayer.instance.consortComp.getConsortList(), consortInfo.consortId);
                        else
                            QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_cidItemData.subId)));
                        return;
                }
            }

            //点击展示物品详情浮窗
            GCommon.clickShowItemToolTip(_m_cidItemData, wnd.detailInterval, _go.GetComponent<RectTransform>());
        }

        #region 点击事件

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnItem(GameObject _go)
        {
            if (wnd == null)
                return;

            //显示物品详情弹窗
            if (wnd.isClickShowDetail)
            {
                showItemDetail(_go);
            }
        }

        //长按按钮
        private void _onLongPressBtn()
        {
            if (wnd == null)
                return;

            //显示物品详情弹窗
            if (wnd.isLongPressShowDetail)
            {
                showItemDetail(wnd.longPressBtn.gameObject);
            }
        }
        
        #endregion
    }
}
