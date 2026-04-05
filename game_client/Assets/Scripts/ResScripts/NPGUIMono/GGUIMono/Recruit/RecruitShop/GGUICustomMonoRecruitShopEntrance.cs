using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUICustomMonoRecruitShopEntrance : MonoBehaviour
    {
        [ALHeader("招募商店ID")]
        public long shopId;

        [ALHeader("商店名")]
        public TextEx shopName;

        [ALHeader("已招募数量")]
        public TextEx hadRecruitNum;
        [ALHeader("已招募数量key")]
        public string hadRecruitNumKey;
        
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("商店UI资源路径ID")]
        public long shopUIResPathId;

        [ALHeader("红点脚本")]
        public NPGGUIMonoCommonRedTip monoRedTip;

#if NP_GAME
        private RecruitShopInfo _m_recruitShopInfo;//招募商店数据
        private NPGGUIWndCommonRedTip _m_wRedTip;// 红点窗口
#endif
        
        private void Awake()
        {
#if NP_GAME
            _m_recruitShopInfo = NPPlayer.instance.recruitComp.getShopInfo(shopId);
            
            if (monoRedTip != null)
                _m_wRedTip = new NPGGUIWndCommonRedTip(monoRedTip);
#endif

            ALUGUICommon.combineBtnClick(btnClick, _onBtnClick);
        }

        private void OnDestroy()
        {
#if NP_GAME
            _m_recruitShopInfo = null;
            
            _m_wRedTip?.discard();
            _m_wRedTip = null;
#endif

            ALUGUICommon.uncombineBtnClick(btnClick, _onBtnClick);
        }

        private void OnEnable()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_RECRUIT_ENTRANCE, _onSimulateClickEntrance);
            _refreshWnd();
        }
        
        private void OnDisable()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_RECRUIT_ENTRANCE, _onSimulateClickEntrance);

#if NP_GAME
            _m_wRedTip?.hideWnd();
#endif
        }

        private void _refreshWnd()
        {
#if NP_GAME
            if(_m_recruitShopInfo == null || _m_recruitShopInfo.recruitShopRefObj == null)
                return;

            if (_m_wRedTip != null)
            {
                _ARedTipNode nodeItem = RedTipMgr.instance.getNodeByRefRedTipId(_m_recruitShopInfo.recruitShopRefObj.red_tip_id);
                if (nodeItem != null && nodeItem.needShow())
                {
                    _m_wRedTip.showWnd();
                    _m_wRedTip.showRedTipNum((int)nodeItem.getCount());       
                }
                else
                {
                    _m_wRedTip.hideWnd();
                }
            }
            
            ALUGUICommon.setLabelTxt(shopName, TextTranslate.instance.getLanguage(_m_recruitShopInfo.recruitShopRefObj.name));
            
            string key = string.IsNullOrEmpty(hadRecruitNumKey) ? TransKeyConst.common_currentTotalNum_num_num : hadRecruitNumKey;
            ALUGUICommon.setLabelTxt(hadRecruitNum, TextTranslate.instance.getLanguage(key, _m_recruitShopInfo.hadRecruitCount, _m_recruitShopInfo.recruitShopRefObj.recruitRefObjList?.Count ?? 0));
#endif
        }
        
        /// <summary>
        /// 按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnClick(GameObject _go)
        {
#if NP_GAME
            QueueMgr.instance.AddNode(new GNodeRecruitShop(_m_recruitShopInfo, shopUIResPathId));
#endif
        }

        //模拟点击招募顾问入口
        private void _onSimulateClickEntrance(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long targetShopId = (long) _objects[0];
            if(targetShopId == shopId)
                _onBtnClick(null);
        }
    }
}