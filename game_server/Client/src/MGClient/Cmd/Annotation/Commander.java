package MGClient.Cmd.Annotation;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface Commander
{

    /**
     * @return 执行者名称
     */
    String name();

    /**
     * @return 说明
     */
    String comment();
}
