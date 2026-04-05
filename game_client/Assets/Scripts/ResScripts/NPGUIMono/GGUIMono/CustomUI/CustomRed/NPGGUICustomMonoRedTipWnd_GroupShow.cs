using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //红点组mono
    [System.Serializable]
    public class NPGGUIRedTipGroupMono
    {
        [ALHeader("红点列表，这边配置的多个红点会按列表顺序按优先级显示")]
        public List<NPGGUIRedTipTypeMono> redTipList;
    }

    /// <summary>
    ///  玩家自定义的红点显示窗口 支持同一个组里面的红点互斥显示
    /// </summary>
    public class NPGGUICustomMonoRedTipWnd_GroupShow : MonoBehaviour
    {
        [ALInfo("这个脚本不能挂在控制显隐的红点物体上 \n" +
                "支持同一个组里面的红点互斥显示")]

        [ALHeader("红点组列表")]
        public List<NPGGUIRedTipGroupMono> redGroupMonoList;

        //是否需要检测
        private bool _m_bNeedCheck = false;

#if UNITY_EDITOR
        private void Awake()
        {
            if (redGroupMonoList != null)
            {
                foreach (NPGGUIRedTipGroupMono npgguiRedTipGroupMono in redGroupMonoList)
                {
                    if(null == npgguiRedTipGroupMono)
                        continue;
                    
                    foreach (NPGGUIRedTipTypeMono gguiRedTipTypeMono in npgguiRedTipGroupMono.redTipList)
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
                foreach (NPGGUIRedTipGroupMono npgguiRedTipGroupMono in redGroupMonoList)
                {
                    if(null == npgguiRedTipGroupMono)
                        continue;

                    //这边开始的循环是同个组的红点，同个组里面的红点按照顺序互斥只显示第一个亮的
                    if(npgguiRedTipGroupMono.redTipList != null)
                    {
                        bool thisGroupHasShow = false; //这个组里面是否有需要显示了的
                        foreach (var redTip in npgguiRedTipGroupMono.redTipList)
                        {
                            if (redTip == null)
                                continue;
                        
                            _ARedTipNode nodeItem = RedTipMgr.instance.getNodeByRefRedTipId(redTip.redTipId);
                            if (nodeItem == null)
                            {
#if UNITY_EDITOR
                                Debug.LogError($"本窗口的{this.gameObject.name}：redTipList设置id错误", this.gameObject);
#endif
                                continue;
                            }

                            //本身是否显示
                            bool selfNeedShow = nodeItem.needShow();
                            //结果是否显示 = 自己显示  且  本组之前的红点都不需要显示
                            bool needShow = selfNeedShow && !thisGroupHasShow;

                            ALUGUICommon.setGameObjEnable(redTip.goRedTips, needShow);
                            ALUGUICommon.setGameObjEnable(redTip.goRedHide, !needShow);
                            ALUGUICommon.setGameObjEnable(redTip.texRedTipCount, needShow);
                            if (needShow)
                            {
                                ALUGUICommon.setLabelTxt(redTip.texRedTipCount, nodeItem.getCount());
                                _playerAnim(redTip.anim,redTip.animName);

                                //标记本组已经有要显示的红点了
                                thisGroupHasShow = true;
                            }

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