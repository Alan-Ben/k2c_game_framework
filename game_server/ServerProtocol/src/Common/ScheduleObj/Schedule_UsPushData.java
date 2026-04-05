package Common.ScheduleObj;

import java.nio.ByteBuffer;
/*********
 * 下发US的排期数据
 **/
public class Schedule_UsPushData implements ALBasicProtocolPack._IALProtocolStructure {
/** 排期实例ID */
private long scheduleId;
/** US分组ID */
private long usGroupId;
/** 跨服实例ID */
private long crossInstanceId;
/** 排期活动数据 */
private Common.ScheduleObj.Schedule_ActivityInfo activity;
/** US列表 */
private java.util.ArrayList<Integer> usIdList;
/** 资源文件名 */
private String resFile;
/** 资源文件MD5 */
private String resFileMd5;
/** 资源文件目录 */
private String resFileDir;
/** 提交次数 */
private int submitCount;
/** 游戏逻辑主体实例ID */
private long gameLogicInstanceId;


public Schedule_UsPushData() {
	scheduleId = (long)0;
	usGroupId = (long)0;
	crossInstanceId = (long)0;
	activity = new Common.ScheduleObj.Schedule_ActivityInfo();
	usIdList = new java.util.ArrayList<Integer>();
	resFile = "";
	resFileMd5 = "";
	resFileDir = "";
	submitCount = 0;
	gameLogicInstanceId = (long)0;
}

public Schedule_UsPushData(
	 long _scheduleId
	, long _usGroupId
	, long _crossInstanceId
	, Common.ScheduleObj.Schedule_ActivityInfo _activity
	, java.util.ArrayList<Integer> _usIdList
	, String _resFile
	, String _resFileMd5
	, String _resFileDir
	, int _submitCount
	, long _gameLogicInstanceId
) {	scheduleId = _scheduleId;
	usGroupId = _usGroupId;
	crossInstanceId = _crossInstanceId;
	activity = _activity;
	usIdList = _usIdList;
	resFile = _resFile;
	resFileMd5 = _resFileMd5;
	resFileDir = _resFileDir;
	submitCount = _submitCount;
	gameLogicInstanceId = _gameLogicInstanceId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 排期实例ID */
public long getScheduleId() { return scheduleId; }
/** 排期实例ID */
public void setScheduleId(long _scheduleId) { scheduleId = _scheduleId; }
/** US分组ID */
public long getUsGroupId() { return usGroupId; }
/** US分组ID */
public void setUsGroupId(long _usGroupId) { usGroupId = _usGroupId; }
/** 跨服实例ID */
public long getCrossInstanceId() { return crossInstanceId; }
/** 跨服实例ID */
public void setCrossInstanceId(long _crossInstanceId) { crossInstanceId = _crossInstanceId; }
/** 排期活动数据 */
public Common.ScheduleObj.Schedule_ActivityInfo getActivity() { return activity; }
/** 排期活动数据 */
public void setActivity(Common.ScheduleObj.Schedule_ActivityInfo _activity) { activity = _activity; }
/** US列表 */
public java.util.ArrayList<Integer> getUsIdList() { return usIdList; }
/** US列表 */
public void addUsIdList(int _usIdList) { usIdList.add(_usIdList); }
/** 资源文件名 */
public String getResFile() { return resFile; }
/** 资源文件名 */
public void setResFile(String _resFile) { resFile = _resFile; }
/** 资源文件MD5 */
public String getResFileMd5() { return resFileMd5; }
/** 资源文件MD5 */
public void setResFileMd5(String _resFileMd5) { resFileMd5 = _resFileMd5; }
/** 资源文件目录 */
public String getResFileDir() { return resFileDir; }
/** 资源文件目录 */
public void setResFileDir(String _resFileDir) { resFileDir = _resFileDir; }
/** 提交次数 */
public int getSubmitCount() { return submitCount; }
/** 提交次数 */
public void setSubmitCount(int _submitCount) { submitCount = _submitCount; }
/** 游戏逻辑主体实例ID */
public long getGameLogicInstanceId() { return gameLogicInstanceId; }
/** 游戏逻辑主体实例ID */
public void setGameLogicInstanceId(long _gameLogicInstanceId) { gameLogicInstanceId = _gameLogicInstanceId; }


public final int GetBufSize() {
	int _size = 72;
	_size += 2 + (usIdList.size() * 4);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFile);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileDir);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 74;
	_size += 2 + (usIdList.size() * 4);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFile);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileMd5);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resFileDir);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scheduleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usGroupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) crossInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _activityCustLen = _buf.getInt();
	int _activityCurPos = _buf.position();
	activity.ReadUnzipBuf(_buf, _activityCurPos + _activityCustLen);
	_buf.position(_activityCurPos + _activityCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		if(_buf.remaining() > 0) _usIdList = _buf.getInt();
		usIdList.add(_usIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resFile = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resFileMd5 = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resFileDir = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) submitCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gameLogicInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(scheduleId);
	_buf.putLong(usGroupId);
	_buf.putLong(crossInstanceId);
	_buf.putInt(activity.GetBufSize());
	activity.PutUnzipBuf(_buf);
	_buf.putShort((short)usIdList.size());
	for(int _i = 0; _i < usIdList.size(); _i++) { 
		_buf.putInt(usIdList.get(_i));
	}
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resFile);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resFileMd5);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resFileDir);
	_buf.putInt(submitCount);
	_buf.putLong(gameLogicInstanceId);
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

