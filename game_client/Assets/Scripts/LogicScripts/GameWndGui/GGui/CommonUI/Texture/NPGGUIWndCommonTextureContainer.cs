
using System.Collections.Generic;
using GOE;

public class NPGGUIWndCommonTextureContainer : _AGGUISubWndCommonContainer<NPGGUIMonoCommonTexture, NPGGUIMonoCommonTextureContainer, NPGGUIWndCommonTexture>
{
    private List<NPGTextureIndex> _m_textureList;
    
    
    public NPGGUIWndCommonTextureContainer(NPGGUIMonoCommonTextureContainer _containerMono) 
        : base(_containerMono)
    {
    }
    

    protected override NPGGUIWndCommonTexture _createItemWnd(NPGGUIMonoCommonTexture _itemMono)
    {
        return new NPGGUIWndCommonTexture(_itemMono);
    }
    protected override void _refreshItemWnd(NPGGUIWndCommonTexture _itemWnd, int _index)
    {
        if (_m_textureList == null)
            return;
        
        if (_index < 0 || _index >= _m_textureList.Count)
            return;
        
        NPGTextureIndex textureIndex = _m_textureList[_index];
        _itemWnd.setTexture(textureIndex);
    }


    public void refreshWnd(List<NPGTextureIndex> _textureList)
    {
        _m_textureList = _textureList;
        refreshWnd(_m_textureList?.Count ?? 0);
    }
}