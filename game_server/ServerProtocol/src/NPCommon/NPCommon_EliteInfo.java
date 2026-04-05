package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_EliteInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private long refId;
private int passtime;
private int battleTask;
private int dailyPassCount;


public NPCommon_EliteInfo() {
	id = (long)0;
	refId = (long)0;
	passtime = 0;
	battleTask = 0;
	dailyPassCount = 0;
}

public NPCommon_EliteInfo(
	 long _id
	, long _refId
	, int _passtime
	, int _battleTask
	, int _dailyPassCount
) {	id = _id;
	refId = _refId;
	passtime = _passtime;
	battleTask = _battleTask;
	dailyPassCount = _dailyPassCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public long getRefId() { return refId; }
public void setRefId(long _refId) { refId = _refId; }
public int getPasstime() { return passtime; }
public void setPasstime(int _passtime) { passtime = _passtime; }
public int getBattleTask() { return battleTask; }
public void setBattleTask(int _battleTask) { battleTask = _battleTask; }
public int getDailyPassCount() { return dailyPassCount; }
public void setDailyPassCount(int _dailyPassCount) { dailyPassCount = _dailyPassCount; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) passtime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) battleTask = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dailyPassCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(refId);
	_buf.putInt(passtime);
	_buf.putInt(battleTask);
	_buf.putInt(dailyPassCount);
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

