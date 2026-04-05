using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class ClassroomView : _AALBasicLoadObj
    {
        private GTDMonoChildClassroom _m_mono;
        private SeatInfo _m_currentSeatInfo;
        [NotNull] private readonly List<_ISfxObj> _m_sfxList;


        public ClassroomView()
        {
            _m_sfxList = new List<_ISfxObj>();
        }


        protected override void _loadOp()
        {
            MainAdditionChildTDScene.instance.createClassroom(_mono =>
            {
                if (_mono == null)
                {
                    _setLoadDone();
                    return;
                }
                
                _m_mono = _mono;
                if (_m_mono.monoVideoAni != null)
                    _m_mono.monoVideoAni.regVideoPrePreparedDone(updateSeatInfo);
                _setLoadDone();
            });
        }
        
        protected override void _discard()
        {
            _clearAllSfx();

            if (_m_mono == null)
                return;

            MainAdditionChildTDScene.instance.discardClassroom(_m_mono);
            _m_mono = null;
        }
        
        public void updateSeatInfo(SeatInfo _newSeatInfo)
        {
            _m_currentSeatInfo = _newSeatInfo;
            updateSeatInfo();
        }
        public void updateSeatInfo()
        {
            if (_m_mono == null || _m_mono.monoVideoAni == null || _m_currentSeatInfo == null)
                return;

            _m_mono.monoVideoAni.setAniTag(_m_currentSeatInfo.getClassroomVideoName());
        }


        public void playEducateSfx()
        {
            if (_m_mono == null)
                return;

            _playSfx(_m_mono.educateSfxId, _m_mono.educateSfxParent);
        }


        private void _playSfx(long _sfxId, Transform _sfxParent)
        {
            if (_sfxParent == null)
                return;

            _ISfxObj sfx = PlaySfxMgr.instance.playTDSfx(_sfxId, _sfxParent);
            if (sfx != null)
            {
                _m_sfxList.Add(sfx);
                _limitSfxCount();
            }
        }
        private void _limitSfxCount()
        {
            if (_m_mono == null || _m_mono.maxSfxCount < 0)
                return;

            while (_m_sfxList.Count > _m_mono.maxSfxCount)
            {
                _ISfxObj oldestSfx = _m_sfxList[0];
                _m_sfxList.RemoveAt(0);
                oldestSfx?.forceDiscard();
            }
        }
        private void _clearAllSfx()
        {
            foreach (_ISfxObj sfx in _m_sfxList)
                sfx?.forceDiscard();
            _m_sfxList.Clear();
        }
    }
}