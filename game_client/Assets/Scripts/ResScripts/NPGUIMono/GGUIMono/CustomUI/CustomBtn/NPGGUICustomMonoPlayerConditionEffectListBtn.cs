using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    //单个效果节点的信息
    [System.Serializable]
    public class NPGGUIMonoPlayerConditionEffectInfo
    {
        public string tutorialEffectStr;//效果字符串
        public string clickConditionStr;//点击的判断条件字符串

        //效果数据
        private bool _m_bIsInited = false;
        private List<NPPlayerEffectSerializeInfo> _m_lPlayerEffectList;
        private NPPlayerConditionGroupObj _m_lClickCondition;
        public List<NPPlayerEffectSerializeInfo> playerEffectList
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lPlayerEffectList;

                _init();

                return _m_lPlayerEffectList;
            }
        }
        public NPPlayerConditionGroupObj clickCondition
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lClickCondition;

                _init();

                return _m_lClickCondition;
            }
        }

        //初始操作
        protected void _init()
        {
            _m_lPlayerEffectList = NPPlayerEffectSerializeInfo.readEffectList(tutorialEffectStr);
            _m_lClickCondition = NPPlayerConditionGroupObj.readConditionGroupList(clickConditionStr, "custLB");
            _m_bIsInited = true;
        }

        /** 处理函数 */
        public bool dealEffect()
        {
            //调用点击处理
            //判断条件  当提示信息为空时不弹
            if (!clickCondition.IsEnable(null))
            {
                return false;
            }

            NPPlayerEffectSerializeInfo.dealEffect(playerEffectList, null);

            return true;
        }
    }


    /// <summary>
    /// 效果条件判断脚本
    /// </summary>
    public class NPGGUICustomMonoPlayerConditionEffectListBtn : _AALBasicUIWndMono
    {
        public GameObject Btn; // 按钮

        //需要处理的效果队列
        public List<NPGGUIMonoPlayerConditionEffectInfo> dealPlayerEffectList;
        //最多处理多少个效果，-1表示全处理
        public int maxDealEffect;
        //当没有效果可执行的时候的提示信息翻译索引
        public string noticeTxt;

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
            //逐个效果处理
            int canDealCount = maxDealEffect;
            //是否处理成功
            bool dealSuc = false;
            NPGGUIMonoPlayerConditionEffectInfo info = null;
            for (int i = 0; i < dealPlayerEffectList.Count; i++)
            {
                info = dealPlayerEffectList[i];
                if (null == info)
                    continue;

                if (info.dealEffect())
                {
                    //设置成功
                    dealSuc = true;
                    //减少可处理数量
                    canDealCount--;
                }

                //判断数量
                if (canDealCount == 0)
                    break;
            }

            //调用点击处理
            //判断条件  当提示信息为空时不弹
            if (!dealSuc && !string.IsNullOrEmpty(noticeTxt))
            {
                //展示提示
#if NP_GAME
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(noticeTxt), TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
#endif
                return;
            }
        }

    }
}

