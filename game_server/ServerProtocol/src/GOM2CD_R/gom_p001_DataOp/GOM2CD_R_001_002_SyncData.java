package GOM2CD_R.gom_p001_DataOp;

import java.nio.ByteBuffer;
public class GOM2CD_R_001_002_SyncData implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据类型 */
private int dataType;
/** 分组Id */
private long groupId;
/** US服务器Id */
private int usId;
/** 数据队列 */
private java.util.ArrayList<byte[]> dataList;


public GOM2CD_R_001_002_SyncData() {
	dataType = 0;
	groupId = (long)0;
	usId = 0;
	dataList = new java.util.ArrayList<byte[]>();
}

public GOM2CD_R_001_002_SyncData(
	 int _dataType
	, long _groupId
	, int _usId
	, java.util.ArrayList<byte[]> _dataList
) {	dataType = _dataType;
	groupId = _groupId;
	usId = _usId;
	dataList = _dataList;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

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
/** 数据队列 */
public java.util.ArrayList<byte[]> getDataList() { return dataList; }
/** 数据队列 */
public void addDataList(byte[] _dataList) { dataList.add(_dataList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < dataList.size(); _i++) {
	_size += 4 + (dataList.get(_i) == null ? 0 : dataList.get(_i).length);
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < dataList.size(); _i++) {
	_size += 4 + (dataList.get(_i) == null ? 0 : dataList.get(_i).length);
	}


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
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		byte[] _dataList = null;
		if(_buf.remaining() <= 0) return;
	int __dataListCount = _buf.getInt();
	if(0 < __dataListCount){
		_dataList = new byte[__dataListCount];
		_buf.get(_dataList);
	}

		dataList.add(_dataList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dataType);
	_buf.putLong(groupId);
	_buf.putInt(usId);
	_buf.putShort((short)dataList.size());
	for(int _i = 0; _i < dataList.size(); _i++) { 
		_buf.putInt((dataList.get(_i) == null ? 0 : dataList.get(_i).length));
	if(null != dataList.get(_i)){_buf.put(dataList.get(_i));}

	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

