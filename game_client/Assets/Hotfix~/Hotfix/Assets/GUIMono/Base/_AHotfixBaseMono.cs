using System.Reflection;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// Hotfix里面Mono的基类
    /// </summary>
    public abstract class _AHotfixBaseMono
    {
        //初始化方法自动赋值属性
        //这边是用反射机制给每个字段赋值的，性能不好的话可以每个子类每个字段单独getProperty<T>初始化赋值
        public virtual void init(MonoSkin _monoSkin)
        {
            if(null == _monoSkin)
                return;
            
            FieldInfo[] fieldInfos = this.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                HotfixMonoAttribute customAttribute = fieldInfo.GetCustomAttribute<HotfixMonoAttribute>();
                if(null == customAttribute)
                    continue;
					
                //不序列化不生成
                if(!customAttribute.isSerialize)
                    continue;
                
                fieldInfo.SetValue(this, _monoSkin.getProperty(fieldInfo.FieldType, fieldInfo.Name));
            }
        }
    }
}