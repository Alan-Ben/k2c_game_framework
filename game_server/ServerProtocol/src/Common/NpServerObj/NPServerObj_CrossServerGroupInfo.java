package Common.NpServerObj;

import java.nio.ByteBuffer;
/*********
 * 跨服分组信息
 **/
public class NPServerObj_CrossServerGroupInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 跨服分组id */
private long groupId;
/** 包含usId列表 */
private java.util.ArrayList<Integer> usIdList;
/** 跨服排行实例id */
private long crsInstanceId;


public NPServerObj_CrossServerGroupInfo() {
	groupId = (long)0;
	usIdList = new java.util.ArrayList<Integer>();
	crsInstanceId = (long)0;
}

public NPServerObj_CrossServerGroupInfo(
	 long _groupId
	, java.util.ArrayList<Integer> _usIdList
	, long _crsInstanceId
) {	groupId = _groupId;
	usIdList = _usIdList;
	crsInstanceId = _crsInstanceId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 跨服分组id */
public long getGroupId() { return groupId; }
/** 跨服分组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 包含usId列表 */
public java.util.ArrayList<Integer> getUsIdList() { return usIdList; }
/** 包含usId列表 */
public void addUsIdList(int _usIdList) { usIdList.add(_usIdList); }
/** 跨服排行实例id */
public long getCrsInstanceId() { return crsInstanceId; }
/** 跨服排行实例id */
public void setCrsInstanceId(long _crsInstanceId) { crsInstanceId = _crsInstanceId; }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (usIdList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (usIdList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		if(_buf.remaining() > 0) _usIdList = _buf.getInt();
		usIdList.add(_usIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) crsInstanceId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putShort((short)usIdList.size());
	for(int _i = 0; _i < usIdList.size(); _i++) { 
		_buf.putInt(usIdList.get(_i));
	}
	_buf.putLong(crsInstanceId);
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

