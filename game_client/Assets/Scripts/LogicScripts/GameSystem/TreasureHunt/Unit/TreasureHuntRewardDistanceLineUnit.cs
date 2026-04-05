using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class TreasureHuntRewardDistanceLineUnit : _AGameLoadUnit, _IRelativePosUnit, _IGameTickUnit
    {
        private readonly NPGGoIndex _m_goIndex;
        [NotNull] private readonly Action<TreasureHuntRewardDistanceLineUnit> _m_destroyFunc;
        private readonly Vector3 _m_spawnRelativePosition;
        private readonly Vector3 _m_spawnLogicPosition;
        private readonly float _m_worldScale;
        private float _m_distance;

        private GTDMonoTreasureHuntRewardDistanceLine _m_mono;

        private Vector3 _m_position;


        public TreasureHuntRewardDistanceLineUnit([NotNull] _AGameLogic _gameLogic, NPGGoIndex _goIndex, [NotNull] Action<TreasureHuntRewardDistanceLineUnit> _destroyFunc, Vector3 _spawnRelativePosition, Vector3 _spawnLogicPosition, float _distance, float _worldScale)
            : base(_gameLogic)
        {
            _m_goIndex = _goIndex;
            _m_destroyFunc = _destroyFunc;
            _m_spawnRelativePosition = _spawnRelativePosition;
            _m_spawnLogicPosition = _spawnLogicPosition;
            _m_distance = _distance;
            _m_worldScale = _worldScale;
        }


        public Vector3 position { get { return _m_position; } }
        public float distance { get { return _m_distance; } }


        public override void init()
        {
        }
        public override void discard()
        {
        }
        public void tick(float _deltaTime)
        {
            if (_m_mono == null)
                return;

            // Check if distance line is out of back stage bound
            if (_isOutOfBackBound())
            {
                _m_destroyFunc.Invoke(this);
                return;
            }
        }


        public void updateDistance(float _newDistance)
        {
            _m_distance = _newDistance;
            refreshDistanceText();
        }
        public void updateDistanceFromPlayerPosition(Vector3 _playerPosition)
        {
            float rawDistance = Vector3.Distance(_playerPosition, _m_position);
            _m_distance = rawDistance / _m_worldScale;
            refreshDistanceText();
        }
        public void refreshDistanceText()
        {
            if (_m_mono == null)
                return;

            ALUGUICommon.setLabelTxt(_m_mono.txtDistance, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_commonDistance_value, (int)_m_distance));
        }


        #region Relative Position
        Vector3 _IRelativePosUnit.position { get { return _m_position; } }
        Vector3 _IRelativePosUnit.relativePosition { set { if (_m_mono == null) return; _m_mono.transform.localPosition = value; } }
        #endregion


        protected override void _loadOp(Action _complete)
        {
            // Create distance line mono using scene's createUnit function with relative position
            MainAdditionTreasureHuntGameTDScene.instance.createUnit<GTDMonoTreasureHuntRewardDistanceLine>(_m_goIndex, _m_spawnRelativePosition, _mono =>
            {
                if (_mono == null)
                {
                    ALLog.Error($"TreasureHuntRewardDistanceLineUnit load failed, could not create mono with index {_m_goIndex}");
                    _complete.Invoke();
                    return;
                }

                _m_mono = _mono;

                // Initialize position and properties using logic position
                _m_position = _m_spawnLogicPosition;

                // Set initial distance text
                refreshDistanceText();

                _complete.Invoke();
            });
        }
        protected override void _unloadOp()
        {
            if (_m_mono != null)
            {
                MainAdditionTreasureHuntGameTDScene.instance.discardUnit(_m_goIndex, _m_mono);
                _m_mono = null;
            }
        }


        private bool _isOutOfBackBound()
        {
            if (_m_mono == null)
                return true;

            GTDMonoTreasureHuntGame sceneMono = MainAdditionTreasureHuntGameTDScene.instance.getSceneMono();
            if (sceneMono == null)
                return false;

            if (sceneMono.transGameStageRangeBack == null)
                return false;

            float backZ = sceneMono.transGameStageRangeBack.localPosition.z;
            float lineZ = _m_mono.transform.localPosition.z;

            // Check if distance line is beyond back boundary (further backward)
            return lineZ < backZ;
        }
    }
}