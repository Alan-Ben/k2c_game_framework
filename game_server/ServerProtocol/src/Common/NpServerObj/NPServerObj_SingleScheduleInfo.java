package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 单服排期信息
 **/
public class NPServerObj_SingleScheduleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 排期id */
private long scheduleId;
/** 涉及服务器id列表 */
private java.util.ArrayList<Integer> usIdList;
/** 排期预推送时间,开始推送到US的时间 */
private long prePushTimeMs;
/** 排期开始时间,排期下发到US后的既定开始执行的时间 */
private long startTimeMs;
/** 排期结束时间,用于报警,如果该排期下所属活动,超时5-10分钟未结束需要报警 */
private long stopTimeMs;
/** 活动排期列表 */
private java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityScheduleInfo> activityScheduleList;
/** 活动组排期列表 */
private java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo> activityGroupScheduleList;


public NPServerObj_SingleScheduleInfo() {
	scheduleId = (long)0;
	usIdList = new java.util.ArrayList<Integer>();
	prePushTimeMs = (long)0;
	startTimeMs = (long)0;
	stopTimeMs = (long)0;
	activityScheduleList = new java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityScheduleInfo>();
	activityGroupScheduleList = new java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo>();
}

public NPServerObj_SingleScheduleInfo(
	 long _scheduleId
	, java.util.ArrayList<Integer> _usIdList
	, long _prePushTimeMs
	, long _startTimeMs
	, long _stopTimeMs
	, java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityScheduleInfo> _activityScheduleList
	, java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo> _activityGroupScheduleList
) {	scheduleId = _scheduleId;
	usIdList = _usIdList;
	prePushTimeMs = _prePushTimeMs;
	startTimeMs = _startTimeMs;
	stopTimeMs = _stopTimeMs;
	activityScheduleList = _activityScheduleList;
	activityGroupScheduleList = _activityGroupScheduleList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 排期id */
public long getScheduleId() { return scheduleId; }
/** 排期id */
public void setScheduleId(long _scheduleId) { scheduleId = _scheduleId; }
/** 涉及服务器id列表 */
public java.util.ArrayList<Integer> getUsIdList() { return usIdList; }
/** 涉及服务器id列表 */
public void addUsIdList(int _usIdList) { usIdList.add(_usIdList); }
/** 排期预推送时间,开始推送到US的时间 */
public long getPrePushTimeMs() { return prePushTimeMs; }
/** 排期预推送时间,开始推送到US的时间 */
public void setPrePushTimeMs(long _prePushTimeMs) { prePushTimeMs = _prePushTimeMs; }
/** 排期开始时间,排期下发到US后的既定开始执行的时间 */
public long getStartTimeMs() { return startTimeMs; }
/** 排期开始时间,排期下发到US后的既定开始执行的时间 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 排期结束时间,用于报警,如果该排期下所属活动,超时5-10分钟未结束需要报警 */
public long getStopTimeMs() { return stopTimeMs; }
/** 排期结束时间,用于报警,如果该排期下所属活动,超时5-10分钟未结束需要报警 */
public void setStopTimeMs(long _stopTimeMs) { stopTimeMs = _stopTimeMs; }
/** 活动排期列表 */
public java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityScheduleInfo> getActivityScheduleList() { return activityScheduleList; }
/** 活动排期列表 */
public void addActivityScheduleList(Common.NpServerObj.NPServerObj_ActivityScheduleInfo _activityScheduleList) { activityScheduleList.add(_activityScheduleList); }
/** 活动组排期列表 */
public java.util.ArrayList<Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo> getActivityGroupScheduleList() { return activityGroupScheduleList; }
/** 活动组排期列表 */
public void addActivityGroupScheduleList(Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo _activityGroupScheduleList) { activityGroupScheduleList.add(_activityGroupScheduleList); }


public final int GetBufSize() {
	int _size = 32;
	_size += 2 + (usIdList.size() * 4);
	_size += 2 + (activityScheduleList.size() * 36);
	_size += 2 + (activityGroupScheduleList.size() * 36);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 2 + (usIdList.size() * 4);
	_size += 2 + (activityScheduleList.size() * 36);
	_size += 2 + (activityGroupScheduleList.size() * 36);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scheduleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		if(_buf.remaining() > 0) _usIdList = _buf.getInt();
		usIdList.add(_usIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) prePushTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) stopTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _activityScheduleListCount = _buf.getShort();
	for(int _i = 0; _i < _activityScheduleListCount; _i++) { 
		Common.NpServerObj.NPServerObj_ActivityScheduleInfo _activityScheduleList = new Common.NpServerObj.NPServerObj_ActivityScheduleInfo();
		if(_buf.remaining() <= 0) return;
	int __activityScheduleListCustLen = _buf.getInt();
	int __activityScheduleListCurPos = _buf.position();
	_activityScheduleList.ReadUnzipBuf(_buf, __activityScheduleListCurPos + __activityScheduleListCustLen);
	_buf.position(__activityScheduleListCurPos + __activityScheduleListCustLen);

		activityScheduleList.add(_activityScheduleList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _activityGroupScheduleListCount = _buf.getShort();
	for(int _i = 0; _i < _activityGroupScheduleListCount; _i++) { 
		Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo _activityGroupScheduleList = new Common.NpServerObj.NPServerObj_ActivityGroupScheduleInfo();
		if(_buf.remaining() <= 0) return;
	int __activityGroupScheduleListCustLen = _buf.getInt();
	int __activityGroupScheduleListCurPos = _buf.position();
	_activityGroupScheduleList.ReadUnzipBuf(_buf, __activityGroupScheduleListCurPos + __activityGroupScheduleListCustLen);
	_buf.position(__activityGroupScheduleListCurPos + __activityGroupScheduleListCustLen);

		activityGroupScheduleList.add(_activityGroupScheduleList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(scheduleId);
	_buf.putShort((short)usIdList.size());
	for(int _i = 0; _i < usIdList.size(); _i++) { 
		_buf.putInt(usIdList.get(_i));
	}
	_buf.putLong(prePushTimeMs);
	_buf.putLong(startTimeMs);
	_buf.putLong(stopTimeMs);
	_buf.putShort((short)activityScheduleList.size());
	for(int _i = 0; _i < activityScheduleList.size(); _i++) { 
		_buf.putInt(activityScheduleList.get(_i).GetBufSize());
	activityScheduleList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)activityGroupScheduleList.size());
	for(int _i = 0; _i < activityGroupScheduleList.size(); _i++) { 
		_buf.putInt(activityGroupScheduleList.get(_i).GetBufSize());
	activityGroupScheduleList.get(_i).PutUnzipBuf(_buf);
	}
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

