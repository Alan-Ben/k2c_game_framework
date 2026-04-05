using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    public class GUICustomMonoConsortEntranceBubble : MonoBehaviour
    { 
        [ALHeader("打字机附加窗口")]
        public GGUIMonoTextTypewriter monoTypewriter;
         
        [ALHeader("文本显示完气泡隐藏的延迟时间")]
        public float hideBubbleDelay = 3f;
        [ALHeader("气泡隐藏后再显示下一个气泡的延迟时间")]
        public float showNextBubbleDelay = 3f;

        /// <summary>
        /// 气泡显示文本列表
        /// </summary>
        [NotNull] private List<string> _m_lBubbleText = new List<string>();

        private int _m_iNowShowTextIndex;//当前显示的文本索引
        
        private long _m_lShowSerialize;//显示序列号
        
#if NP_GAME
        /// <summary>
        /// 气泡显示窗口
        /// </summary>
        private GGUIWndTextTypewriter _m_wTextTypewriter;
#endif

#if NP_GAME
        private void Awake()
        {
            if (monoTypewriter != null)
                _m_wTextTypewriter = new GGUIWndTextTypewriter(monoTypewriter);
        }

        private void OnDestroy()
        {
            _m_lBubbleText.Clear();
            _m_iNowShowTextIndex = -1;
            
            _m_wTextTypewriter?.discard();
            _m_wTextTypewriter = null;
        }

        private void OnEnable()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            
            _updateShowBubbleText();
            _showBubbleText();
            
            WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_ENTRANCE_CONSORT_CHG, _onConsortEntranceConsortChg);
        }

        private void OnDisable()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_ENTRANCE_CONSORT_CHG, _onConsortEntranceConsortChg);

            _m_wTextTypewriter?.hideWnd();
        }

        /// <summary>
        /// 更新气泡文本
        /// </summary>
        private void _updateShowBubbleText()
        {
            long selectConsortId = NPPlayer.instance.consortComp.remarkInfo?.getConsortEntranceShowConsortId() ?? 0;
            GConsortRefObj selectConsortRefObj = GRefdataCoreMgr.instance.consortRefCore.getRef(selectConsortId);
            
            _m_lBubbleText.Clear();
            _m_iNowShowTextIndex = -1;
            if(selectConsortRefObj != null && selectConsortRefObj.entrance_bubble_key_list != null)
                _m_lBubbleText.AddRange(selectConsortRefObj.entrance_bubble_key_list);
        }
        
        /// <summary>
        /// 进行气泡文本显示
        /// </summary>
        private void _showBubbleText()
        {
            if(_m_wTextTypewriter == null)
                return;
            
            int totalBubbleTextCount = _m_lBubbleText.Count;
            string bubbleKey = string.Empty;
            if (totalBubbleTextCount == 1)
                bubbleKey = _m_lBubbleText[0];
            else if (totalBubbleTextCount > 1)
            {
                int showKeyIndex = 0;
                if (_m_iNowShowTextIndex < 0)
                {
                    showKeyIndex = Random.Range(0, totalBubbleTextCount);
                }
                else
                {
                    showKeyIndex = Random.Range(0, totalBubbleTextCount - 1);
                    if (showKeyIndex >= _m_iNowShowTextIndex)
                        showKeyIndex ++;
                }

                _m_iNowShowTextIndex = showKeyIndex % totalBubbleTextCount;
                bubbleKey = _m_lBubbleText[_m_iNowShowTextIndex];
            }

            if (string.IsNullOrEmpty(bubbleKey))
            {
                _m_wTextTypewriter.hideWnd();
                return;
            }

            long serialize = _m_lShowSerialize;
            _m_wTextTypewriter.showWnd();
            _m_wTextTypewriter.showTextTypewriter(TextTranslate.instance.getLanguage(bubbleKey), null, () =>
            {
                if (serialize != _m_lShowSerialize)
                    return;
                
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != _m_lShowSerialize)
                        return;
                    
                    _m_wTextTypewriter?.hideWnd();
                }, hideBubbleDelay);
                
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serialize != _m_lShowSerialize)
                        return;

                    _showBubbleText();
                }, hideBubbleDelay + showNextBubbleDelay);
            });
        }

        /// <summary>
        /// 当入口妃子发生变化时
        /// </summary>
        private void _onConsortEntranceConsortChg()
        {
            _updateShowBubbleText();
            _showBubbleText();
        }
#endif
    }
}