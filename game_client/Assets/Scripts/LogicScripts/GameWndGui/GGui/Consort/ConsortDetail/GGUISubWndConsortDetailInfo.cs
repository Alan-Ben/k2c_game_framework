using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子详细信息子窗口
    /// </summary>
    public class GGUISubWndConsortDetailInfo : _ATALBasicUISubWnd<GGUISubMonoConsortDetailInfo>
    {
        private _IConsortShowInfo _m_consortShowInfo;//妃子展示信息
        private GConsortRefObj _m_rConsortRefObj;//妃子配表数据
        
        private NPGGuiWndTexture _m_wConsortRawImg;//妃子半身像
        private NPGGUIWndCommonShowCase _m_wndCommonShowCase;//展示窗口
        
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        
        public GGUISubWndConsortDetailInfo(GGUISubMonoConsortDetailInfo _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.consortRawImage != null)
                _m_wConsortRawImg = new NPGGuiWndTexture(wnd.consortRawImage);
            
            if (wnd.monoShowcase != null)
                _m_wndCommonShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowcase);
        }
        
        protected override void _onDiscard()
        {
            _m_wConsortRawImg?.discard();
            _m_wConsortRawImg = null;
            
            _m_wndCommonShowCase?.discard();
            _m_wndCommonShowCase = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _pushBackQualityGo();

            _m_wConsortRawImg?.hideWnd();
            _m_wndCommonShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortRawImg?.discardTexture();
            _m_wndCommonShowCase?.resetWnd();
        }

        public void setData(_IConsortShowInfo _consortShowInfo, bool _isPlayEnterAni = false)
        {
            _m_consortShowInfo = _consortShowInfo;
            _m_rConsortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_consortShowInfo?.consortId ?? 0);
            
            _refreshWnd(_isPlayEnterAni);
        }

        private void _refreshWnd(bool _isPlayEnterAni)
        {
            if(_m_consortShowInfo == null || wnd == null || _m_rConsortRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtConsortName, _m_consortShowInfo.consortTransName);
            if (wnd.txtConsortNameMesh)
                wnd.txtConsortNameMesh.text = _m_consortShowInfo.consortTransName;
            
            ALUGUICommon.setLabelTxt(wnd.txtConsortTitle, _m_consortShowInfo.consortTransTitle);
            ALUGUICommon.setLabelTxt(wnd.txtConsortBirthPlace, TextTranslate.instance.getLanguage(_m_rConsortRefObj.birthplace));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_rConsortRefObj.transDesc);
            
            long curSkinId = _m_consortShowInfo == null || _m_consortShowInfo.consortSkinShowInfo == null ? _m_rConsortRefObj.default_skin_id 
                : _m_consortShowInfo.consortSkinShowInfo.skinId;
            ALUGUICommon.setLabelTxt(wnd.txtConsortSkinName, GCommon.getItemName(ENPItemType.CONSORT_SKIN, curSkinId));

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }
            
            if (curSkinId == _m_rConsortRefObj.default_skin_id)//默认皮肤
            {
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinShow, true);
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinHide, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinShow, false);
                ALUGUICommon.setGameObjEnable(wnd.onWearingDefaultSkinHide, true);
            }

            if(_m_wConsortRawImg != null)
            {
                _m_wConsortRawImg.showWnd();
                _m_wConsortRawImg.setTexture(_m_consortShowInfo?.consortSkinShowInfo?.consortCardImage);
            }
            
            if (_m_wndCommonShowCase != null)
            {
                int maxShowCaseUnitCount = Mathf.Max(wnd.consortActorInShowCaseIndex, wnd.bgInShowCaseIndex) + 1;
                if (maxShowCaseUnitCount > 0)
                {
                    _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[maxShowCaseUnitCount];
                
                    if(wnd.consortActorInShowCaseIndex >= 0)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_consortShowInfo?.consortSkinShowInfo?.tdShow), wnd.consortActorInShowCaseIndex);
                
                    if(wnd.bgInShowCaseIndex >= 0)
                        showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_consortShowInfo?.consortSkinShowInfo?.tdBgIndex), wnd.bgInShowCaseIndex);
                
                    _m_wndCommonShowCase.showWnd(showCaseUnitInfoObjList);
                    _m_wndCommonShowCase.regInitDoneDelegate(() =>
                    {
                        if(_isPlayEnterAni)
                            _m_wndCommonShowCase.playAnim(wnd.consortActorInShowCaseIndex, wnd.enterAniName);
                    });
                }
                else
                {
                    _m_wndCommonShowCase.hideWnd();
                }
            }
            
            if(string.IsNullOrEmpty(wnd.charmTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtCharm, _m_consortShowInfo.charm);
            else 
                ALUGUICommon.setLabelTxt(wnd.txtCharm, TextTranslate.instance.getLanguage(wnd.charmTransKey, _m_consortShowInfo.charm));
        
            if(string.IsNullOrEmpty(wnd.intimacyTransKey))
                ALUGUICommon.setLabelTxt(wnd.txtIntimacy, _m_consortShowInfo.intimacy);
            else 
                ALUGUICommon.setLabelTxt(wnd.txtIntimacy, TextTranslate.instance.getLanguage(wnd.intimacyTransKey, _m_consortShowInfo.intimacy));
        
            ALUGUICommon.setLabelTxt(wnd.txtConsortSource, GCommon.getItemSource(ENPItemType.CONSORT, _m_consortShowInfo.consortId));
            
            EGameCommonUnlockType consortUnlockType = _m_consortShowInfo.unlockType;
            wnd.setUnlockState(consortUnlockType);
            
        }
        
        #region 品质图标GO

        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_consortShowInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.CONSORT, _m_consortShowInfo.consortId);
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

        #endregion
    }
}