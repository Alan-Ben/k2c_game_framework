package NPCommon.GMCommand.Annotation;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.METHOD)
@Retention(RetentionPolicy.RUNTIME)
public @interface ACommand
{

    /**
     * @return 指令名称，默认使用函数名
     */
    String command() default "";

    /**
     * @return 指令说明
     */
    String comment();
}
