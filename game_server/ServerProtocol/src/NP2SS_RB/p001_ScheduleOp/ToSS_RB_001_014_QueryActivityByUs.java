package NP2SS_RB.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 返回排期列表
 **/
public class ToSS_RB_001_014_QueryActivityByUs implements ALBasicProtocolPack._IALProtocolStructure {
/** 排期列表 */
private java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfo> activityList;


public ToSS_RB_001_014_QueryActivityByUs() {
	activityList = new java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfo>();
}

public ToSS_RB_001_014_QueryActivityByUs(
	 java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfo> _activityList
) {	activityList = _activityList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)14; }

/** 排期列表 */
public java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfo> getActivityList() { return activityList; }
/** 排期列表 */
public void addActivityList(Common.ScheduleObj.Schedule_SimpleActivityInfo _activityList) { activityList.add(_activityList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (activityList.size() * 48);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (activityList.size() * 48);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _activityListCount = _buf.getShort();
	for(int _i = 0; _i < _activityListCount; _i++) { 
		Common.ScheduleObj.Schedule_SimpleActivityInfo _activityList = new Common.ScheduleObj.Schedule_SimpleActivityInfo();
		if(_buf.remaining() <= 0) return;
	int __activityListCustLen = _buf.getInt();
	int __activityListCurPos = _buf.position();
	_activityList.ReadUnzipBuf(_buf, __activityListCurPos + __activityListCustLen);
	_buf.position(__activityListCurPos + __activityListCustLen);

		activityList.add(_activityList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)activityList.size());
	for(int _i = 0; _i < activityList.size(); _i++) { 
		_buf.putInt(activityList.get(_i).GetBufSize());
	activityList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)14);
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

