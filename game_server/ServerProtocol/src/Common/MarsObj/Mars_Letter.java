package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星居民-信件数据
 **/
public class Mars_Letter implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 信件ID */
private long letterId;
private long npcId;
/** 已处理 */
private boolean isDealed;


public Mars_Letter() {
	id = (long)0;
	letterId = (long)0;
	npcId = (long)0;
	isDealed = false;
}

public Mars_Letter(
	 long _id
	, long _letterId
	, long _npcId
	, boolean _isDealed
) {	id = _id;
	letterId = _letterId;
	npcId = _npcId;
	isDealed = _isDealed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 信件ID */
public long getLetterId() { return letterId; }
/** 信件ID */
public void setLetterId(long _letterId) { letterId = _letterId; }
public long getNpcId() { return npcId; }
public void setNpcId(long _npcId) { npcId = _npcId; }
/** 已处理 */
public boolean getIsDealed() { return isDealed; }
/** 已处理 */
public void setIsDealed(boolean _isDealed) { isDealed = _isDealed; }


public final int GetBufSize() {
	int _size = 25;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) letterId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) npcId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isDealed = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(letterId);
	_buf.putLong(npcId);
	_buf.put(isDealed?(byte)1:(byte)0);
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

