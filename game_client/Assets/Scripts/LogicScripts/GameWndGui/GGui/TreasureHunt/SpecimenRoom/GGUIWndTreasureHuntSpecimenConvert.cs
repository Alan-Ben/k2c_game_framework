using System.Collections.Generic;
using ALPackage;
using GS2GC.p036_TreasureHuntOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 标本转化窗口
    /// </summary>
    public class GGUIWndTreasureHuntSpecimenConvert : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntSpecimenConvert>
    {
        public static GGUIWndTreasureHuntSpecimenConvert _g_instance;
        public static GGUIWndTreasureHuntSpecimenConvert instance { get { return _g_instance ??= new GGUIWndTreasureHuntSpecimenConvert(); } }
        
        private List<TreasureHuntCommonOreInfo> _m_lConvertOreList;//转化的矿石列表
        private int _m_iNormalOreCount = 0; // 普通矿石数量
        private int _m_iAdvancedOreCount = 0; // 高级矿石数量
        
        private GGUIWndTreasureHuntSpecimenConvertItemGrid _m_wSpecimenConvertItemGrid;

        public GGUIWndTreasureHuntSpecimenConvert() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntSpecimenConvert.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntSpecimenConvert.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoSpecimenConvertItemGrid != null)
            {
                _m_wSpecimenConvertItemGrid = new GGUIWndTreasureHuntSpecimenConvertItemGrid(wnd.monoSpecimenConvertItemGrid);
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
            }

            _m_lConvertOreList?.Clear();
            _m_lConvertOreList = null;
            
            _m_wSpecimenConvertItemGrid?.discard();
            _m_wSpecimenConvertItemGrid = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lConvertOreList?.Clear();

            _m_wSpecimenConvertItemGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lConvertOreList?.Clear();

            _m_wSpecimenConvertItemGrid?.resetWnd();
        }

        public void setData(GS2GC_036_009_RetTreasureHuntTransOre _retMsg)
        {
            if(_retMsg == null)
                return;

            if (_m_lConvertOreList == null)
                _m_lConvertOreList = new List<TreasureHuntCommonOreInfo>();
            _m_lConvertOreList.Clear();
            
            // 重置计数器
            _m_iNormalOreCount = 0;
            _m_iAdvancedOreCount = 0;
            
            if (_retMsg.getTransResultList() != null)
            {
                foreach (var result in _retMsg.getTransResultList())
                {
                    if (result != null)
                    {
                        ETreasureHuntOreState oreState = result.getIsNormal() ? ETreasureHuntOreState.ACTIVATED_NORMAL : ETreasureHuntOreState.ACTIVATED_ADVANCED;
                        _m_lConvertOreList.Add(new TreasureHuntCommonOreInfo(result.getOreId(), oreState, result.getNum(), 0, 0, null, null));
                        
                        // 统计普通和高级矿石数量
                        if (result.getIsNormal())
                        {
                            _m_iNormalOreCount += result.getNum();
                        }
                        else
                        {
                            _m_iAdvancedOreCount += result.getNum();
                        }
                    }
                }
            }
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;
            
            // 刷新转化物品网格
            if (_m_wSpecimenConvertItemGrid != null)
            {
                if (_m_lConvertOreList != null && _m_lConvertOreList.Count > 0)
                {
                    _m_wSpecimenConvertItemGrid.showWnd();
                    _m_wSpecimenConvertItemGrid.setData(_m_lConvertOreList);
                }
                else
                {
                    _m_wSpecimenConvertItemGrid.hideWnd();
                }
            }

            // 刷新普通矿石转化数量文本
            if (wnd.txtNormalConvertNum != null)
            {
                if (string.IsNullOrEmpty(wnd.txtNormalConvertNumKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtNormalConvertNum, _m_iNormalOreCount);
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtNormalConvertNum, TextTranslate.instance.getLanguage(wnd.txtNormalConvertNumKey, _m_iNormalOreCount));
                }
            }

            // 刷新高级矿石转化数量文本
            if (wnd.txtAdvancedConvertNum != null)
            {
                if (string.IsNullOrEmpty(wnd.txtAdvancedConvertNumKey))
                {
                    ALUGUICommon.setLabelTxt(wnd.txtAdvancedConvertNum, _m_iAdvancedOreCount);
                }
                else
                {
                    ALUGUICommon.setLabelTxt(wnd.txtAdvancedConvertNum, TextTranslate.instance.getLanguage(wnd.txtAdvancedConvertNumKey, _m_iAdvancedOreCount));
                }
            }
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_CONVERT);
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickSure(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_SPECIMEN_CONVERT);
        }
    }
}