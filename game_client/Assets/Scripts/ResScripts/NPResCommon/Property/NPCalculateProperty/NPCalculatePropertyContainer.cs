// using System;
// using NPEnum;
//
// namespace GOE
// {
//     /// <summary>
//     /// 辅助计算枚举Container
//     /// </summary>
//     public class NPCalculatePropertyContainer : _ATNPBasicPropertyContainer<ENPPropertyCalculateType, NPCalculatePropertyModifier, NPCalculatePropertyContainer>
//     {
//         //获取标识名字，没有实际用处
//         private string _m_containerName = String.Empty;
//
//         public NPCalculatePropertyContainer() : base()
//         {
//             
//         }
//         
//         public NPCalculatePropertyContainer(string _containerName): base()
//         {
//             _m_containerName = _containerName;
//         }
//
//         public override NPCalculatePropertyContainer _createContainer()
//         {
//             return new NPCalculatePropertyContainer(_m_containerName + "copy");
//         }
//
//         public override string getName()
//         {
//             return _m_containerName;
//         }
//     }
// }