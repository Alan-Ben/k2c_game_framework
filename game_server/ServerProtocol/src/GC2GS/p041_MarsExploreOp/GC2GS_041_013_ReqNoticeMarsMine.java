package GC2GS.p041_MarsExploreOp;

import java.nio.ByteBuffer;
/*********
 * 火星探险-通知火星矿最新数据
 **/
public class GC2GS_041_013_ReqNoticeMarsMine implements ALBasicProtocolPack._IALProtocolStructure {
/** 火星矿实例ID */
private long id;
/** 检查状态序列号，0表示不检查都要返回 */
private long checkSerialize;


public GC2GS_041_013_ReqNoticeMarsMine() {
	id = (long)0;
	checkSerialize = (long)0;
}

public GC2GS_041_013_ReqNoticeMarsMine(
	 long _id
	, long _checkSerialize
) {	id = _id;
	checkSerialize = _checkSerialize;
}

public final byte getMainOrder() { return (byte)41; }

public final byte getSubOrder() { return (byte)13; }

/** 火星矿实例ID */
public long getId() { return id; }
/** 火星矿实例ID */
public void setId(long _id) { id = _id; }
/** 检查状态序列号，0表示不检查都要返回 */
public long getCheckSerialize() { return checkSerialize; }
/** 检查状态序列号，0表示不检查都要返回 */
public void setCheckSerialize(long _checkSerialize) { checkSerialize = _checkSerialize; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) checkSerialize = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(checkSerialize);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)13);
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

