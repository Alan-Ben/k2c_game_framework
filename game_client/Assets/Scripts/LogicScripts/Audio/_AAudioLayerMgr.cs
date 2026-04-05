using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public class AudioLayerMgr : _AAudioLayerMgr
    {
        
    }
    
    /// <summary>
    /// 音效层级管理器
    /// </summary>
    public abstract class _AAudioLayerMgr
    {
        [NotNull] private Dictionary<int, long> _m_lNowPlayingAudioObjInstanceIdDic = new Dictionary<int, long>();//当前正在播放的音效实例id字典 key:音效layer(同layer只会存在一个音效对象) value:音效实例id
        private Action<int> _m_aLayerAudioPlayCallback;//某一层级有音效播放时的回调
        private Action<int> _m_aLayerAudioStopCallback;//某一layer音效停止播放时的回调字典
        private Action _m_aAllAudioStopCallback;//所有音效停止播放时的回调

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="_audioLayer">自身所处音效layer(自身layer的音效在播放前也会停止上一个同layer音效)</param>
        /// <param name="_stopAudioLayerList">需要停止的音效layer列表</param>
        /// <param name="_audioRefId"></param>
        /// <param name="_onAudioStartPlay">配表开始播放的回调，（实例id）</param>
        public bool playAudio(int _audioLayer, List<int> _stopAudioLayerList, long _audioRefId, Action<long> _onAudioStartPlay = null, Action _onPlayFail = null, Action _onAudioStop = null)
        {
            // 若传入的音效配表id小于等于0或者等于无效音效id，则直接返回
            if (_audioRefId <= 0 || _audioRefId == NPAudioRefObj.invaildAudioRefId)
                return false;

            PlayAudioMgr.instance.playClip(_audioRefId, false, (_audioObject) =>
            {
                if (null != _audioObject && _audioObject.id != AudioObject.g_iInvalidAudioObjectID)//若音效实例id有效
                {
                    stopNowPlayingAudioByLayer(_audioLayer);//停止自身layer的其他音效
                    stopNowPlayingAudioByLayer(_stopAudioLayerList);//停止指定layer音效

                    _onAudioStartPlay?.Invoke(_audioObject.id);
                    _m_aLayerAudioPlayCallback?.Invoke(_audioLayer);//调用音效播放回调
                    _m_lNowPlayingAudioObjInstanceIdDic[_audioLayer] = _audioObject.id;
                }
            }, _onPlayFail, (audioObjInstanceId) =>
            {
                _onAudioStop?.Invoke();
                if (_m_lNowPlayingAudioObjInstanceIdDic.Remove(_audioLayer)) //若_m_lNowPlayingAudioObjInstanceIdDic中还存在指定layer数据, 说明不是通过下面的几个stop方法手动停止音效, 而是音效自己播放完成的
                {
                    _m_aLayerAudioStopCallback?.Invoke(_audioLayer);
                    
                    if(_m_lNowPlayingAudioObjInstanceIdDic.Count <= 0)
                        _m_aAllAudioStopCallback?.Invoke();
                }
                else//若_m_lNowPlayingAudioObjInstanceIdDic中不存在指定layer数据, 说明在某个地方已经调用了下面的几个stop方法手动停止音效, 做这个区分可以将自动播放完成状态和手动停止状态区分开来调用执行不同逻辑(虽然这里逻辑相同, 但是之后有需要就可以改成不同)
                {
                    _m_aLayerAudioStopCallback?.Invoke(_audioLayer);
                    
                    if(_m_lNowPlayingAudioObjInstanceIdDic.Count <= 0)
                        _m_aAllAudioStopCallback?.Invoke();
                }
            });

            return true;
        }
        
        /// <summary>
        /// 停止指定layer播放的对话语音
        /// </summary>
        public void stopNowPlayingAudioByLayer(int _audioLayer)
        {
            if(_m_lNowPlayingAudioObjInstanceIdDic.TryGetValue(_audioLayer, out long audioInstanceId))
            {
                _m_lNowPlayingAudioObjInstanceIdDic.Remove(_audioLayer);
                // 这里先从_m_lNowPlayingAudioObjInstanceIdDic中移除后, 再调用PlayAudioMgr.instance.stopClip(audioInstanceId)
                // 因为在playDialogAudio方法中的PlayAudioMgr.instance.playClip中注册了_stopAction方法, _stopAction方法会在调用PlayAudioMgr.instance.stopClip时被被调用, 同时也会在音效自身持续时间完成后被调用
                // 这里想对两种不同的音效停止情况做不同的处理, 所以 当手动停止音效时(即调用本方法时), 先从_m_lNowPlayingAudioObjInstanceIdDic中移除, 再调用PlayAudioMgr.instance.stopClip(audioInstanceId),
                // 样可以在playDialogAudio方法中的PlayAudioMgr.instance.playClip中注册的_stopAction方法中 通过判断_m_lNowPlayingAudioObjInstanceIdDic中是否存在该音效实例id, 区分不同的移除方式
                PlayAudioMgr.instance.stopClip(audioInstanceId);
            }
        }

        /// <summary>
        /// 停止指定_layerList列表播放的对话语音
        /// </summary>
        /// <param name="_layerList"></param>
        public void stopNowPlayingAudioByLayer(List<int> _layerList)
        {
            if(_layerList == null)
                return;

            foreach (int layer in _layerList)
            {
                stopNowPlayingAudioByLayer(layer);
            }
        }

        /// <summary>
        /// 停止所有正在播放的音效
        /// </summary>
        public void stopAllAudio()
        {
            Dictionary<int, long> tempDic = new Dictionary<int, long>(_m_lNowPlayingAudioObjInstanceIdDic);
            _m_lNowPlayingAudioObjInstanceIdDic.Clear();
            // 这里先将_m_lNowPlayingAudioObjInstanceIdDic数据暂存到临时字典tempDic中, 然后清除_m_lNowPlayingAudioObjInstanceIdDic, 再遍历临时字典tempDic调用PlayAudioMgr.instance.stopClip(audioInstanceId)
            // 因为在playDialogAudio方法中的PlayAudioMgr.instance.playClip中注册了_stopAction方法, _stopAction方法会在调用PlayAudioMgr.instance.stopClip时被被调用, 同时也会在音效自身持续时间完成后被调用
            // 这里想对两种不同的音效停止情况做不同的处理, 所以 当手动停止音效时(即调用本方法时), 先从_m_lNowPlayingAudioObjInstanceIdDic中移除, 再调用PlayAudioMgr.instance.stopClip(audioInstanceId),
            // 这样可以在playDialogAudio方法中的PlayAudioMgr.instance.playClip中注册的_stopAction方法中 通过判断_m_lNowPlayingAudioObjInstanceIdDic中是否存在该音效实例id, 区分不同的移除方式
            
            foreach (var layerAudioInstanceIdKv in tempDic)
            {
                PlayAudioMgr.instance.stopClip(layerAudioInstanceIdKv.Value);
            }
            tempDic.Clear();
        }
        
        #region 注册与反注册回调
        
        /// <summary>
        /// 注册音效播放时回调
        /// </summary>
        /// <param name="_callback"></param>
        public void regAudioPlayCallback(Action<int> _callback)
        {
            if(_callback == null)
                return;

            if (_m_aLayerAudioPlayCallback == null)
                _m_aLayerAudioPlayCallback = _callback;
            else
                _m_aLayerAudioPlayCallback += _callback;
        }
        
        /// <summary>
        /// 反注册音效播放时回调
        /// </summary>
        /// <param name="_callback"></param>
        public void unRegAudioPlayCallback(Action<int> _callback)
        {
            if(_callback == null)
                return;

            if (_m_aLayerAudioPlayCallback == null)
                return;

            _m_aLayerAudioPlayCallback -= _callback;
        }
        
        /// <summary>
        /// 注册某layer层级音效停止播放时回调
        /// </summary>
        /// <param name="_callback"></param>
        public void regLayerAudioStopCallback(Action<int> _callback)
        {
            if(_callback == null)
                return;

            if (_m_aLayerAudioStopCallback == null)
                _m_aLayerAudioStopCallback = _callback;
            else
                _m_aLayerAudioStopCallback += _callback;
        }
        
        /// <summary>
        /// 反注册某layer层级音效停止播放时回调
        /// </summary>
        /// <param name="_callback"></param>
        public void unRegLayerAudioStopCallback(Action<int> _callback)
        {
            if(_callback == null)
                return;

            if (_m_aLayerAudioStopCallback == null)
                return;

            _m_aLayerAudioStopCallback -= _callback;
        }
        
        /// <summary>
        /// 注册所有音效停止播放时回调
        /// </summary>
        /// <param name="_callback"></param>
        public void regAllAudioStopCallback(Action _callback)
        {
            if(_callback == null)
                return;

            if (_m_aAllAudioStopCallback == null)
                _m_aAllAudioStopCallback = _callback;
            else
                _m_aAllAudioStopCallback += _callback;
        }
        
        /// <summary>
        /// 反注册所有音效停止播放时回调
        /// </summary>
        /// <param name="_callback"></param>
        public void unRegAllAudioStopCallback(Action _callback)
        {
            if(_callback == null)
                return;

            if (_m_aAllAudioStopCallback == null)
                return;

            _m_aAllAudioStopCallback -= _callback;
        }
        
        #endregion
    }
}