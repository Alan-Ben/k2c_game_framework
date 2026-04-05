using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 玩家四大基础属性容器
    /// </summary>
    public class PlayerAttrPropertyContainer : _ATNPBasicPropertyContainer<EBasicAttrType, PlayerAttrPropertyModifier, PlayerAttrPropertyContainer>
    {
        //容器标识
        private string _m_containerName = string.Empty;

        public PlayerAttrPropertyContainer() : base()
        {

        }

        public PlayerAttrPropertyContainer(string _containerName) : base()
        {
            _m_containerName = _containerName;
        }

        public override PlayerAttrPropertyContainer _createContainer()
        {
            return new PlayerAttrPropertyContainer(_m_containerName + "copy");
        }

        public override string getName()
        {
            return _m_containerName;
        }
    }
}
