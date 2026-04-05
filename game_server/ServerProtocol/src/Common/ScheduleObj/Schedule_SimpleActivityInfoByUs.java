package Common.ScheduleObj;

import java.nio.ByteBuffer;
public class Schedule_SimpleActivityInfoByUs implements ALBasicProtocolPack._IALProtocolStructure {
private int usId;
private Common.ScheduleObj.Schedule_SimpleActivityInfo activityInfo;


public Schedule_SimpleActivityInfoByUs() {
	usId = 0;
	activityInfo = new Common.ScheduleObj.Schedule_SimpleActivityInfo();
}

public Schedule_SimpleActivityInfoByUs(
	 int _usId
	, Common.ScheduleObj.Schedule_SimpleActivityInfo _activityInfo
) {	usId = _usId;
	activityInfo = _activityInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
public Common.ScheduleObj.Schedule_SimpleActivityInfo getActivityInfo() { return activityInfo; }
public void setActivityInfo(Common.ScheduleObj.Schedule_SimpleActivityInfo _activityInfo) { activityInfo = _activityInfo; }


public final int GetBufSize() {
	int _size = 52;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _activityInfoCustLen = _buf.getInt();
	int _activityInfoCurPos = _buf.position();
	activityInfo.ReadUnzipBuf(_buf, _activityInfoCurPos + _activityInfoCustLen);
	_buf.position(_activityInfoCurPos + _activityInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(usId);
	_buf.putInt(activityInfo.GetBufSize());
	activityInfo.PutUnzipBuf(_buf);
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

