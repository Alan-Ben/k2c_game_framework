package NPGameRes.LogicEvent.MetaData.Annotation;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

/*******************
 * 用于标记LogicEvent数据结构的类型以及参数数量的说明类
 * 需要在LogicEvent子类类型前声明此类，用于在Event响应的时候进行处理
 * @author mj
 *
 */
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface EventDesc
{

    int id() default 0;

    /**
     * @return 事件名称
     */
    String name() default "";

    /**
     * @return 事件参数列表
     */
    String[] params() default {};
}
