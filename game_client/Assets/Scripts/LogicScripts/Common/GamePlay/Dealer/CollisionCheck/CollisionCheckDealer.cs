
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个轻量级的简单碰撞检测类
    /// </summary>
    public class CollisionCheckDealer : _AGameDealer
    {
        [ItemNotNull][NotNull] private readonly List<_ICollisionUnit> _m_collisionUnits;
        [NotNull] private readonly HashSet<(int, int)> _m_currentCollisions;
        

        public CollisionCheckDealer([NotNull] _AGameLogic _gameLogic) : base(_gameLogic)
        {
            _m_collisionUnits = new List<_ICollisionUnit>();
            _m_currentCollisions = new HashSet<(int, int)>();
        }
        

        protected override void _onStart()
        {
            _endAllCollisions();
        }
        protected override void _onStop()
        {
            _endAllCollisions();
        }
        protected override void _onTick(float _deltaTime)
        {
            HashSet<(int, int)> newCollisions = new HashSet<(int, int)>();

            for (int i = 0; i < _m_collisionUnits.Count; i++)
            {
                _ICollisionUnit unitA = _m_collisionUnits[i];
                if (!unitA.isEnabled)
                    continue;

                for (int j = i + 1; j < _m_collisionUnits.Count; j++)
                {
                    _ICollisionUnit unitB = _m_collisionUnits[j];
                    if (!unitB.isEnabled)
                        continue;

                    if (_checkCollision(unitA, unitB))
                    {
                        var collisionPair = (i, j);
                        newCollisions.Add(collisionPair);

                        if (!_m_currentCollisions.Contains(collisionPair))
                        {
                            unitA.onCollisionStart(unitB);
                            unitB.onCollisionStart(unitA);
                        }

                        unitA.onCollision(unitB);
                        unitB.onCollision(unitA);
                    }
                }
            }

            foreach (var oldCollision in _m_currentCollisions)
            {
                if (!newCollisions.Contains(oldCollision))
                {
                    var (i, j) = oldCollision;
                    if (i < _m_collisionUnits.Count && j < _m_collisionUnits.Count)
                    {
                        _ICollisionUnit unitA = _m_collisionUnits[i];
                        _ICollisionUnit unitB = _m_collisionUnits[j];
                        unitA.onCollisionEnd(unitB);
                        unitB.onCollisionEnd(unitA);
                    }
                }
            }

            _m_currentCollisions.Clear();
            foreach (var collision in newCollisions)
            {
                _m_currentCollisions.Add(collision);
            }
        }
        protected override void _onAddGameUnit(_AGameUnit _unit)
        {
            if (_unit is _ICollisionUnit collisionUnit)
                _m_collisionUnits.Add(collisionUnit);
        }
        protected override void _onRemoveGameUnit(_AGameUnit _unit)
        {
            if (_unit is _ICollisionUnit collisionUnit)
            {
                int removedIndex = _m_collisionUnits.IndexOf(collisionUnit);
                if (removedIndex >= 0)
                {
                    _endCollisionsForUnit(removedIndex);
                    _m_collisionUnits.RemoveAt(removedIndex);
                    _updateCollisionIndicesAfterRemoval(removedIndex);
                }
            }
        }
        

        private void _endAllCollisions()
        {
            foreach (var (i, j) in _m_currentCollisions)
            {
                if (i < _m_collisionUnits.Count && j < _m_collisionUnits.Count)
                {
                    _ICollisionUnit unitA = _m_collisionUnits[i];
                    _ICollisionUnit unitB = _m_collisionUnits[j];
                    unitA.onCollisionEnd(unitB);
                    unitB.onCollisionEnd(unitA);
                }
            }
            _m_currentCollisions.Clear();
        }
        private void _endCollisionsForUnit(int _unitIndex)
        {
            HashSet<(int, int)> toRemove = new HashSet<(int, int)>();
            
            foreach (var (i, j) in _m_currentCollisions)
            {
                if (i == _unitIndex || j == _unitIndex)
                {
                    toRemove.Add((i, j));
                    
                    if (i < _m_collisionUnits.Count && j < _m_collisionUnits.Count)
                    {
                        _ICollisionUnit unitA = _m_collisionUnits[i];
                        _ICollisionUnit unitB = _m_collisionUnits[j];
                        unitA.onCollisionEnd(unitB);
                        unitB.onCollisionEnd(unitA);
                    }
                }
            }
            
            foreach (var collision in toRemove)
            {
                _m_currentCollisions.Remove(collision);
            }
        }
        private void _updateCollisionIndicesAfterRemoval(int _removedIndex)
        {
            HashSet<(int, int)> updatedCollisions = new HashSet<(int, int)>();
            
            foreach (var (i, j) in _m_currentCollisions)
            {
                int newI = i > _removedIndex ? i - 1 : i;
                int newJ = j > _removedIndex ? j - 1 : j;
                
                if (newI != newJ && newI >= 0 && newJ >= 0)
                {
                    updatedCollisions.Add((newI, newJ));
                }
            }
            
            _m_currentCollisions.Clear();
            foreach (var collision in updatedCollisions)
            {
                _m_currentCollisions.Add(collision);
            }
        }
        private bool _checkCollision([NotNull] _ICollisionUnit _unitA, [NotNull] _ICollisionUnit _unitB)
        {
            if (_unitA is _ISphereCollisionUnit sphereA && _unitB is _ISphereCollisionUnit sphereB)
            {
                return _checkSphereCollision(sphereA, sphereB);
            }
            if (_unitA is _IAABBCollisionUnit aabbA && _unitB is _IAABBCollisionUnit aabbB)
            {
                return _checkAABBCollision(aabbA, aabbB);
            }
            if (_unitA is _ISphereCollisionUnit sphere && _unitB is _IAABBCollisionUnit aabb)
            {
                return _checkSphereAABBCollision(sphere, aabb);
            }
            if (_unitA is _IAABBCollisionUnit aabb2 && _unitB is _ISphereCollisionUnit sphere2)
            {
                return _checkSphereAABBCollision(sphere2, aabb2);
            }

            return false;
        }
        private bool _checkSphereCollision([NotNull] _ISphereCollisionUnit _sphereA, [NotNull] _ISphereCollisionUnit _sphereB)
        {
            float distance = Vector3.Distance(_sphereA.position, _sphereB.position);
            return distance <= (_sphereA.radius + _sphereB.radius);
        }
        private bool _checkAABBCollision([NotNull] _IAABBCollisionUnit _aabbA, [NotNull] _IAABBCollisionUnit _aabbB)
        {
            Vector3 centerA = _aabbA.position + _aabbA.offset;
            Vector3 centerB = _aabbB.position + _aabbB.offset;
            Vector3 halfSizeA = _aabbA.size * 0.5f;
            Vector3 halfSizeB = _aabbB.size * 0.5f;

            return (Mathf.Abs(centerA.x - centerB.x) <= (halfSizeA.x + halfSizeB.x)) &&
                   (Mathf.Abs(centerA.y - centerB.y) <= (halfSizeA.y + halfSizeB.y)) &&
                   (Mathf.Abs(centerA.z - centerB.z) <= (halfSizeA.z + halfSizeB.z));
        }
        private bool _checkSphereAABBCollision([NotNull] _ISphereCollisionUnit _sphere, [NotNull] _IAABBCollisionUnit _aabb)
        {
            Vector3 aabbCenter = _aabb.position + _aabb.offset;
            Vector3 halfSize = _aabb.size * 0.5f;
            Vector3 closest = new Vector3(
                Mathf.Clamp(_sphere.position.x, aabbCenter.x - halfSize.x, aabbCenter.x + halfSize.x),
                Mathf.Clamp(_sphere.position.y, aabbCenter.y - halfSize.y, aabbCenter.y + halfSize.y),
                Mathf.Clamp(_sphere.position.z, aabbCenter.z - halfSize.z, aabbCenter.z + halfSize.z)
            );

            float distance = Vector3.Distance(_sphere.position, closest);
            return distance <= _sphere.radius;
        }
    }
}