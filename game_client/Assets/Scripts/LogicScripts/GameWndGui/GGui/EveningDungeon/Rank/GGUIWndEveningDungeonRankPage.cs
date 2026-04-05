using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndEveningDungeonRankPage : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoEveningDungeonRankPage>
    {
        private string _m_sAssetPath;//窗口对象的资源 加载路径
        private string _m_sObjName;//窗口对象的资源 名字
        
        private GGUIWndEveningDungeonRankDetailGrid _m_wRankGrid;//排行榜列表

        public GGUIWndEveningDungeonRankPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPathInfo?.asset_path;
            _m_sObjName = _assetPathInfo?.obj_name;
        }

        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sObjName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wRankGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRankGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRankGrid?.discard();
            _m_wRankGrid = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRankGrid != null)
                _m_wRankGrid = new GGUIWndEveningDungeonRankDetailGrid(wnd.monoRankGrid);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_rankShowInfoList"></param>
        public void setInfo(List<EveningDungeonRankInfo> _rankShowInfoList)
        {
            if (_m_wRankGrid != null)
            {
                _m_wRankGrid.showWnd();
                _m_wRankGrid.setInfo(_rankShowInfoList);
            }
        }
    }
}