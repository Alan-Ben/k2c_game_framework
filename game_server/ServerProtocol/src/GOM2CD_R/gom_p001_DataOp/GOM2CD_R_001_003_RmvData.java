package GOM2CD_R.gom_p001_DataOp;

import java.nio.ByteBuffer;
public class GOM2CD_R_001_003_RmvData implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据类型 */
private int dataType;
/** 分组Id */
private long groupId;
/** US服务器Id */
private int usId;
/** 数据Id标识，这个根据每个数据格式不同各自确认 */
private java.util.ArrayList<Long> dataId;


public GOM2CD_R_001_003_RmvData() {
	dataType = 0;
	groupId = (long)0;
	usId = 0;
	dataId = new java.util.ArrayList<Long>();
}

public GOM2CD_R_001_003_RmvData(
	 int _dataType
	, long _groupId
	, int _usId
	, java.util.ArrayList<Long> _dataId
) {	dataType = _dataType;
	groupId = _groupId;
	usId = _usId;
	dataId = _dataId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)3; }

/** 数据类型 */
public int getDataType() { return dataType; }
/** 数据类型 */
public void setDataType(int _dataType) { dataType = _dataType; }
/** 分组Id */
public long getGroupId() { return groupId; }
/** 分组Id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** US服务器Id */
public int getUsId() { return usId; }
/** US服务器Id */
public void setUsId(int _usId) { usId = _usId; }
/** 数据Id标识，这个根据每个数据格式不同各自确认 */
public java.util.ArrayList<Long> getDataId() { return dataId; }
/** 数据Id标识，这个根据每个数据格式不同各自确认 */
public void addDataId(long _dataId) { dataId.add(_dataId); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (dataId.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (dataId.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dataType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dataIdCount = _buf.getShort();
	for(int _i = 0; _i < _dataIdCount; _i++) { 
		long _dataId = (long)0;
		if(_buf.remaining() > 0) _dataId = _buf.getLong();
		dataId.add(_dataId);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dataType);
	_buf.putLong(groupId);
	_buf.putInt(usId);
	_buf.putShort((short)dataId.size());
	for(int _i = 0; _i < dataId.size(); _i++) { 
		_buf.putLong(dataId.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)3);
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

