using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作地图预览弹窗
    /// </summary>
    public class GGUIWndGuildCooperateMap : _ANPGGUIBasicWnd<GGUIMonoGuildCooperateMap>
    {
        private static GGUIWndGuildCooperateMap _g_instance;
        public static GGUIWndGuildCooperateMap instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndGuildCooperateMap();
                return _g_instance;
            }
        }

        // 选择区域回调
        private Action<long> _m_aOnSelectArea;
        // 区域列表
        private List<GGUIWndGuildCooperateMapAreaItem> _m_lMapAreaItemList;

        public GGUIWndGuildCooperateMap() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildCooperateMap.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildCooperateMap.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            if (_m_lMapAreaItemList != null)
            {
                for (int i = 0; i < _m_lMapAreaItemList.Count; i++)
                {
                    _m_lMapAreaItemList[i]?.showWnd();
                }
            }
        }

        protected override void _onHideWnd()
        {
            if (_m_lMapAreaItemList != null)
            {
                for (int i = 0; i < _m_lMapAreaItemList.Count; i++)
                {
                    _m_lMapAreaItemList[i]?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            if (_m_lMapAreaItemList != null)
            {
                for (int i = 0; i < _m_lMapAreaItemList.Count; i++)
                {
                    _m_lMapAreaItemList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lMapAreaItemList != null)
            {
                for (int i = 0; i < _m_lMapAreaItemList.Count; i++)
                {
                    _m_lMapAreaItemList[i]?.discard();
                }
                _m_lMapAreaItemList.Clear();
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lMapAreaItemList = new List<GGUIWndGuildCooperateMapAreaItem>();
            if (wnd.monoAreaItemList != null)
            {
                List<GuildCooperateAreaRefObj> areaRefList = GRefdataCoreMgr.instance.guildCooperateAreaRefCore.makeNewAllRefList();
                if (areaRefList != null)
                {
                    for (int i = 0; i < areaRefList.Count; i++)
                    {
                        if (areaRefList[i] != null && wnd.monoAreaItemList.Count > i)
                        {
                            _m_lMapAreaItemList.Add(new GGUIWndGuildCooperateMapAreaItem(areaRefList[i], _onClickAreaItem, wnd.monoAreaItemList[i]));
                        }
                    }
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onSelectArea"></param>
        public void setInfo(Action<long> _onSelectArea)
        {
            _m_aOnSelectArea = _onSelectArea;
        }

        /// <summary>
        /// 点击区域item
        /// </summary>
        /// <param name="_areaId"></param>
        private void _onClickAreaItem(long _areaId)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_MAP);
            _m_aOnSelectArea?.Invoke(_areaId);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_GUIlD_COOPERATE_MAP);
        }
    }
}