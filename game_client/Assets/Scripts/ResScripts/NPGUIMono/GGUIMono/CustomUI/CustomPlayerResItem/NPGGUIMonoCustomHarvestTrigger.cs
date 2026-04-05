using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 触发收集展示的CustomMono
    /// </summary>
    public class NPGGUIMonoCustomHarvestTrigger : MonoBehaviour
    {
        [ALHeader("展示的收集对象")]
        public NPCommonCostItem showItem;

        [ALHeader("起始点")]
        public RectTransform srcUIObj;
        
        [ALHeader("特殊粒子id")]
        public long specialParticleId = 0;

        private bool _m_bNeedShowParticle;
        
        private void OnEnable()
        {
            if(showItem == null || !showItem.IsValid || srcUIObj == null || _m_bNeedShowParticle)
                return;

            _m_bNeedShowParticle = true;
            // 因为刚加载出来时可能是显示状态, 随后被隐藏, 所以这里延迟一帧处理
            ALCommonActionMonoTask.addNextFrameTask(dealShowItemParticle);
        }

        private void OnDisable()
        {
            // 这里不需要设置为false, 因为GCommon.showItemParticle是全局展示, 不会因为物体被隐藏而停止
            // _m_bNeedShowParticle = false;
        }

        private void dealShowItemParticle()
        {
            if(showItem == null || !showItem.IsValid || srcUIObj == null || !gameObject.activeInHierarchy || !_m_bNeedShowParticle)
                return;

#if NP_GAME
            GCommon.showItemParticle(showItem.getItemType(), showItem.subId, showItem.getCount(), srcUIObj, specialParticleId,
                () =>
                {
                    WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.HARVEST_DONE);
                    _m_bNeedShowParticle = false;
                });
#endif
        }
    }
}