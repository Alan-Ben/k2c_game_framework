using System.Collections.Generic;
using System.Text;
using Common.HeroObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 伙伴皮肤数据管理器
    /// </summary>
    public class HeroSkinInfoMgr
    {
        //伙伴已获得皮肤列表
        [NotNull]private List<HeroSkinInfo> _m_lSkinList = new List<HeroSkinInfo>();

        public HeroSkinInfoMgr()
        {
        }

        /// <summary>
        /// 初始化皮肤列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void initSkinList(List<Hero_SkinInfo> _infoList)
        {
            if (_infoList == null)
                return;

            _m_lSkinList.Clear();

            for (int i = 0; i < _infoList.Count; i++)
            {
                _m_lSkinList.Add(new HeroSkinInfo(_infoList[i]));
            }
        }

        /// <summary>
        /// 更新皮肤数据
        /// </summary>
        /// <param name="_skinInfo"></param>
        public void updateSkinInfo(Hero_SkinInfo _skinInfo)
        {
            if (_skinInfo == null)
                return;

            HeroSkinInfo heroSkinInfo = getSkinInfo(_skinInfo.getSkinId());
            if(heroSkinInfo != null)
                heroSkinInfo.updateInfo(_skinInfo);
            else
                _m_lSkinList.Add(new HeroSkinInfo(_skinInfo));
        }

        /// <summary>
        /// 更新皮肤数据
        /// </summary>
        /// <param name="_skinId"></param>
        /// <param name="_level"></param>
        public void updateSkinInfo(long _skinId, int _level)
        {
            if (_skinId <= 0 || _level <= 0)
                return;

            HeroSkinInfo heroSkinInfo = getSkinInfo(_skinId);
            if (heroSkinInfo != null)
                heroSkinInfo.updateInfo(_skinId, _level);
            else
                _m_lSkinList.Add(new HeroSkinInfo(_skinId, _level));
        }

        /// <summary>
        /// 获取伙伴皮肤数据
        /// </summary>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public HeroSkinInfo getSkinInfo(long _skinId)
        {
            for (int i = 0; i < _m_lSkinList.Count; i++)
            {
                if (_m_lSkinList[i] != null && _m_lSkinList[i].skinId == _skinId)
                {
                    return _m_lSkinList[i];
                }
            }

            return null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _m_lSkinList.Count; i++)
            {
                if(_m_lSkinList[i] != null)
                    sb.Append(_m_lSkinList[i].skinId).Append(":").AppendLine(_m_lSkinList[i].level.ToString());
            }
            return sb.ToString();
        }
    }
}