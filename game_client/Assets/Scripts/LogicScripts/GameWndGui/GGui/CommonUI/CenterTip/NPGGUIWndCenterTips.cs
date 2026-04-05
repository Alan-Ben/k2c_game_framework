using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


namespace GOE
{
    /// <summary>
    /// 上浮提示窗体
    /// </summary>
    public class NPGGUIWndCenterTips : _ANPGGUIBasicWnd<NPGGUIMonoCenterTips>
    {
        private static NPGGUIWndCenterTips _g_instance = new NPGGUIWndCenterTips();
        public static NPGGUIWndCenterTips instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new NPGGUIWndCenterTips();
                return _g_instance;
            }
        }

        private _INPTipDealerInterface _m_dCurSysTip;//当前展示的系统类型tip
        private Dictionary<ENPTipShowType, NPGGUICommonTipDealerMgr> _m_dTipMgrDic;//队列tip管理器字典


        protected NPGGUIWndCenterTips() : base(EALUIWndLayer.NOTICE)
        {

        }


        protected override string _monoAssetPath { get { return NPGGUIMonoCenterTips.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCenterTips.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            _m_dCurSysTip?.discard();
            _m_dCurSysTip = null;

            if (_m_dTipMgrDic != null)
            {
                foreach (NPGGUICommonTipDealerMgr item in _m_dTipMgrDic.Values)
                {
                    item?.clear();
                }
            }
        }

        protected override void _onReset()
        {
            _m_dCurSysTip?.discard();
            _m_dCurSysTip = null;

            if (_m_dTipMgrDic != null)
            {
                foreach (NPGGUICommonTipDealerMgr item in _m_dTipMgrDic.Values)
                {
                    item?.clear();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_dCurSysTip?.discard();
            _m_dCurSysTip = null;

            if (_m_dTipMgrDic != null)
            {
                foreach (NPGGUICommonTipDealerMgr item in _m_dTipMgrDic.Values)
                {
                    item?.clear();
                }
                _m_dTipMgrDic.Clear();
            }
            _m_dTipMgrDic = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //队列tip管理器
            _m_dTipMgrDic = new Dictionary<ENPTipShowType, NPGGUICommonTipDealerMgr>();
        }

        /// <summary>
        /// 显示默认上浮提示
        /// </summary>
        /// <param name="_info"></param>
        public void showTextInfo(string _info)
        {
            showWnd();
            showTextSysTip(new List<string>() { _info });
        }

        #region 系统提示类型tip，只存在一个，重复触发覆盖上一个

        /// <summary>
        /// 显示系统提示类型tip，直接覆盖上一个
        /// </summary>
        /// <param name="_tipData"></param>
        private void _showSysTip(_INPTipDealerInterface _tipData)
        {
            if (_tipData == null)
                return;

            //系统提示类型tip直接覆盖
            _m_dCurSysTip?.discard();
            _m_dCurSysTip = _tipData;
            _m_dCurSysTip?.showTip(0, wnd.parentGo, 1f, null, null);
        }

        /// <summary>
        /// 显示 文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_textList"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextSysTip(List<string> _textList, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null)
        {
            NPCenterTipsRefObj tipRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
            if (tipRef != null)
            {
                NPTextTipDealer data = new NPTextTipDealer(_textList, tipRef, _onPop);
                _showSysTip(data);
            }
        }

        /// <summary>
        /// 显示 图片+文本类型 系统提示，直接覆盖上一个
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextSysTip(NPGTextureIndex _icon, string _text, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null)
        {
            NPCenterTipsRefObj tipRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
            if (tipRef != null)
            {
                NPIconTextTipDealer data = new NPIconTextTipDealer(_icon, _text, tipRef, _onPop);
                _showSysTip(data);
            }
        }

        #endregion


        #region 队列类型tip，按顺序逐个显示

        /// <summary>
        /// 添加一个待显示的tip
        /// </summary>
        /// <param name="_tip"></param>
        /// <param name="_queueType"></param>
        private void _addTip(_INPTipDealerInterface _tip, ENPTipShowType _queueType = ENPTipShowType.CENTER)
        {
            if (wnd == null || _m_dTipMgrDic == null)
                return;

            NPGGUICommonTipDealerMgr tipMgr = null;
            if (_m_dTipMgrDic.TryGetValue(_queueType, out tipMgr))
            {
                tipMgr?.addTip(_tip);
            }
            else
            {
                NPGGUITipShowParam showParam = _getTipShowParam(_queueType);
                if (showParam != null)
                {
                    if(showParam.parentGo != null)
                        tipMgr = new NPGGUICommonTipDealerMgr(showParam.parentGo, showParam.tipsSpeedLimitRate, showParam.tipsSpeedLimitNum, showParam.tipsSpeedMaxNum);
                    else
                        tipMgr = new NPGGUICommonTipDealerMgr(wnd.parentGo, showParam.tipsSpeedLimitRate, showParam.tipsSpeedLimitNum, showParam.tipsSpeedMaxNum);
                }
                else
                    tipMgr = new NPGGUICommonTipDealerMgr(wnd.parentGo);
                tipMgr.addTip(_tip);
                _m_dTipMgrDic[_queueType] = tipMgr;
            }
        }

        /// <summary>
        /// 清除所有提示队列
        /// </summary>
        public void clearAllTip()
        {
            if (_m_dTipMgrDic != null)
            {
                foreach (NPGGUICommonTipDealerMgr item in _m_dTipMgrDic.Values)
                {
                    item?.clear();
                }
                _m_dTipMgrDic.Clear();
            }
        }

        /// <summary>
        /// 显示文本类型tip
        /// </summary>
        /// <param name="_textList"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showTextTip(List<string> _textList, long _tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID, Action<NPGGUIWndTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPCenterTipsRefObj tipRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
            if (tipRef != null)
            {
                NPTextTipDealer dealer = new NPTextTipDealer(_textList, tipRef, _onPop, _tag);
                _addTip(dealer, tipRef.show_type);
            }
        }

        /// <summary>
        /// 显示图片+文本类型tip
        /// </summary>
        /// <param name="_icon"></param>
        /// <param name="_text"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showIconTextTip(NPGTextureIndex _icon, List<string> _textList, long _tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID, Action<NPGGUIWndIconTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPCenterTipsRefObj tipRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
            if (tipRef != null)
            {
                NPIconTextTipDealer dealer = new NPIconTextTipDealer(_icon, _textList, tipRef, _onPop, _tag);
                _addTip(dealer, tipRef.show_type);
            }
        }

        /// <summary>
        /// 显示Item+Text+Text类型tip
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_textOne"></param>
        /// <param name="_textTow"></param>
        /// <param name="_tipId"></param>
        /// <param name="_onPop"></param>
        public void showItemTextTextTip(_IItem _item, string _textOne, string _textTow, long _tipId = NPConst.C_DEFAULT_ITEM_TEXT_TEXT_TIP_REF_ID, Action<NPGGUIWndItemTextTextTip> _onPop = null, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPCenterTipsRefObj tipRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
            if (tipRef != null)
            {
                NPItemTextTextTipDealer dealer = new NPItemTextTextTipDealer(_item, _textOne, _textTow, tipRef, _onPop, _tag);
                _addTip(dealer, tipRef.show_type);
            }
        }

        public void showEmptyTip(long _tipId, Action<NPGGUIWndEmptyTip> _onPop, ETipDealerTagType _tag = ETipDealerTagType.NONE)
        {
            NPCenterTipsRefObj tipRef = GRefdataCoreMgr.instance.tipMap.getRef(_tipId);
            if (tipRef != null)
            {
                NPEmptyTipDealer dealer = new NPEmptyTipDealer(tipRef, _onPop, _tag);
                _addTip(dealer, tipRef.show_type);
            }
        }
        
        #endregion

        /// <summary>
        /// 根据tag移除为展示的tip
        /// </summary>
        public void clearQueueTipByTag(ETipDealerTagType _tag)
        {
            if(null == _m_dTipMgrDic)
                return;
            
            foreach (NPGGUICommonTipDealerMgr commonTipDealerMgr in _m_dTipMgrDic.Values)
            {
                if(null == commonTipDealerMgr)
                    continue;
                
                commonTipDealerMgr.clearQueueTipByTag(_tag);
            }
        }
        
        /// <summary>
        /// 根据tag移除所有的tip
        /// </summary>
        public void clearAllTipByTag(ETipDealerTagType _tag)
        {
            if(null == _m_dTipMgrDic)
                return;
            
            foreach (NPGGUICommonTipDealerMgr commonTipDealerMgr in _m_dTipMgrDic.Values)
            {
                if(null == commonTipDealerMgr)
                    continue;
                
                commonTipDealerMgr.clearAllTipByTag(_tag);
            }
        }
        
        /// <summary>
        /// 获取显示配置
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        private NPGGUITipShowParam _getTipShowParam(ENPTipShowType _type)
        {
            if (wnd == null || wnd.tipShowParamList == null)
                return null;

            for (int i = 0; i < wnd.tipShowParamList.Count; i++)
            {
                NPGGUITipShowParam temp = wnd.tipShowParamList[i];
                if (temp == null)
                    continue;

                if (temp.showType == _type)
                    return temp;
            }

            return null;
        }
    }
}
