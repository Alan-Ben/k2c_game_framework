using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 显示 条件满足时加载go 隐藏是卸载go
    /// </summary>
    public class GGUICustomMonoConditionLoadGo : MonoBehaviour
    {
        [ALInfo("加载的go会放到自己底下")]
        [ALHeader("条件字符串")]
        public string conditionStr;
        [ALHeader("资源id")]
        public long res_id;
        
        //是否需要检测
        private bool _m_bNeedCheck = false;
        
        //条件处理
        private bool _m_bIsInited = false;
        private NPPlayerConditionGroupObj _m_cgConditionGroupObj;
        public NPPlayerConditionGroupObj conditionGroup
        {
            get
            {
                if (_m_bIsInited)
                    return _m_cgConditionGroupObj;

                _m_cgConditionGroupObj = NPPlayerConditionGroupObj.readConditionGroupList(conditionStr, "condPref");
                _m_bIsInited = true;
                return _m_cgConditionGroupObj;
            }
        }

#if NP_GAME
        //加载出来的对象
        [NotNull]private CommonAssetLoader _m_assetLoader = new CommonAssetLoader();  
#endif

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);

            //注册消息
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _doCheck);
        }

        private void OnDisable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);

            //注销消息
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _doCheck);
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck)
                return;

            _doCheck();
        }

        protected void _doCheck()
        {
#if NP_GAME
            if (null == this || null == gameObject)
                return;

            _m_bNeedCheck = false;
            
            //显示状态并且条件满足加载go
            if (gameObject.activeInHierarchy 
                && null != conditionGroup && conditionGroup.IsEnable(null))
            {
                //已经加载好了不处理
                if(null != _m_assetLoader.ShowGO)
                    return;
                _m_assetLoader.loadAsset(res_id, this.transform);
            }
            else
            {
                //无效时卸载go
                _discardGo();
            }
#endif
        }

        private void _discardGo()
        {
#if NP_GAME
            _m_assetLoader.discard();
#endif
        }
    }
}