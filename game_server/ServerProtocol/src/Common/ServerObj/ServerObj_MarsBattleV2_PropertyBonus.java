package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 火星探险-队伍战斗属性加成信息
 **/
public class ServerObj_MarsBattleV2_PropertyBonus implements ALBasicProtocolPack._IALProtocolStructure {
/** 属性类型 */
private int type;
/** 属性值 */
private long value;


public ServerObj_MarsBattleV2_PropertyBonus() {
	type = 0;
	value = (long)0;
}

public ServerObj_MarsBattleV2_PropertyBonus(
	 int _type
	, long _value
) {	type = _type;
	value = _value;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 属性类型 */
public int getType() { return type; }
/** 属性类型 */
public void setType(int _type) { type = _type; }
/** 属性值 */
public long getValue() { return value; }
/** 属性值 */
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
	if(_buf.remaining() > 0) type = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type);
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

