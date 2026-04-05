using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class AudioCache : _AALCacheController<AudioObject, AudioObject>
    {
        private NPGAudioIndex _m_aiAudioIndex;
        // 缓存父节点对象
        private GameObject _m_goRootAudioGo;

        //模板对象
        private AudioObject _m_TemplateAO;
        
        // 当前语音缓存对应的语言
        private ENPLanguage _m_eLanguage;
        private bool _m_isLangAudio;

        // 是否缓存初始化完成
        private bool _m_bInit;
        
        private long _m_mergeIndex;

        public int usedCount
        {
            get
            {
                if (usedItemList != null) return usedItemList.Count;
                return 0;
            }
        }
        
        public ENPLanguage MELanguage => _m_eLanguage;
        public bool isLangAudio => _m_isLangAudio;
        
        public long mergeIndex =>_m_mergeIndex;
        
        public AudioCache(NPAudioRefObj _refObj, Transform _parentTrans, ENPLanguage _language = ENPLanguage.NONE)
            : base(_refObj.min_cache_count < 2 ? 2 : _refObj.min_cache_count, _refObj.max_cache_count < 30 ? 30 : _refObj.max_cache_count)
        {
            _m_mergeIndex = ALCommon.mergeInt(_refObj.audio_index.mainId, _refObj.audio_index.subId);
            _m_aiAudioIndex = _refObj.audio_index;
            _m_isLangAudio = _refObj.is_language_audio;
            _m_bInit = false;

            //构建本缓存的父节点
            _m_goRootAudioGo = new GameObject();
            _m_goRootAudioGo.name = $"audio_{_m_aiAudioIndex.mainId}_{_m_aiAudioIndex.subId}";
            _m_goRootAudioGo.transform.SetParent(_parentTrans);
            _m_goRootAudioGo.transform.localScale = Vector3.one;
            _m_goRootAudioGo.transform.position = Vector3.zero;
            
            _m_eLanguage = _language;
        }

        // 是否缓存初始化完成
        public bool isInitialized()
        {
            return _m_bInit;
        }
        
        public new void discard()
        {
            base.discard();
        }

        //警告信息文字
        protected override string _warningTxt { get { return $"audio_{_m_eLanguage}_{_m_aiAudioIndex.mainId}_{_m_aiAudioIndex.subId}"; } }

        protected override void _onInit(AudioObject _template)
        {
            _m_TemplateAO = _template;
            if(null != _m_TemplateAO)
                _m_TemplateAO.stop();
            
            _m_bInit = true;
        }

        protected override AudioObject _createItem(AudioObject _template)
        {
            AudioObject newItem = _template == null ? null : _template.clone();
            newItem?.setLanguage(_m_eLanguage);
            return newItem;
        }

        protected override void _discardItem(AudioObject _item)
        {
            if(null == _item)
                return;

            _item.discard();
        }

        protected override void _resetItem(AudioObject _item)
        {
            if(null == _item || null == _item.go)
                return;

            //数据无效直接释放
            if(null == _m_goRootAudioGo)
            {
                _item.discard();
                return;
            }

            _item.go.transform.parent = _m_goRootAudioGo.transform;
            _item.reset();
        }

        protected override void _discard()
        {
            if(null != _m_TemplateAO)
                _m_TemplateAO.discard();
            _m_TemplateAO = null;
            
            ALUnityCommon.releaseGameObj(_m_goRootAudioGo);
            _m_goRootAudioGo = null;
        }
        
        public override string ToString()
        {
            return $"audio:{_m_aiAudioIndex},clipLength:{(_m_TemplateAO== null ||_m_TemplateAO.audioSource == null || _m_TemplateAO.audioSource.clip == null ?-1:_m_TemplateAO.audioSource.clip.length)}," +
                   $"use:{usedCount},{_m_bInit},{isLangAudio},{_m_eLanguage}";
        }
    }
}
