package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
public class PlayerInfo_Record implements ALBasicProtocolPack._IALProtocolStructure {
private NPEnum.ENPPlayerRecordParam type;
private long count;


public PlayerInfo_Record() {
	type = NPEnum.ENPPlayerRecordParam.values()[0];
	count = (long)0;
}

public PlayerInfo_Record(
	 NPEnum.ENPPlayerRecordParam _type
	, long _count
) {	type = _type;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public NPEnum.ENPPlayerRecordParam getType() { return type; }
public void setType(NPEnum.ENPPlayerRecordParam _type) { type = _type; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }


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
	if(_buf.remaining() > 0) type = NPEnum.ENPPlayerRecordParam.ENPPlayerRecordParam_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putLong(count);
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

