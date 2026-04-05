package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟派遣信息
 **/
public class Guild_DispatchData implements ALBasicProtocolPack._IALProtocolStructure {
/** 相性派遣列表 */
private java.util.ArrayList<Common.GuildObj.Guild_AttrDispatchInfo> dispatchList;


public Guild_DispatchData() {
	dispatchList = new java.util.ArrayList<Common.GuildObj.Guild_AttrDispatchInfo>();
}

public Guild_DispatchData(
	 java.util.ArrayList<Common.GuildObj.Guild_AttrDispatchInfo> _dispatchList
) {	dispatchList = _dispatchList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 相性派遣列表 */
public java.util.ArrayList<Common.GuildObj.Guild_AttrDispatchInfo> getDispatchList() { return dispatchList; }
/** 相性派遣列表 */
public void addDispatchList(Common.GuildObj.Guild_AttrDispatchInfo _dispatchList) { dispatchList.add(_dispatchList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < dispatchList.size(); _i++) {
	_size += 4 + dispatchList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < dispatchList.size(); _i++) {
	_size += 4 + dispatchList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _dispatchListCount = _buf.getShort();
	for(int _i = 0; _i < _dispatchListCount; _i++) { 
		Common.GuildObj.Guild_AttrDispatchInfo _dispatchList = new Common.GuildObj.Guild_AttrDispatchInfo();
		if(_buf.remaining() <= 0) return;
	int __dispatchListCustLen = _buf.getInt();
	int __dispatchListCurPos = _buf.position();
	_dispatchList.ReadUnzipBuf(_buf, __dispatchListCurPos + __dispatchListCustLen);
	_buf.position(__dispatchListCurPos + __dispatchListCustLen);

		dispatchList.add(_dispatchList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)dispatchList.size());
	for(int _i = 0; _i < dispatchList.size(); _i++) { 
		_buf.putInt(dispatchList.get(_i).GetBufSize());
	dispatchList.get(_i).PutUnzipBuf(_buf);
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

