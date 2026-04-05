package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 白名单列表
 **/
public class ServerObj_WhiteAccList implements ALBasicProtocolPack._IALProtocolStructure {
/** 账号列表 */
private java.util.ArrayList<String> accList;


public ServerObj_WhiteAccList() {
	accList = new java.util.ArrayList<String>();
}

public ServerObj_WhiteAccList(
	 java.util.ArrayList<String> _accList
) {	accList = _accList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 账号列表 */
public java.util.ArrayList<String> getAccList() { return accList; }
/** 账号列表 */
public void addAccList(String _accList) { accList.add(_accList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2;
	for(int _i = 0; _i < accList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accList.get(_i));
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
	for(int _i = 0; _i < accList.size(); _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accList.get(_i));
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _accListCount = _buf.getShort();
	for(int _i = 0; _i < _accListCount; _i++) { 
		String _accList = "";
		if(_buf.remaining() > 0) _accList = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
		accList.add(_accList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)accList.size());
	for(int _i = 0; _i < accList.size(); _i++) { 
		ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, accList.get(_i));
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

