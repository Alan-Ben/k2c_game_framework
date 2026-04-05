package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 玩家繁荣度信息
 **/
public class CommPlayerInfo_FlourishCategory implements ALBasicProtocolPack._IALProtocolStructure {
private NPEnum.ENPFlourishCategory category;
private long value;


public CommPlayerInfo_FlourishCategory() {
	category = NPEnum.ENPFlourishCategory.values()[0];
	value = (long)0;
}

public CommPlayerInfo_FlourishCategory(
	 NPEnum.ENPFlourishCategory _category
	, long _value
) {	category = _category;
	value = _value;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public NPEnum.ENPFlourishCategory getCategory() { return category; }
public void setCategory(NPEnum.ENPFlourishCategory _category) { category = _category; }
public long getValue() { return value; }
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
	if(_buf.remaining() > 0) category = NPEnum.ENPFlourishCategory.ENPFlourishCategory_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(category.ordinal());

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

