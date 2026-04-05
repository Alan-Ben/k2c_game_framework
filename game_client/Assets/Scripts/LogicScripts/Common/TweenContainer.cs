using System.Collections.Generic;
using DG.Tweening;

namespace GOE
{
    public class TweenContainer
    {
        private List<TweenHolder> _m_allTween = new List<TweenHolder>();

        public void regDoTween(Tween _tween)
        {
            _m_allTween.Add(new TweenHolder(_tween));
        }

        public void killAllDoTween()
        {
            for (var i = 0; i < _m_allTween.Count; i++)
            {
                _m_allTween[i].kill();
            }
            _m_allTween.Clear();
        }

        /// <summary>
        /// 暂停所有DoTween
        /// </summary>
        public void pauseAllDoTween()
        {
            for (var i = 0; i < _m_allTween.Count; i++)
            {
                _m_allTween[i].pause();
            }
        }
        /// <summary>
        /// 恢复播放所有DoTween
        /// </summary>
        public void resumeAllDoTween()
        {
            for (var i = 0; i < _m_allTween.Count; i++)
            {
                _m_allTween[i].resume();
            }
        }

        private class TweenHolder
        {
            private Tween _m_tween;
            private bool _m_hasKill = false;

            public TweenHolder(Tween _tween)
            {
                _m_tween = _tween;
                _m_tween.OnKill(() => { _m_hasKill = true; });
            }

            public void kill()
            {
                if(_m_hasKill == false)
                {
                    _m_tween.Kill();
                }
            }

            public void pause()
            {
                if(_m_tween != null)
                    _m_tween.timeScale = 0;
            }

            public void resume()
            {
                if(_m_tween != null)
                    _m_tween.timeScale = 1;
            }
        }
    }
}