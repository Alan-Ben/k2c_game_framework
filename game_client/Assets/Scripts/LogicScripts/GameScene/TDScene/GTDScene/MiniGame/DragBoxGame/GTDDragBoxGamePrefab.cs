
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public class GTDDragBoxGamePrefab
    {
        [NotNull] private GTDMonoDragBoxGamePrefab _m_tdMono;
        
        [NotNull] private List<GTDDragBoxGameBox> _m_dBoxWndList = new List<GTDDragBoxGameBox>();
        
        private Action _m_aOnGameSuccess;
        
        public GTDDragBoxGamePrefab([NotNull]GTDMonoDragBoxGamePrefab _mono)
        {
            _m_tdMono = _mono;
        }
        
        public GameObject go { get { return _m_tdMono.gameObject; } }
        public GTDMonoDragBoxGamePlayerCuteActor playerCuteActorMono { get { return _m_tdMono.monoPlayerCuteActor; } }

        public void show()
        {
            ALUGUICommon.setGameObjEnable(_m_tdMono, true);
        }
        
        public void hide()
        {
            ALUGUICommon.setGameObjEnable(_m_tdMono, false);
        }
        
        public void onInit(Action _onGameSuccess)
        {
            _m_dBoxWndList.Clear();
            if (_m_tdMono != null && _m_tdMono.monoList != null)
            {
                GTDDragBoxGameBox boxWnd = null;
                for(int i = 0;i < _m_tdMono.monoList.Count; i++)
                {
                    GTDMonoDragBoxGameBox boxMono = _m_tdMono.monoList[i];
                    if(boxMono == null)
                        continue;

                    boxWnd = new GTDDragBoxGameBox(boxMono);
                    boxWnd.onInit();
                    
                    _m_dBoxWndList.Add(boxWnd);
                }
            }

            _m_aOnGameSuccess = _onGameSuccess;
            if (_m_tdMono != null)
            {
                _m_tdMono.gameSuccess += _m_aOnGameSuccess;
            }
        }
        
        public void onDiscard()
        {
            foreach (GTDDragBoxGameBox wnd in _m_dBoxWndList)
            {
                if(wnd != null)
                    wnd.onDiscard();
            }
            _m_dBoxWndList.Clear();   
            
            if (_m_tdMono != null)
            {
                _m_tdMono.gameSuccess -= _m_aOnGameSuccess;
            }
            _m_aOnGameSuccess = null;
        }
        
        /// <summary>
        /// 对所有物品窗口的操作
        /// </summary>
        /// <param name="_action"></param>
        public void dealAllBox(Action<GTDDragBoxGameBox> _action)
        {
            if (_action == null)
                return;

            _m_dBoxWndList.ForEach(_action);
        }
    }
}