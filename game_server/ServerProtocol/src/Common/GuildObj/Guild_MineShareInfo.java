package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟分享矿信息
 **/
public class Guild_MineShareInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
/** 矿实例ID */
private long mineInstanceId;
/** 分享者CID */
private long finderCid;
/** 矿有效期毫秒时间戳 */
private long mineEndShowMs;


public Guild_MineShareInfo() {
	id = (long)0;
	mineInstanceId = (long)0;
	finderCid = (long)0;
	mineEndShowMs = (long)0;
}

public Guild_MineShareInfo(
	 long _id
	, long _mineInstanceId
	, long _finderCid
	, long _mineEndShowMs
) {	id = _id;
	mineInstanceId = _mineInstanceId;
	finderCid = _finderCid;
	mineEndShowMs = _mineEndShowMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
/** 矿实例ID */
public long getMineInstanceId() { return mineInstanceId; }
/** 矿实例ID */
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
/** 分享者CID */
public long getFinderCid() { return finderCid; }
/** 分享者CID */
public void setFinderCid(long _finderCid) { finderCid = _finderCid; }
/** 矿有效期毫秒时间戳 */
public long getMineEndShowMs() { return mineEndShowMs; }
/** 矿有效期毫秒时间戳 */
public void setMineEndShowMs(long _mineEndShowMs) { mineEndShowMs = _mineEndShowMs; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) finderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineEndShowMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(mineInstanceId);
	_buf.putLong(finderCid);
	_buf.putLong(mineEndShowMs);
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

