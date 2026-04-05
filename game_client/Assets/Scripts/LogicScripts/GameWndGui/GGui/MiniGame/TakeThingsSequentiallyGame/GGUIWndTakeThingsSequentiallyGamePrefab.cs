using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTakeThingsSequentiallyGamePrefab : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoTakeThingsSequentiallyGamePrefab>
    {
        private NPCommonAssetPathInfo _m_assetPathInfo;
        
        [NotNull] private Dictionary<long, GGUIWndTakeThingsSequentiallyGameThing> _m_dThingWndDic =
            new Dictionary<long, GGUIWndTakeThingsSequentiallyGameThing>();
        
        protected override string _monoAssetPath { get { return _m_assetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_assetPathInfo?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public GGUIWndTakeThingsSequentiallyGamePrefab(NPCommonAssetPathInfo _assetPath, Transform _parent) : base(_parent)
        {
            _m_assetPathInfo = _assetPath;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoThingList != null)
            {
                Dictionary<long , int> thingIdIndexDic = new Dictionary<long, int>();
                GGUIWndTakeThingsSequentiallyGameThing thingWnd = null;
                for(int i = 0;i < wnd.monoThingList.Count; i++)
                {
                    GGUIMonoTakeThingsSequentiallyGameThing thingMono = wnd.monoThingList[i];
                    if(thingMono == null)
                        continue;

                    thingWnd = new GGUIWndTakeThingsSequentiallyGameThing(thingMono);
                    if (thingIdIndexDic.TryGetValue(thingMono.thingId, out int _index))
                    {
                        Debug.LogError($"脚本配置错误, monoThingList列表中存在同id:{thingMono.thingId}物体, 请检查配置monoThingList列表中元素:{_index}和元素:{i}", wnd.gameObject);
                    }
                    
                    thingIdIndexDic[thingMono.thingId] = i;
                    _m_dThingWndDic[thingMono.thingId] = thingWnd;
                }
                thingIdIndexDic.Clear();
                thingIdIndexDic = null;
            }
        }
        
        protected override void _onDiscard()
        {
            foreach (GGUIWndTakeThingsSequentiallyGameThing thing in _m_dThingWndDic.Values)
            {
                if(thing != null)
                    thing.discard();
            }
            
            _m_dThingWndDic.Clear();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 获取物品窗口
        /// </summary>
        /// <param name="_thingId"></param>
        /// <returns></returns>
        public GGUIWndTakeThingsSequentiallyGameThing getThingWnd(long _thingId)
        {
            if(!_m_dThingWndDic.TryGetValue(_thingId, out GGUIWndTakeThingsSequentiallyGameThing thingWnd) || thingWnd == null)
            {
                Debug.LogError($"[GGUIWndTakeThingsSequentiallyGamePrefab getThingWnd] 找不到物品:{_thingId}对应的窗口, 请检查UI是否配置该物品");
            }
            return thingWnd;
        }

        /// <summary>
        /// 对所有物品窗口的操作
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllThingWnd(Action<GGUIWndTakeThingsSequentiallyGameThing> _action)
        {
            if (_action == null)
                return;

            _m_dThingWndDic.Values.ForEach(_action);
        }

        public void setGameState(ETakeThingsSequentiallyGameState _gameState)
        {
            if (wnd == null || wnd.stateShow == null)
                return;
            
            wnd.stateShow.setShowData(_gameState);
        }
    }
}