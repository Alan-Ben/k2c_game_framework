package Common.PlayerObj;

import java.nio.ByteBuffer;
/*********
 * 玩家事件记录信息
 **/
public class Player_EventRecordInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.PlayerEnum.EPlayerEventRecordType type;
private long subId;
private long count;


public Player_EventRecordInfo() {
	type = Common.PlayerEnum.EPlayerEventRecordType.values()[0];
	subId = (long)0;
	count = (long)0;
}

public Player_EventRecordInfo(
	 Common.PlayerEnum.EPlayerEventRecordType _type
	, long _subId
	, long _count
) {	type = _type;
	subId = _subId;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.PlayerEnum.EPlayerEventRecordType getType() { return type; }
public void setType(Common.PlayerEnum.EPlayerEventRecordType _type) { type = _type; }
public long getSubId() { return subId; }
public void setSubId(long _subId) { subId = _subId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.PlayerEnum.EPlayerEventRecordType.EPlayerEventRecordType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) subId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putLong(subId);
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

