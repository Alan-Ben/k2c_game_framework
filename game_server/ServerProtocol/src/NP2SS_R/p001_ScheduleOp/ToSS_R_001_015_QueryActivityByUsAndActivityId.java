package NP2SS_R.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 查询指定US列表的指定活动排期
 **/
public class ToSS_R_001_015_QueryActivityByUsAndActivityId implements ALBasicProtocolPack._IALProtocolStructure {
/** 用户服务器ID列表 */
private java.util.ArrayList<Integer> usIdList;
/** 活动ID */
private long activityId;


public ToSS_R_001_015_QueryActivityByUsAndActivityId() {
	usIdList = new java.util.ArrayList<Integer>();
	activityId = (long)0;
}

public ToSS_R_001_015_QueryActivityByUsAndActivityId(
	 java.util.ArrayList<Integer> _usIdList
	, long _activityId
) {	usIdList = _usIdList;
	activityId = _activityId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)15; }

/** 用户服务器ID列表 */
public java.util.ArrayList<Integer> getUsIdList() { return usIdList; }
/** 用户服务器ID列表 */
public void addUsIdList(int _usIdList) { usIdList.add(_usIdList); }
/** 活动ID */
public long getActivityId() { return activityId; }
/** 活动ID */
public void setActivityId(long _activityId) { activityId = _activityId; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (usIdList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (usIdList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		if(_buf.remaining() > 0) _usIdList = _buf.getInt();
		usIdList.add(_usIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)usIdList.size());
	for(int _i = 0; _i < usIdList.size(); _i++) { 
		_buf.putInt(usIdList.get(_i));
	}
	_buf.putLong(activityId);
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

