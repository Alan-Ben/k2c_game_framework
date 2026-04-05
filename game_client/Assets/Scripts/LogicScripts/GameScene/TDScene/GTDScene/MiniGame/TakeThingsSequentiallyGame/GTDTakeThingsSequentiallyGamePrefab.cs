using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame.TakeThingsSequentiallyGame
{
    public class GTDTakeThingsSequentiallyGamePrefab
    {
        [NotNull] private GTDMonoTakeThingsSequentiallyGamePrefab _m_tdMono;
        
        [NotNull] private Dictionary<long, GTDTakeThingsSequentiallyGameThing> _m_dThingWndDic = new Dictionary<long, GTDTakeThingsSequentiallyGameThing>();

        public GameObject go { get { return _m_tdMono.gameObject; } }

        public GTDTakeThingsSequentiallyGamePrefab([NotNull]GTDMonoTakeThingsSequentiallyGamePrefab _mono)
        {
            _m_tdMono = _mono;
        }
        
        public void show()
        {
            ALUGUICommon.setGameObjEnable(_m_tdMono, true);
        }
        
        public void hide()
        {
            ALUGUICommon.setGameObjEnable(_m_tdMono, false);
        }

        public void onInit()
        {
            _m_dThingWndDic.Clear();
            if (_m_tdMono != null && _m_tdMono.monoThingList != null)
            {
                Dictionary<long , int> thingIdIndexDic = new Dictionary<long, int>();
                GTDTakeThingsSequentiallyGameThing thingWnd = null;
                for(int i = 0;i < _m_tdMono.monoThingList.Count; i++)
                {
                    GTDMonoTakeThingsSequentiallyGameThing thingMono = _m_tdMono.monoThingList[i];
                    if(thingMono == null)
                        continue;

                    thingWnd = new GTDTakeThingsSequentiallyGameThing(thingMono);
                    thingWnd.onInit();
                    if (thingIdIndexDic.TryGetValue(thingMono.thingId, out int _index))
                    {
                        Debug.LogError($"脚本配置错误, monoThingList列表中存在同id:{thingMono.thingId}物体, 请检查配置monoThingList列表中元素:{_index}和元素:{i}", _m_tdMono.gameObject);
                    }
                    
                    thingIdIndexDic[thingMono.thingId] = i;
                    _m_dThingWndDic[thingMono.thingId] = thingWnd;
                }
                thingIdIndexDic.Clear();
                thingIdIndexDic = null;
            }
        }
        
        public void onDiscard()
        {
            foreach (GTDTakeThingsSequentiallyGameThing thing in _m_dThingWndDic.Values)
            {
                if(thing != null)
                    thing.onDiscard();
            }
            _m_dThingWndDic.Clear();   
        }
        
        /// <summary>
        /// 获取物品td
        /// </summary>
        /// <param name="_thingId"></param>
        /// <returns></returns>
        public GTDTakeThingsSequentiallyGameThing getThingWnd(long _thingId)
        {
            if(!_m_dThingWndDic.TryGetValue(_thingId, out GTDTakeThingsSequentiallyGameThing thingWnd) || thingWnd == null)
            {
                Debug.LogError($"[GTDMonoTakeThingsSequentiallyGamePrefab getThingWnd] 找不到物品:{_thingId}对应的窗口, 请检查UI是否配置该物品");
            }
            return thingWnd;
        }

        /// <summary>
        /// 对所有物品窗口的操作
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllThingTd(Action<GTDTakeThingsSequentiallyGameThing> _action)
        {
            if (_action == null)
                return;

            _m_dThingWndDic.Values.ForEach(_action);
        }

        public void setGameState(ETakeThingsSequentiallyGameState _gameState)
        {
            if (_m_tdMono == null || _m_tdMono.stateShow == null)
                return;
            
            _m_tdMono.stateShow.setShowData(_gameState);
        }
    }
}