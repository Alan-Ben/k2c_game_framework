using System;
using ALPackage;
using JetBrains.Annotations;

namespace Hotfix
{
    public class HotfixNPPlayer
    {
        private static HotfixNPPlayer _g_instance = new HotfixNPPlayer();
        public static HotfixNPPlayer instance
        {
            get
            {
                if (_g_instance == null)
                {
                    _g_instance = new HotfixNPPlayer();
                }
                return _g_instance;
            }
        }
        
        /// <summary>
        /// 客户端数据序列号，用于避免gs消息被错误处理增加的识别序列号
        /// </summary>
        private long _m_lPlayerSerialize;
        
        private bool _m_bIsInited = false;
        private bool _m_bIsInitDone = false;
        private bool _m_bIsInitSucc = false;
        
        private Action<bool> _m_aInitDelegate;
        
        [NotNull] private HotfixComponentMgr _m_componentMgr;

        private TileMatchComponent _m_tileMatchComponent;
        private NumMergeComponent _m_numMergeComponent;
        //万能活动组件
        private RegularEventComponent _m_regularEventComponent;
        
        private HotfixNPPlayer()
        {
            _m_lPlayerSerialize = ALSerializeOpMgr.next();

            _m_bIsInited = false;
            _m_bIsInitDone = false;
            _m_bIsInitSucc = false;
            
            _m_aInitDelegate = default(Action<bool>);
            
            _m_componentMgr = new HotfixComponentMgr();

            _m_tileMatchComponent = new TileMatchComponent();
            _m_numMergeComponent = new NumMergeComponent();
            _m_regularEventComponent = new RegularEventComponent();
        }
        
        public long npplayerSerialize { get { return _m_lPlayerSerialize; } }
        
        public bool isInited { get { return _m_bIsInited; } }
        public bool isInitDone { get { return _m_bIsInitDone; } }
        public bool isInitSucc { get { return _m_bIsInitSucc; } }

        #region component

        public TileMatchComponent tileMatchComponent { get { return _m_tileMatchComponent; } }
        public NumMergeComponent numMergeComponent { get { return _m_numMergeComponent; } }
        /// <summary>
        /// 万能活动组件
        /// </summary>
        public RegularEventComponent regularEventComponent { get { return _m_regularEventComponent; } }

        #endregion

        public void init(Action<bool> _onInitDone)
        {
            if (_m_bIsInited)
            {
                if (_m_bIsInitDone)
                {
                    _onInitDone?.Invoke(_m_bIsInitSucc);
                }
                else
                {
                    if (_onInitDone != null)
                        _m_aInitDelegate += _onInitDone;
                }
                
                return;
            }
            
            _m_bIsInited = true;
            //注册回调
            if(null != _onInitDone)
                _m_aInitDelegate += _onInitDone;

            _registerComponent();
            
            _m_componentMgr.initComponents((_isAllInitDone) =>
            {
                _m_bIsInitDone = true;

                _m_bIsInitSucc = _isAllInitDone;
                
                //调用回调
                if(null != _m_aInitDelegate)
                    _m_aInitDelegate(_m_bIsInitSucc);
                _m_aInitDelegate = default(Action<bool>);
            });
        }

        private void _registerComponent()
        {
            _m_componentMgr.RegisterComponent(_m_tileMatchComponent);
            _m_componentMgr.RegisterComponent(_m_numMergeComponent);
            _m_componentMgr.RegisterComponent(_m_regularEventComponent);

        }

        public void forceInitDone()
        {
            _m_bIsInited = true;
            _m_bIsInitDone = true;
            _m_bIsInitSucc = true;
            
            _m_componentMgr.forceInited();
            
            //调用回调
            if(null != _m_aInitDelegate)
                _m_aInitDelegate(_m_bIsInitSucc);
            _m_aInitDelegate = default(Action<bool>);
        }
        
        public void discard()
        {
            _m_bIsInited = false;
            _m_bIsInitDone = false;
            _m_bIsInitSucc = false;
            
            _m_aInitDelegate = default(Action<bool>);
            
            _m_componentMgr.discardComponents();

            _m_componentMgr.UnregisterComponent(_m_tileMatchComponent);
            _m_componentMgr.UnregisterComponent(_m_numMergeComponent);
            _m_componentMgr.UnregisterComponent(_m_regularEventComponent);
        }

        public string getAllUnInitComp()
        {
            return _m_componentMgr.getAllUnInitComp();
        }
    }
}