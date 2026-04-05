package NP2SS_RB.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 返回指定活动在各US的排期信息
 **/
public class ToSS_RB_001_015_QueryActivityByUsAndActivityId implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动列表，每个US一条记录 */
private java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfoByUs> activityList;


public ToSS_RB_001_015_QueryActivityByUsAndActivityId() {
	activityList = new java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfoByUs>();
}

public ToSS_RB_001_015_QueryActivityByUsAndActivityId(
	 java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfoByUs> _activityList
) {	activityList = _activityList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)15; }

/** 活动列表，每个US一条记录 */
public java.util.ArrayList<Common.ScheduleObj.Schedule_SimpleActivityInfoByUs> getActivityList() { return activityList; }
/** 活动列表，每个US一条记录 */
public void addActivityList(Common.ScheduleObj.Schedule_SimpleActivityInfoByUs _activityList) { activityList.add(_activityList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (activityList.size() * 56);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (activityList.size() * 56);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _activityListCount = _buf.getShort();
	for(int _i = 0; _i < _activityListCount; _i++) { 
		Common.ScheduleObj.Schedule_SimpleActivityInfoByUs _activityList = new Common.ScheduleObj.Schedule_SimpleActivityInfoByUs();
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
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)15);
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

