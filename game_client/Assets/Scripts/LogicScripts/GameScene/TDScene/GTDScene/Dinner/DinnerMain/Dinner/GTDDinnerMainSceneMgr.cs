using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 宴会主场景mono管理器
    /// </summary>
    public class GTDDinnerMainSceneMgr
    {
        private static GTDDinnerMainSceneMgr _m_instance = new GTDDinnerMainSceneMgr();

        public static GTDDinnerMainSceneMgr instance
        {
            get
            {
                if (null == _m_instance)
                    _m_instance = new GTDDinnerMainSceneMgr();
                return _m_instance;
            }
        }

        private bool _m_isInit = false;
        private GTDDinnerMainSceneMono _m_sceneMono;
        private GDinnerInfo _m_dinnerInfo;
        private List<GTDDinnerMainPosItemView> _m_posItemViewList;
        private List<GDinnerJoinerInfo> _m_seatInfoList;

        public GTDDinnerMainSceneMgr()
        {
            _m_posItemViewList = new List<GTDDinnerMainPosItemView>();
        }
        
        public void init(GTDDinnerMainSceneMono _mono, GDinnerInfo _dinnerInfo)
        {
            if (_m_isInit)
                return;
            _m_sceneMono = _mono;
            _m_dinnerInfo = _dinnerInfo;
            GTDDinnerMainPosItemMono posItemMono = null;
            for (int i = 0; i < _m_sceneMono.seatPosList.Count; i++)
            {
                posItemMono = _m_sceneMono.seatPosList[i];
                if(null == posItemMono)
                    continue;
                _m_posItemViewList.Add(new GTDDinnerMainPosItemView(posItemMono));
            }

            _m_isInit = true;

        }

        public void discard()
        {
            _m_sceneMono = null;
            _m_dinnerInfo = null;
            

            foreach (GTDDinnerMainPosItemView posItemView in _m_posItemViewList)
            {
                posItemView.discard();
            }
            _m_posItemViewList.Clear();
            _m_isInit = false;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void show()
        {
            _refreshAll();   
            WinMsg.RegisterMsg(WinMsgType.ON_CUR_LOOK_DINNER_CHG, _onCurLookDinnerChg);
        }

        private void _refreshAll()
        {
            if(null == _m_dinnerInfo)
                return;
            // _m_seatInfoList = _m_dinnerInfo.seatInfoList;
            //
            // GTDDinnerMainPosItemView posItemView = null;
            // for (int i = 0; i < _m_posItemViewList.Count; i++)
            // {
            //     posItemView = _m_posItemViewList[i];
            //     if(null == posItemView)
            //         continue;
            //     if (i < _m_seatInfoList.Count)
            //     {
            //         GDinnerSeatInfo joinInfo = _m_seatInfoList[i];
            //         posItemView.show(joinInfo);
            //     }
            //     else
            //     {
            //         posItemView.hide();
            //     }
            // }

            // _m_ownerPlayer.refresh();
        }
        
        /// <summary>
        /// 当前进入的宴会发生变化的时候
        /// </summary>
        /// <param name="_objs"></param>
        private void _onCurLookDinnerChg(object[] _objs)
        {
            if(_objs.Length < 1)
                return;
            long dinnerId = (long) _objs[0];
            if (dinnerId == _m_dinnerInfo.dinnerId)
            {
                _refreshAll();
            }
        }

        /// <summary>
        /// 隐藏所有
        /// </summary>
        public void hideAll()
        {
            
            GTDDinnerMainPosItemView posItemView = null;
            for (int i = 0; i < _m_posItemViewList.Count; i++)
            {
                posItemView = _m_posItemViewList[i];
                if(null == posItemView)
                    continue;
                posItemView.hide();
            }
            WinMsg.UnregisterMsg(WinMsgType.ON_CUR_LOOK_DINNER_CHG, _onCurLookDinnerChg);
        }
        


        public void dealClickSeatItem(GDinnerJoinerInfo _joinerInfo)
        {
            GTDDinnerMainPosItemView posItemView = null;
            for (int i = 0; i < _m_posItemViewList.Count; i++)
            {
                posItemView = _m_posItemViewList[i];
                if(null == posItemView)
                    continue;
                posItemView.dealClickSeatItem();
            }
        }
    }
}