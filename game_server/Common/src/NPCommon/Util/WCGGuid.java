package NPCommon.Util;

import java.nio.ByteBuffer;
import java.util.UUID;

public class WCGGuid
{
    public static long newGuid()
    {

        UUID guid = java.util.UUID.randomUUID();
        ByteBuffer buf16 = ByteBuffer.allocate(16);
        buf16.putLong(guid.getMostSignificantBits());
        buf16.putLong(guid.getLeastSignificantBits());

        ByteBuffer buf8 = ByteBuffer.allocate(8);
        for (int i = 0; i < 8; i++)
        {
            buf8.put(buf16.get(i * 2));
        }
        buf8.flip();
        return Math.abs(buf8.getLong());
    }
}