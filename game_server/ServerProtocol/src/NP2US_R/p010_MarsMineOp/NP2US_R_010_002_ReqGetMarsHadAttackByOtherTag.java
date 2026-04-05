package NP2US_R.p010_MarsMineOp;

import java.nio.ByteBuffer;
public class NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿实例ID */
private long instanceId;
/** 公会ID */
private long guildId;


public NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag() {
	instanceId = (long)0;
	guildId = (long)0;
}

public NP2US_R_010_002_ReqGetMarsHadAttackByOtherTag(
	 long _instanceId
	, long _guildId
) {	instanceId = _instanceId;
	guildId = _guildId;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)2; }

/** 矿实例ID */
public long getInstanceId() { return instanceId; }
/** 矿实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 公会ID */
public long getGuildId() { return guildId; }
/** 公会ID */
public void setGuildId(long _guildId) { guildId = _guildId; }


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
	if(_buf.remaining() > 0) guildId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(guildId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)2);
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

