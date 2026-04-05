package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星队伍-采集状态数据
 **/
public class MarsTeamState_Collect implements ALBasicProtocolPack._IALProtocolStructure {
/** 火星矿实例ID */
private long mineInstanceId;
/** 行军用时（毫秒），用于返程时长 */
private long marchTimeMS;
/** 所在位置 */
private long pos;
/** 采集截至时间（毫秒） */
private long endCollectMs;
/** 采集开始时间（毫秒） */
private long startCollectMs;


public MarsTeamState_Collect() {
	mineInstanceId = (long)0;
	marchTimeMS = (long)0;
	pos = (long)0;
	endCollectMs = (long)0;
	startCollectMs = (long)0;
}

public MarsTeamState_Collect(
	 long _mineInstanceId
	, long _marchTimeMS
	, long _pos
	, long _endCollectMs
	, long _startCollectMs
) {	mineInstanceId = _mineInstanceId;
	marchTimeMS = _marchTimeMS;
	pos = _pos;
	endCollectMs = _endCollectMs;
	startCollectMs = _startCollectMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 火星矿实例ID */
public long getMineInstanceId() { return mineInstanceId; }
/** 火星矿实例ID */
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }
/** 行军用时（毫秒），用于返程时长 */
public long getMarchTimeMS() { return marchTimeMS; }
/** 行军用时（毫秒），用于返程时长 */
public void setMarchTimeMS(long _marchTimeMS) { marchTimeMS = _marchTimeMS; }
/** 所在位置 */
public long getPos() { return pos; }
/** 所在位置 */
public void setPos(long _pos) { pos = _pos; }
/** 采集截至时间（毫秒） */
public long getEndCollectMs() { return endCollectMs; }
/** 采集截至时间（毫秒） */
public void setEndCollectMs(long _endCollectMs) { endCollectMs = _endCollectMs; }
/** 采集开始时间（毫秒） */
public long getStartCollectMs() { return startCollectMs; }
/** 采集开始时间（毫秒） */
public void setStartCollectMs(long _startCollectMs) { startCollectMs = _startCollectMs; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) mineInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marchTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endCollectMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startCollectMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(mineInstanceId);
	_buf.putLong(marchTimeMS);
	_buf.putLong(pos);
	_buf.putLong(endCollectMs);
	_buf.putLong(startCollectMs);
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

