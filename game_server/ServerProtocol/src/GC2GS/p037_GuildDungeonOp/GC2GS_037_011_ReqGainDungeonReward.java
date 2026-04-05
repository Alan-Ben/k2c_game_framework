package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-领取奖励
 **/
public class GC2GS_037_011_ReqGainDungeonReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 公会实例ID */
private long id;
/** 怪物ID */
private long monsterId;


public GC2GS_037_011_ReqGainDungeonReward() {
	id = (long)0;
	monsterId = (long)0;
}

public GC2GS_037_011_ReqGainDungeonReward(
	 long _id
	, long _monsterId
) {	id = _id;
	monsterId = _monsterId;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)11; }

/** 公会实例ID */
public long getId() { return id; }
/** 公会实例ID */
public void setId(long _id) { id = _id; }
/** 怪物ID */
public long getMonsterId() { return monsterId; }
/** 怪物ID */
public void setMonsterId(long _monsterId) { monsterId = _monsterId; }


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
	if(_buf.remaining() > 0) monsterId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(monsterId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)11);
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

