using NPCommon;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 固定时间恢复CD组件
    /// </summary>
    public class NPPlayerFixedCDComponent : _ANPBasicPlayerComponent
    {
        private List<NPPlayerFixedCDInfo> _m_lCdList;//列表

        public NPPlayerFixedCDComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lCdList = new List<NPPlayerFixedCDInfo>();
        }


        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.FIXED_CD; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }


        public override void presendInitProtocol()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_048_ReqFixedCdInfo());
        }

        protected override void _dealInit()
        {

        }

        protected override void _onInitDone()
        {

        }

        protected override void _onInitFail()
        {

        }

        protected override void _discard()
        {
            _m_lCdList?.Clear();
        }


        #region 功能方法

        /// <summary>
        /// 根据配置id获取CD信息
        /// </summary>
        /// <param name="_refId"></param>
        /// <returns></returns>
        public NPPlayerFixedCDInfo getCDInfoByRefId(long _refId)
        {
            if (_m_lCdList == null)
                return null;

            for (int i = 0; i < _m_lCdList.Count; i++)
            {
                NPPlayerFixedCDInfo temp = _m_lCdList[i];
                if (temp == null)
                    continue;

                if (temp.refId == _refId)
                    return temp;
            }

            return null;
        }

        /// <summary>
        /// 获取数量
        /// </summary>
        /// <param name="_refId"></param>
        /// <returns></returns>
        public int getCount(long _refId)
        {
            NPPlayerFixedCDInfo cdInfo = getCDInfoByRefId(_refId);
            return cdInfo == null ? 0 : cdInfo.getCount();
        }

        /// <summary>
        /// 获取自然增长上限
        /// </summary>
        /// <param name="_refId"></param>
        /// <returns></returns>
        public int getLimitCount(long _refId)
        {
            NPPlayerFixedCDInfo cdInfo = getCDInfoByRefId(_refId);
            return cdInfo == null ? 0 : cdInfo.getMaxCount();
        }

        #endregion


        #region S2C

        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_msg"></param>
        public void retFixedCdInfo(GS2GC_002_048_RetFixedCdInfo _msg)
        {
            if (_msg == null)
            {
                setInitDone();
                return;
            }

            List<NPCommon_PlayerFixedCD> list = _msg.getCdList();
            if (_m_lCdList != null && list != null)
            {
                _m_lCdList.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    NPCommon_PlayerFixedCD temp = list[i];
                    if (temp == null)
                        continue;

                    _m_lCdList.Add(new NPPlayerFixedCDInfo(temp));
                }
            }

            setInitDone();
        }

        /// <summary>
        /// 数据变更推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onFixedCDChged(GS2GC_021_052_OnFixedCDChged _msg)
        {
            if (_m_lCdList == null || _msg == null)
                return;

            NPCommon_PlayerFixedCD info = _msg.getCdInfo();
            if (info == null)
                return;

            NPPlayerFixedCDInfo cdInfo = getCDInfoByRefId(info.getCdId());
            if (cdInfo == null)
            {
                cdInfo = new NPPlayerFixedCDInfo(info);
                _m_lCdList.Add(cdInfo);
            }
            else
            {
                cdInfo.update(info);
            }
        }

        #endregion
    }
}
