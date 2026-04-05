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
    /// <summary>
    /// 按钮集合，且只可选中一个按钮的自定义UI，选中按钮后触发配置effect
    /// </summary>
    public class NPGGUICustomMonoSingleSelectBtn : MonoBehaviour
    {
        public GameObject Btn; // 按钮
        public GameObject selectTag; //选中标记

        public string playerEffectStr;//效果字符串
        public string clickConditionStr;//点击的判断条件字符串

        //提示信息翻译索引
        public string noticeTxt;

        //效果数据
        private bool _m_bIsInited = false;
        private List<NPPlayerEffectSerializeInfo> _m_lPlayerEffectList;
        private NPPlayerConditionGroupObj _m_lClickCondition;
        //父容器对象
        private NPGGUICustomMonoSingleSelectBtnGroup _m_parentGroup;

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

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _dealClickBtn);
        }

        /// <summary>
        /// 设置父对象
        /// </summary>
        /// <param name="_parent"></param>
        public void initParent(NPGGUICustomMonoSingleSelectBtnGroup _parent)
        {
            _m_parentGroup = _parent;
        }

        /// <summary>
        /// 处理选中以及取消选中的处理
        /// </summary>
        public void dealSelect()
        {
            NPPlayerEffectSerializeInfo.dealEffect(playerEffectList, null);
        }

        //初始操作
        protected void _init()
        {
            _m_lPlayerEffectList = NPPlayerEffectSerializeInfo.readEffectList(playerEffectStr);
            _m_lClickCondition = NPPlayerConditionGroupObj.readConditionGroupList(clickConditionStr, "custB");
            _m_bIsInited = true;
        }

        /** 点击响应事件 */
        protected void _dealClickBtn(GameObject _go)
        {
            //调用点击处理
            //判断条件  当提示信息为空时不弹
            if (!clickCondition.IsEnable(null) && !noticeTxt.Equals(""))
            {
#if NP_GAME
                //展示提示
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(noticeTxt), TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
#endif
                return;
            }

            //此时设置选中处理
            if (null != _m_parentGroup)
                _m_parentGroup.setSelectObj(this);
        }
    }
}

