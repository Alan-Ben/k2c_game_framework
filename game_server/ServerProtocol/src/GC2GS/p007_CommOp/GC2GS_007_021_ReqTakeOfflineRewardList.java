package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 获取离线奖励列表
 **/
public class GC2GS_007_021_ReqTakeOfflineRewardList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Long> idList;


public GC2GS_007_021_ReqTakeOfflineRewardList() {
	idList = new java.util.ArrayList<Long>();
}

public GC2GS_007_021_ReqTakeOfflineRewardList(
	 java.util.ArrayList<Long> _idList
) {	idList = _idList;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)21; }

public java.util.ArrayList<Long> getIdList() { return idList; }
public void addIdList(long _idList) { idList.add(_idList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (idList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (idList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _idListCount = _buf.getShort();
	for(int _i = 0; _i < _idListCount; _i++) { 
		long _idList = (long)0;
		if(_buf.remaining() > 0) _idList = _buf.getLong();
		idList.add(_idList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)idList.size());
	for(int _i = 0; _i < idList.size(); _i++) { 
		_buf.putLong(idList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)21);
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

