package GC2GS.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理妃子邀约游历事件
 **/
public class GC2GS_008_006_ReqDealInvitationTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件ID */
private long instanceId;
/** 妃子ID */
private long consortId;


public GC2GS_008_006_ReqDealInvitationTravel() {
	instanceId = (long)0;
	consortId = (long)0;
}

public GC2GS_008_006_ReqDealInvitationTravel(
	 long _instanceId
	, long _consortId
) {	instanceId = _instanceId;
	consortId = _consortId;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)6; }

/** 游历事件ID */
public long getInstanceId() { return instanceId; }
/** 游历事件ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 妃子ID */
public long getConsortId() { return consortId; }
/** 妃子ID */
public void setConsortId(long _consortId) { consortId = _consortId; }


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
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(consortId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)6);
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

