package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 联盟宝箱结算数据列表
 **/
public class ServerObj_GuildBoxSettleList implements ALBasicProtocolPack._IALProtocolStructure {
private java.util.ArrayList<Common.ServerObj.ServerObj_GuildBoxSettle> settleList;


public ServerObj_GuildBoxSettleList() {
	settleList = new java.util.ArrayList<Common.ServerObj.ServerObj_GuildBoxSettle>();
}

public ServerObj_GuildBoxSettleList(
	 java.util.ArrayList<Common.ServerObj.ServerObj_GuildBoxSettle> _settleList
) {	settleList = _settleList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public java.util.ArrayList<Common.ServerObj.ServerObj_GuildBoxSettle> getSettleList() { return settleList; }
public void addSettleList(Common.ServerObj.ServerObj_GuildBoxSettle _settleList) { settleList.add(_settleList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < settleList.size(); _i++) {
	_size += 4 + settleList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < settleList.size(); _i++) {
	_size += 4 + settleList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _settleListCount = _buf.getShort();
	for(int _i = 0; _i < _settleListCount; _i++) { 
		Common.ServerObj.ServerObj_GuildBoxSettle _settleList = new Common.ServerObj.ServerObj_GuildBoxSettle();
		if(_buf.remaining() <= 0) return;
	int __settleListCustLen = _buf.getInt();
	int __settleListCurPos = _buf.position();
	_settleList.ReadUnzipBuf(_buf, __settleListCurPos + __settleListCustLen);
	_buf.position(__settleListCurPos + __settleListCustLen);

		settleList.add(_settleList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)settleList.size());
	for(int _i = 0; _i < settleList.size(); _i++) { 
		_buf.putInt(settleList.get(_i).GetBufSize());
	settleList.get(_i).PutUnzipBuf(_buf);
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

