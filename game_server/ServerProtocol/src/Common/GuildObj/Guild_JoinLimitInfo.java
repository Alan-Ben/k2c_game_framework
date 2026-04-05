package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 加入限制信息
 **/
public class Guild_JoinLimitInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 限制类型 */
private Common.GuildEnum.EGuildJoinLimitType type;
/** 数值 */
private long value;


public Guild_JoinLimitInfo() {
	type = Common.GuildEnum.EGuildJoinLimitType.values()[0];
	value = (long)0;
}

public Guild_JoinLimitInfo(
	 Common.GuildEnum.EGuildJoinLimitType _type
	, long _value
) {	type = _type;
	value = _value;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 限制类型 */
public Common.GuildEnum.EGuildJoinLimitType getType() { return type; }
/** 限制类型 */
public void setType(Common.GuildEnum.EGuildJoinLimitType _type) { type = _type; }
/** 数值 */
public long getValue() { return value; }
/** 数值 */
public void setValue(long _value) { value = _value; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.GuildEnum.EGuildJoinLimitType.EGuildJoinLimitType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putLong(value);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

