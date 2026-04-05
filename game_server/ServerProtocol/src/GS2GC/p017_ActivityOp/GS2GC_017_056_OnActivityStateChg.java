package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动状态变更推送
 **/
public class GS2GC_017_056_OnActivityStateChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例ID */
private long instanceId;
/** 活动状态 */
private Common.ActivityEnum.EActivityState state;


public GS2GC_017_056_OnActivityStateChg() {
	instanceId = (long)0;
	state = Common.ActivityEnum.EActivityState.values()[0];
}

public GS2GC_017_056_OnActivityStateChg(
	 long _instanceId
	, Common.ActivityEnum.EActivityState _state
) {	instanceId = _instanceId;
	state = _state;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)56; }

/** 活动实例ID */
public long getInstanceId() { return instanceId; }
/** 活动实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 活动状态 */
public Common.ActivityEnum.EActivityState getState() { return state; }
/** 活动状态 */
public void setState(Common.ActivityEnum.EActivityState _state) { state = _state; }


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
	if(_buf.remaining() > 0) state = Common.ActivityEnum.EActivityState.EActivityState_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(state.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)56);
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

