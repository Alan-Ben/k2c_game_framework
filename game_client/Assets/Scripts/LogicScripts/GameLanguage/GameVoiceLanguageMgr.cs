namespace GOE
{
    public class GameVoiceLanguageMgr
    {
        private static GameVoiceLanguageMgr _g_instance;
        public static GameVoiceLanguageMgr instance { get { return _g_instance ??= new GameVoiceLanguageMgr(); } }
        
        /// <summary>
        /// 设置游戏语音的语言
        /// </summary>
        /// <param name="_language"></param>
        /// <param name="_forceChg"></param>
        public void setVoiceLanguage(ENPLanguage _language, bool _forceChg = false)
        {
            ENPLanguage oldLanguage = GameSetting.instance.getCurrentVoiceLanguage();
            if(oldLanguage == _language && !_forceChg)//若旧语言和新语言相同, 且不强制修改的话, 直接返回
                return;
            
            //设置保存选择的语言
            GameSetting.instance.setCurrentVoiceLanguage(_language);
            onVoiceLanguageChanged();
        }

        public void onVoiceLanguageChanged()
        {
            long beStoppedBgAudioRefId = PlayAudioMgr.instance.clearAllLanguageAudio();
            if (beStoppedBgAudioRefId != NPAudioRefObj.invaildAudioRefId)
                PlayAudioMgr.instance.playBackgroundMusic(beStoppedBgAudioRefId);
        }
    }
}