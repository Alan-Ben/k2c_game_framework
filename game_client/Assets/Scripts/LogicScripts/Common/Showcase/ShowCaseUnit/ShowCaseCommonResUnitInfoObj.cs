using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用index加载的showcase
    /// </summary>
    public class ShowCaseCommonResUnitInfoObj : _AShowCaseUnitInfoCommonObj
    {
        private GameObject _m_unit;
        //resObjMono go类型资源通用mono
        private ShowCaseCommonResObjMono _m_resObjMono;
        //特效容器
        private CommonSfxCtrlContainer _m_sfxContainer;
        
        private BasicResIndexInfo _m_indexInfo;
        //当前序列号
        private long _m_lOpSerialize;
        //此预制体原本的Layer
        private int _m_iLayer;
        //show_case_actor_behavior表的唯一key
        private long _m_actorKey = 0;
        
        public ShowCaseCommonResUnitInfoObj(BasicResIndexInfo _indexInfo)
        {
            _m_indexInfo = _indexInfo;

            if (_indexInfo is NPGModelIndex)
                _m_actorKey = _indexInfo.GetHashCode();

        }

        public GameObject unit { get => _m_unit; }


        protected override void _loadObj(Action<GameObject> _doneAction)
        {
            _m_lOpSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lOpSerialize;
            
            ShowCaseResMgr.instance.popUnit(_m_indexInfo, ((resIndex, go) =>
            {
                if (serialize != _m_lOpSerialize)
                {
                    if (null != _doneAction)
                        _doneAction(null);
                    return;
                }

                if (null == go)
                {
                    Debug.LogError($"[showcase]加载出来的单位是null,{resIndex}");
                    if (null != _doneAction)
                        _doneAction(null);
                    return;
                }
                
                go.name = $"showCaseUnit_{_m_indexInfo}";
                _m_unit = go;

                _m_resObjMono = go.GetComponent<ShowCaseCommonResObjMono>();
                
                if (null != _doneAction)
                    _doneAction(_m_unit);
                
                if (_m_unit != null)
                {
                    _m_iLayer = _m_unit.layer;
                    WCGResCommon.SetLayer(_m_unit, WCGResCommon.WCG_C_LAYER_SHOWCASE, true);
                }
            }));   
        }

        protected override void _discardObj()
        {
            if (_m_unit != null)
                WCGResCommon.SetLayer(_m_unit, _m_iLayer, true);
            
            _m_lOpSerialize = ALSerializeOpMgr.next();
            ShowCaseResMgr.instance.pushBackUnit(_m_indexInfo, _m_unit);

            _m_unit = null;
            _m_resObjMono = null;

            if (_m_sfxContainer != null) 
                _m_sfxContainer.clear();
            _m_sfxContainer = null;
        }
        
        // 表现准备完成函数
        protected override void _regShowPrepareDone(Action _showPrepareDone)
        {
            if(null == _showPrepareDone)
                return;
         
            //执行表现准备完成回调,加载后才会执行
            if (_m_resObjMono != null && _m_resObjMono.aniShowInterface != null)
                _m_resObjMono.aniShowInterface.doPreDoneAction(_showPrepareDone);
            else
                _showPrepareDone?.Invoke(); 
        }
        
        protected override long _getShowCaseActorBehaviorKey { get { return _m_actorKey; } }
        
        //播放动画
        public override void playAnim(string _aniName)
        {
            if(null == _m_resObjMono)
                return;

            _m_resObjMono.playAni(_aniName);
        }
        
        //设置动画触发器
        public override void setAnimTrigger(string _name)
        {
            if(null == _m_resObjMono)
                return;

            _m_resObjMono.setAnimTrigger(_name);
        }
        
        //设置动画速度
        public override void setSpeed(float _speed)
        {
            if(null == _m_resObjMono)
                return;
            
            _m_resObjMono.setSpeed(_speed);
        }
        
        //设置透明度
        public override void setAlpha(float _alpha)
        {
            if(null == _m_resObjMono)
                return;

            _m_resObjMono.setAlpha(_alpha);
        }

        //播放特效
        public override void playSfx(long _sfxId)
        {
            if (null == _m_resObjMono || null == _m_resObjMono.sfxParent)
                return;

            if (null == _m_sfxContainer)
                _m_sfxContainer = new CommonSfxCtrlContainer();
            
            CommonTDSfxObj commonTdSfxObj = PlaySfxMgr.instance.playTDSfx(_sfxId, _m_resObjMono.sfxParent);
            _m_sfxContainer.addSfxObj(commonTdSfxObj);
        }

        //设置ZSpacing
        public override void setZSpacing(float _value)
        {
            if (null == _m_resObjMono)
                return;

            _m_resObjMono.setZSpacing(_value);
        }
        
        //强制切换动画
        public override void forceSetAni(string _aniName)
        {
            if(null == _m_resObjMono)
                return;

            _m_resObjMono.forceSetAni(_aniName);
        }

        public override bool isPlayingAni(string _aniName)
        {
            if(null == _m_resObjMono)
                return false;

            return _m_resObjMono.isPlayingAni(_aniName);
        }

        public override void enableRender(bool _isEnable)
        {
            if(null == _m_resObjMono)
                return;

            _m_resObjMono.enableRender(_isEnable);
        }
    }
}