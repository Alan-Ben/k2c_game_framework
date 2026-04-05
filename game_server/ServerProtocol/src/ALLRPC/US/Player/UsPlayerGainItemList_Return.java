package ALLRPC.US.Player;

import java.nio.ByteBuffer;
public class UsPlayerGainItemList_Return implements ALBasicProtocolPack._IALProtocolStructure {
private Common.RESULT result;
/** 获得物品列表 */
private NPCommon.NPCommon_ItemList gainItemList;


public UsPlayerGainItemList_Return() {
	result = new Common.RESULT();
	gainItemList = new NPCommon.NPCommon_ItemList();
}

public UsPlayerGainItemList_Return(
	 Common.RESULT _result
	, NPCommon.NPCommon_ItemList _gainItemList
) {	result = _result;
	gainItemList = _gainItemList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.RESULT getResult() { return result; }
public void setResult(Common.RESULT _result) { result = _result; }
/** 获得物品列表 */
public NPCommon.NPCommon_ItemList getGainItemList() { return gainItemList; }
/** 获得物品列表 */
public void setGainItemList(NPCommon.NPCommon_ItemList _gainItemList) { gainItemList = _gainItemList; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + result.GetBufSize();
	_size += 4 + gainItemList.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + result.GetBufSize();
	_size += 4 + gainItemList.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _resultCustLen = _buf.getInt();
	int _resultCurPos = _buf.position();
	result.ReadUnzipBuf(_buf, _resultCurPos + _resultCustLen);
	_buf.position(_resultCurPos + _resultCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _gainItemListCustLen = _buf.getInt();
	int _gainItemListCurPos = _buf.position();
	gainItemList.ReadUnzipBuf(_buf, _gainItemListCurPos + _gainItemListCustLen);
	_buf.position(_gainItemListCurPos + _gainItemListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(result.GetBufSize());
	result.PutUnzipBuf(_buf);
	_buf.putInt(gainItemList.GetBufSize());
	gainItemList.PutUnzipBuf(_buf);
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

