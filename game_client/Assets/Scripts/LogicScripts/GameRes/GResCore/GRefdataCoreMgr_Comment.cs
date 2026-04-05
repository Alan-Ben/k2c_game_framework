using System.Collections.Generic;
using Common.GuildEnum;
using JetBrains.Annotations;

namespace GOE
{
    //弹幕随机信息
    public class CommentRandomInfo
    {
        public NPGTextureIndex icon; // 头像
        public string name; // 名字
            
        public NPCommonAssetPathInfo assetPathInfo; // 资源ab路径
            
        public string comment;//弹幕评论文本
        public List<string> comment_args;//弹幕评论文本参数
    }
    
    public partial class GRefdataCoreMgr
    {
        [NotNull]private Dictionary<long, List<CommentRandomGroupRefObj>> _m_dicComment = new Dictionary<long, List<CommentRandomGroupRefObj>>();
        
        //通过groupId随机一个弹幕
        public CommentRandomInfo getCommentRandomInfoByGroupId(long _groupId)
        {
            CommentRandomGroupRefObj commentRandomGroupRefObj = _getCommentRandomGroupRefObjByGroupId(_groupId);
            if (null == commentRandomGroupRefObj)
                return null;
            
            CommentRandomInfo commentRandomInfo = new CommentRandomInfo();
            commentRandomInfo.comment = commentRandomGroupRefObj.comment;
            commentRandomInfo.comment_args = commentRandomGroupRefObj.comment_args;

            CommentIconRandomRefObj iconRandomRefObj = commentIconRandomRefCore.refList.GetRandomItem();
            if(null != iconRandomRefObj)
                commentRandomInfo.icon = iconRandomRefObj.icon;
            
            CommentNameRandomRefObj nameRandomRefObj = commentNameRandomRefCore.refList.GetRandomItem();
            if(null != nameRandomRefObj)
                commentRandomInfo.name = nameRandomRefObj.name;
            
            CommentPrefabRandomRefObj prefabRandomRefObj = commentPrefabRandomRefCore.refList.GetRandomItem();
            if(null != prefabRandomRefObj)
            {
                commentRandomInfo.assetPathInfo = prefabRandomRefObj.asset_path_info;
            }
            
            return commentRandomInfo;
        }
        
        /// <summary>
        /// 通过一个组随机一个气泡
        /// </summary>
        /// <param name="_groupId"></param>
        /// <returns></returns>
        private CommentRandomGroupRefObj _getCommentRandomGroupRefObjByGroupId(long _groupId)
        {
            //如果这个组有缓存直接随机
            if (_m_dicComment.TryGetValue(_groupId, out List<CommentRandomGroupRefObj> chapterCommentRandomGroupRefList))
            {
                return chapterCommentRandomGroupRefList.GetRandomItem();
            }
            
            if (null == commentRandomGroupRefCore || null == commentRandomGroupRefCore.refList)
                return null;

            chapterCommentRandomGroupRefList = new List<CommentRandomGroupRefObj>();
            _m_dicComment.Add(_groupId, chapterCommentRandomGroupRefList);
            
            //构造缓存数据
            foreach (CommentRandomGroupRefObj randomGroupRefObj in commentRandomGroupRefCore.refList)
            {
                if(null == randomGroupRefObj)
                    continue;
                if (randomGroupRefObj.group_id == _groupId)
                {
                    chapterCommentRandomGroupRefList.Add(randomGroupRefObj);
                }
            }

            return chapterCommentRandomGroupRefList.GetRandomItem();
        }
    }
}