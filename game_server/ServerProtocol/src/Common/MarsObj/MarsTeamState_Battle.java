package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星队伍-battle战斗状态数据
 **/
public class MarsTeamState_Battle implements ALBasicProtocolPack._IALProtocolStructure {
/** 事件实例ID */
private long eventInstanceId;
/** 行军用时（毫秒），用于返程时长 */
private long marchTimeMS;
/** 所在位置 */
private long pos;


public MarsTeamState_Battle() {
	eventInstanceId = (long)0;
	marchTimeMS = (long)0;
	pos = (long)0;
}

public MarsTeamState_Battle(
	 long _eventInstanceId
	, long _marchTimeMS
	, long _pos
) {	eventInstanceId = _eventInstanceId;
	marchTimeMS = _marchTimeMS;
	pos = _pos;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 事件实例ID */
public long getEventInstanceId() { return eventInstanceId; }
/** 事件实例ID */
public void setEventInstanceId(long _eventInstanceId) { eventInstanceId = _eventInstanceId; }
/** 行军用时（毫秒），用于返程时长 */
public long getMarchTimeMS() { return marchTimeMS; }
/** 行军用时（毫秒），用于返程时长 */
public void setMarchTimeMS(long _marchTimeMS) { marchTimeMS = _marchTimeMS; }
/** 所在位置 */
public long getPos() { return pos; }
/** 所在位置 */
public void setPos(long _pos) { pos = _pos; }


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
	if(_buf.remaining() > 0) eventInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marchTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pos = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(eventInstanceId);
	_buf.putLong(marchTimeMS);
	_buf.putLong(pos);
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

