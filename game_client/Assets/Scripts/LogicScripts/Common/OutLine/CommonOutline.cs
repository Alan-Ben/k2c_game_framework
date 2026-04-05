using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用描边
    /// </summary>
    public class CommonOutline
    {
        //描边类型
        private ENPOutLineType _m_outLineType;
        //描边设置
        private OutLineSetting _m_outLineSetting;

        //2D贴图描边的go
        private GameObject _m_spriteOutlineGo;
        //序列号
        private long _m_serialize;
        
        public CommonOutline(OutLineSetting _outLineSetting)
        {
            if(null == _outLineSetting)
                return;

            _m_outLineType = _outLineSetting.outLineType;
            _m_outLineSetting = _outLineSetting;
            _m_serialize = 0;
        }
        
        //开启描边
        public void openOutLine()
        {
            if(null == _m_outLineSetting)
                return;

            _m_serialize++;
            long serialize = _m_serialize;
            
            if (_m_outLineType == ENPOutLineType.MODEL)
            {
                //没有对应配置不处理
                if (null == _m_outLineSetting.modelSetting)
                    return;
                
                OutlineMgr.instance.openOutlineShow(_m_outLineSetting.modelSetting.skinnedMeshRenderer);
            }
            else if (_m_outLineType == ENPOutLineType.SPRITE)
            {
                //没有对应配置不处理
                if (null == _m_outLineSetting.spriteSetting)
                    return;
                
                //已经生成描边了不重复处理
                if(null != _m_spriteOutlineGo)
                    return;
                
                OutlineMgr.instance.openSpriteOutlineShow(_m_outLineSetting.spriteSetting.spriteOutLineParent, _m_outLineSetting.spriteSetting.spriteOutLineGoIndex,
                    (go) =>
                    {
                        if (serialize != _m_serialize)
                        {
                            OutlineMgr.instance.closeSpriteOutlineShow(_m_outLineSetting.spriteSetting.spriteOutLineGoIndex, go);
                            return;
                        }
                        
                        _m_spriteOutlineGo = go;
                    });
            }
        }

        //关闭描边
        public void closeOutline()
        {
            if(null == _m_outLineSetting)
                return;
            
            _m_serialize++;

            if (_m_outLineType == ENPOutLineType.MODEL)
            {
                //没有对应配置不处理
                if (null == _m_outLineSetting.modelSetting)
                    return;
                
                OutlineMgr.instance.closeOutlineShow(_m_outLineSetting.modelSetting.skinnedMeshRenderer);
            }
            else if (_m_outLineType == ENPOutLineType.SPRITE)
            {
                //没有对应配置不处理
                if (null == _m_outLineSetting.spriteSetting)
                    return;
                
                //没有描边不处理
                if(null == _m_spriteOutlineGo)
                    return;
                
                OutlineMgr.instance.closeSpriteOutlineShow( _m_outLineSetting.spriteSetting.spriteOutLineGoIndex, _m_spriteOutlineGo);
                _m_spriteOutlineGo = null;
            }
        }
    }
}