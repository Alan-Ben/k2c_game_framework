using Common.MarsEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 火星属性容器
    /// </summary>
    public class MarsPropertyContainer : _ATNPBasicPropertyContainer<EMarsPropertyType, MarsPropertyModifier, MarsPropertyContainer>
    {
        //容器标识
        private string _m_containerName = string.Empty;

        public MarsPropertyContainer() : base()
        {

        }

        public MarsPropertyContainer(string _containerName) : base()
        {
            _m_containerName = _containerName;
        }

        public override MarsPropertyContainer _createContainer()
        {
            return new MarsPropertyContainer(_m_containerName + "copy");
        }

        public override string getName()
        {
            return _m_containerName;
        }
    }
}