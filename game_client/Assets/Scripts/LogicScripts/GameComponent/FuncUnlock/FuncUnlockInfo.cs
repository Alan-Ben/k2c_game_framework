using NPEnum;

namespace GOE
{
    /// <summary>
    /// 功能解锁信息
    /// </summary>
    public class FuncUnlockInfo
    {
        //功能解锁配置信息
        private FunctionUnlockRefObj _m_refObj;
        //解锁配置信息
        private NPSimpleUnlockRef _m_unlockRefObj;
        //是否已经展示过提示
        private bool _m_hasShowTip;
        //是否已经领取奖励
        private bool _m_bHasGetReward;
        //是否已经通知服务端
        private bool _m_bHasNotified;
        
        
        public FuncUnlockInfo(FunctionUnlockRefObj _refObj)
        {
            if(null == _refObj)
                return;

            _m_hasShowTip = false;
            _m_bHasGetReward = false;
            _m_bHasNotified = false;
            _m_refObj = _refObj;
            _m_unlockRefObj = GRefdataCoreMgr.instance.simpleUnlockMap.getRef(_m_refObj.simple_unlock_id);
        }
        
        public long funcId { get { if (_m_refObj == null) return 0; return (long)_m_refObj.type; } }
        /// <summary> 功能类型 </summary>
        public ENPFunctionType funcType { get { if (_m_refObj == null) return ENPFunctionType.NONE; return _m_refObj.type; } }
        /// <summary> 功能名称 </summary>
        public string funcName { get { if (_m_refObj == null) return string.Empty; return TextTranslate.instance.getLanguage(_m_refObj.name); } }
        /// <summary> 描述(预告界面描述) </summary>
        public string funcDesc { get { if (_m_refObj == null) return string.Empty; return TextTranslate.instance.getLanguage(_m_refObj.desc, _m_refObj.desc_args); } }
        /// <summary> 图标 </summary>
        public NPGTextureIndex texIcon { get { if (_m_refObj == null) return null; return _m_refObj.tex_icon; } }
        /// <summary> 系统图片 </summary>
        public NPGTextureIndex texBanner { get { if (_m_refObj == null) return null; return _m_refObj.tex_banner; } }
        /// <summary> 排序id </summary>
        public long sortId { get { if (_m_refObj == null) return 0; return _m_refObj.sort_id; } }
        /// <summary> 解锁顺序id </summary>
        public long unlockOrderId { get { if (_m_refObj == null) return 0; return _m_refObj.unlock_order_id; } }
        /// <summary> 解锁配表 </summary>
        public FunctionUnlockRefObj functionUnlockRef { get { return _m_refObj; } }
        /// <summary> 是否已经领取奖励 </summary>
        public bool hasGetReward { get { return _m_bHasGetReward; } }
        /// <summary> 是否忽略展示 </summary>
        public bool ignorePopUnlockTip { get { return _m_refObj == null ? true : _m_refObj.ignore_pop_unlock_tip; } }
        /// <summary> 解锁配置信息 </summary>
        public NPSimpleUnlockRef simpleUnlockRefObj { get { return _m_unlockRefObj; } }
        
        //是否已经展示过提示
        public bool hasShowTip
        {
            get { return _m_hasShowTip; }
        }

        /// <summary>
        /// 是否解锁
        /// </summary>
        public bool isUnlock {
            get
            {
                if (null == _m_unlockRefObj)
                    return true;

                return _m_unlockRefObj.isConditionEnable(null);
            }
        }
        
        /// <summary>
        /// 是否需要解锁提示
        /// </summary>
        public bool isNeedTip {
            get
            {
                //如果配置忽略解锁提示不需要提示
                if (ignorePopUnlockTip)
                    return false;
                //已经提示过了不需要重复提示
                if (hasShowTip)
                    return false;
                //还未解锁不需要提示
                if (!isUnlock)
                    return false;

                return true;
            }
        }

        /// <summary>
        /// 是否可以领取奖励
        /// </summary>
        public bool canGetReward
        {
            get
            {
                return !_m_bHasGetReward && isUnlock && _m_refObj != null && !_m_refObj.ignore_function_list_show;
            }
        }
        
        /// <summary>
        /// 是否可以通知服务端
        /// </summary>
        public bool canNotifyServer
        {
            get
            {
                return !_m_bHasNotified && isUnlock && _m_refObj != null;
            }
        }
        
        /// <summary>
        /// 获取解锁提示文本
        /// </summary>
        /// <returns></returns>
        public string getUnlockTip()
        {
            if (_m_unlockRefObj == null)
                return string.Empty;

            return TextTranslate.instance.getLanguage(_m_unlockRefObj.unlock_tip, _m_unlockRefObj.unlock_tip_args);
        }

        /// <summary>
        /// 设置已经展示过提示
        /// </summary>
        public void setTipShowDone()
        {
            _m_hasShowTip = true;
        }

        /// <summary>
        /// 重置展示
        /// </summary>
        public void resetShowTip()
        {
            _m_hasShowTip = false;
        }

        /// <summary>
        /// 设置已经领取奖励
        /// </summary>
        public void setHadGetReward()
        {
            _m_bHasGetReward = true;
        }
        
        /// <summary>
        /// 设置已经通知服务端
        /// </summary>
        public void setHadNotified()
        {
            _m_bHasNotified = true;
        }
    }
}
