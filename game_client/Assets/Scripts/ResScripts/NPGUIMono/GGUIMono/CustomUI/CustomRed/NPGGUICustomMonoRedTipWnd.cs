using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [System.Serializable]
    public class NPGGUIRedTipTypeMono
    {
        [ALHeader("注释，会展示在列表上")]
        public string annotation;
        [ALHeader("红点Id")]
        public long redTipId;
        [ALHeader("红点控制显隐的GO,有红点显示，无红点隐藏")]
        public GameObject[] goRedTips;
        [ALHeader("有红点隐藏，无红点显示")]
        public GameObject[] goRedHide;
        [ALHeader("红点上的数字（可不配置）")]
        public Text texRedTipCount;
        
        [ALHeader("动画器")] public Animation anim;
        [ALHeader("红点生效时需要的动画的名字")] public string animName;
    }
    
    /// <summary>
    ///  玩家自定义的红点显示窗口 _拓展
    /// </summary>
    public class NPGGUICustomMonoRedTipWnd : MonoBehaviour
    {
        [ALInfo("这个脚本不能挂在控制显隐的红点物体上")]
        
        [ALHeader("红点列表（如果有多个其他红点可配置在这里）")]
        public List<NPGGUIRedTipTypeMono> redTipList;
        
        //是否需要检测
        private bool _m_bNeedCheck = false;

#if UNITY_EDITOR
        private void Awake()
        {
            if (redTipList != null)
            {
                foreach (NPGGUIRedTipTypeMono gguiRedTipTypeMono in redTipList)
                {
                    if(null == gguiRedTipTypeMono)
                        continue;
                    
                    foreach (var goRedTip in gguiRedTipTypeMono.goRedTips)
                    {
                        if (gameObject == goRedTip)
                            Debug.LogError($"【这个问题会导致红点显示异常！！】 本窗口{this.gameObject.name}，不能把红点的脚本节点挂在goRedTip上面");
                    }   
                }
            }
        }
#endif

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_bNeedCheck = true;

            WinMsg.RegisterMsg(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChg);

            //到管理对象中进行处理
            _check();
        }

        private void OnDisable()
        {
            _m_bNeedCheck = true;

            WinMsg.UnregisterMsg(WinMsgType.ON_RED_TIP_CHANGE, _onRedTipChg);
            //到管理对象中进行处理
            _check();
        }

        private void _onRedTipChg(params object[] _objects)
        {
#if NP_GAME
            _m_bNeedCheck = true;
            //到管理对象中进行处理
            _check();
#endif
        }

        private void _onFunctionUnlock(params object[] _objects)
        {
#if NP_GAME
            _m_bNeedCheck = true;
            //到管理对象中进行处理
            _check();
#endif
        }

        protected void _check()
        {
            if(!_m_bNeedCheck || null == this || null == gameObject)
            {
#if UNITY_EDITOR
                if(_m_bNeedCheck)
                    UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
                return;
            }

            _m_bNeedCheck = false;

            if(gameObject.activeInHierarchy)
            {
#if NP_GAME
                if(redTipList != null)
                {
                    foreach (var redTip in redTipList)
                    {
                        if (redTip == null)
                            continue;
                        
                        _ARedTipNode nodeItem = RedTipMgr.instance.getNodeByRefRedTipId(redTip.redTipId);
                        if (nodeItem == null)
                        {
#if UNITY_EDITOR
                            Debug.LogError($"本窗口的{this.gameObject.name}：redTipList设置id错误,id:{redTip.redTipId}", this.gameObject);
#endif
                            continue;
                        }

                        bool needShow = nodeItem.needShow();
                        
                        ALUGUICommon.setGameObjEnable(redTip.goRedTips, needShow);
                        ALUGUICommon.setGameObjEnable(redTip.goRedHide, !needShow);
                        ALUGUICommon.setGameObjEnable(redTip.texRedTipCount, needShow);
                        if (needShow)
                        {
                            ALUGUICommon.setLabelTxt(redTip.texRedTipCount, nodeItem.getCount());
                            _playerAnim(redTip.anim,redTip.animName);
                        }

                    }
                }
#endif
            }
        }

        private void _playerAnim(Animation _anim,string _animName)
        {
            if (_anim == null && string.IsNullOrEmpty(_animName))
                return;
            _anim.Sample(_animName, 0f);
            _anim.ForcePlay(_animName, 0f);
        }
        
    }
}