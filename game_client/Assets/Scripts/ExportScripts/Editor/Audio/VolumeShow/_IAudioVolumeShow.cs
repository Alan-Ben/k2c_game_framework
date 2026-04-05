namespace MJSoundEditor
{
    public interface _IAudioVolumeShow
    {
        public string instruction { get; }
        public string name { get; }
        public float volume { get; set; }
        public bool isPlaying { get; }

        public void play( bool _isAutoSelectGo);
        public void stop();
    }

}