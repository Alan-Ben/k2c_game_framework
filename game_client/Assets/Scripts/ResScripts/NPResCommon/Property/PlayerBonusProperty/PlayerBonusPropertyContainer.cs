using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 玩家四大基础属性容器
    /// </summary>
    public class PlayerBonusPropertyContainer : _ATNPBasicPropertyContainer<EBonusPropertyType, PlayerBonusPropertyModifier, PlayerBonusPropertyContainer>
    {
        //容器标识
        private string _m_containerName = string.Empty;

        public PlayerBonusPropertyContainer() : base()
        {

        }

        public PlayerBonusPropertyContainer(string _containerName) : base()
        {
            _m_containerName = _containerName;
        }

        public override PlayerBonusPropertyContainer _createContainer()
        {
            return new PlayerBonusPropertyContainer(_m_containerName + "copy");
        }

        public override string getName()
        {
            return _m_containerName;
        }
    }
}
