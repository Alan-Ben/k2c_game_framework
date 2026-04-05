using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 文本打字机效果
    /// </summary>
    public class GGUICustomMonoTypewriter : MonoBehaviour
    {
        [ALHeader("文本翻译key")]
        public string transKey;
        [ALHeader("文本txt")]
        public TextEx txtContent;
        [ALHeader("延时开始打字机效果时间（秒）")]
        public float delayShowTextSec;
        [ALHeader("每个字符显示间隔时间（毫秒）")]
        public float perCharShowIntervalMs;
        [ALHeader("点击显示全部文本内容按钮")]
        public GameObject btnShowAllText;


        private string _m_sContent;//去掉标签打字机效果用的内容文本
        private string _m_sOriContent;//原始内容文本
        private int _m_iPreShowLength;//上次显示的长度
        private long _m_lShowSerialize;//显示序列
        private bool _m_bTypewriterIsShowing;//是否正在显示打字机效果
        private long _m_fTypewriterShowStartTimeMs;//打字机文本开始显示的时间
#if NP_GAME
        private NPDialogueTextTagDealer _m_tagDealer;//标签处理器
#endif
        private ALCommonEnableTaskController _m_tickTask;//文本打字机效果任务


#if NP_GAME
        private void Awake()
        {
            ALUGUICommon.combineBtnClick(this.btnShowAllText, _showAllBtnClick);
            _m_tagDealer = new NPDialogueTextTagDealer();
        }

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            if (string.IsNullOrEmpty(transKey))
                return;

            //设置默认状态
            ALUGUICommon.setGameObjEnable(this.btnShowAllText, false);
            ALUGUICommon.setLabelTxt(this.txtContent, "");
            _m_lShowSerialize = ALSerializeOpMgr.next();

            //原始文本
            _m_sOriContent = TextTranslate.instance.getLanguage(transKey);
            //初始化标签信息，并获取返回去除标签后的文本内容
            _m_sContent = _m_tagDealer?.initTagInfo(_m_sOriContent);

            if (delayShowTextSec > 0)
            {
                long serialize = _m_lShowSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (this == null || this.gameObject == null || !this.gameObject.activeInHierarchy || _m_lShowSerialize != serialize)
                        return;

                    showTextTypewriter();
                }, delayShowTextSec);
            }
            else
                showTextTypewriter();
        }

        private void OnDisable()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_tickTask.setDisable();
            _typewriterShowDone();
            _m_tagDealer?.clear();
        }

        private void OnDestroy()
        {
            ALUGUICommon.uncombineBtnClick(this.btnShowAllText, _showAllBtnClick);
            _m_tagDealer?.clear();
            _m_tagDealer = null;
        }

        /// <summary>
        ///  显示打字机效果
        /// </summary>
        public void showTextTypewriter()
        {
            if (this == null || this.gameObject == null)
                return;

            if (_m_bTypewriterIsShowing)//若还有打字机效果在显示, 直接完成
                _typewriterShowDone();

            //打字机每个字符显示间隔 <= 0, 直接显示文本
            if (this.perCharShowIntervalMs < 0 || Mathf.Approximately(this.perCharShowIntervalMs, 0))
                ALUGUICommon.setLabelTxt(this.txtContent, _m_sOriContent);
            else
                _startTypewriterShow();//开始显示打字机效果
        } 
        
        /// <summary>
        /// 开始显示打字机效果
        /// </summary>
        private void _startTypewriterShow()
        {
            if (this == null || this.gameObject == null)
                return;

            ALUGUICommon.setGameObjEnable(this.btnShowAllText, true);
            _m_bTypewriterIsShowing = true;
            _m_fTypewriterShowStartTimeMs = TimeUtil.getTimeStampMill();//记录打字机开始显示时间
            _m_iPreShowLength = -1;
            _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tickAction, 0);
        }

        /// <summary>
        /// 每帧执行任务
        /// </summary>
        private void _tickAction()
        {
            if (this == null || this.gameObject == null || string.IsNullOrEmpty(_m_sContent) || perCharShowIntervalMs < 0 || Mathf.Approximately(perCharShowIntervalMs, 0))
            {
                _typewriterShowDone();
                return;
            }

            long typewriterShowPassTimeMs = TimeUtil.getTimeStampMill() - _m_fTypewriterShowStartTimeMs;//开始显示打字机效果经过的时间
            int curShowLength = (int)(typewriterShowPassTimeMs / perCharShowIntervalMs);//本次需要显示的文本长度
            if (curShowLength >= _m_sContent.Length)//若本次显示的文本长度超过了文本总长度
            {
                _typewriterShowDone();
                return;
            }

            if (curShowLength >= 0 && curShowLength != _m_iPreShowLength)//若本次显示的文本长度与上次不同
            {
                ALUGUICommon.setLabelTxt(this.txtContent, _m_tagDealer?.applyTag(_m_sContent.Substring(0, curShowLength)));
                _m_iPreShowLength = curShowLength;
            }
        }

        /// <summary>
        /// 显示所有文本内容按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _showAllBtnClick(GameObject _go)
        {
            _typewriterShowDone();
        }

        /// <summary>
        /// 打字机效果显示完成
        /// </summary>
        private void _typewriterShowDone()
        {
            if (!_m_bTypewriterIsShowing)//若当前没有打字机效果在显示, 直接返回
                return;

            _m_tickTask.setDisable();

            if (this != null)
            {
                ALUGUICommon.setGameObjEnable(this.btnShowAllText, false);
                ALUGUICommon.setLabelTxt(this.txtContent, _m_sOriContent);
            }

            _m_bTypewriterIsShowing = false;
        }
#endif
    }
}