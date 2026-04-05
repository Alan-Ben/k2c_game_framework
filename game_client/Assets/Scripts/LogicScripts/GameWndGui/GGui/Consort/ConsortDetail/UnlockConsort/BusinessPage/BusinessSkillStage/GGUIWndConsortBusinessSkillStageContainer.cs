using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortBusinessSkillStageContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoConsortBusinessSkillStage, GGUIMonoConsortBusinessSkillStageContainer, GGUIWndConsortBusinessSkillStage>
    {
        private _IGGUIWndUnLockConsortDetailBusinessPageParam _m_param;
        private GGottenConsortInfo _m_iConsortInfo;//妃子信息
        
        [NotNull] private List<GGUIWndConsortBusinessSkillStage> _m_lBusinessSkillStageList = new List<GGUIWndConsortBusinessSkillStage>();
        /// <summary>
        /// //当前选中的技能item, 不要直接使用, 要调用nowSelectSkillItem
        /// </summary>
        private GGUIPrefabSubWndConsortBusinessSkillItem _m_wNowSelectSkillItem;
        private bool _m_bSkillItemWndInitDone = false;
        private Action _m_aOnSkillItemWndInitDone;
        
        public GGUIWndConsortBusinessSkillStageContainer(GGUIMonoConsortBusinessSkillStageContainer _containerMono, _IGGUIWndUnLockConsortDetailBusinessPageParam _param) : base(_containerMono)
        {
            _m_param = _param;
            initWnd();
        }

        public event Action<GGUIPrefabSubWndConsortBusinessSkillItem> onSkillItemClick;
        
        public long nowSelectSkillId { get { return _m_param?.consortDetailBusinessPageSelectSkillId ?? 0; } }
        public GGUIPrefabSubWndConsortBusinessSkillItem nowSelectSkillItem
        {
            get
            {
                if (_m_wNowSelectSkillItem == null || _m_wNowSelectSkillItem.businessSkillRefObj == null ||
                    _m_wNowSelectSkillItem.businessSkillRefObj.id != nowSelectSkillId)
                {
                    _m_wNowSelectSkillItem = _getBusinessSKillItemWnd(nowSelectSkillId);
                }
                
                return _m_wNowSelectSkillItem;
            }
        }

        protected override void _onWndInitDone()
        {
            _initBusinessSkillStage();
        }
        
        protected override void _onDiscard()
        {
            onSkillItemClick = null;
            
            _discardBusinessSkillStage();
        }
        
        protected override void _onShowWnd()
        {
            _dealAllBusinessSkillStageList((skillStage) =>
            {
                skillStage?.showWnd();
            });
            
            setSelectSkill(_m_param?.consortDetailBusinessPageSelectSkillId ?? 0);
        }

        protected override void _onHideWnd()
        {
            _dealAllBusinessSkillStageList((skillStage) =>
            {
                skillStage?.hideWnd();
            });
            _m_wNowSelectSkillItem = null;//
        }

        protected override void _onReset()
        {
            _dealAllBusinessSkillStageList((skillStage) =>
            {
                skillStage?.resetWnd();
            });
            _m_wNowSelectSkillItem = null;//
        }

        protected override GGUIWndConsortBusinessSkillStage _createItemWnd(GGUIMonoConsortBusinessSkillStage _itemMono)
        {
            GGUIWndConsortBusinessSkillStage itemWnd = new GGUIWndConsortBusinessSkillStage(_itemMono);
            itemWnd.onSkillItemClick += _onItemClick;
            return itemWnd;
        }

        /// <summary>
        /// 设置当前亲密度
        /// </summary>
        /// <param name="_nowIntimacy"></param>
        public void setNowIntimacy(long _nowIntimacy)
        {
            if(wnd == null)
                return;

            _dealAllBusinessSkillStageList((skillStage) =>
            {
                skillStage?.setNowIntimacy(_nowIntimacy);
            });
        }
        
        /// <summary>
        /// 初始化妃子经营技能阶段
        /// </summary>
        private void _initBusinessSkillStage()
        {
            _discardBusinessSkillStage();
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                _m_bSkillItemWndInitDone = true;
                _m_aOnSkillItemWndInitDone?.Invoke();
                _m_aOnSkillItemWndInitDone = null;
            });
            
            List<ConsortBusinessSkillRefObj> businessSkillRefObjList = new List<ConsortBusinessSkillRefObj>();
            businessSkillRefObjList.AddRange(GRefdataCoreMgr.instance.consortBusinessSkillRefCore.refList);
            businessSkillRefObjList.Sort(ConsortBusinessSkillRefObj.sort);
            
            // 每个阶段显示技能数量
            int eachStageSkillCount = wnd == null || wnd.eachStageShowSkillNum <= 0 ? 6 : wnd.eachStageShowSkillNum;
            long perStageSkillLastNeedIntimacy = 0;//上一个阶段最后一个技能解锁需要的亲密度
            List<ConsortBusinessSkillRefObj> tmpBusinessSkillRefObjList = new List<ConsortBusinessSkillRefObj>();//临时存放技能列表
            // 遍历经营技能配表列表数据
            for (int i = 0; i < businessSkillRefObjList.Count; i++)
            {
                ConsortBusinessSkillRefObj businessSkillRefObj = businessSkillRefObjList[i];
                if(businessSkillRefObj == null)
                    continue;
                
                tmpBusinessSkillRefObjList.Add(businessSkillRefObj);
                // 若技能数量达到每个阶段显示技能数量或者遍历到最后一个技能, 则创建一个阶段
                if(tmpBusinessSkillRefObjList.Count >= eachStageSkillCount || (i >= businessSkillRefObjList.Count - 1 && tmpBusinessSkillRefObjList.Count > 0))
                {
                    ConsortBusinessSkillRefObj lastBusinessSkillRefObj = tmpBusinessSkillRefObjList.GetLast();
                    ConsortBusinessSkillStageShowInfo stageShowInfo = new ConsortBusinessSkillStageShowInfo(perStageSkillLastNeedIntimacy, lastBusinessSkillRefObj?.unlock_need_intimacy ?? 0, tmpBusinessSkillRefObjList);
                    tmpBusinessSkillRefObjList.Clear();
                    perStageSkillLastNeedIntimacy = lastBusinessSkillRefObj?.unlock_need_intimacy ?? 0;
                    
                    GGUIWndConsortBusinessSkillStage skillStage = addItemWnd();
                    if(skillStage == null)
                        continue;
                    
                    stepCounter.chgTotalStepCount(1);
                    _m_lBusinessSkillStageList.Add(skillStage);
                    skillStage.initSlider(stageShowInfo);
                    skillStage.regSkillItemWndInitDone(() =>
                    {
                        stepCounter.addDoneStepCount();
                    });
                }
            }
            setSelectSkill(nowSelectSkillId);
            
            stepCounter.addDoneStepCount();
        }
        
        /// <summary>
        /// 销毁
        /// </summary>
        private void _discardBusinessSkillStage()
        {
            foreach (var skillStage in _m_lBusinessSkillStageList)
            {
                if(skillStage == null)
                    continue;
                
                skillStage.onSkillItemClick -= _onItemClick;
                skillStage.discard();
            }
            _m_lBusinessSkillStageList.Clear();

            _m_wNowSelectSkillItem = null;
            _m_bSkillItemWndInitDone = false;
        }
        
        private void _regSkillItemWndInitDone(Action _action)
        {
            if(_m_bSkillItemWndInitDone)
            {
                _action?.Invoke();
                return;
            }
            
            _m_aOnSkillItemWndInitDone += _action;
        }
        
        /// <summary>
        /// 处理所有技能Stage列表
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllBusinessSkillStageList(Action<GGUIWndConsortBusinessSkillStage> _action)
        {
            if(_action == null)
                return;

            foreach (var skillStage in _m_lBusinessSkillStageList)
            {
                if(skillStage == null)
                    continue;

                _action(skillStage);
            }
        }
        
        /// <summary>
        /// 处理所有技能item列表
        /// </summary>
        /// <param name="_action"></param>
        private void _dealAllBusinessSkillItemList(Action<GGUIPrefabSubWndConsortBusinessSkillItem> _action)
        {
            if(_action == null)
                return;

            _dealAllBusinessSkillStageList((_skillStage) =>
            {
                if (_skillStage == null)
                    return;

                _skillStage.dealAllBusinessSkillItemList(_action);
            });
        }

        /// <summary>
        /// 获取技能item窗口
        /// </summary>
        /// <returns></returns>
        private GGUIPrefabSubWndConsortBusinessSkillItem _getBusinessSKillItemWnd(long _skillId)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem itemWnd = null;
            foreach (var skillStage in _m_lBusinessSkillStageList)
            {
                if (skillStage == null)
                    continue;

                itemWnd = skillStage.getBusinessSKillItemWnd(_skillId);
                
                if(itemWnd != null)
                    return itemWnd;
            }

            return null;
        }
        
        /// <summary>
        /// 设置当前服务端经营技能信息
        /// </summary>
        /// <param name="_businessSkillInfo"></param>
        public void setBusinessSkillInfo(ConsortBusinessSkillInfo _businessSkillInfo)
        {
            if(_businessSkillInfo == null)
                return;

            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = _getBusinessSKillItemWnd(_businessSkillInfo.skillId);
            if (itemPrefabSubWnd != null)
            {
                itemPrefabSubWnd.showWnd();
                itemPrefabSubWnd.setBusinessSkillInfo(_businessSkillInfo);
            }
        }

        public void setBusinessSkillInfo(List<ConsortBusinessSkillInfo> _businessSkillInfoList)
        {
            if(_businessSkillInfoList == null)
                return;

            foreach (ConsortBusinessSkillInfo skillInfo in _businessSkillInfoList)
            {
                setBusinessSkillInfo(skillInfo);
            }
        }

        /// <summary>
        /// 播放加成提升成功特效
        /// </summary>
        /// <param name="_skillId"></param>
        public void playProAddSuccessSfx(long _skillId)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = _getBusinessSKillItemWnd(_skillId);
            if (itemPrefabSubWnd != null)
            {
                itemPrefabSubWnd.playProAddSuccessSfx();
            }
        }
        
        /// <summary>
        /// 播放加成提升失败特效
        /// </summary>
        /// <param name="_skillId"></param>
        public void playProAddFailSfx(long _skillId)
        {
            GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd = _getBusinessSKillItemWnd(_skillId);
            if (itemPrefabSubWnd != null)
            {
                itemPrefabSubWnd.playProAddFailSfx();
            }
        }
        
        /// <summary>
        /// 重置所有item显示信息
        /// </summary>
        public void resetAllItemShowInfo()
        {
            _dealAllBusinessSkillItemList((_itemWnd) =>
            {
                _itemWnd?.setBusinessSkillInfo(null);
            });
        }

        /// <summary>
        /// 设置选中某一技能
        /// </summary>
        /// <param name="_skillId"></param>
        public void setSelectSkill(long _skillId)
        {
            nowSelectSkillItem?.setSelect(false);

            if(_m_param != null)
                _m_param.consortDetailBusinessPageSelectSkillId = _skillId;

            GGUIPrefabSubWndConsortBusinessSkillItem selectItemWnd = nowSelectSkillItem;
            if (selectItemWnd != null)//若找到对应技能item, 则选中
            {
                selectItemWnd.setSelect(true);
            }
            else//否则直接选中第一个技能
            {
                selectFirstSKill();
            }
        }

        /// <summary>
        /// 选中第一个技能
        /// </summary>
        public void selectFirstSKill()
        {
            nowSelectSkillItem?.setSelect(false);

            GGUIWndConsortBusinessSkillStage firstBusinessSkillStage = _m_lBusinessSkillStageList.GetFirst();
            if(firstBusinessSkillStage == null)
                return;

            _m_wNowSelectSkillItem = firstBusinessSkillStage.businessSkillItemWndList.GetFirst();
            if(_m_param != null)
                _m_param.consortDetailBusinessPageSelectSkillId = _m_wNowSelectSkillItem?.businessSkillRefObj?.id ?? 0;
            _m_wNowSelectSkillItem?.setSelect(true);
        }
        
        private void _onItemClick(GGUIPrefabSubWndConsortBusinessSkillItem itemPrefabSubWnd)
        {
            onSkillItemClick?.Invoke(itemPrefabSubWnd);
        }
    }
}