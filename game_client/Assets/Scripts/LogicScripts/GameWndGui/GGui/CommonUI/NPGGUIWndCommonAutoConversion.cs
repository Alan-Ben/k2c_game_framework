using System;
using ALPackage;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndCommonAutoConversion:_ANPGGUIBasicWnd<NPGGUIMonoCommonAutoConversion>
    {
        private static NPGGUIWndCommonAutoConversion _g_instance = new NPGGUIWndCommonAutoConversion();

        public static NPGGUIWndCommonAutoConversion instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new NPGGUIWndCommonAutoConversion();
                }
                return _g_instance;
            }
        }
        private string _m_conversionDesc;//转化描述
        private _IItem _m_targetItem;//转化后的item
        private _IItem _m_sourceItem;//源item

        private NPGGUIWndCommonItem _m_sourceItemWnd;
        private NPGGUIWndCommonItem _m_targetItemWnd;
        private CommonUISfxObj _m_sfxObj;
        private Action _m_actionClose;


        public NPGGUIWndCommonAutoConversion() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get => NPGGUIMonoCommonAutoConversion.assetPath; }
        protected override string _monoObjName { get => NPGGUIMonoCommonAutoConversion.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

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
            if (_m_actionClose != null)
            {
                Action onClose = _m_actionClose;
                _m_actionClose = null;
                onClose?.Invoke();
            }
            
            if (null != _m_sourceItemWnd)
            {
                _m_sourceItemWnd.discard();
                _m_sourceItemWnd = null;
            }
            if (null != _m_targetItemWnd)
            {
                _m_targetItemWnd.discard();
                _m_targetItemWnd = null;
            }

            if (null != _m_sfxObj)
            {
                _m_sfxObj.forceDiscard();
                _m_sfxObj = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (null != wnd.sourceItem)
            {
                _m_sourceItemWnd = new NPGGUIWndCommonItem(wnd.sourceItem);
            }
            
            if (null != wnd.targetItem)
            {
                _m_targetItemWnd = new NPGGUIWndCommonItem(wnd.targetItem);
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        public void showWnd(NPCommon_ItemInfo _itemInfo)
        {
            if(null == _itemInfo || _itemInfo.getItemType() != (int)ENPItemType.EXCHANGE_ITEM)
                return;
            ItemExchangeRefObj itemExchangeRefObj = GRefdataCoreMgr.instance.itemExchangeCore.getRef(_itemInfo.getSubId());
            if(null == itemExchangeRefObj)
                return;
            //从转换配表。获取转换的_sourceItem和_targetItem
            _m_sourceItem = new CommonItemData(itemExchangeRefObj.ori_item, _itemInfo.getCount());
            _m_targetItem = new CommonItemData(itemExchangeRefObj.target_item.item, _itemInfo.getCount() * itemExchangeRefObj.target_item.count);
            _m_conversionDesc = TextTranslate.instance.getLanguage(itemExchangeRefObj.desc, itemExchangeRefObj.desc_args);
            base.showWnd();
            _refreshWnd();
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="_sourceItem">转化前的源item</param>
        /// <param name="_targetItem">转化后的目标item</param>
        /// <param name="_conversionDesc">转化描述</param>
        public void showWnd(_IItem _sourceItem, _IItem _targetItem, string _conversionDesc, Action _actionClose)
        {
            _m_sourceItem = _sourceItem;
            _m_targetItem = _targetItem;
            _m_conversionDesc = _conversionDesc;
            _m_actionClose = _actionClose;
            base.showWnd();
            _refreshWnd();
        }

        /// <summary>
        /// 刷洗你窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (null != _m_sourceItemWnd)
            {
                _m_sourceItemWnd.showWnd(_m_sourceItem);
            }
            if (null != _m_targetItemWnd)
            {
                _m_targetItemWnd.showWnd(_m_targetItem);
            }

            ALUGUICommon.setLabelTxt(wnd.textDesc, _m_conversionDesc);

            _playSfx();
        }

        /// <summary>
        /// 播放转化特效
        /// </summary>
        private void _playSfx()
        {
            if(null == wnd)
                return;
            
            if(null != _m_sfxObj)
                _m_sfxObj.forceDiscard();
            
            _m_sfxObj = PlaySfxMgr.instance.playUISfx(wnd.sfxId, wnd.sfxParent);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ITEM_AUTO_CONVERSION);
        }
    }
}