using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 跑马灯列表控制脚本，用于控制不同跑马灯预制体在有跑马灯时才显示，方便控制自适应位置
    /// </summary>
    public class GGUICustomMonoMarqueeListControl : MonoBehaviour
    {
        [ALHeader("跑马灯预制体列表")]
        public List<GGUICustomMonoMarquee> marqueeList;

        private void Awake()
        {
#if NP_GAME
            if (marqueeList != null)
            {
                for (int i = 0; i < marqueeList.Count; i++)
                {
                    if (marqueeList[i] != null)
                        marqueeList[i].onShowDone += _onMarqueeShowDone;
                }
            }
#endif
        }

        private void OnEnable()
        {
#if NP_GAME
            WinMsg.RegisterMsg(WinMsgType.ON_ADD_MARQUEE, _onAddMarquee);
            if (marqueeList != null)
            {
                for (int i = 0; i < marqueeList.Count; i++)
                {
                    if (marqueeList[i] != null)
                    {
                        //默认先隐藏跑马灯
                        ALUGUICommon.setGameObjEnable(marqueeList[i].gameObject, false);

                        //设置子对象忽略可见跑马灯预制体记录，在这里统一记录
                        marqueeList[i].setIgnoreShowRecord(true);
                        //添加到可见跑马灯预制体队列里
                        NPPlayer.instance.marqueeComp.recordShowInfo.addMarqueeQueueRecord(marqueeList[i].showPosIdList, marqueeList[i]);
                    }
                }
            }

            //检查是否可以展示跑马灯
            _checkChanShow();
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            WinMsg.UnregisterMsg(WinMsgType.ON_ADD_MARQUEE, _onAddMarquee);

            //从可见跑马灯预制体队列里移除
            for (int i = 0; i < marqueeList.Count; i++)
            {
                if (marqueeList[i] != null)
                {
                    NPPlayer.instance.marqueeComp.recordShowInfo.removeMarqueeQueueRecord(marqueeList[i].showPosIdList, marqueeList[i]);
                }
            }
#endif
        }

        private void OnDestroy()
        {
#if NP_GAME
            if (marqueeList != null)
            {
                for (int i = 0; i < marqueeList.Count; i++)
                {
                    if (marqueeList[i] != null)
                        marqueeList[i].onShowDone -= _onMarqueeShowDone;
                }
            }
#endif
        }

#if NP_GAME
        /// <summary>
        /// 检查是否可以展示跑马灯
        /// </summary>
        private void _checkChanShow()
        {
            if (marqueeList == null)
                return;

            for (int i = 0; i < marqueeList.Count; i++)
            {
                if(marqueeList[i] == null || marqueeList[i].showPosIdList == null)
                    continue;

                for (int j = 0; j < marqueeList[i].showPosIdList.Count; j++)
                {
                    MarqueeInfo curMarqueeInfo = NPPlayer.instance.marqueeComp.getMarqueeInfo(marqueeList[i].showPosIdList[j]);
                    //如果有跑马灯数据 或者有记录未展示完的跑马灯，则直接开始展示
                    if (curMarqueeInfo != null || NPPlayer.instance.marqueeComp.recordShowInfo.checkIsRecordPosId(marqueeList[i].showPosIdList[j]))
                    {
                        ALUGUICommon.setGameObjEnable(marqueeList[i].gameObject, true);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 跑马灯展示完回调
        /// </summary>
        /// <param name="_go"></param>
        private void _onMarqueeShowDone(GameObject _go)
        {
            ALUGUICommon.setGameObjEnable(_go, false);
        }

        /// <summary>
        /// 新增跑马灯事件
        /// </summary>
        /// <param name="_objects"></param>
        private void _onAddMarquee(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null)
                return;

            int showPosId = (int)_objects[0];
            if (marqueeList == null)
                return;

            //检查是否有相同的showPosId，有则显示开始展示跑马灯
            for (int i = 0; i < marqueeList.Count; i++)
            {
                if (marqueeList[i] != null && marqueeList[i].showPosIdList != null && marqueeList[i].showPosIdList.Contains(showPosId))
                    ALUGUICommon.setGameObjEnable(marqueeList[i].gameObject, true);
            }
        }
#endif

    }
}