package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_接待队列列表
 **/
public class Inn_ReceiveList implements ALBasicProtocolPack._IALProtocolStructure {
/** 已接待数量 */
private long hadBeenReceiveNum;
/** 接待队列列表 */
private java.util.ArrayList<Common.InnObj.Inn_ReceiveInfo> receiveList;


public Inn_ReceiveList() {
	hadBeenReceiveNum = (long)0;
	receiveList = new java.util.ArrayList<Common.InnObj.Inn_ReceiveInfo>();
}

public Inn_ReceiveList(
	 long _hadBeenReceiveNum
	, java.util.ArrayList<Common.InnObj.Inn_ReceiveInfo> _receiveList
) {	hadBeenReceiveNum = _hadBeenReceiveNum;
	receiveList = _receiveList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 已接待数量 */
public long getHadBeenReceiveNum() { return hadBeenReceiveNum; }
/** 已接待数量 */
public void setHadBeenReceiveNum(long _hadBeenReceiveNum) { hadBeenReceiveNum = _hadBeenReceiveNum; }
/** 接待队列列表 */
public java.util.ArrayList<Common.InnObj.Inn_ReceiveInfo> getReceiveList() { return receiveList; }
/** 接待队列列表 */
public void addReceiveList(Common.InnObj.Inn_ReceiveInfo _receiveList) { receiveList.add(_receiveList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (receiveList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (receiveList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadBeenReceiveNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _receiveListCount = _buf.getShort();
	for(int _i = 0; _i < _receiveListCount; _i++) { 
		Common.InnObj.Inn_ReceiveInfo _receiveList = new Common.InnObj.Inn_ReceiveInfo();
		if(_buf.remaining() <= 0) return;
	int __receiveListCustLen = _buf.getInt();
	int __receiveListCurPos = _buf.position();
	_receiveList.ReadUnzipBuf(_buf, __receiveListCurPos + __receiveListCustLen);
	_buf.position(__receiveListCurPos + __receiveListCustLen);

		receiveList.add(_receiveList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(hadBeenReceiveNum);
	_buf.putShort((short)receiveList.size());
	for(int _i = 0; _i < receiveList.size(); _i++) { 
		_buf.putInt(receiveList.get(_i).GetBufSize());
	receiveList.get(_i).PutUnzipBuf(_buf);
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

