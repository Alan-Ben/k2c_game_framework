package GOM2CD_R.gom_p001_DataOp;

import java.nio.ByteBuffer;
public class GOM2CD_R_001_001_InitData implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据类型 */
private int dataType;
/** 分组Id */
private long groupId;
/** US服务器Id */
private int usId;


public GOM2CD_R_001_001_InitData() {
	dataType = 0;
	groupId = (long)0;
	usId = 0;
}

public GOM2CD_R_001_001_InitData(
	 int _dataType
	, long _groupId
	, int _usId
) {	dataType = _dataType;
	groupId = _groupId;
	usId = _usId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

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


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dataType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(dataType);
	_buf.putLong(groupId);
	_buf.putInt(usId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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

