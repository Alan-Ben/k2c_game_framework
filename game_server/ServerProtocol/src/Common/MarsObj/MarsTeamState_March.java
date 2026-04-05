package Common.MarsObj;

import java.nio.ByteBuffer;
/*********
 * 火星队伍-行军状态数据
 **/
public class MarsTeamState_March implements ALBasicProtocolPack._IALProtocolStructure {
/** 对象实例ID，对象类型根据进入的状态确认 */
private long instanceId;
/** 目标状态类型 */
private int targetState;


public MarsTeamState_March() {
	instanceId = (long)0;
	targetState = 0;
}

public MarsTeamState_March(
	 long _instanceId
	, int _targetState
) {	instanceId = _instanceId;
	targetState = _targetState;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 对象实例ID，对象类型根据进入的状态确认 */
public long getInstanceId() { return instanceId; }
/** 对象实例ID，对象类型根据进入的状态确认 */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 目标状态类型 */
public int getTargetState() { return targetState; }
/** 目标状态类型 */
public void setTargetState(int _targetState) { targetState = _targetState; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetState = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(targetState);
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

