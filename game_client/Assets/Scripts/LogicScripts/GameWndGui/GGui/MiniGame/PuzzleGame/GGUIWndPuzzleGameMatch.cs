using System;
using System.Collections.Generic;
using ALPackage;
using GOE.MiniGame;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 拼图游戏匹配prefab
    /// </summary>
    public class GGUIWndPuzzleGameMatch : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoPuzzleGameMatch>
    {
        private NPCommonAssetPathInfo _m_iAssetPath;
        protected PuzzleGameController _m_controller;
        
        private List<GGUIWndPuzzleGameMatchItem> _m_lMatchItemWndList;
        private List<GGUIWndPuzzleGameMatchItemBg> _m_lMatchItemBgWndList;
        private EPuzzleGameState _m_eGameState;
        
        public GGUIWndPuzzleGameMatch(NPCommonAssetPathInfo _assetPath, Transform _parent) : base(_parent)
        {
            _m_iAssetPath = _assetPath;
        }

        protected override string _monoAssetPath { get { return _m_iAssetPath?.asset_path; } }
        protected override string _monoObjName { get { return _m_iAssetPath?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public void setGameController(PuzzleGameController _controller)
        {
            _m_controller = _controller;
        }
        
        protected override void _onWndInitDone()
        {
            _discardAllMatchItem();
            if (_m_lMatchItemWndList == null)
                _m_lMatchItemWndList = new List<GGUIWndPuzzleGameMatchItem>();
            _m_lMatchItemWndList.Clear();
            
            _discardAllMatchItemBg();
            if(_m_lMatchItemBgWndList == null)
                _m_lMatchItemBgWndList = new List<GGUIWndPuzzleGameMatchItemBg>();
            _m_lMatchItemBgWndList.Clear();
            
            if(wnd == null)
                return;

            Dictionary<long, int> itemIndexDic = new Dictionary<long, int>();
            if (wnd.matchItemList != null)
            {
                for (int i = 0; i < wnd.matchItemList.Count; i++)
                {
                    GGUIMonoPuzzleGameMatchItem itemMono = wnd.matchItemList[i];
                    if(itemMono == null)
                        continue;
                    
                    GGUIWndPuzzleGameMatchItem itemWnd = new GGUIWndPuzzleGameMatchItem(itemMono);
                    itemWnd.onItemPointDown += _onMatchItemPointDown;
                    itemWnd.onItemDrag += _onMatchItemDrag;
                    itemWnd.onItemPointUp += _onMatchItemPointUp;
                    _m_lMatchItemWndList.Add(itemWnd);
                    
                    if (itemIndexDic.TryGetValue(itemMono.matchItemId, out int _index))
                    {
                        Debug.LogError($"脚本配置错误, matchItemList列表中存在同id:{itemMono.matchItemId}物体, 请检查配置matchItemList列表中元素:{_index}和元素:{i}", wnd.gameObject);
                    }
                    itemIndexDic[itemMono.matchItemId] = i;
                }
            }
            itemIndexDic.Clear();

            if (wnd.matchItemBgList != null)
            {
                for (int i = 0; i < wnd.matchItemBgList.Count; i++)
                {
                    GGUIMonoPuzzleGameMatchItemBg itemBgMono = wnd.matchItemBgList[i];
                    if(itemBgMono == null)
                        continue;
                    
                    GGUIWndPuzzleGameMatchItemBg itemBgWnd = new GGUIWndPuzzleGameMatchItemBg(itemBgMono);
                    _m_lMatchItemBgWndList.Add(itemBgWnd);
                    
                    if (itemIndexDic.TryGetValue(itemBgMono.matchItemId, out int _index))
                    {
                        Debug.LogError($"脚本配置错误, matchItemBgList列表中存在同id:{itemBgMono.matchItemId}物体, 请检查配置matchItemBgList列表中元素:{_index}和元素:{i}", wnd.gameObject);
                    }
                    itemIndexDic[itemBgWnd.matchItemId] = i;
                }
            }
            itemIndexDic.Clear();
            itemIndexDic = null;
        }
        
        protected override void _onDiscard()
        {
            _discardAllMatchItemBg();
            _m_lMatchItemWndList = null;
            
            _discardAllMatchItem();
            _m_lMatchItemBgWndList = null;
        }
        
        protected override void _onReset()
        {
            dealAllMatchItem((_item) =>
            {
                if(_item != null)
                    _item.resetWnd();

                return false;
            });
            
            dealAllMatchItemBg((_item) =>
            {
                if(_item != null)
                    _item.resetWnd();

                return false;
            });
        }
        
        protected override void _onShowWnd()
        {
            // 子窗口显隐由自身unit处理，这里不做显隐
            // dealAllMatchItem((_item) =>
            // {
            //     if(_item != null)
            //         _item.showWnd();
            //
            //     return false;
            // });
            //
            // dealAllMatchItemBg((_item) =>
            // {
            //     if(_item != null)
            //         _item.showWnd();
            //
            //     return false;
            // });
        }

        protected override void _onHideWnd()
        {
            // 子窗口显隐由自身unit处理，这里不做显隐
            // dealAllMatchItem((_item) =>
            // {
            //     if(_item != null)
            //         _item.hideWnd();
            //
            //     return false;
            // });
            //
            // dealAllMatchItemBg((_item) =>
            // {
            //     if(_item != null)
            //         _item.hideWnd();
            //
            //     return false;
            // });
        }

        /// <summary>
        /// 设置游戏状态
        /// </summary>
        public void setGameState(EPuzzleGameState _state, bool _forceChg = false)
        {
            if (wnd == null || !isShow)
            {
                Debug.LogError_EditorOnly($"[GGUIWndPuzzleGameMatch] setGameState 在wnd == null或窗口未显示时, 就尝试设置窗口状态为:{_state}");
                return;
            }

            if (_m_eGameState == _state && !_forceChg)
                return;

            _m_eGameState = _state;
            wnd.setState(_m_eGameState);
        }
        
        /// <summary>
        /// 进行游戏成功表现
        /// </summary>
        public void playGameSuccess(Func<long> _getGameSerialize, Action _playDone)
        {
            if (_getGameSerialize == null)
            {
                Debug.LogError("[GGUIWndPuzzleGame] playGameSuccess 必须带有一个获取SerializeId的方法");
                _playDone?.Invoke();
                return;
            }
            
            long serialize = _getGameSerialize();
            if (wnd == null || wnd.gameSuccessExitTime <= 0)
            {
                _playDone?.Invoke();
            }
            else
            {
                CommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if(serialize == _getGameSerialize())
                        _playDone?.Invoke();
                }, wnd.gameSuccessExitTime);
            }
        }
        
        #region GGUIWndPuzzleGameMatchItem方法

        public void dealAllMatchItem(Func<GGUIWndPuzzleGameMatchItem, bool> _action)
        {
            if (_m_lMatchItemWndList == null || _action == null)
                return;

            foreach (GGUIWndPuzzleGameMatchItem matchItem in _m_lMatchItemWndList)
            {
                if(_action(matchItem))//若找到匹配item, 不需要继续
                    break;
            }
        }

        private void _discardAllMatchItem()
        {
            dealAllMatchItem((_item) =>
            {
                if (_item != null)
                    _item.discard();

                return false;
            });
            
            _m_lMatchItemWndList?.Clear();
        }
        
        private void _hideAllMatchItem()
        {
            dealAllMatchItem((_item) =>
            {
                if (_item != null)
                    _item.hideWnd();

                return false;
            });
            
            _m_lMatchItemWndList?.Clear();
        }

        /// <summary>
        /// 当匹配item上鼠标点下时
        /// </summary>
        /// <param name="_matchItem"></param>
        private void _onMatchItemPointDown(GGUIWndPuzzleGameMatchItem _matchItem)
        {
            if (_matchItem == null)
                return;
            
            _m_controller?.onMatchItemPointDown(_matchItem);
        }
        
        /// <summary>
        /// 当匹配item被拖动时
        /// </summary>
        private void _onMatchItemDrag(GGUIWndPuzzleGameMatchItem _matchItem, PointerEventData _pointerEventData)
        {
            if (_matchItem == null || _pointerEventData == null || wnd == null)
                return;
            
            // Debug.Log($"[_onMatchItemDrag] _pointerEventData.position:{_pointerEventData.position} _pointerEventData.delta:{_pointerEventData.delta}");

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, _pointerEventData.position, _pointerEventData.enterEventCamera, out Vector3 _worldPosition))
            {
                // Debug.Log($"[_onMatchItemDrag] _worldPosition:{_worldPosition}");
                _matchItem.setItemPosition(_worldPosition);
                _m_controller?.onMatchItemDrag(_matchItem);
            }
        }

        /// <summary>
        /// 当匹配item上鼠标放开时
        /// </summary>
        /// <param name="_matchItem"></param>
        private void _onMatchItemPointUp(GGUIWndPuzzleGameMatchItem _matchItem)
        {
            if (_matchItem == null)
                return;
            
            _m_controller?.onMatchItemPointUp(_matchItem);
        }
        
        #endregion

        #region GGUIWndPuzzleGameMatchItemBg方法

        public void dealAllMatchItemBg(Func<GGUIWndPuzzleGameMatchItemBg, bool> _action)
        {
            if (_m_lMatchItemBgWndList == null || _action == null)
                return;

            foreach (GGUIWndPuzzleGameMatchItemBg matchItemBg in _m_lMatchItemBgWndList)
            {
                if(_action(matchItemBg))//若找到匹配item, 不需要继续
                    break;
            }
        }
        
        private void _discardAllMatchItemBg()
        {
            dealAllMatchItemBg((_item) =>
            {
                if (_item != null)
                    _item.discard();

                return false;
            });
            
            _m_lMatchItemBgWndList?.Clear();
        }
        
        private void _hideAllMatchItemBg()
        {
            dealAllMatchItemBg((_item) =>
            {
                if (_item != null)
                    _item.hideWnd();

                return false;
            });
            
            _m_lMatchItemBgWndList?.Clear();
        }

        #endregion
    }
}