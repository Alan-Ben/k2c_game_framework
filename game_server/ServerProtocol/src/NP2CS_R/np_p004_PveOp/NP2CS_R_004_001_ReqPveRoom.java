package NP2CS_R.np_p004_PveOp;

import java.nio.ByteBuffer;
public class NP2CS_R_004_001_ReqPveRoom implements ALBasicProtocolPack._IALProtocolStructure {
private long missionId;
private int missionType;
private long stageSerial;


public NP2CS_R_004_001_ReqPveRoom() {
	missionId = (long)0;
	missionType = 0;
	stageSerial = (long)0;
}

public NP2CS_R_004_001_ReqPveRoom(
	 long _missionId
	, int _missionType
	, long _stageSerial
) {	missionId = _missionId;
	missionType = _missionType;
	stageSerial = _stageSerial;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)1; }

public long getMissionId() { return missionId; }
public void setMissionId(long _missionId) { missionId = _missionId; }
public int getMissionType() { return missionType; }
public void setMissionType(int _missionType) { missionType = _missionType; }
public long getStageSerial() { return stageSerial; }
public void setStageSerial(long _stageSerial) { stageSerial = _stageSerial; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) missionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) missionType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stageSerial = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(missionId);
	_buf.putInt(missionType);
	_buf.putLong(stageSerial);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)1);
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

