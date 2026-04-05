package NP2GLS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GLS_R_001_005_RegGroupInstance implements ALBasicProtocolPack._IALProtocolStructure {
private long groupId;
private long activityId;
private byte[] addInfo;


public NP2GLS_R_001_005_RegGroupInstance() {
	groupId = (long)0;
	activityId = (long)0;
	addInfo = null;
}

public NP2GLS_R_001_005_RegGroupInstance(
	 long _groupId
	, long _activityId
	, byte[] _addInfo
) {	groupId = _groupId;
	activityId = _activityId;
	addInfo = _addInfo;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)5; }

public long getGroupId() { return groupId; }
public void setGroupId(long _groupId) { groupId = _groupId; }
public long getActivityId() { return activityId; }
public void setActivityId(long _activityId) { activityId = _activityId; }
public byte[] getAddInfo() { return addInfo; }
public java.nio.ByteBuffer get_buffer_AddInfo() { if(null == addInfo)return null; else return ByteBuffer.wrap(addInfo); }

public void setAddInfo(byte[] _addInfo) { addInfo = _addInfo; }
public void setAddInfo(java.nio.ByteBuffer _addInfo) 
{
	if(null == _addInfo){return;}
	int _oldPos = _addInfo.position();
	int _bufLength = _addInfo.remaining();
	addInfo = new byte[_bufLength];
	_addInfo.get(addInfo);
	_addInfo.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 16;
	_size += 4 + (addInfo == null ? 0 : addInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + (addInfo == null ? 0 : addInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) activityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _addInfoCount = _buf.getInt();
	if(0 < _addInfoCount){
		addInfo = new byte[_addInfoCount];
		_buf.get(addInfo);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(activityId);
	_buf.putInt((addInfo == null ? 0 : addInfo.length));
	if(null != addInfo){_buf.put(addInfo);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)5);
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

