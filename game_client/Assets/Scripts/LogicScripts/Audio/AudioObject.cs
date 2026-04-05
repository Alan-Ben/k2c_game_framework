using System;
using ALPackage;
using UnityEngine;
using UnityEngine.Audio;

namespace GOE
{
    public interface _IAudioResObj
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_container"></param>
        /// <param name="_isInstanceGo">是否是全新实例化的go</param>
        /// <returns></returns>
        public GameObject createObj(_AALResObjContainer _container, out bool _isInstanceGo);

        public void discard();
    }
    
    // 播放完成后自动回收进缓存的音效对象
    public class AudioObject
    {
        // 资源容器对象，用于从cache中实例化资源并管理的对象
        protected ALResObjSingleContainer _m_alResObj = new ALResObjSingleContainer();
        
        // 无效的音效实例ID
        public const long g_iInvalidAudioObjectID = -1;

        //操作序列号
        private int _m_iAudioFadeOpSerialize = 1;

        //音效对象
        private AudioSource _m_asAudioSource;

        //原始音量
        private float _m_fRefAudioVolue;
        
        //停止播放时的回调
        private Action<AudioObject> _m_stopAction;

        //是否实例对象，表示释放的时候是否可以使用release释放
        private bool _m_bIsIntanceGo = true;
        
        // 音效实例ID
        public long id { get; private set; }
        // 音效配置
        public NPAudioRefObj refObj { get; private set; }
        public bool isLanguageAudio { get { return refObj?.is_language_audio ?? false; } }
        // 音效GameObject
        public GameObject go { get; private set; }
        // 这个音效对应的语言，有可能会是NONE注意
        public ENPLanguage language { get; private set; }
        //音源
        public AudioSource audioSource { get { return _m_asAudioSource; } }
        //原始音量大小
        public float refAudioVolue { get { return _m_fRefAudioVolue; } }

        //改变音量比例
        public void setAudioScale(float _scale)
        {
            if(_m_asAudioSource == null)
                return;

            _m_asAudioSource.volume = _m_fRefAudioVolue * _scale;
        }

        //改变音量比例
        public void setAudioScale(bool _isPlay, float _scale)
        {
            if(_m_asAudioSource == null)
                return;

            if(_isPlay)
            {
                _m_asAudioSource.volume = _m_fRefAudioVolue * _scale;
                play();
            }
            else
            {
                stop();
            }
        }

        //设置音量
        public void setAudioVolue(float _volue)
        {
            if(_m_asAudioSource == null)
                return;

            _m_asAudioSource.volume = _volue;
        }
        
        // 设置这个音效的语言
        public void setLanguage(ENPLanguage _language)
        {
            language = _language;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_go"></param>
        /// <param name="_refObj"></param>
        /// <param name="_isInstanceGo">资源对象是否可使用destroy释放</param>
        public AudioObject(GameObject _go, NPAudioRefObj _refObj, bool _isInstanceGo)
        {
            go = _go;
            refObj = _refObj;
            id = g_iInvalidAudioObjectID;
            _m_bIsIntanceGo = _isInstanceGo;

            if(null != go && null != _refObj)
            {
                //初始化时获取GO上的AudioSoure组件
                _m_asAudioSource = go.GetComponent<AudioSource>();

                if(_m_asAudioSource != null)
                {
                    //原始音量赋值
                    _m_fRefAudioVolue = _m_asAudioSource.volume;
                    
                    //有混音器组设置对应输出
                    AudioMixerGroup audioMixerGroup = AudioMixerMgr.instance.getAudioMixerGroup(_refObj.audio_group_id);
                    if (audioMixerGroup != null) 
                        _m_asAudioSource.outputAudioMixerGroup = audioMixerGroup;
                }
            }
        }
        
        public AudioObject(_IAudioResObj _resObj, NPAudioRefObj _refObj)
        {
            if(_resObj != null)
                go = _resObj.createObj(_m_alResObj, out _m_bIsIntanceGo);
            //创建的go放到DontDestroy防止放到active scene被销毁的时候，一起把go销毁导致异常
            if(go != null)
                UnityEngine.Object.DontDestroyOnLoad(go);
            refObj = _refObj;
            id = g_iInvalidAudioObjectID;

            if (null != go && null != _refObj)
            {
                //初始化时获取GO上的AudioSoure组件
                _m_asAudioSource = go.GetComponent<AudioSource>();

                //原始音量赋值
                if(_m_asAudioSource != null)
                {
                    _m_fRefAudioVolue = _m_asAudioSource.volume;
                    AudioMixerGroup audioMixerGroup = AudioMixerMgr.instance.getAudioMixerGroup(_refObj.audio_group_id);
                    if (audioMixerGroup != null) 
                        _m_asAudioSource.outputAudioMixerGroup = audioMixerGroup;
                }
            }
        }

        public int fadeOpSerialize { get { return _m_iAudioFadeOpSerialize; } }

        //添加操作序列号
        public int addFadeOp()
        {
            return ++_m_iAudioFadeOpSerialize;
        }

        /****************
         * 设置父节点
         **/
        public void setParent(Transform _parent, Vector3 _vec)
        {
            if(null == go)
                return;

            go.transform.SetParent(_parent);
            go.transform.localPosition = Vector3.zero;

            //_vec为Vector.Zero代表不改变go的全局坐标，也就是坐标与父节点坐标相同
            //当_vec为Vector时,默认的AudioMgr的坐标也是Vector.Zero，所以此时不用设置全局坐标
            //此处的处理为如果带进的_vec不为Vector.Zero,就调整go对象的全局坐标，将GO对象在指定Vec坐标处播放

            //如果是_vec不为 vector.Zero,则需要调整GO的全局坐标 
            if(_vec != Vector3.zero)
                go.transform.position = _vec;
        }

        // 开始播放，指定时间完成后回收进缓存
        public void play(long _id, Transform _parent, Vector3 _vec, Action<AudioObject> _startPlayAction = null, Action<AudioObject> _stopAction = null)
        {
            id = _id;
            setParent(_parent, _vec);
            _startPlayAction?.Invoke(this);
            addCloseAction(_stopAction);
            ALUGUICommon.setGameObjEnable(go, true);
        }


        //单纯的继续播放
        public void play(Action<AudioObject> _startPlayAction = null, Action<AudioObject> _stopAction = null)
        {
            ALUGUICommon.setGameObjEnable(go, true);
            addCloseAction(_stopAction);
            _startPlayAction?.Invoke(this);
        }

        // 停止播放
        public void stop()
        {
            ALUGUICommon.setGameObjEnable(go, false);
            
            Action<AudioObject> stopAction = _m_stopAction;
            _m_stopAction = null;
            stopAction?.Invoke(this);
        }

        //检测是否不在屏幕内
        public bool isInDisToScreenCenterPos(Vector3 _centerPos, float _distance)
        {
            return Vector3.SqrMagnitude(_centerPos - go.transform.position) < _distance * _distance;
        }

        //是否正在播放
        public bool isPlaying()
        {
            return go.activeSelf;
        }

        //是否在离开屏幕时关闭
        public bool isNeedToStopWhenOutScreen()
        {
            return !refObj.is_ignore_screen_distance;
        }

        // 复制一份复本
        public AudioObject clone()
        {
            if (null == go)
                return new AudioObject((GameObject)null, refObj, false);

            return new AudioObject(UnityEngine.GameObject.Instantiate(go), refObj, true);
        }

        // 重置音效对象数据
        public void reset()
        {
            stop();
            id = g_iInvalidAudioObjectID;

            //音量还原回默认值
            if (null != _m_asAudioSource)
            {
                _m_asAudioSource.volume = refAudioVolue;
            }
        }

        // 销毁
        public void discard()
        {
            stop();
            
            //只有实例对象才可以释放
            if(_m_bIsIntanceGo)
                ALUnityCommon.releaseGameObj(go);
            
            go = null;
            _m_asAudioSource = null;
            
            _m_alResObj?.discard();
        }
        
        /// <summary>
        /// 增加关闭回到
        /// </summary>
        public void addCloseAction(Action<AudioObject> _action)
        {
            if(null == _action)
                return;
            
            if (go == null)
            {
#if UNITY_EDITOR
                Debug.LogError("检查音效已经销毁了还在添加回调");
#endif
                return;
            }
            
            if(_m_stopAction == null)
                _m_stopAction = _action;
            else
            {
                _m_stopAction += _action;
            }
        }
    }
}
