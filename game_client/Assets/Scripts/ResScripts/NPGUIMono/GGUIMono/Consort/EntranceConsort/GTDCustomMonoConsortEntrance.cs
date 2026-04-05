using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子入口CustomMono
    /// </summary>
    public class GTDCustomMonoConsortEntrance : MonoBehaviour
    {
        [ALHeader("形象加载父节点")]
        public Transform parent;
        [ALHeader("形象加载的初始位置")]
        public Vector3 initLocalPos = Vector3.zero;
        [ALHeader("形象加载的初始缩放")]
        public Vector3 initLocalScale = Vector3.one;
        
        [ALHeader("初始播放的动画")]
        public string startAniTag;

        [ALHeader("默认显示的妃子id")]
        public long defaultShowConsortId = 0;
        
        private long _m_lShowSerialize;
        
        //加载的妃子形象goIndex
        private NPGGoIndex _m_ConsortTdShowGoIndex;
        //加载的妃子形象
        private GameObject _m_ConsortTdShowGo;


#if NP_GAME
        /// <summary>
        /// 默认显示妃子的配表展示数据
        /// </summary>
        private ConsortRefShowInfo _m_iDefaultShowConsortRefShowInfo;  
#endif
     
        
        
#if NP_GAME
        private void Awake()
        {
            _m_iDefaultShowConsortRefShowInfo = new ConsortRefShowInfo(defaultShowConsortId);
        }

        private void OnDestroy()
        {
            _m_iDefaultShowConsortRefShowInfo = null;
        }

        private void OnEnable()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshConsort();
            
            WinMsg.RegisterMsgAct(WinMsgType.MAIN_ROOM_WND_SHOW, _refreshConsort);
        }
        
        private void OnDisable()
        {
            long serialize = _m_lShowSerialize = ALSerializeOpMgr.next();

            WinMsg.UnregisterMsgAct(WinMsgType.MAIN_ROOM_WND_SHOW, _refreshConsort);
            
            ALCommonTaskController.CommonActionAddNextFrameTask(()=>
            {
                if(serialize != _m_lShowSerialize)
                    return;
                
                _discardConsortTdShow();
            });
        }

        private void _discardConsortTdShow()
        {
            if(_m_ConsortTdShowGo)
                GGoIndexCacheMgr.instance.pushbackItem(_m_ConsortTdShowGoIndex, _m_ConsortTdShowGo);
            
            _m_ConsortTdShowGoIndex = null;
            _m_ConsortTdShowGo = null;
        }
        
        /// <summary>
        /// 刷新妃子显示
        /// </summary>
        private void _refreshConsort()
        {
            GGottenConsortInfo needShowConsortInfo = null;
            NPGGoIndex consortTdShowGoIndex = null;   
            
            // 先根据客户端存储在服务器的数据获取当前要展示的妃子
            needShowConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(NPPlayer.instance.consortComp.remarkInfo?.getConsortEntranceShowConsortId() ?? 0);
            // 若找不到对应已经有的妃子, 则根据默认妃子id获取
            if (needShowConsortInfo == null)
                needShowConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(defaultShowConsortId);
            // 若默认显示妃子玩家不拥有, 则尝试显示以获取妃子的第一个
            if (needShowConsortInfo == null)
            {
                List<GGottenConsortInfo> consortList = ConsortUtil.getUnlockConsortList(false, (_consortInfo) => true);
                needShowConsortInfo = consortList?.SafeGet(0);
            }
            // 若上述都没有找到已有妃子, 显示默认妃子形象
            if (needShowConsortInfo == null)
            {
                consortTdShowGoIndex = _m_iDefaultShowConsortRefShowInfo?.consortSkinShowInfo?.tdShow;
                NPPlayer.instance.consortComp.remarkInfo?.setConsortEntranceShowConsortId(defaultShowConsortId);
            }
            else
            {
                consortTdShowGoIndex = needShowConsortInfo.consortSkinShowInfo?.tdShow;
                NPPlayer.instance.consortComp.remarkInfo?.setConsortEntranceShowConsortId(needShowConsortInfo.consortId);
            }

            if (consortTdShowGoIndex == null || !consortTdShowGoIndex.isValid())
            {
                _discardConsortTdShow();
                return;
            }

            // 若原来的妃子形象和当前要展示的妃子形象不一致, 则重新加载
            if (_m_ConsortTdShowGoIndex != consortTdShowGoIndex)
            {
                _discardConsortTdShow();
                _m_ConsortTdShowGoIndex = consortTdShowGoIndex;
            }

            if (_m_ConsortTdShowGo == null)
            {
                GGoIndexCacheMgr.instance.popItem(_m_ConsortTdShowGoIndex, (_go) =>
                {
                    if (_go == null)
                        return;
                    
                    _m_ConsortTdShowGo = _go;
                    
                    _m_ConsortTdShowGo.transform.SetParent(parent == null ? transform : parent);
                    _m_ConsortTdShowGo.transform.localPosition = initLocalPos;
                    _m_ConsortTdShowGo.transform.localScale = initLocalScale;
                    
                    _m_ConsortTdShowGo.GetComponent<_AShowCaseCommonResObjAniEffect>()?.playAni(startAniTag);
                });
            }
        }
#endif
    }
}