using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星科技加成总览窗口
    /// </summary>
    public class GGUIWndMasrTechnologyAddOverview : _ANPGGUIBasicWnd<GGUIMonoMasrTechnologyAddOverview>
    {
        private static GGUIWndMasrTechnologyAddOverview _g_instance;
        public static GGUIWndMasrTechnologyAddOverview instance { get { return _g_instance ??= new GGUIWndMasrTechnologyAddOverview(); } }

        private List<MasrTechnologyAddPropertyShow> _m_propertyList;
        private Dictionary<MarsTechnologyTypeRefObj, int> _m_typePropertyCountDic;
        
        private GGUIWndMasrTechnologyAddOverviewPropertyGrid _m_wPropertyGrid;
        
        public GGUIWndMasrTechnologyAddOverview() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMasrTechnologyAddOverview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMasrTechnologyAddOverview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建属性列表Grid
            if (wnd.monoPropertyGrid != null)
            {
                _m_wPropertyGrid = new GGUIWndMasrTechnologyAddOverviewPropertyGrid(wnd.monoPropertyGrid);
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        protected override void _onDiscard()
        {
            // 解绑关闭按钮
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }

            // 销毁属性列表Grid
            _m_wPropertyGrid?.discard();
            _m_wPropertyGrid = null;
            
            _m_propertyList?.Clear();
            _m_propertyList = null;
            
            _m_typePropertyCountDic?.Clear();
            _m_typePropertyCountDic = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wPropertyGrid?.hideWnd();
            
            _m_propertyList?.Clear();
            _m_typePropertyCountDic?.Clear();
        }

        protected override void _onReset()
        {
            _m_wPropertyGrid?.resetWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            if (_m_propertyList == null)
                _m_propertyList = new List<MasrTechnologyAddPropertyShow>();
            _m_propertyList.Clear();
            if(_m_typePropertyCountDic == null)
                _m_typePropertyCountDic = new Dictionary<MarsTechnologyTypeRefObj, int>();
            _m_typePropertyCountDic.Clear();

            int propertyCount;
            foreach (var technologyTypeRefObj in GRefdataCoreMgr.instance.marsTechnologyTypeRefCore.refList)
            {
                if(technologyTypeRefObj == null)
                    continue;

                propertyCount = 0;
                if (technologyTypeRefObj.mars_property_type != null)
                {
                    foreach (var marsPropertyType in technologyTypeRefObj.mars_property_type)
                    {
                        propertyCount++;
                        MasrTechnologyAddPropertyShow propertyShow = new MasrTechnologyAddPropertyShow
                        {
                            propertyShow = marsPropertyType.getPropertyShow(),
                            value = NPPlayer.instance.marsComp.technologySubComponent.marsPropertyContainer.getValue(marsPropertyType)
                        };
                        
                        _m_propertyList.Add(propertyShow);
                    }
                }

                if (technologyTypeRefObj.player_property_type != null)
                {
                    foreach (var playerPropertyType in technologyTypeRefObj.player_property_type)
                    {
                        propertyCount++;
                        MasrTechnologyAddPropertyShow propertyShow = new MasrTechnologyAddPropertyShow
                        {
                            propertyShow = playerPropertyType.getPropertyShow(),
                            value = NPPlayer.instance.marsComp.technologySubComponent.playerPropertyContainer.getValue(playerPropertyType)
                        };
                        
                        _m_propertyList.Add(propertyShow);
                    }
                }
                
                _m_typePropertyCountDic[technologyTypeRefObj] = propertyCount;
            }

            if (_m_wPropertyGrid != null)
            {
                _m_wPropertyGrid.showWnd();
                _m_wPropertyGrid.setData(_m_propertyList, _m_typePropertyCountDic);
            }
        }

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_TECHNOLOGY_ADD_OVERVIEW);
        }
    }
}