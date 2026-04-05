using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子皮肤展示子窗口
    /// </summary>
    public class GGUISubWndConsortSkinShow : _ATALBasicUISubWnd<GGUISubMonoConsortSkinShow>
    {
        private _IConsortSkinShowInfo _m_skinShowInfo; // 皮肤展示信息
        private GConsortRefObj _m_rConsortRefObj; // 妃子配表数据
        
        private GGUIWndConsortQualityShow _m_wConsortQualityShow; // 妃子品质显示Mono
        private GGUIWndHeroCommonStar _m_wStar; // 星级窗口
        
        private NPGGuiWndTexture _m_wHeadIcon; // 头像纹理窗口
        private GGuiWndSprite _m_wHeadBg; // 头像背景精灵窗口
        private NPGGuiWndTexture _m_wCardRawImage; // 半身像纹理窗口
        private NPGGuiWndTexture _m_wCardBg; // 半身像背景纹理窗口
        private NPGGUIWndCommonShowCase _m_wShowCase; // 3D展示窗口
        
        public GGUISubWndConsortSkinShow(GGUISubMonoConsortSkinShow _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoConsortQualityShow != null)
                _m_wConsortQualityShow = new GGUIWndConsortQualityShow(wnd.monoConsortQualityShow);
            
            // 构建星级窗口
            if (wnd.monoStar != null)
                _m_wStar = new GGUIWndHeroCommonStar(wnd.monoStar);
            
            // 构建头像纹理窗口
            if (wnd.headIcon != null)
                _m_wHeadIcon = new NPGGuiWndTexture(wnd.headIcon);
            
            // 构建头像背景精灵窗口
            if (wnd.headBg != null)
                _m_wHeadBg = new GGuiWndSprite(wnd.headBg);
            
            // 构建半身像纹理窗口
            if (wnd.cardRawImage != null)
                _m_wCardRawImage = new NPGGuiWndTexture(wnd.cardRawImage);
            
            // 构建半身像背景纹理窗口
            if (wnd.cardBg != null)
                _m_wCardBg = new NPGGuiWndTexture(wnd.cardBg);
            
            // 构建3D展示窗口
            if (wnd.monoShowcase != null)
                _m_wShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowcase);
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
        
        protected override void _onHideWnd()
        {
            _m_wConsortQualityShow?.hideWnd();
            _m_wStar?.hideWnd();
            
            // 隐藏所有子窗口，不做数据重置
            if (_m_wHeadIcon != null)
                _m_wHeadIcon.hideWnd();
            
            if (_m_wHeadBg != null)
                _m_wHeadBg.hideWnd();
            
            if (_m_wCardRawImage != null)
                _m_wCardRawImage.hideWnd();
            
            if (_m_wCardBg != null)
                _m_wCardBg.hideWnd();
            
            if (_m_wShowCase != null)
                _m_wShowCase.hideWnd();
        }
        
        protected override void _onReset()
        {
            _m_wConsortQualityShow?.resetWnd();
            _m_wStar?.resetWnd();
            
            // 重置纹理
            if (_m_wHeadIcon != null)
                _m_wHeadIcon.discardTexture();
            
            if (_m_wCardRawImage != null)
                _m_wCardRawImage.discardTexture();
            
            if (_m_wCardBg != null)
                _m_wCardBg.discardTexture();
            
            if (_m_wShowCase != null)
                _m_wShowCase.resetWnd();
        }
        
        protected override void _onDiscard()
        {
            _m_wConsortQualityShow?.discard();
            _m_wConsortQualityShow = null;
            
            _m_wStar?.discard();
            _m_wStar = null;
            
            // 销毁头像纹理窗口
            if (_m_wHeadIcon != null)
            {
                _m_wHeadIcon.discard();
                _m_wHeadIcon = null;
            }
            
            // 销毁头像背景精灵窗口
            if (_m_wHeadBg != null)
            {
                _m_wHeadBg.discard();
                _m_wHeadBg = null;
            }
            
            // 销毁半身像纹理窗口
            if (_m_wCardRawImage != null)
            {
                _m_wCardRawImage.discard();
                _m_wCardRawImage = null;
            }
            
            // 销毁半身像背景纹理窗口
            if (_m_wCardBg != null)
            {
                _m_wCardBg.discard();
                _m_wCardBg = null;
            }
            
            // 销毁3D展示窗口
            if (_m_wShowCase != null)
            {
                _m_wShowCase.discard();
                _m_wShowCase = null;
            }
            
            // 清空皮肤展示信息引用
            _m_skinShowInfo = null;
            _m_rConsortRefObj = null;
        }
        
        /// <summary>
        /// 设置皮肤展示数据并刷新
        /// </summary>
        /// <param name="_skinShowInfo">皮肤展示信息</param>
        public void setData(_IConsortSkinShowInfo _skinShowInfo)
        {
            _m_skinShowInfo = _skinShowInfo;
            _m_rConsortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_skinShowInfo?.skinRefObj?.consort_id ?? 0);
            
            _refreshWnd();
        }

        
        /// <summary>
        /// 内部刷新逻辑
        /// </summary>
        private void _refreshWnd()
        {
            if (_m_skinShowInfo == null || wnd == null)
                return;
            
            // 刷新妃子基础信息
            _refreshConsortInfo();
            
            // 刷新皮肤名称
            ALUGUICommon.setLabelTxt(wnd.txtConsortSkinName, GCommon.getItemName(NPEnum.ENPItemType.CONSORT_SKIN, _m_skinShowInfo.skinId));
            ALUGUICommon.setLabelTxt(wnd.txtConsortSkinDesc, GCommon.getItemDesc(NPEnum.ENPItemType.CONSORT_SKIN, _m_skinShowInfo.skinId));
            
            NPQualityExtRefObj qualityExtRef = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long)GCommon.getItemQuality(NPEnum.ENPItemType.CONSORT_SKIN, _m_skinShowInfo.skinId));
            
            // 刷新星级（根据皮肤品质获取星级）
            if (_m_wStar != null)
            {
                _m_wStar.showWnd();
                _m_wStar.setInfo(qualityExtRef != null ? qualityExtRef.consort_skin_star : 0);
            }
            
            // 判断是否为默认皮肤，控制显示/隐藏列表
            bool isDefaultSkin = _m_rConsortRefObj != null && _m_rConsortRefObj.default_skin_id == _m_skinShowInfo.skinId;
            ALUGUICommon.setGameObjEnable(wnd.isDefaultSkinShow, isDefaultSkin);
            ALUGUICommon.setGameObjEnable(wnd.isDefaultSkinHide, !isDefaultSkin);
            
            // 刷新头像
            if (_m_wHeadIcon != null)
            {
                _m_wHeadIcon.showWnd();
                _m_wHeadIcon.setTexture(_m_skinShowInfo.skinIcon);
            }
            
            // 刷新头像背景
            if (_m_wHeadBg != null)
            {
                _m_wHeadBg.showWnd();
                _m_wHeadBg.setTexture(GCommon.getQualityExtRefObj(NPEnum.ENPItemType.CONSORT, _m_rConsortRefObj?.id ?? 0)?.consort_head_bg);
            }
            
            // 刷新半身像
            if (_m_wCardRawImage != null)
            {
                _m_wCardRawImage.showWnd();
                _m_wCardRawImage.setTexture(_m_skinShowInfo.skinCardImg);
            }
            
            // 刷新半身像背景
            if (_m_wCardBg != null)
            {
                _m_wCardBg.showWnd();
                _m_wCardBg.setTexture(GCommon.getQualityExtRefObj(NPEnum.ENPItemType.CONSORT, _m_rConsortRefObj?.id ?? 0)?.consort_card_bg);
            }
            
            // 刷新3D展示
            if (_m_wShowCase != null)
            {
                int maxShowCaseUnitCount = Mathf.Max(wnd.consortActorInShowCaseIndex, wnd.bgInShowCaseIndex) + 1;
                
                if (maxShowCaseUnitCount > 0)
                {
                    _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[maxShowCaseUnitCount];
                    
                    // 设置妃子形象
                    if (wnd.consortActorInShowCaseIndex >= 0 && _m_skinShowInfo.tdShow != null)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_skinShowInfo.tdShow), wnd.consortActorInShowCaseIndex);
                    
                    // 设置背景
                    if (wnd.bgInShowCaseIndex >= 0 && _m_skinShowInfo.tdBgIndex != null)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_skinShowInfo.tdBgIndex), wnd.bgInShowCaseIndex);
                    
                    _m_wShowCase.showWnd(showCaseUnitInfoObjList);
                    
                    if (!string.IsNullOrEmpty(wnd.enterAniName))
                    {
                        _m_wShowCase.regInitDoneDelegate(() =>
                        {
                            if (_m_wShowCase != null)
                                _m_wShowCase.playAnim(wnd.consortActorInShowCaseIndex, wnd.enterAniName);
                        });
                    }
                }
                else
                {
                    _m_wShowCase.hideWnd();
                }
            }
        }
        
        /// <summary>
        /// 刷新妃子基础信息
        /// </summary>
        private void _refreshConsortInfo()
        {
            if (_m_rConsortRefObj == null || wnd == null)
                return;
            
            // 刷新妃子名称
            string consortName = _m_rConsortRefObj.transName;
            ALUGUICommon.setLabelTxt(wnd.txtConsortName, consortName);
            if (wnd.txtConsortNameMesh != null)
                wnd.txtConsortNameMesh.text = consortName;
            
            // 刷新妃子称号  
            ALUGUICommon.setLabelTxt(wnd.txtConsortTitle, TextTranslate.instance.getLanguage(_m_rConsortRefObj.consort_title));

            if (_m_wConsortQualityShow != null)
            {
                _m_wConsortQualityShow.showWnd();
                _m_wConsortQualityShow.setQuality(GCommon.getItemQuality(ENPItemType.CONSORT, _m_rConsortRefObj.id));
            }
        }
    }
}