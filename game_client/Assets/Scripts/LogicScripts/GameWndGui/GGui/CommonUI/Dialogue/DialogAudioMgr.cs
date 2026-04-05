using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话音效管理器
    /// </summary>
    public class DialogAudioMgr : _AAudioLayerMgr
    {
        private static DialogAudioMgr _g_instance;
        public static DialogAudioMgr instance { get { return  _g_instance ??= new DialogAudioMgr(); } }
        
        private MonoBehaviour _m_nowDialogWnd;//当前对话窗口
        
        /// <summary>
        /// 注册当前对话窗口
        /// </summary>
        /// <param name="_dialogWnd"></param>
        public void regNowDialogWnd(MonoBehaviour _dialogWnd)
        {
            if(_dialogWnd == null)
                return;
            
            if (_m_nowDialogWnd != null)
            {
                Debug.LogWarning($"[DialogAudioMgr] 当前已经存在一个对话窗口:{_m_nowDialogWnd.gameObject.name} 将要被替换为新对话窗口:{_dialogWnd.gameObject.name}, 不太可能出现这种情况, 请检查是否存在问题");
                unRegNowDialogWnd(_m_nowDialogWnd);//反注册当前对话窗口
            }
            
            _m_nowDialogWnd = _dialogWnd;
        }
        
        /// <summary>
        /// 反注册当前对话窗口
        /// </summary>
        /// <param name="_dialogWnd"></param>
        public void unRegNowDialogWnd(MonoBehaviour _dialogWnd)
        {
            if(_m_nowDialogWnd == null)
                return;
            
            if (_dialogWnd != _m_nowDialogWnd)
            {
                string unRegNowDialogWndName = _dialogWnd == null ? "null" : _dialogWnd.gameObject.name;
                string nowDialogWndName = _m_nowDialogWnd == null ? "null" : _m_nowDialogWnd.gameObject.name;
                Debug.LogWarning($"[DialogAudioMgr] 要移除的对话窗口:{unRegNowDialogWndName} 不是DialogAudioMgr中记录的当前对话窗口:{nowDialogWndName}, 不太可能出现这种情况, 请检查是否存在问题");
            }

            // 不管记录窗口是否正确，都将当前对话窗口置空, 并且停止所有音效, 防止在不该播放音效的地方播放了音效
            _m_nowDialogWnd = null;
            stopAllAudio();
        }
        
        /// <summary>
        /// 播放对话音效
        /// </summary>
        /// <param name="_audioLayer">自身所处音效layer(自身layer的音效在播放前也会停止上一个同layer音效)</param>
        /// <param name="_stopAudioLayerList">需要停止的音效layer列表</param>
        /// <param name="_audioRefId"></param>
        public new void playAudio(int _audioLayer, List<int> _stopAudioLayerList, long _audioRefId)
        {
            if (_m_nowDialogWnd == null)
            {
                Debug.LogWarning("[DialogAudioMgr] 当前不存在注册的显示对话窗口, 但是却要播放音效");
                return;
            }

            base.playAudio(_audioLayer, _stopAudioLayerList, _audioRefId);
        }
    }
}