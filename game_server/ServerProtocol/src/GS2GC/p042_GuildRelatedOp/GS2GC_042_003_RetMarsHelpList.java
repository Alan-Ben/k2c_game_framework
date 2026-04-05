package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
public class GS2GC_042_003_RetMarsHelpList implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家自身发起的火星求助数据列表 */
private java.util.ArrayList<Common.GuildObj.Guild_MarsHelpInfo> myHelpList;
/** 求助数据列表 */
private java.util.ArrayList<Common.GuildObj.Guild_MarsHelpShowInfo> helpList;


public GS2GC_042_003_RetMarsHelpList() {
	myHelpList = new java.util.ArrayList<Common.GuildObj.Guild_MarsHelpInfo>();
	helpList = new java.util.ArrayList<Common.GuildObj.Guild_MarsHelpShowInfo>();
}

public GS2GC_042_003_RetMarsHelpList(
	 java.util.ArrayList<Common.GuildObj.Guild_MarsHelpInfo> _myHelpList
	, java.util.ArrayList<Common.GuildObj.Guild_MarsHelpShowInfo> _helpList
) {	myHelpList = _myHelpList;
	helpList = _helpList;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)3; }

/** 玩家自身发起的火星求助数据列表 */
public java.util.ArrayList<Common.GuildObj.Guild_MarsHelpInfo> getMyHelpList() { return myHelpList; }
/** 玩家自身发起的火星求助数据列表 */
public void addMyHelpList(Common.GuildObj.Guild_MarsHelpInfo _myHelpList) { myHelpList.add(_myHelpList); }
/** 求助数据列表 */
public java.util.ArrayList<Common.GuildObj.Guild_MarsHelpShowInfo> getHelpList() { return helpList; }
/** 求助数据列表 */
public void addHelpList(Common.GuildObj.Guild_MarsHelpShowInfo _helpList) { helpList.add(_helpList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < myHelpList.size(); _i++) {
	_size += 4 + myHelpList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < helpList.size(); _i++) {
	_size += 4 + helpList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < myHelpList.size(); _i++) {
	_size += 4 + myHelpList.get(_i).GetBufSize();
	}

	_size += 2;
	for(int _i = 0; _i < helpList.size(); _i++) {
	_size += 4 + helpList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _myHelpListCount = _buf.getShort();
	for(int _i = 0; _i < _myHelpListCount; _i++) { 
		Common.GuildObj.Guild_MarsHelpInfo _myHelpList = new Common.GuildObj.Guild_MarsHelpInfo();
		if(_buf.remaining() <= 0) return;
	int __myHelpListCustLen = _buf.getInt();
	int __myHelpListCurPos = _buf.position();
	_myHelpList.ReadUnzipBuf(_buf, __myHelpListCurPos + __myHelpListCustLen);
	_buf.position(__myHelpListCurPos + __myHelpListCustLen);

		myHelpList.add(_myHelpList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _helpListCount = _buf.getShort();
	for(int _i = 0; _i < _helpListCount; _i++) { 
		Common.GuildObj.Guild_MarsHelpShowInfo _helpList = new Common.GuildObj.Guild_MarsHelpShowInfo();
		if(_buf.remaining() <= 0) return;
	int __helpListCustLen = _buf.getInt();
	int __helpListCurPos = _buf.position();
	_helpList.ReadUnzipBuf(_buf, __helpListCurPos + __helpListCustLen);
	_buf.position(__helpListCurPos + __helpListCustLen);

		helpList.add(_helpList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)myHelpList.size());
	for(int _i = 0; _i < myHelpList.size(); _i++) { 
		_buf.putInt(myHelpList.get(_i).GetBufSize());
	myHelpList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)helpList.size());
	for(int _i = 0; _i < helpList.size(); _i++) { 
		_buf.putInt(helpList.get(_i).GetBufSize());
	helpList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
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

