using ALPackage;
using System;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获得伙伴弹窗
    /// </summary>
    public class GGUIWndHeroGet : _ANPGGUIBasicWnd<GGUIMonoHeroGet>
    {
        private static GGUIWndHeroGet _g_instance = new GGUIWndHeroGet();
        public static GGUIWndHeroGet instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndHeroGet();
                return _g_instance;
            }
        }

        //伙伴配置数据
        private HeroRefObj _m_heroRef;
        //关闭事件
        private Action _m_closeAction;
        //伙伴形象
        private NPGGUIWndCommonShowCase _m_heroShowCase;
        //相性图标
        private NPGGuiWndTexture _m_wSpecAttrIcon;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;

        public GGUIWndHeroGet() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroGet.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroGet.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            if (_m_heroShowCase != null)
                _m_heroShowCase.hideWnd();

            if(_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.hideWnd();

            _pushBackQualityGo();
            HeroVoiceMgr.instance.stopAllVoice();
        }

        protected override void _onReset()
        {
            if(_m_heroShowCase != null)
                _m_heroShowCase.resetWnd();

            if(_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (_m_heroShowCase != null)
                _m_heroShowCase.discard();
            _m_heroShowCase = null;

            if (_m_wSpecAttrIcon != null)
                _m_wSpecAttrIcon.discard();
            _m_wSpecAttrIcon = null;

            if (_m_closeAction != null)
            {
                Action onClose = _m_closeAction;
                _m_closeAction = null;
                onClose?.Invoke();
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.showCaseWnd != null)
                _m_heroShowCase = new NPGGUIWndCommonShowCase(wnd.showCaseWnd);

            if (wnd.imgSpecAttrIcon != null)
                _m_wSpecAttrIcon = new NPGGuiWndTexture(wnd.imgSpecAttrIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(HeroRefObj _heroRef, Action _closeAction)
        {
            if (_heroRef == null)
                return;

            _m_heroRef = _heroRef;
            _m_closeAction = _closeAction;
            
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_heroRef == null)
                return;

            //形象展示
            if (_m_heroShowCase != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[wnd.showcaseBgIndex + 1];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_heroRef.td_show), 0);
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_heroRef.td_bg_index), wnd.showcaseBgIndex);
                _m_heroShowCase.showWnd(showCaseUnitInfoObjList);
            }

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //相性图标
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)_m_heroRef.spec_attr_type);
            if (_m_wSpecAttrIcon != null)
            {
                _m_wSpecAttrIcon.showWnd();
                _m_wSpecAttrIcon.setTexture(basicAttrRef?.icon);
            }

            //相性名称
            ALUGUICommon.setLabelTxt(wnd.txtSpecAttrName, TextTranslate.instance.getLanguage(basicAttrRef?.name));

            //名称描述等
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_heroRef.transName);
            ALUGUICommon.setLabelTxt(wnd.txtTitle, GCommon.getItemName(ENPItemType.HERO_SKIN, _m_heroRef.default_skin_id));
            ALUGUICommon.setLabelTxt(wnd.txtBirthplace, TextTranslate.instance.getLanguage(_m_heroRef.birthplace));
            ALUGUICommon.setLabelTxt(wnd.txtOccupation, TextTranslate.instance.getLanguage(_m_heroRef.occupation_name));
            ALUGUICommon.setLabelTxt(wnd.txtIntroduction, TextTranslate.instance.getLanguage(_m_heroRef.introduction_desc));

            //播放音效
            HeroVoiceMgr.instance.playVoice(_m_heroRef.id, EHeroVoiceType.UNLOCK);
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_heroRef == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.HERO, _m_heroRef.id);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityIconParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityIconParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_GET);
        }
    }
}
