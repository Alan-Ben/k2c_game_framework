using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 按钮通用CD脚本，在CD时间内设置按钮不可点击并置灰
    /// </summary>
    public class NPGGUICustomMonoButtonClickCD : MonoBehaviour
    {
        [Header("点击按钮")]
        public Selectable button;
        [Header("延时时间秒")]
        public float CDTimeSec = 1.5f;
        
        
        [Header("不可点击时需要变灰的列表")]
        public List<MaskableGraphic> grayList;
        [Header("不可点击时候的点击按钮，默认做一个跟上面一样的，默认会隐藏，功能按钮不生效时候会显示")]
        public GameObject disableButton;
        [Header("不可点击时候点击按钮时的提示文本的翻译Key")]
        public string buttonClickTip;

        private long _m_serialize = 0;


        //显示
        private void OnEnable()
        {
#if NP_GAME
            if (button == null)
                return;
            
            //添加点击监听
            ALUGUICommon.combineBtnClick(button.gameObject, _onClickBtn);
            ALUGUICommon.combineBtnClick(disableButton, _onClickTipBtn);

            //重置状态
            _resetState();
            _m_serialize++;
#endif
        }

        //失效
        private void OnDisable()
        {
#if NP_GAME
            if (button == null)
                return;

            //取消点击监听
            ALUGUICommon.uncombineBtnClick(button.gameObject,_onClickBtn);
            ALUGUICommon.uncombineBtnClick(disableButton,_onClickTipBtn);

            //重置状态
            _resetState();
            _m_serialize++;
#endif
        }

        //销毁
        private void OnDestroy()
        {
            _m_serialize++;
        }

        //不可点击提示
        private void _onClickTipBtn(GameObject _go)
        {
#if NP_GAME
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TextTranslate.instance.getLanguage(buttonClickTip));
#endif
        }
        
        private void _onClickBtn(GameObject _go)
        {
#if NP_GAME
            if (button == null || !button.interactable)
                return;

            //下一帧设置按钮无效
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (button == null || button.gameObject == null)
                    return;

                //提示按钮生效
                ALUGUICommon.setGameObjEnable(disableButton, true);
                //功能按钮设置不可点击
                button.interactable = false;
                
                //设置置灰
                GGameCommonInfo.grayImage(grayList);
                _m_serialize++;

                long serialize = _m_serialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if(serialize != _m_serialize)
                        return;
                    
                    _resetState();
                }, CDTimeSec);
            });
#endif
        }

        //重置状态
        private void _resetState()
        {
#if NP_GAME
            if (button == null || button.gameObject == null)
                return;

            //提示按钮不生效
            ALUGUICommon.setGameObjEnable(disableButton, false);
            //功能按钮生效
            button.interactable = true;
            //取消置灰
            GGameCommonInfo.disgrayImage(grayList);
            //如果按钮有点击动画，需要重置为Normal状态，否则关闭点击事件后会一直停留在Disable状态，导致点击按钮不会有点击动画
            Animator buttonAnimator = button.animator;
            if(buttonAnimator != null)
                buttonAnimator.Play("Normal");
#endif
        }
    }
}