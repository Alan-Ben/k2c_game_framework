package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求联盟协作攻击日志列表
 **/
public class GC2GS_032_041_ReqGuildCooperateAttackLogList implements ALBasicProtocolPack._IALProtocolStructure {
/** 上一次读取的数据id */
private long lastDbId;
/** 所需日志数量 */
private int num;


public GC2GS_032_041_ReqGuildCooperateAttackLogList() {
	lastDbId = (long)0;
	num = 0;
}

public GC2GS_032_041_ReqGuildCooperateAttackLogList(
	 long _lastDbId
	, int _num
) {	lastDbId = _lastDbId;
	num = _num;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)41; }

/** 上一次读取的数据id */
public long getLastDbId() { return lastDbId; }
/** 上一次读取的数据id */
public void setLastDbId(long _lastDbId) { lastDbId = _lastDbId; }
/** 所需日志数量 */
public int getNum() { return num; }
/** 所需日志数量 */
public void setNum(int _num) { num = _num; }


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
	if(_buf.remaining() > 0) lastDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(lastDbId);
	_buf.putInt(num);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)41);
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

