using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase每个挂载点的位置数据信息
    /// </summary>
    public class ShowCaseIndexObj : _IAnimatorControlRegInfo
    {
        //动画标记,用于注册到全局动画管理器
        private string _m_aniTagName;
        //单位根节点
        private Transform _m_parent;
        //是否使用默认位置，0
        private bool _m_useDefaultPos;
        
        //具体的showcase单位对象
        private _AShowCaseUnitInfoObj _m_caseUnitInfoObj;

        //是否初始化
        private bool _m_isInit;
        //当前序列号
        private long _m_lOpSerialize;
        //返回单位对象
        public _AShowCaseUnitInfoObj caseUnitInfoObj
        {
            get { return _m_caseUnitInfoObj; }
        }

        public string controlNameTag
        {
            get { return _m_aniTagName; }
        }
        
        public ShowCaseIndexObj(Transform _parent, string _aniTagName, bool _useDefaultPos)
        {
            _m_parent = _parent;
            _m_aniTagName = _aniTagName;
            _m_useDefaultPos = _useDefaultPos;
        }

        //初始化
        public void init()
        {
            if(_m_isInit)
                return;

            _m_isInit = true;

            //有配置才注册
            if (!string.IsNullOrEmpty(controlNameTag))
            {
                AnimatorControllerMgr.instance.regMono(this);
            }
        }
        
        //卸载
        public void discard()
        {
            _m_isInit = false;
            _m_useDefaultPos = false;

            //有配置才注册
            if (!string.IsNullOrEmpty(controlNameTag))
            {
                AnimatorControllerMgr.instance.unregMono(this);
            }

            discardUnitObj();
        }
        
        //添加加载单位
        public void addUnitObj(_AShowCaseUnitInfoObj _caseUnitInfo, Action _action)
        {
            if(null == _caseUnitInfo)
            {
                if (null != _action)
                    _action();
                return;
            }
            
            discardUnitObj();
            _m_caseUnitInfoObj = _caseUnitInfo;
            
            _m_lOpSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lOpSerialize;
            
            _m_caseUnitInfoObj.Init(_m_parent, _m_useDefaultPos, () =>
            {
                if(serialize != _m_lOpSerialize || null == _m_caseUnitInfoObj)
                    return;

                if (_action != null) 
                    _action();
            });
        }
        
        //销毁处理,包括反注册和单位卸载
        public void discardUnitObj()
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            
            if(null == _m_caseUnitInfoObj)
                return;
            
            _m_caseUnitInfoObj.Discard();
            _m_caseUnitInfoObj = null;
        }
        
        /// <summary>
        /// 注册表现准备完成回调
        /// </summary>
        /// <param name="_showPrepareDone"></param>
        public void regShowPrepareDone(Action _showPrepareDone)
        {
            if(null == _m_caseUnitInfoObj)
            {
                if (_showPrepareDone != null) 
                    _showPrepareDone();
                return;
            }
            
            //加载完成后再注册
            _m_caseUnitInfoObj.regShowPrepareDone(_showPrepareDone);
        }

        #region _IAnimatorControlRegInfo 方法

        public void playAnimator(string _stateName, float _normalizedTime)
        {
            if(null == _m_caseUnitInfoObj)
                return;
            
            _m_caseUnitInfoObj.playAnim(_stateName);
        }

        public void setAni(string _stateName)
        {
            if(null == _m_caseUnitInfoObj)
                return;
            
            _m_caseUnitInfoObj.forceSetAni(_stateName);
        }

        public void setBool(string _variableName, bool _value)
        {
           
        }

        public void setFloat(string _variableName, float _value)
        {
            
        }

        public void setInteger(string _variableName, int _value)
        {
            
        }

        public void setSpeed(float _speed)
        {
           
        }

        #endregion
    }
}