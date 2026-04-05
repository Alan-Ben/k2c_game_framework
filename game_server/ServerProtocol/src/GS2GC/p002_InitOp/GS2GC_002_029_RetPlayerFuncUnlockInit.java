package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_029_RetPlayerFuncUnlockInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 已领取功能类型列表 */
private java.util.ArrayList<NPEnum.ENPFunctionType> hadUnlockTypeList;
/** 客户端已通知功能列表 */
private java.util.ArrayList<NPEnum.ENPFunctionType> clientNotifiedTypeList;


public GS2GC_002_029_RetPlayerFuncUnlockInit() {
	hadUnlockTypeList = new java.util.ArrayList<NPEnum.ENPFunctionType>();
	clientNotifiedTypeList = new java.util.ArrayList<NPEnum.ENPFunctionType>();
}

public GS2GC_002_029_RetPlayerFuncUnlockInit(
	 java.util.ArrayList<NPEnum.ENPFunctionType> _hadUnlockTypeList
	, java.util.ArrayList<NPEnum.ENPFunctionType> _clientNotifiedTypeList
) {	hadUnlockTypeList = _hadUnlockTypeList;
	clientNotifiedTypeList = _clientNotifiedTypeList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)29; }

/** 已领取功能类型列表 */
public java.util.ArrayList<NPEnum.ENPFunctionType> getHadUnlockTypeList() { return hadUnlockTypeList; }
/** 已领取功能类型列表 */
public void addHadUnlockTypeList(NPEnum.ENPFunctionType _hadUnlockTypeList) { hadUnlockTypeList.add(_hadUnlockTypeList); }
/** 客户端已通知功能列表 */
public java.util.ArrayList<NPEnum.ENPFunctionType> getClientNotifiedTypeList() { return clientNotifiedTypeList; }
/** 客户端已通知功能列表 */
public void addClientNotifiedTypeList(NPEnum.ENPFunctionType _clientNotifiedTypeList) { clientNotifiedTypeList.add(_clientNotifiedTypeList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadUnlockTypeList.size() * 4);
	_size += 2 + (clientNotifiedTypeList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadUnlockTypeList.size() * 4);
	_size += 2 + (clientNotifiedTypeList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadUnlockTypeListCount = _buf.getShort();
	for(int _i = 0; _i < _hadUnlockTypeListCount; _i++) { 
		NPEnum.ENPFunctionType _hadUnlockTypeList = NPEnum.ENPFunctionType.values()[0];
		if(_buf.remaining() > 0) _hadUnlockTypeList = NPEnum.ENPFunctionType.ENPFunctionType_FromInt(_buf.getInt());
		hadUnlockTypeList.add(_hadUnlockTypeList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _clientNotifiedTypeListCount = _buf.getShort();
	for(int _i = 0; _i < _clientNotifiedTypeListCount; _i++) { 
		NPEnum.ENPFunctionType _clientNotifiedTypeList = NPEnum.ENPFunctionType.values()[0];
		if(_buf.remaining() > 0) _clientNotifiedTypeList = NPEnum.ENPFunctionType.ENPFunctionType_FromInt(_buf.getInt());
		clientNotifiedTypeList.add(_clientNotifiedTypeList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)hadUnlockTypeList.size());
	for(int _i = 0; _i < hadUnlockTypeList.size(); _i++) { 
		_buf.putInt(hadUnlockTypeList.get(_i).ordinal());

	}
	_buf.putShort((short)clientNotifiedTypeList.size());
	for(int _i = 0; _i < clientNotifiedTypeList.size(); _i++) { 
		_buf.putInt(clientNotifiedTypeList.get(_i).ordinal());

	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)29);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)29);
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

