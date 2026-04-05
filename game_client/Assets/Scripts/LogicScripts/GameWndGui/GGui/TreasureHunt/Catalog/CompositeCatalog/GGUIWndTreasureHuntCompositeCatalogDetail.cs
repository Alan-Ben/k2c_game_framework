using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 组合图鉴详情窗口
    /// </summary>
    public class GGUIWndTreasureHuntCompositeCatalogDetail : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntCompositeCatalogDetail>
    {
        private static GGUIWndTreasureHuntCompositeCatalogDetail _g_instance;
        public static GGUIWndTreasureHuntCompositeCatalogDetail instance { get { return _g_instance ??= new GGUIWndTreasureHuntCompositeCatalogDetail(); } }

        private List<TreasureHuntCompositeCatalogInfo> _m_lAllCompositeCatalogList;//所有组合图鉴列表
        private int _m_iCurShowCatalogIndex = -1;//当前显示的组合图鉴索引
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        
        private GGUISubWndQualityShowGo _m_wQualityShowGo;
        private GGUIWndTreasureHuntOreItemContainer _m_wOreItemContainer;
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wNormalSkillInfo;
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wAdvancedSkillInfo;

        public GGUIWndTreasureHuntCompositeCatalogDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCompositeCatalogDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCompositeCatalogDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.qualityShowGoMono != null)
                _m_wQualityShowGo = new GGUISubWndQualityShowGo(wnd.qualityShowGoMono);

            if (wnd.monoOreItemContainer != null)
            {
                _m_wOreItemContainer = new GGUIWndTreasureHuntOreItemContainer(wnd.monoOreItemContainer);
                _m_wOreItemContainer.onOreClick += _onOreClick;
            }

            if (wnd.monoNormalSkillInfo != null)
            {
                _m_wNormalSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoNormalSkillInfo);
                _m_wNormalSkillInfo.onSkillOpClick += _onNormalSkillOpClick;
            }

            if (wnd.monoAdvancedSkillInfo != null)
            {
                _m_wAdvancedSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoAdvancedSkillInfo);
                _m_wAdvancedSkillInfo.onSkillOpClick += _onAdvancedSkillOpClick;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickPre);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickPre);
                ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
            }

            _m_wQualityShowGo?.discard();
            _m_wQualityShowGo = null;

            if (_m_wOreItemContainer != null)
            {
                _m_wOreItemContainer.onOreClick -= _onOreClick;
                _m_wOreItemContainer.discard();
                _m_wOreItemContainer = null;                
            }

            if (_m_wNormalSkillInfo != null)
            {
                _m_wNormalSkillInfo.onSkillOpClick -= _onNormalSkillOpClick;
                _m_wNormalSkillInfo.discard();
                _m_wNormalSkillInfo = null;
            }

            if (_m_wAdvancedSkillInfo != null)
            {
                _m_wAdvancedSkillInfo.onSkillOpClick -= _onAdvancedSkillOpClick;
                _m_wAdvancedSkillInfo.discard();
                _m_wAdvancedSkillInfo = null;
            }

            _m_lOreInfoList?.Clear();
            _m_lOreInfoList = null;
            _m_lAllCompositeCatalogList?.Clear();
            _m_lAllCompositeCatalogList = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wQualityShowGo?.hideWnd();
            _m_wOreItemContainer?.hideWnd();
            _m_wNormalSkillInfo?.hideWnd();
            _m_wAdvancedSkillInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wQualityShowGo?.resetWnd();
            _m_wOreItemContainer?.resetWnd();
            _m_wNormalSkillInfo?.resetWnd();
            _m_wAdvancedSkillInfo?.resetWnd();
        }

        public void setData(List<TreasureHuntCompositeCatalogInfo> _allCompositeCatalogList, TreasureHuntCompositeCatalogInfo _showCompositeCatalogInfo, List<_ITreasureHuntOreInfo> _oreInfoList)
        {
            if (_m_lAllCompositeCatalogList == null)
                _m_lAllCompositeCatalogList = new List<TreasureHuntCompositeCatalogInfo>();
            _m_lAllCompositeCatalogList.Clear();
            if (_allCompositeCatalogList != null)
                _m_lAllCompositeCatalogList.AddRange(_allCompositeCatalogList);

            if (_showCompositeCatalogInfo != null)
            {
                _m_iCurShowCatalogIndex = _m_lAllCompositeCatalogList.IndexOf(_showCompositeCatalogInfo);
                if (_m_iCurShowCatalogIndex < 0)//若没有在传入的组合图鉴列表中找到要显示的组合图鉴
                {
                    _m_lAllCompositeCatalogList.Add(_showCompositeCatalogInfo);// 则将要显示的组合图鉴添加到列表中
                    _m_iCurShowCatalogIndex = _m_lAllCompositeCatalogList.Count - 1;
                }
            }
            else// 若没有传入要显示的组合图鉴信息, 则默认显示第一个
            {
                _m_iCurShowCatalogIndex = 0;
            }

            if (_m_lOreInfoList == null)
                _m_lOreInfoList = new List<_ITreasureHuntOreInfo>();
            _m_lOreInfoList.Clear();
            if(_oreInfoList != null)
                _m_lOreInfoList.AddRange(_oreInfoList);
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            TreasureHuntCompositeCatalogInfo catalogInfo = _m_lAllCompositeCatalogList?.SafeGet(_m_iCurShowCatalogIndex);
            if(catalogInfo == null || catalogInfo.compositeCatalogRefObj == null)
                return;
            
            // 设置组合名称
            ALUGUICommon.setLabelTxt(wnd.txtCompositeName, TextTranslate.instance.getLanguage(catalogInfo.compositeCatalogRefObj.name));

            // 设置图鉴描述
            ALUGUICommon.setLabelTxt(wnd.txtCatalogDesc, TextTranslate.instance.getLanguage(catalogInfo.compositeCatalogRefObj.desc));

            // 设置品质显示
            NPQualityExtRefObj qualityExtRefObj = GRefdataCoreMgr.instance.qualityExtRefCore.getRef((long) catalogInfo.compositeCatalogRefObj.quality);
            if (qualityExtRefObj != null && _m_wQualityShowGo != null)
            {
                _m_wQualityShowGo.showWnd();
                _m_wQualityShowGo.setData(qualityExtRefObj);
            }
            else
            {
                _m_wQualityShowGo?.hideWnd();
            }

            // 设置矿石列表
            if (_m_wOreItemContainer != null)
            {
                _m_wOreItemContainer.showWnd();
                _m_wOreItemContainer.setData(_m_lOreInfoList);
            }

            // 设置图鉴组合完成时间
            ALUGUICommon.setLabelTxt(wnd.txtConstituteTime, 
                TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_compositeCatalogCompletedTime_str,
                    TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(catalogInfo.collectedTimeMs))));

            // 设置普通技能信息
            if (_m_wNormalSkillInfo != null && catalogInfo.normalSkillInfo != null)
            {
                _m_wNormalSkillInfo.showWnd();
                _m_wNormalSkillInfo.setData(catalogInfo.normalSkillInfo);
            }
            else
            {
                _m_wNormalSkillInfo?.hideWnd();
            }

            // 设置高级技能信息
            if (_m_wAdvancedSkillInfo != null && catalogInfo.advancedSkillInfo != null)
            {
                _m_wAdvancedSkillInfo.showWnd();
                _m_wAdvancedSkillInfo.setData(catalogInfo.advancedSkillInfo);
            }
            else
            {
                _m_wAdvancedSkillInfo?.hideWnd();
            }
            
            NPCommonEnumStatInfo<ETreasureHuntCompositeCatalogState>.setStat(wnd.compositeCatalogStateShowList, catalogInfo.state);
            
            _refreshPreNextBtn();
        }

        /// <summary>
        /// 矿石被点击
        /// </summary>
        private void _onOreClick(_ITreasureHuntOreInfo _oreInfo)
        {
            if(_oreInfo == null)
                return;
            
            GGUIWndTreasureHuntOreDetailInfo.instance.setData(null, _oreInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndTreasureHuntOreDetailInfo.instance, () =>
            {
                GGUIWndTreasureHuntOreDetailInfo.instance.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_ORE_DETAIL_INFO);
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_COMPOSITE_CATALOG_DETAIL);
        }

        /// <summary>
        /// 点击普通技能操作按钮
        /// </summary>
        /// <param name="_skillInfo">技能信息</param>
        private void _onNormalSkillOpClick(_ITreasureHuntSkillInfo _skillInfo)
        {
            if (_skillInfo == null)
                return;

            TreasureHuntCompositeCatalogInfo catalogInfo = _m_lAllCompositeCatalogList?.SafeGet(_m_iCurShowCatalogIndex);
            if(catalogInfo == null)
                return;
            
            ETreasureHuntSkillState skillState = _skillInfo.skillState;
            if (skillState == ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE)
            {
                // 激活普通技能
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntCompositeActive(catalogInfo.compositeCatalogId, true,
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wNormalSkillInfo == null)
                            return;

                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillActiveSuccTip_none);
                        // 进行激活表现
                        _m_wNormalSkillInfo.showSkillActive();
                    });
            }
        }

        /// <summary>
        /// 点击高级技能操作按钮
        /// </summary>
        /// <param name="_skillInfo">技能信息</param>
        private void _onAdvancedSkillOpClick(_ITreasureHuntSkillInfo _skillInfo)
        {
            if (_skillInfo == null)
                return;

            TreasureHuntCompositeCatalogInfo catalogInfo = _m_lAllCompositeCatalogList?.SafeGet(_m_iCurShowCatalogIndex);
            if(catalogInfo == null)
                return;
            
            ETreasureHuntSkillState skillState = _skillInfo.skillState;
            if (skillState == ETreasureHuntSkillState.UNLOCK_NOT_ACTIVATE)
            {
                // 激活高级技能
                NPPlayer.instance.treasureHuntComponent.reqTreasureHuntCompositeActive(catalogInfo.compositeCatalogId, false,
                    (_isSucc, _msg) =>
                    {
                        if(!_isSucc || wnd == null || !isShow || _m_wAdvancedSkillInfo == null)
                            return;

                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.treasureHunt_skillActiveSuccTip_none);
                        // 进行激活表现
                        _m_wAdvancedSkillInfo.showSkillActive();
                    });
            }
        }

        /// <summary>
        /// 刷新前后翻页按钮
        /// </summary>
        private void _refreshPreNextBtn()
        {
            if (wnd == null)
                return;

            // 若可展示的组合图鉴列表为空或仅有一个, 则前后翻页按钮均不显示
            if (_m_lAllCompositeCatalogList == null || _m_lAllCompositeCatalogList.Count <= 1)
            {
                ALUGUICommon.setGameObjEnable(wnd.btnPre, false);
                ALUGUICommon.setGameObjEnable(wnd.btnNext, false);
                return;
            }

            ALUGUICommon.setGameObjEnable(wnd.btnPre, _m_iCurShowCatalogIndex > 0);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, _m_iCurShowCatalogIndex < _m_lAllCompositeCatalogList.Count - 1);
        }

        /// <summary>
        /// 点击上一个图鉴按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickPre(GameObject _go)
        {
            if (_m_lAllCompositeCatalogList == null || _m_lAllCompositeCatalogList.Count <= 1)
                return;

            if (_m_iCurShowCatalogIndex > 0)
            {
                _m_iCurShowCatalogIndex--;
                TreasureHuntCompositeCatalogInfo catalogInfo = _m_lAllCompositeCatalogList.SafeGet(_m_iCurShowCatalogIndex);
                if (catalogInfo != null && catalogInfo.compositeCatalogRefObj != null && catalogInfo.compositeCatalogRefObj.ore_list != null)
                {
                    if (_m_lOreInfoList == null)
                        _m_lOreInfoList = new List<_ITreasureHuntOreInfo>();
                    _m_lOreInfoList.Clear();
                    foreach (var oreId in catalogInfo.compositeCatalogRefObj.ore_list)
                    {
                        _m_lOreInfoList.Add(TreasureHuntUtil.getOreInfo(oreId));
                    }
                }
                
                _refreshWnd();
            }
        }

        /// <summary>
        /// 点击下一个图鉴按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickNext(GameObject _go)
        {
            if (_m_lAllCompositeCatalogList == null || _m_lAllCompositeCatalogList.Count <= 1)
                return;

            if (_m_iCurShowCatalogIndex < _m_lAllCompositeCatalogList.Count - 1)
            {
                _m_iCurShowCatalogIndex++;
                TreasureHuntCompositeCatalogInfo catalogInfo = _m_lAllCompositeCatalogList.SafeGet(_m_iCurShowCatalogIndex);
                if (catalogInfo != null && catalogInfo.compositeCatalogRefObj != null && catalogInfo.compositeCatalogRefObj.ore_list != null)
                {
                    if (_m_lOreInfoList == null)
                        _m_lOreInfoList = new List<_ITreasureHuntOreInfo>();
                    _m_lOreInfoList.Clear();
                    foreach (var oreId in catalogInfo.compositeCatalogRefObj.ore_list)
                    {
                        _m_lOreInfoList.Add(TreasureHuntUtil.getOreInfo(oreId));
                    }
                }
                
                _refreshWnd();
            }
        }
    }
}