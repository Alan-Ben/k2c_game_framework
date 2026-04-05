using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 通用特效控制容器
    /// </summary>
    public class CommonSfxCtrlContainer
    {
        [NotNull]private List<_ISfxObj> _m_lSfxObjList = new List<_ISfxObj>();
        
        //添加对象
        public void addSfxObj(_ISfxObj _sfxObj)
        {
            if (_sfxObj == null)
                return;
            _m_lSfxObjList.Add(_sfxObj);
        }
        
        public void setLayer(int _layer)
        {
            foreach (_ISfxObj sfxObj in _m_lSfxObjList)
            {
                if(null == sfxObj)
                    continue;
                sfxObj.setLayer(_layer);
            }
        }

        //销毁情况
        public void clear()
        {
            foreach (_ISfxObj sfxObj in _m_lSfxObjList)
            {
                if(null == sfxObj)
                    continue;
                sfxObj.forceDiscard();
            }
            
            _m_lSfxObjList.Clear();
        }
    }
}