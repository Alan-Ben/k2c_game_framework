using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTravelMessActorContainer: _ANPGGUIBasicSubWnd<GGUIMonoTravelMessActorContainer>
    {
        private TravelPosRefObj _m_travelPosRef;
        
        //妃子item缓存
        private GTravelactorItemCache<GGUIWndTravelConsortItem , GGUIMonoTravelConsortItem> _m_cacheConsortItem;
        //npcItem缓存
        private GTravelactorItemCache<GGUIWndTravelNpcItem , GGUIMonoTravelNpcItem> _m_cacheNpcItem;
        private bool _m_isShowUp;
        private bool _m_isNpcLeave;
        private bool _m_isFiltUnGet;
        private List<GGUIWndTravelConsortItem> _m_showConsortList;
        private bool _m_isUnlock;


        public GGUIWndTravelMessActorContainer(GGUIMonoTravelMessActorContainer _wnd) : base(_wnd)
        {
            _m_showConsortList = new List<GGUIWndTravelConsortItem>();
            initWnd();
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

        protected override void _onDiscard()
        {
            _m_showConsortList?.Clear();
            
            if(_m_cacheConsortItem != null)
                _m_cacheConsortItem.discard();
            _m_cacheConsortItem = null;
            
            if(_m_cacheNpcItem != null)
                _m_cacheNpcItem.discard();
            _m_cacheNpcItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            //item 缓存池
            if (wnd.itemContainer != null)
            {
                if (wnd.consortItem != null)
                {
                    _m_cacheConsortItem = new GTravelactorItemCache<GGUIWndTravelConsortItem , GGUIMonoTravelConsortItem>(wnd.itemContainer, 0, 5);
                    _m_cacheConsortItem.init(wnd.consortItem);
                }

                if (wnd.npcItem != null)
                {
                    _m_cacheNpcItem = new GTravelactorItemCache<GGUIWndTravelNpcItem , GGUIMonoTravelNpcItem>(wnd.itemContainer, 0, 2);
                    _m_cacheNpcItem.init(wnd.npcItem);
                }
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_travelPosRef"></param>
        /// <param name="_isShowUp">是否显示提升效果</param>
        /// <param name="_isNpcLeave">是否过滤Npc</param>
        /// <param name="_isFiltUnGet">过滤未获得妃子</param>
        public void setInfo(TravelPosRefObj _travelPosRef, bool _isShowUp, bool _isNpcLeave, bool _isFiltUnGet, bool _isUnlock = true)
        {
            _m_travelPosRef = _travelPosRef;
            _m_isShowUp = _isShowUp;
            _m_isNpcLeave = _isNpcLeave;
            _m_isFiltUnGet = _isFiltUnGet;
            _m_isUnlock = _isUnlock;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (null == _m_travelPosRef)
                return;

            
            //先重置item
            _resetAllItem();

            //去重展示妃子item
            Dictionary<long,ConsortInfo> consortShowDataDic = new Dictionary<long, ConsortInfo>();
            NPCommonKeyValueInfo keyValueInfo = null;
            // for (int i = 0; i < _m_travelPosRef.got_consort_weight.Count; i++)
            // {
            //     keyValueInfo = _m_travelPosRef.got_consort_weight[i];
            //     if(null == keyValueInfo)
            //         continue;
            //     if(consortShowDataDic.ContainsKey(keyValueInfo.key))
            //         continue;
            //     GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(keyValueInfo.key);
            //     if(null == consortRefObj)
            //         continue;
            //     //过滤未获得，并且妃子未获得
            //     if(_m_isFiltUnGet && NPPlayer.instance.consortComp.getConsortUnlockType(consortRefObj.id) == EGameCommonUnlockType.LOCK)
            //         continue;
            //     consortShowDataDic.Add(keyValueInfo.key,new ConsortInfo(consortRefObj));
            // }
            // for (int i = 0; i < _m_travelPosRef.unget_consort_weight.Count; i++)
            // {
            //     keyValueInfo = _m_travelPosRef.unget_consort_weight[i];
            //     if(null == keyValueInfo)
            //         continue;
            //     if(consortShowDataDic.ContainsKey(keyValueInfo.key))
            //         continue;
            //     GConsortRefObj consortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(keyValueInfo.key);
            //     if(null == consortRefObj)
            //         continue;
            //     consortShowDataDic.Add(keyValueInfo.key,new ConsortInfo(consortRefObj));
            // }
            //
            // foreach (ConsortInfo showData in consortShowDataDic.Values)
            // {
            //     GGUIWndTravelConsortItem consortItem = _m_cacheConsortItem.popAccessItem();
            //     if (consortItem != null)
            //     {
            //         consortItem.showWnd();
            //         consortItem.setInfo(showData,_m_isShowUp, _m_isUnlock);
            //         _m_showConsortList.Add(consortItem);
            //     }
            // }
            //
            // //展示npcitem
            // for (int i = 0; i < _m_travelPosRef.npc_event_weight.Count; i++)
            // {
            //     keyValueInfo = _m_travelPosRef.npc_event_weight[i];
            //     if(null == keyValueInfo)
            //         continue;
            //     GTravelNpcEventRefObj npcEventRefObj = GRefdataCoreMgr.instance.travelNpcEventCore.getRef(keyValueInfo.key);
            //     if(null == npcEventRefObj)
            //         continue;
            //     NPNPCRefObj npcRef = GRefdataCoreMgr.instance.npcRefCore.getRef(npcEventRefObj.npc_id);
            //     if(null == npcRef)
            //         continue;
            //     GGUIWndTravelNpcItem npcItem = _m_cacheNpcItem.popAccessItem();
            //     if (npcItem != null)
            //     {
            //         npcItem.showWnd();
            //         npcItem.setInfo(npcRef,_m_isNpcLeave, _m_isUnlock);
            //     }
            // }
        }
        //重置item
        private void _resetAllItem()
        {
            if (_m_cacheConsortItem != null)
                _m_cacheConsortItem.pushBackAllAccessItem();

            if (_m_cacheNpcItem != null)
                _m_cacheNpcItem.pushBackAllAccessItem();
        }
    }
}