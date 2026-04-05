using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴卡牌基础信息item
    /// </summary>
    public class GGUIWndHeroCommonCardItem : _ATALBasicUISubWnd<GGUIMonoHeroCommonCardItem>
    {
        //伙伴展示数据
        private _IHeroCardShow _m_heroCardShow;
        //图片
        private NPGGuiWndTexture _m_wIconBodyWnd;
        //背景
        private NPGGuiWndTexture _m_wIconBgWnd;
        //相性图标
        private NPGGuiWndTexture _m_wIconSpecAttrWnd;
        //名称背景
        private NPGGuiWndTexture _m_wNameBgWnd;
        //星级
        private GGUIWndHeroCommonStar _m_wHeroStar;
        //品质
        private GGUISubWndQualityShowGo _m_wQualityShowGo;
        //点击回调
        private Action<GGUIWndHeroCommonCardItem> _m_clickAction;
        //实力值
        private long _m_power;

        public GGUIWndHeroCommonCardItem(GGUIMonoHeroCommonCardItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        /// <summary>
        /// 卡牌信息
        /// </summary>
        public _IHeroCardShow heroCardShow { get { return _m_heroCardShow; } }
        public Action<GGUIWndHeroCommonCardItem> ClickAction { get { return _m_clickAction; } set { _m_clickAction = value; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (null != _m_wIconBodyWnd)
                _m_wIconBodyWnd.hideWnd();

            if (null != _m_wIconBgWnd)
                _m_wIconBgWnd.hideWnd();

            if (null != _m_wIconSpecAttrWnd)
                _m_wIconSpecAttrWnd.hideWnd();

            if (null != _m_wNameBgWnd)
                _m_wNameBgWnd.hideWnd();

            if (null != _m_wHeroStar)
                _m_wHeroStar.hideWnd();

            if (null != _m_wQualityShowGo)
                _m_wQualityShowGo.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_wIconBodyWnd)
                _m_wIconBodyWnd.discardTexture();
            
            if (null != _m_wIconBgWnd)
                _m_wIconBgWnd.discardTexture();
            
            if (null != _m_wIconSpecAttrWnd)
                _m_wIconSpecAttrWnd.discardTexture();
            
            if (null != _m_wNameBgWnd)
                _m_wNameBgWnd.discardTexture();
            
            if (null != _m_wHeroStar)
                _m_wHeroStar.resetWnd();

            if (null != _m_wQualityShowGo)
                _m_wQualityShowGo.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_wIconBodyWnd)
                _m_wIconBodyWnd.discard();
            _m_wIconBodyWnd = null;
            
            if(null != _m_wIconBgWnd)
                _m_wIconBgWnd.discard();
            _m_wIconBgWnd = null;
            
            if(null != _m_wIconSpecAttrWnd)
                _m_wIconSpecAttrWnd.discard();
            _m_wIconSpecAttrWnd = null;
            
            if(null != _m_wNameBgWnd)
                _m_wNameBgWnd.discard();
            _m_wNameBgWnd = null;
            
            if(null != _m_wHeroStar)
                _m_wHeroStar.discard();
            _m_wHeroStar = null;
            
            if(null != _m_wQualityShowGo)
                _m_wQualityShowGo.discard();
            _m_wQualityShowGo = null;

            _m_clickAction = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if(null != wnd.imgBg)
                _m_wIconBgWnd = new NPGGuiWndTexture(wnd.imgBg);
            
            if(null != wnd.texBody)
                _m_wIconBodyWnd = new NPGGuiWndTexture(wnd.texBody);

            if (null != wnd.texSpecAttr)
                _m_wIconSpecAttrWnd = new NPGGuiWndTexture(wnd.texSpecAttr);

            if (null != wnd.texNameBg)
                _m_wNameBgWnd = new NPGGuiWndTexture(wnd.texNameBg);

            if (null != wnd.monoStar)
                _m_wHeroStar = new GGUIWndHeroCommonStar(wnd.monoStar);

            if (null != wnd.monoQualityShowGo)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.monoQualityShowGo);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(_IHeroCardShow _heroInfo)
        {
            if(null == _heroInfo)
                return;

            _m_heroCardShow = _heroInfo;
            _m_power = _heroInfo.power;
            
            _refreshShow();
        }
        
        public void setPower(long _power)
        {
            _m_power = _power;

            _refreshShow();
        }


        //刷新显示
        private void _refreshShow()
        {
            if (null == wnd || null == _m_heroCardShow || null == _m_heroCardShow.heroRefObj)
                return;
            
            //背景
            if (null != _m_wIconBgWnd)
            {
                _m_wIconBgWnd.showWnd();
                _m_wIconBgWnd.setTexture(_m_heroCardShow.getCardBg());
            }
            
            //伙伴形象
            if (null != _m_wIconBodyWnd)
            {
                _m_wIconBodyWnd.showWnd();
                _m_wIconBodyWnd.setTexture(_m_heroCardShow.getCardImage());
            }

            //相性图标
            if (null != _m_wIconSpecAttrWnd)
            {
                BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _m_heroCardShow.heroRefObj.spec_attr_type);
                _m_wIconSpecAttrWnd.showWnd();
                _m_wIconSpecAttrWnd.setTexture(basicAttrRef?.icon);
            }

            //名称背景
            if (null != _m_wNameBgWnd)
            {
                NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroCardShow.id);
                _m_wNameBgWnd.showWnd();
                _m_wNameBgWnd.setTexture(qualityExtRef?.hero_name_bg);
            }

            //星级
            if (_m_heroCardShow.star < 1)
            {
                _m_wHeroStar?.hideWnd();
            }
            else
            {
                _m_wHeroStar?.showWnd();
                _m_wHeroStar?.setInfo(_m_heroCardShow.star);
            }

            //品质
            _m_wQualityShowGo?.showWnd();
            _m_wQualityShowGo?.setData(ENPItemType.HERO, _m_heroCardShow.heroRefObj.id);

            //名字
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.HERO, _m_heroCardShow.heroRefObj.id));
            ALUGUICommon.setLabelTxt(wnd.txtSkinName, GCommon.getItemName(ENPItemType.HERO_SKIN, _m_heroCardShow.heroRefObj.default_skin_id));
            
            //等级
            if(string.IsNullOrEmpty(wnd.lvlTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtLvl, _m_heroCardShow.level);
            else
                ALUGUICommon.setLabelTxt(wnd.txtLvl, TextTranslate.instance.getLanguage(wnd.lvlTransKey, _m_heroCardShow.level));

            //实力
            if (string.IsNullOrEmpty(wnd.powerTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtPower, _m_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            else
                ALUGUICommon.setLabelTxt(wnd.txtPower, TextTranslate.instance.getLanguage(wnd.powerTransKey, _m_power.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            //总资质
            if (string.IsNullOrEmpty(wnd.talentTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtTalent, _m_heroCardShow.getTotalTalent().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            else
                ALUGUICommon.setLabelTxt(wnd.txtTalent, TextTranslate.instance.getLanguage(wnd.talentTransKey, _m_heroCardShow.getTotalTalent().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            //是否解锁
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !_m_heroCardShow.isUnlock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, _m_heroCardShow.isUnlock);
            if(wnd.isSetBodyColor)
                ALUGUICommon.setUIObjColor(wnd.texBody, _m_heroCardShow.isUnlock ? wnd.unLockBodyColor : wnd.lockBodyColor);
        }

        //点击事件
        private void _onClick(GameObject _gameObject)
        {
            if (null != _m_clickAction)
                _m_clickAction(this);
        }
    }
}
