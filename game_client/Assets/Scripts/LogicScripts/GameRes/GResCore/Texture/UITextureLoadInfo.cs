
    using ALPackage;
    using MG;
    using UnityEngine;

    public class UITextureLoadInfo : ALUITextureLoadInfo
    {
        private long _m_lastUsedTime;
        
        private int _m_usedCountFromStart;
      
        
        public UITextureLoadInfo(ALBasicResIndexInfo _indexInfo) : base(_indexInfo)
        {
        }

        public UITextureLoadInfo(int _mainId, int _subId) : base(_mainId, _subId)
        {
        }

        public long lastUsedTime => _m_lastUsedTime;

        public int usedCountFromStart => _m_usedCountFromStart;

        public void updateLastUsedTime()
        {
            _m_lastUsedTime = Time.frameCount;
        }
        
        public void addUsedCountFromStart() { _m_usedCountFromStart ++; }
    }
