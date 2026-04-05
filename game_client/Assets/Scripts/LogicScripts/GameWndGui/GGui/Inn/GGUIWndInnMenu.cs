using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndInnMenu : _ATALBasicUIWnd<GGUIMonoInnMenu>
    {
        [NotNull] public static GGUIWndInnMenu instance { get { return _g_instance ??= new GGUIWndInnMenu(); } }
        private static GGUIWndInnMenu _g_instance;


        [ItemNotNull, NotNull] private readonly List<InnDishInfo> _m_dishList;
        private GGUISubWndInnMenuGrid _m_itemGrid;
        

        public GGUIWndInnMenu() 
            : base(EALUIWndLayer.ADDITION)
        {
            _m_dishList = new List<InnDishInfo>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoInnMenu.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoInnMenu.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public GGUISubWndInnMenuGrid itemGridWnd { get { return _m_itemGrid; } }


        protected override void _onShowWnd()
        {
            _m_itemGrid?.showWnd();

            refreshWnd();
            
            NPPlayer.instance.innComp.onDishChg += _onDishChg;
        }
        protected override void _onHideWnd()
        {
            NPPlayer.instance.innComp.onDishChg -= _onDishChg;

            _m_itemGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_itemGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_itemGrid?.discard();
            _m_itemGrid = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemGrid != null)
                _m_itemGrid = new GGUISubWndInnMenuGrid(wnd.monoItemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            if (wnd.listValueList != null)
            {
                foreach (GGUIMonoInnMenuPropertyAdd propertyAdd in wnd.listValueList)
                {
                    if (propertyAdd == null || propertyAdd.txtValue == null)
                        continue;

                    long value = NPPlayer.instance.innComp.bonusMgr.getTotalPropertyBonus(wnd.propertyType, EBonusFilterType.BUILDING_ATTR, (long)propertyAdd.attrType);
                    string key = wnd.propertyValueKey;
                    string valueStr = wnd.propertyIsPercentage ? (value / 100f).ToString() : value.ToString();
                    ALUGUICommon.setLabelTxt(propertyAdd.txtValue, string.IsNullOrEmpty(key) ? valueStr : TextTranslate.instance.getLanguage(key, valueStr));
                }
            }
            _m_itemGrid?.refreshWnd();
            NPPlayer.instance.innComp.getUnlockedDishListNonAlloc(_m_dishList);
            int allDishCount = NPPlayer.instance.innComp.allDishCount;
            ALUGUICommon.setLabelTxt(wnd.txtUnlockProgress, TextTranslate.instance.getLanguage(TransKeyConst.inn_dishUnlockProgress_num_num, _m_dishList.Count, allDishCount));
        }
        
        
        private void _onDishChg(InnDishInfo _)
        {
            refreshWnd();
        }


        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_MENU);
        }
    }
}