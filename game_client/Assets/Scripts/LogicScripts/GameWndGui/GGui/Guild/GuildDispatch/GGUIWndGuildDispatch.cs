using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟派遣已获取的大臣信息
    /// </summary>
    public class GuildDispatchGottenHeroInfo
    {
        private HeroInfo _m_iHeroInfo;
        
        private ESpecAttrType _m_eSpecAttrType;//该大臣派遣的相性
        private long _m_lSpecAttrAddPro;//派遣相性的建筑收益加成
        
        public GuildDispatchGottenHeroInfo(HeroInfo _heroInfo, ESpecAttrType _specAttrType, long _specAttrAddPro)
        {
            _m_iHeroInfo = _heroInfo;
            _m_eSpecAttrType = _specAttrType;
            _m_lSpecAttrAddPro = _specAttrAddPro;
        }

        public GuildDispatchGottenHeroInfo(HeroInfo _heroInfo, ESpecAttrType _specAttrType)
        {
            _m_iHeroInfo = _heroInfo;
            _m_eSpecAttrType = _specAttrType;
            _m_lSpecAttrAddPro = 0;
            if (_m_iHeroInfo != null)
            {
                NPVarInfo varInfo = new NPVarInfo();
                varInfo.addObj(ENPPlayerVariableVarType.SPEC_ATTR_TYPE, (long)_m_eSpecAttrType);
                _m_lSpecAttrAddPro = _m_iHeroInfo.getBusinessSkillAddPropValue(null, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, varInfo);
            }
        }

        public HeroInfo heroInfo { get { return _m_iHeroInfo; } }
        public ESpecAttrType specAttrType { get { return _m_eSpecAttrType; } }
        public long specAttrAddPro { get { return _m_lSpecAttrAddPro; } }

        // /// <summary>
        // /// 不同相性的建筑收益加成
        // /// </summary>
        // [NotNull] private Dictionary<ESpecAttrType, long> _m_lBusinessSpecAttrAddProDic = new Dictionary<ESpecAttrType, long>();
        //
        // public GuildDispatchGottenHeroInfo(HeroInfo _heroInfo)
        // {
        //     _m_iHeroInfo = _heroInfo;
        //     if (_m_iHeroInfo != null)
        //     {
        //         NPVarInfo varInfo = new NPVarInfo();
        //         foreach (ESpecAttrType attr in Enum.GetValues(typeof(ESpecAttrType)))
        //         {
        //             varInfo.reset();
        //             varInfo.addObj(ENPPlayerVariableVarType.SPEC_ATTR_TYPE, (long)attr);
        //             
        //             _m_lBusinessSpecAttrAddProDic[attr] = _m_iHeroInfo.getBusinessSkillAddPropValue(null, EBonusPropertyType.BUILDING_PROFIT_ADD_PER, varInfo);
        //         }
        //     }
        // }
        //
        // public long getBusinessSpecAttrAddPro(ESpecAttrType _attr)
        // {
        //     long _addPro = 0;
        //     _m_lBusinessSpecAttrAddProDic.TryGetValue(_attr, out _addPro);
        //     return _addPro;
        // }
    }
    
    /// <summary>
    /// 派遣窗口
    /// </summary>
    public class GGUIWndGuildDispatch : _ANPGGUIBasicWnd<GGUIMonoGuildDispatch>
    {
        private static GGUIWndGuildDispatch _g_instance;
        public static GGUIWndGuildDispatch instance { get { return _g_instance ??= new GGUIWndGuildDispatch(); } }
        
        /// <summary>
        /// 显示的大臣字典
        /// </summary>
        [NotNull] private Dictionary<ESpecAttrType, GuildDispatchGottenHeroInfo> _m_dShowHeroInfoDic = new Dictionary<ESpecAttrType, GuildDispatchGottenHeroInfo>();
        private HeroInfo _m_iCurDispatchHeroInfo;//当前派遣的大臣信息
        
        private GGUIWndGuildDispatchHeroCardContainer _m_wHeroCardContainer;//大臣卡片列表
        private GGUIWndHeroIconItem _m_wSelectedHeroIconItem;//选中的大臣图标
        
        public GGUIWndGuildDispatch() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildDispatch.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildDispatch.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.heroCardContainer != null)
            {
                _m_wHeroCardContainer = new GGUIWndGuildDispatchHeroCardContainer(wnd.heroCardContainer);
                _m_wHeroCardContainer.onSelectItemChg += _onSelectHeroChg;
            }

            if (wnd.monoSelectedHeroIcon != null)
                _m_wSelectedHeroIconItem = new GGUIWndHeroIconItem(wnd.monoSelectedHeroIcon);
         
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onSureBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if(wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onSureBtnClick);
            }
            
            if (_m_wHeroCardContainer != null)
            {
                _m_wHeroCardContainer.onSelectItemChg -= _onSelectHeroChg;
                _m_wHeroCardContainer.discard();
                _m_wHeroCardContainer = null;    
            }
            
            _m_wSelectedHeroIconItem?.discard();
            _m_wSelectedHeroIconItem = null;
        }
        
        protected override void _onShowWnd()
        {
            _updateShowHeroInfo();

            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wHeroCardContainer?.hideWnd();
            
            _m_wSelectedHeroIconItem?.hideWnd();
            
            _m_dShowHeroInfoDic.Clear();
        }

        protected override void _onReset()
        {
            _m_wHeroCardContainer?.resetWnd();
            
            _m_wSelectedHeroIconItem?.resetWnd();
        }

        /// <summary>
        /// 更新显示大臣数据
        /// </summary>
        private void _updateShowHeroInfo()
        {
            // 获取玩家自身派遣的大臣id
            long dispatchHeroId = NPPlayer.instance.guildComp.guildInfo?.getDispatchHeroId(NPPlayer.instance.playerInfo.CID) ?? 0;
            
            NPVarInfo varInfo = new NPVarInfo();
            _m_dShowHeroInfoDic.Clear();
            NPPlayer.instance.heroComponent.dealAllHero((_heroInfo) =>
            {
                if (_heroInfo == null)
                    return;

                if (_heroInfo.id == dispatchHeroId)
                    _m_iCurDispatchHeroInfo = _heroInfo;

                ESpecAttrType heroSpecAttrType = _heroInfo.specAttrType; //大臣的相性

                varInfo.reset();
                varInfo.addObj(ENPPlayerVariableVarType.SPEC_ATTR_TYPE, (long) heroSpecAttrType);

                long heroSpecAttrAddPro =
                    _heroInfo.getBusinessSkillAddPropValue(null, EBonusPropertyType.BUILDING_PROFIT_ADD_PER,
                        varInfo); //大臣的相性建筑收益加成

                // 若在_m_dShowHeroInfoDic字典中找不到该相性的大臣信息 或者 该相性的大臣信息为空 或者 该相性的大臣信息的相性建筑收益加成小于当前大臣的相性建筑收益加成
                // 则更新该相性的派遣大臣信息
                if (!_m_dShowHeroInfoDic.TryGetValue(heroSpecAttrType, out GuildDispatchGottenHeroInfo dispatchHeroInfo) || dispatchHeroInfo == null || dispatchHeroInfo.specAttrAddPro < heroSpecAttrAddPro)
                {
                    dispatchHeroInfo = new GuildDispatchGottenHeroInfo(_heroInfo, heroSpecAttrType, heroSpecAttrAddPro);
                    _m_dShowHeroInfoDic[heroSpecAttrType] = dispatchHeroInfo;
                }
            });
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (_m_wHeroCardContainer != null)
            {
                _m_wHeroCardContainer.showWnd();
                _m_wHeroCardContainer.setData(_m_dShowHeroInfoDic.Values.ToList());
                
                if(_m_iCurDispatchHeroInfo != null)
                    _m_wHeroCardContainer.setSelectAttrHero(_m_iCurDispatchHeroInfo.specAttrType);
            }
            
            _refreshSelectedHeroInfo();;
        }

        /// <summary>
        /// 刷新选中大臣信息
        /// </summary>
        private void _refreshSelectedHeroInfo()
        {
            string addProKey = string.IsNullOrEmpty(wnd.txtSelectedAddProKey) ? TransKeyConst.common_addPropPer_num : wnd.txtSelectedAddProKey;
            if (_m_wHeroCardContainer == null || _m_wHeroCardContainer.selectedDispatchGottenHeroInfo == null ||
                _m_wHeroCardContainer.selectedDispatchGottenHeroInfo.heroInfo == null)
            {
                if (wnd != null)
                {
                    ALUGUICommon.setGameObjEnable(wnd.hasSelectedHeroShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.noSelectedHeroShow, true);
                    
                    ALUGUICommon.setLabelTxt(wnd.txtSelectedAddPro, TextTranslate.instance.getLanguage(addProKey, 0));
                }
            }
            else
            {
                if (_m_wSelectedHeroIconItem != null)
                {
                    _m_wSelectedHeroIconItem.showWnd();
                    _m_wSelectedHeroIconItem.setData(_m_wHeroCardContainer.selectedDispatchGottenHeroInfo.heroInfo);
                }
                
                ALUGUICommon.setLabelTxt(wnd.txtSelectedAddPro,
                    TextTranslate.instance.getLanguage(addProKey, (long) Math.Ceiling(_m_wHeroCardContainer.selectedDispatchGottenHeroInfo.specAttrAddPro / 100d)));
            }
        }

        /// <summary>
        /// 当选中大臣变化
        /// </summary>
        private void _onSelectHeroChg()
        {
            _refreshSelectedHeroInfo();
        }

        /// <summary>
        /// 关闭按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_DISPATCH_HERO);
        }
        
        /// <summary>
        /// 当确认按钮被点击时
        /// </summary>
        /// <param name="_go"></param>
        private void _onSureBtnClick(GameObject _go)
        {
            if (_m_wHeroCardContainer == null || _m_wHeroCardContainer.selectedDispatchGottenHeroInfo == null ||
                _m_wHeroCardContainer.selectedDispatchGottenHeroInfo.heroInfo == null)
            {
                if(wnd != null)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(wnd.noSelectedHeroTipKey);
                
                return;
            }
            
            NPPlayer.instance.guildComp.reqGuildDispatchHero(_m_wHeroCardContainer.selectedDispatchGottenHeroInfo.heroInfo.id,
                (_msg) =>
                {
                    if(_msg == null || !isShow)
                        return;

                    //关闭窗口
                    _onCloseBtnClick(null);
                }, null);
        }
    }
}