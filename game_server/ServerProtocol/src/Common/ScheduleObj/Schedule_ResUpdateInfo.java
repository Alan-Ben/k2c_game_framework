package Common.ScheduleObj;

import java.nio.ByteBuffer;
/*********
 * 后台排期资源更新信息
 **/
public class Schedule_ResUpdateInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** PHP排期ID */
private long phpScheduleId;
/** 提交次数 */
private int submitCount;
/** 分组列表 */
private java.util.ArrayList<Common.ScheduleObj.Schedule_GroupInfo> groupList;


public Schedule_ResUpdateInfo() {
	phpScheduleId = (long)0;
	submitCount = 0;
	groupList = new java.util.ArrayList<Common.ScheduleObj.Schedule_GroupInfo>();
}

public Schedule_ResUpdateInfo(
	 long _phpScheduleId
	, int _submitCount
	, java.util.ArrayList<Common.ScheduleObj.Schedule_GroupInfo> _groupList
) {	phpScheduleId = _phpScheduleId;
	submitCount = _submitCount;
	groupList = _groupList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** PHP排期ID */
public long getPhpScheduleId() { return phpScheduleId; }
/** PHP排期ID */
public void setPhpScheduleId(long _phpScheduleId) { phpScheduleId = _phpScheduleId; }
/** 提交次数 */
public int getSubmitCount() { return submitCount; }
/** 提交次数 */
public void setSubmitCount(int _submitCount) { submitCount = _submitCount; }
/** 分组列表 */
public java.util.ArrayList<Common.ScheduleObj.Schedule_GroupInfo> getGroupList() { return groupList; }
/** 分组列表 */
public void addGroupList(Common.ScheduleObj.Schedule_GroupInfo _groupList) { groupList.add(_groupList); }


public final int GetBufSize() {
	int _size = 12;
	_size += 2;
	for(int _i = 0; _i < groupList.size(); _i++) {
	_size += 4 + groupList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 2;
	for(int _i = 0; _i < groupList.size(); _i++) {
	_size += 4 + groupList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) phpScheduleId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) submitCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupListCount = _buf.getShort();
	for(int _i = 0; _i < _groupListCount; _i++) { 
		Common.ScheduleObj.Schedule_GroupInfo _groupList = new Common.ScheduleObj.Schedule_GroupInfo();
		if(_buf.remaining() <= 0) return;
	int __groupListCustLen = _buf.getInt();
	int __groupListCurPos = _buf.position();
	_groupList.ReadUnzipBuf(_buf, __groupListCurPos + __groupListCustLen);
	_buf.position(__groupListCurPos + __groupListCustLen);

		groupList.add(_groupList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(phpScheduleId);
	_buf.putInt(submitCount);
	_buf.putShort((short)groupList.size());
	for(int _i = 0; _i < groupList.size(); _i++) { 
		_buf.putInt(groupList.get(_i).GetBufSize());
	groupList.get(_i).PutUnzipBuf(_buf);
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

