package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GC2GS_021_040_ReqDealAnecdoteChoiceEvent implements ALBasicProtocolPack._IALProtocolStructure {
/** 政务实例ID */
private long instanceId;
/** 选项ID */
private long optionId;


public GC2GS_021_040_ReqDealAnecdoteChoiceEvent() {
	instanceId = (long)0;
	optionId = (long)0;
}

public GC2GS_021_040_ReqDealAnecdoteChoiceEvent(
	 long _instanceId
	, long _optionId
) {	instanceId = _instanceId;
	optionId = _optionId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)40; }

/** 政务实例ID */
public long getInstanceId() { return instanceId; }
/** 政务实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 选项ID */
public long getOptionId() { return optionId; }
/** 选项ID */
public void setOptionId(long _optionId) { optionId = _optionId; }


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
	if(_buf.remaining() > 0) optionId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(optionId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)40);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)40);
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

