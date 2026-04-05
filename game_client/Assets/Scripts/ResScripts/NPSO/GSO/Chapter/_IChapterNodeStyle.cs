namespace GOE
{
    //每个节点的表现样式接口
    public interface _IChapterNodeStyle
    {
        public NPGTextureIndex nodeMiniImg { get; }//小贴图
        public NPGGoIndex videoGoPath { get; }//视频资源
        public NPGSpriteIndex maskUiImage { get; }//遮罩资源贴图
        public int videoAnimatorControllerId { get; }//视频动画控制器id
    }
}