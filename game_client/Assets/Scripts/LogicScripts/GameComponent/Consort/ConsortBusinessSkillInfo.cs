using System;
using System.Collections.Generic;
using Common.ConsortObj;
using CommonEnum;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 妃子经营技能信息列表
    /// </summary>
    public class ConsortBusinessSkillInfoList : _AReqBaseItem<List<ConsortBusinessSkillInfo>>
    {
        private long _m_lConsortId;//妃子id
        
        private List<Consort_BusinessSkillPropertySum> _m_lBusinessSkillPropertySumList;//经营技能属性加成汇总列表

        // 补充的经营技能信息
        [NotNull] private List<ConsortBusinessSkillInfo> _m_lSupplementBusinessSkillInfo = new List<ConsortBusinessSkillInfo>();
        
        public ConsortBusinessSkillInfoList(long _consortId, List<Consort_BusinessSkillPropertySum> _businessSkillPropertySumList)
        {
            _m_lConsortId = _consortId;

            _m_lBusinessSkillPropertySumList = _businessSkillPropertySumList;
        }
        
        public long consortId => _m_lConsortId;
        
        public List<Consort_BusinessSkillPropertySum> businessSkillPropertySumList => _m_lBusinessSkillPropertySumList;
        
        protected override void _onReqData(Action<List<ConsortBusinessSkillInfo>> _doneAction)
        {
            NPPlayer.instance.consortComp.reqGetAllBusinessSkill(_m_lConsortId, (_isSucc, _msg) =>
            {
                List<ConsortBusinessSkillInfo> _list = new List<ConsortBusinessSkillInfo>();
                // 清除经营技能属性加成汇总列表, 进行重新统计
                if(_m_lBusinessSkillPropertySumList == null)
                    _m_lBusinessSkillPropertySumList = new List<Consort_BusinessSkillPropertySum>();
                _clearSkillPropertyAdd();
                
                if (_isSucc && _msg != null && _msg.getDataList() != null)
                {
                    foreach (var item in _msg.getDataList())
                    {
                        if(item == null)
                            continue;

                        ConsortBusinessSkillInfo businessSkillInfo = new ConsortBusinessSkillInfo(item);
                        _list.Add(businessSkillInfo);
                        
                        // 统计技能属性加成
                        if(businessSkillInfo.consortBusinessSkillRef != null)
                            _addSkillPropertyAdd(businessSkillInfo.consortBusinessSkillRef.property, businessSkillInfo.proAdd);
                    }
                }
                
                _doneAction?.Invoke(_list);
            });
        }

        protected override void _onResetData()
        {
            _clearSkillPropertyAdd();//清除技能属性加成
            _m_lSupplementBusinessSkillInfo.Clear();
        }

        public void getSkillInfo(long _skillId, Action<ConsortBusinessSkillInfo> _doneAction)
        {
            getValue((_list) =>
            {
                ConsortBusinessSkillInfo businessSkillInfo = _list?.Find((_item) => _item != null && _item.skillId == _skillId);
                if(businessSkillInfo == null)
                    businessSkillInfo = _m_lSupplementBusinessSkillInfo.Find(_item => _item != null && _item.skillId == _skillId);
                _doneAction?.Invoke(businessSkillInfo);
            });
        }

        public void getAllSkillInfo(Action<List<ConsortBusinessSkillInfo>> _doneAction)
        {
            List<ConsortBusinessSkillInfo> businessSkillInfoList = new List<ConsortBusinessSkillInfo>();
            getValue((_list) =>
            {
                if(_list != null)
                    businessSkillInfoList.AddRange(_list);
                
                businessSkillInfoList.AddRange(_m_lSupplementBusinessSkillInfo);
                
                _doneAction?.Invoke(businessSkillInfoList);
            });
        }
        
        /// <summary>
        /// 更新技能信息
        /// </summary>
        /// <param name="_serverBusinessSkillInfo">服务器给的经营技能数据</param>
        public void updateSkillInfo(Common.ConsortObj.Consort_BusinessSkill _serverBusinessSkillInfo, Action<ConsortBusinessSkillInfo, bool> _doneAction)
        {
            if (_serverBusinessSkillInfo == null)
            {
                _doneAction?.Invoke(null, false);
                return;
            }
            
            // 先获取技能信息
            getValue((_list) =>
            {
                ConsortBusinessSkillInfo businessSkillInfo = _list?.Find((_item) => _item != null && _item.skillId == _serverBusinessSkillInfo.getSkillId());
                if(businessSkillInfo == null)
                    businessSkillInfo = _m_lSupplementBusinessSkillInfo.Find(_item => _item != null && _item.skillId == _serverBusinessSkillInfo.getSkillId());

                bool hasProChg = false;
                
                if (businessSkillInfo != null)
                {
                    hasProChg = businessSkillInfo.proAdd != _serverBusinessSkillInfo.getProAdd();
                    
                    if (hasProChg && businessSkillInfo.consortBusinessSkillRef != null)
                        _removeSkillPropertyAdd(businessSkillInfo.consortBusinessSkillRef.property, businessSkillInfo.proAdd);//先移除原有的属性加成
                    
                    businessSkillInfo.updateSkillInfo(_serverBusinessSkillInfo);
                    
                    if(hasProChg && businessSkillInfo.consortBusinessSkillRef != null)
                        _addSkillPropertyAdd(businessSkillInfo.consortBusinessSkillRef.property, businessSkillInfo.proAdd);// 添加新的属性加成
                    
                    _doneAction?.Invoke(businessSkillInfo, hasProChg);
                }
                else
                {
                    hasProChg = _serverBusinessSkillInfo.getProAdd() != 0;
                    
                    businessSkillInfo = new ConsortBusinessSkillInfo(_serverBusinessSkillInfo);
                    _m_lSupplementBusinessSkillInfo.Add(businessSkillInfo);
                    
                    if(hasProChg && businessSkillInfo.consortBusinessSkillRef != null)
                        _addSkillPropertyAdd(businessSkillInfo.consortBusinessSkillRef.property, businessSkillInfo.proAdd);// 添加新的属性加成
                    
                    _doneAction?.Invoke(businessSkillInfo, hasProChg);
                }
            });
        }

        [NotNull] private Consort_BusinessSkillPropertySum _ensureGetSkillPropertyAddPropertySum(ESpecAttrType _attrType)
        {
            if (_m_lBusinessSkillPropertySumList == null)
                _m_lBusinessSkillPropertySumList = new List<Consort_BusinessSkillPropertySum>();
            
            Consort_BusinessSkillPropertySum propertySum = _m_lBusinessSkillPropertySumList.Find((_item) => _item != null && _item.getAttr() == _attrType);
            if (propertySum == null)
            {
                propertySum = new Consort_BusinessSkillPropertySum(_attrType, 0);
                _m_lBusinessSkillPropertySumList.Add(propertySum);
            }

            return propertySum;
        }
        
        /// <summary>
        /// 获取经营技能对建筑属性加成
        /// </summary>
        /// <returns></returns>
        public Consort_BusinessSkillPropertySum getSkillPropertyAddPropertySum(ESpecAttrType _attrType)
        {
            if (_m_lBusinessSkillPropertySumList == null)
                return null;
            
            return _m_lBusinessSkillPropertySumList.Find((_item) => _item != null && _item.getAttr() == _attrType);
        }
        
        /// <summary>
        /// 获取经营技能对建筑属性加成
        /// </summary>
        /// <returns></returns>
        public Consort_BusinessSkillPropertySum getSkillPropertyAddPropertySumNewInstance(ESpecAttrType _attrType)
        {
            Consort_BusinessSkillPropertySum propertySum = getSkillPropertyAddPropertySum(_attrType);
            if (propertySum != null)
            {
                Consort_BusinessSkillPropertySum newInstance = new Consort_BusinessSkillPropertySum(propertySum.getAttr(), propertySum.getProAddSum());
                return newInstance;
            }

            return null;
        }

        /// <summary>
        /// 获取某类型的技能属性加成万分比(这里会把NONE类型也算入, 因为NONE是对全属性加成)
        /// </summary>
        /// <param name="_attrType"></param>
        /// <returns></returns>
        public long getSkillPropertyAdd(ESpecAttrType _attrType)
        {
            Consort_BusinessSkillPropertySum allPropertySum = getSkillPropertyAddPropertySum(ESpecAttrType.NONE);//全属性加成
            Consort_BusinessSkillPropertySum propertySum = getSkillPropertyAddPropertySum(_attrType);//指定类型的属性加成

            return (allPropertySum?.getProAddSum() ?? 0) + (propertySum?.getProAddSum() ?? 0);
        }
        
        /// <summary>
        /// 更新技能属性加成
        /// </summary>
        /// <param name="_attrType"></param>
        /// <param name="_valueChg"></param>
        private void _updateSkillPropertyAdd(ESpecAttrType _attrType, long _valueChg)
        {
            Consort_BusinessSkillPropertySum propertySum = _ensureGetSkillPropertyAddPropertySum(_attrType);
            propertySum.setProAddSum(_valueChg);
        }

        /// <summary>
        /// 增加技能属性加成
        /// </summary>
        /// <param name="_attrType"></param>
        /// <param name="_valueChg"></param>
        private void _addSkillPropertyAdd(ESpecAttrType _attrType, long _valueChg)
        {
            Consort_BusinessSkillPropertySum propertySum = _ensureGetSkillPropertyAddPropertySum(_attrType);
            propertySum.setProAddSum(propertySum.getProAddSum() + _valueChg);
        }
        
        /// <summary>
        /// 移除技能属性加成
        /// </summary>
        /// <param name="_attrType"></param>
        /// <param name="_valueChg"></param>
        private void _removeSkillPropertyAdd(ESpecAttrType _attrType, long _valueChg)
        {
            Consort_BusinessSkillPropertySum propertySum = _ensureGetSkillPropertyAddPropertySum(_attrType);
            propertySum.setProAddSum(propertySum.getProAddSum() - _valueChg);
        }
        
        /// <summary>
        /// 清除技能属性加成
        /// </summary>
        private void _clearSkillPropertyAdd()
        {
            _m_lBusinessSkillPropertySumList?.Clear();
        }
        
        /// <summary>
        /// 清除技能属性加成
        /// </summary>
        /// <param name="_attrType"></param>
        private void _clearSkillPropertyAdd(ESpecAttrType _attrType)
        {
            Consort_BusinessSkillPropertySum propertySum = getSkillPropertyAddPropertySum(_attrType);
            propertySum?.setProAddSum(0);
        }
    }
    
    /// <summary>
    /// 妃子经营技能信息
    /// </summary>
    public class ConsortBusinessSkillInfo
    {
        private long _m_lSkillId;//技能id
        private long _m_lProAdd;//加成万分比
        
        private int _m_iNormalOpCount;//普通领悟次数
        private int _m_iAdvanceOpCount;//高级领悟次数

        private ConsortBusinessSkillRefObj _m_rConsortBusinessSkillRef;//技能配置表
        
        public ConsortBusinessSkillInfo(Common.ConsortObj.Consort_BusinessSkill _serverBusinessSkill)
        {
            updateSkillInfo(_serverBusinessSkill);
        }
        
        public long skillId => _m_lSkillId;
        public long proAdd => _m_lProAdd;
        public int normalOpCount => _m_iNormalOpCount;
        public int advanceOpCount => _m_iAdvanceOpCount;

        /// <summary>
        /// 加成是否已经达到最大加成值
        /// </summary>
        public bool isReachAddMax
        {
            get
            {
                return _m_lProAdd >= maxAddValue;
            }
        }

        public long maxAddValue
        {
            get
            {
                if (consortBusinessSkillRef == null)
                    return 0;
                
                long maxAddValue = long.MinValue;
                if (consortBusinessSkillRef.normalAddProGroupRefObj != null)
                    maxAddValue = Math.Max(maxAddValue, consortBusinessSkillRef.normalAddProGroupRefObj.maxAddProRefObj?.add ?? long.MinValue);

                if (consortBusinessSkillRef.advanceAddProGroup != null)
                    maxAddValue = Math.Max(maxAddValue, consortBusinessSkillRef.advanceAddProGroup.maxAddProRefObj?.add ?? long.MinValue);

                return maxAddValue;
            }
        }
        
        public ConsortBusinessSkillRefObj consortBusinessSkillRef
        {
            get
            {
                if (_m_rConsortBusinessSkillRef == null || _m_rConsortBusinessSkillRef.id != _m_lSkillId)
                    _m_rConsortBusinessSkillRef = GRefdataCoreMgr.instance.consortBusinessSkillRefCore.getRef(_m_lSkillId);
                
                if(_m_rConsortBusinessSkillRef == null)
                    Debug.LogError($"[ConsortBusinessSkillInfo consortBusinessSkillRef] 获取不到 _m_lSkillId:{_m_lSkillId} 对应的经营技能配置表数据, 请检查配置表");
                return _m_rConsortBusinessSkillRef;
            }
        }
        
        /// <summary>
        /// 更新技能数据
        /// </summary>
        /// <param name="_serverBusinessSkill"></param>
        public void updateSkillInfo(Common.ConsortObj.Consort_BusinessSkill _serverBusinessSkill)
        {
            if(_serverBusinessSkill == null)
                return;
            
            _m_lSkillId = _serverBusinessSkill.getSkillId();
            _m_lProAdd = _serverBusinessSkill.getProAdd();

            _m_iNormalOpCount = _serverBusinessSkill.getNormalOpCount();
            _m_iAdvanceOpCount = _serverBusinessSkill.getAdvanceOpCount();
        }
    }
}