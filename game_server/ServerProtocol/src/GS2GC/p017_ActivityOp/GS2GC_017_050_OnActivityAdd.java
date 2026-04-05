package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动新增推送
 **/
public class GS2GC_017_050_OnActivityAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动信息 */
private Common.ActivityObj.Activity_Info activity;


public GS2GC_017_050_OnActivityAdd() {
	activity = new Common.ActivityObj.Activity_Info();
}

public GS2GC_017_050_OnActivityAdd(
	 Common.ActivityObj.Activity_Info _activity
) {	activity = _activity;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)50; }

/** 活动信息 */
public Common.ActivityObj.Activity_Info getActivity() { return activity; }
/** 活动信息 */
public void setActivity(Common.ActivityObj.Activity_Info _activity) { activity = _activity; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + activity.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + activity.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _activityCustLen = _buf.getInt();
	int _activityCurPos = _buf.position();
	activity.ReadUnzipBuf(_buf, _activityCurPos + _activityCustLen);
	_buf.position(_activityCurPos + _activityCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(activity.GetBufSize());
	activity.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)50);
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

