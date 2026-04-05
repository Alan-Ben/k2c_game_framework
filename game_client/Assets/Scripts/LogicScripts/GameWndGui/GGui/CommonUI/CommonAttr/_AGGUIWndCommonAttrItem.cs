using System;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 属性item
    /// </summary>
    public abstract class _AGGUIWndCommonAttrItem<T> : _ATALBasicUISubWnd<T> where T : GGUIMonoCommonAttrItem
    {
        private CommonAttrItemInfo _m_attrItemInfo; //属性信息
        private NPGGuiWndTexture _m_wIconWnd; //图片
        private bool _m_isShowLarge;//显示科学计数
        private string _m_sValueKey;//翻译key

        public _AGGUIWndCommonAttrItem(T _mono) : base(_mono)
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
            _m_wIconWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            // ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickBtnDetail);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            //图片
            if (wnd.imgAttrIcon != null)
            {
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgAttrIcon);
            }

            // ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickBtnDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_attrItemInfo"></param>
        public void setInfo(CommonAttrItemInfo _attrItemInfo,bool _isShowLarge = false, string _valueKey = null)
        {
            _m_attrItemInfo = _attrItemInfo;
            _m_isShowLarge = _isShowLarge;
            _m_sValueKey = _valueKey;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_attrItemInfo == null)
                return;

            // switch (_m_attrItemInfo.attrType)
            // {
            //     case EClientBasicAttrShowType.STR:
            //         _refreshDetail(EBasicAttrType.STR, false, false);
            //         break;
            //     case EClientBasicAttrShowType.INT:
            //         _refreshDetail(EBasicAttrType.INT, false, false);
            //         break;
            //     case EClientBasicAttrShowType.POL:
            //         _refreshDetail(EBasicAttrType.POL, false, false);
            //         break;
            //     case EClientBasicAttrShowType.LEAD:
            //         _refreshDetail(EBasicAttrType.LEAD, false, false);
            //         break;
            //     case EClientBasicAttrShowType.STR_PER:
            //         _refreshDetail(EBasicAttrType.STR, true, false);
            //         break;
            //     case EClientBasicAttrShowType.INT_PER:
            //         _refreshDetail(EBasicAttrType.INT, true, false);
            //         break;
            //     case EClientBasicAttrShowType.POL_PER:
            //         _refreshDetail(EBasicAttrType.POL, true, false);
            //         break;
            //     case EClientBasicAttrShowType.LEAD_PER:
            //         _refreshDetail(EBasicAttrType.LEAD, true, false);
            //         break;
            //     case EClientBasicAttrShowType.ALL:
            //         _refreshDetail(EBasicAttrType.NONE, false, true);
            //         break;
            //     case EClientBasicAttrShowType.ALL_PER:
            //         _refreshDetail(EBasicAttrType.NONE, true, true);
            //         break;
            // }
        }

        //刷新属性信息
        private void _refreshDetail(EBasicAttrType _showType, bool _isPercentage, bool _isAll)
        {
            // if (wnd == null || _m_attrItemInfo == null)
            //     return;

            NPGTextureIndex iconIndex = null;
            string targetValueStr = null;
            string attrName = null;

            // //是否是全属性
            // if (_isAll)
            // {
            //     iconIndex = GRefdataCoreMgr.instance.npGeneral.hero_all_attr_icon;
            //     attrName = TextTranslate.instance.getLanguage(TransKeyConst.hero_allAttr_none);
            // }
            // else
            // {
            //     BasicAttrRefObj attrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_showType);
            //     iconIndex = attrRefObj != null ? attrRefObj.icon : null;
            //     attrName = attrRefObj != null ? attrRefObj.name : null;
            // }
            //
            // //是否是百分比
            // if(_isPercentage)
            //     targetValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, _m_attrItemInfo.value * 1.0f / 100);
            // else
            // {
            //     targetValueStr = _m_attrItemInfo.value.ToString();
            //     if (_m_isShowLarge)
            //         targetValueStr = _m_attrItemInfo.value.ToLargeString();
            // }

            //属性图标
            _m_wIconWnd?.setTexture(iconIndex);

            //设置名字
            ALUGUICommon.setLabelTxt(wnd.txtAttrName, TextTranslate.instance.getLanguage(attrName));

            //属性数值
            if (!string.IsNullOrEmpty(_m_sValueKey))
                targetValueStr = TextTranslate.instance.getLanguage(_m_sValueKey, targetValueStr);
            ALUGUICommon.setLabelTxt(wnd.txtAttrValue, targetValueStr);
        }

        /// <summary>
        /// 点击查看属性详情
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnDetail(GameObject _go)
        {
            // if (_m_attrItemInfo == null)
            //     return;
            //
            // BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_m_attrItemInfo.attrType);
            // if (basicAttrRef == null)
            //     return;
            //
            // onClick(basicAttrRef);
        }

        protected virtual void onClick(BasicAttrRefObj _basicAttrRef)
        {
            // QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
            //     UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT,
            //     TextTranslate.instance.getLanguage(_basicAttrRef.name),
            //     TextTranslate.instance.getLanguage(_basicAttrRef.desc),
            //     rectTransform, 0, wnd.attrToolTipInterval));
        }
    }
}