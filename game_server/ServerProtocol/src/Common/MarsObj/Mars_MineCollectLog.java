package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星探索-矿采集日志
 **/
public class Mars_MineCollectLog implements ALBasicProtocolPack._IALProtocolStructure {
/** 队伍ID */
private long teamId;
/** 矿点配置ID */
private long mineRefId;
/** 采集数量 */
private long collectNum;


public Mars_MineCollectLog() {
	teamId = (long)0;
	mineRefId = (long)0;
	collectNum = (long)0;
}

public Mars_MineCollectLog(
	 long _teamId
	, long _mineRefId
	, long _collectNum
) {	teamId = _teamId;
	mineRefId = _mineRefId;
	collectNum = _collectNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 队伍ID */
public long getTeamId() { return teamId; }
/** 队伍ID */
public void setTeamId(long _teamId) { teamId = _teamId; }
/** 矿点配置ID */
public long getMineRefId() { return mineRefId; }
/** 矿点配置ID */
public void setMineRefId(long _mineRefId) { mineRefId = _mineRefId; }
/** 采集数量 */
public long getCollectNum() { return collectNum; }
/** 采集数量 */
public void setCollectNum(long _collectNum) { collectNum = _collectNum; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) teamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) collectNum = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(teamId);
	_buf.putLong(mineRefId);
	_buf.putLong(collectNum);
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

