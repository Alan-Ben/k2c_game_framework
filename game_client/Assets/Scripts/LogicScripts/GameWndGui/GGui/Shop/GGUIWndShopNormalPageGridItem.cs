using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    // 普通商店 容器item
    public class GGUIWndShopNormalPageGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoShopNormalPageGridItem>
    {
        //基础展示
        private NPGGUIWndCommonItem _m_commonItemWnd;

        //特殊图片
        private NPGGuiWndTexture _m_itemImgWnd;

        //品质底图
        private NPGGuiWndTexture _m_wQualityBg;

        //购买消耗wnd
        private NPGGUIWndCommonItem _m_costItemWnd;

        //商品数据
        private NPPlayerShopItem _m_shopItem;

        // 资源加载器
        private CommonAssetLoader _m_assetLoader;

        //打折资源加载器
        private CommonAssetLoader _m_offAssetLoader;

        /// <summary>
        /// 商品数据
        /// </summary>
        public NPPlayerShopItem shopItem { get { return _m_shopItem; } }

        public GGUIWndShopNormalPageGridItem(GGUIMonoShopNormalPageGridItem _wnd) : base(_wnd)
        {

        }

        protected override void _onDiscard()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.discard();
            _m_commonItemWnd = null;

            if (null != _m_costItemWnd)
                _m_costItemWnd.discard();
            _m_costItemWnd = null;

            if (null != _m_assetLoader)
                _m_assetLoader.discard();
            _m_assetLoader = null;

            if (null != _m_itemImgWnd)
                _m_itemImgWnd.discard();
            _m_itemImgWnd = null;

            if (null != _m_wQualityBg)
                _m_wQualityBg.discard();
            _m_wQualityBg = null;

            ALUGUICommon.uncombineBtnClick(wnd.buyBtn, _buyBtnDidClick);
        }

        protected override void _onHideWnd()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.hideWnd();

            if (null != _m_costItemWnd)
                _m_costItemWnd.hideWnd();

            if (null != _m_itemImgWnd)
                _m_itemImgWnd.hideWnd();

            if (null != _m_wQualityBg)
                _m_wQualityBg.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.resetWnd();

            if (null != _m_costItemWnd)
                _m_costItemWnd.resetWnd();

            if (null != _m_itemImgWnd)
                _m_itemImgWnd.discardTexture();

            if (null != _m_wQualityBg)
                _m_wQualityBg.discardTexture();
        }

        //重置Grid单个对象
        protected override void _resetGridItem()
        {
            if (null != _m_commonItemWnd)
                _m_commonItemWnd.resetWnd();

            if (null != _m_costItemWnd)
                _m_costItemWnd.resetWnd();

            if (null != _m_itemImgWnd)
                _m_itemImgWnd.discardTexture();

            if (null != _m_wQualityBg)
                _m_wQualityBg.discardTexture();
        }

        protected override void _onShowWnd()
        {
        }

        // 初始化
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            if (null != wnd.commonItemMono)
                _m_commonItemWnd = new NPGGUIWndCommonItem(wnd.commonItemMono);

            if (null != wnd.costItemMono)
                _m_costItemWnd = new NPGGUIWndCommonItem(wnd.costItemMono);

            if (null != wnd.itemImg)
                _m_itemImgWnd = new NPGGuiWndTexture(wnd.itemImg);

            if (null != wnd.qualityImg)
                _m_wQualityBg = new NPGGuiWndTexture(wnd.qualityImg);

            ALUGUICommon.combineBtnClick(wnd.buyBtn, _buyBtnDidClick);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_item"></param>
        public void setItem(NPPlayerShopItem _item,bool _isShowBk)
        {
            if (null == _item || wnd == null)
                return;

            _m_shopItem = _item;

            ALUGUICommon.setGameObjEnable(wnd.bkGo, _isShowBk);
            _refresh();

#if UNITY_EDITOR
            //设置editor下预制体展示的名称
            GCommon.setItemGameObjectName_EditorOnly(go,_item.shopItemRefId.ToString());
#endif
        }

        /// <summary>
        /// 模拟点击购买
        /// </summary>
        /// <param name="_objects"></param>
        public void simulateClickShopItem()
        {
            _buyBtnDidClick(null);
        }

        private void _refresh()
        {
            if (null == _m_shopItem || null == _m_shopItem.gainItem || null == wnd)
                return;

            if (null != _m_commonItemWnd)
                _m_commonItemWnd.setItem(_m_shopItem.gainItem);

            if (null != _m_itemImgWnd)
                _m_itemImgWnd.setTexture(GCommon.getItemTexIcon(_m_shopItem.gainItem.getItemType(), _m_shopItem.gainItem.subId));
            if (null != _m_costItemWnd)
                _m_costItemWnd.setItem(_m_shopItem.getCostItem());

            //特殊数量文本
            ALUGUICommon.setLabelTxt(wnd.numTxt, TextTranslate.instance.getLanguage(TransKeyConst.shop_num_title_count, _m_shopItem.gainItem.count));

            //商品品质框
            NPQualityExtRefObj extRef = GCommon.getQualityExtRefObj(_m_shopItem.gainItem.getItemType(), _m_shopItem.gainItem.subId);
            if (_m_wQualityBg != null && extRef != null)
            {
                _m_wQualityBg.showWnd();
                _m_wQualityBg.setTexture(extRef.shop_item_bg);
            }

            //设置限购
            int buyCountMax = _m_shopItem.buyNumMax;
            ALUGUICommon.setGameObjEnable(wnd.hasBuyLimitList, buyCountMax != 0);
            
            bool isSaleOut = false;
            //如果有限购
            if (buyCountMax != 0)
            {
                //剩余购买次数
                ALUGUICommon.setLabelTxt(wnd.buyLimitTxt, TextTranslate.instance.getLanguage(TransKeyConst.shop_buyLimitCout_num_num, _m_shopItem.lastBuyCount, buyCountMax));

                if (_m_shopItem.lastBuyCount <= 0)
                {
                    isSaleOut = true;
                }
            }

            //资源标识
            if(null != _m_assetLoader)
                _m_assetLoader.discard();

            if (null != wnd.flagPosParent && _m_shopItem.isRecommend)
            {
                if (null != _m_assetLoader)
                    _m_assetLoader.loadAsset(_m_shopItem.recommendResId, wnd.flagPosParent);
                else
                {
                    _m_assetLoader = new CommonAssetLoader();
                    _m_assetLoader.loadAsset(_m_shopItem.recommendResId, wnd.flagPosParent);
                }
            }

            bool isFree = _m_shopItem.getCostItem() == null;
            bool isDiscount = !isFree && _m_shopItem.discountUIResPathId != 0;
            EShopItemDiscountType discountType = EShopItemDiscountType.NO_DISCOUNT;

            if (isFree)
                discountType = EShopItemDiscountType.FREE;
            if (isDiscount)
                discountType = EShopItemDiscountType.DISCOUNT;

            //打折资源标识
            if (null != _m_offAssetLoader)
                _m_offAssetLoader.discard();
            if (null != wnd.offFlagPosParent && isDiscount)
            {
                if (null != _m_offAssetLoader)
                    _m_offAssetLoader.loadAsset(_m_shopItem.discountUIResPathId, wnd.offFlagPosParent);
                else
                {
                    _m_offAssetLoader = new CommonAssetLoader();
                    _m_offAssetLoader.loadAsset(_m_shopItem.discountUIResPathId, wnd.offFlagPosParent);
                }
            }

            NPCommonEnumStatInfo<EShopItemDiscountType>.setStat(wnd.discountStatList,discountType);

            //是否未解锁
            bool isLocked = null != _m_shopItem.shopItemGruopRef && !_m_shopItem.shopItemGruopRef.buy_condition.IsEnable(null);
            EShopItemShowType showType = EShopItemShowType.NORMAL;
            ALUGUICommon.setLabelTxt(wnd.txtBuyCondition, "");
            if (isLocked)
            {
                showType = EShopItemShowType.LOCKED;
                ALUGUICommon.setLabelTxt(wnd.txtBuyCondition, TextTranslate.instance.getLanguage(_m_shopItem.shopItemGruopRef.buy_condition_desc, _m_shopItem.shopItemGruopRef.buy_condition_desc_args));
            }
            else if (isSaleOut)
            {
                showType = EShopItemShowType.SALE_OUT;
            }
            NPCommonEnumStatInfo<EShopItemShowType>.setStat(wnd.showStatList,showType);
            
            for (int i = 0;i < wnd.needGrayList.Count; i ++)
            {
                ShopNormalPageGridItemGrayMono temp = wnd.needGrayList[i];
                if (null == temp)
                    continue;

                if (temp.type == showType)
                {
                    GGameCommonInfo.grayImage(temp.grayList);
                    GGameCommonInfo.disgrayImage(temp.disgrayList);
                    break;
                }
            }

            //设置原价
            int noDiscountCount = _m_shopItem.getBeforeDiscountCount();
            Text txtTemp = null;
            for (int i = 0; i < wnd.noDiscountTxtList.Count; i++)
            {
                txtTemp = wnd.noDiscountTxtList[i];
                if (null == txtTemp)
                    continue;

                ALUGUICommon.setLabelTxt(txtTemp, noDiscountCount);
            }

            //数量为1时隐藏
            if (_m_shopItem.gainItem.count == 1 && wnd.isOneLeftNeedHide)
                ALUGUICommon.setGameObjEnable(wnd.oneLeftHideGoList, false);
        }

        //点击购买按钮
        private void _buyBtnDidClick(GameObject _go)
        {
            if (null == _m_shopItem)
                return;
            
            bool isLocked = null != _m_shopItem.shopItemGruopRef && !_m_shopItem.shopItemGruopRef.buy_condition.IsEnable(null);
            if (isLocked)//未解锁，点击显示未解锁tip
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_m_shopItem.shopItemGruopRef.buy_condition_desc, _m_shopItem.shopItemGruopRef.buy_condition_desc_args));
                return;
            }

            GCommon.showCommonBuyItem(_m_shopItem.gainItem, _m_shopItem.getCostItem(), 
                _m_shopItem.lastBuyCount, _m_shopItem.buyNumMax, _m_shopItem.timePriceTypeId, 
                _m_shopItem.hasBuyNum + 1, _m_shopItem.discountRefId, 
                _m_shopItem.isRecommend ? _m_shopItem.recommendResId : 0, null, (count) =>
             {
                 NPPlayer.instance.shopComp.reqBuyShopItem(_m_shopItem.shop.shopRefId, _m_shopItem.instanceId, (int)count, () =>
                 {
                     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.shop_buy_succ));
                 });
             });
        }
    }
}
