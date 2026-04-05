package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 通用刷新变更
 **/
public class GS2GC_021_073_OnCommonRefreshChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 刷新数据 */
private Common.CommonFuncObj.CommonFunc_Refresh refreshData;


public GS2GC_021_073_OnCommonRefreshChg() {
	refreshData = new Common.CommonFuncObj.CommonFunc_Refresh();
}

public GS2GC_021_073_OnCommonRefreshChg(
	 Common.CommonFuncObj.CommonFunc_Refresh _refreshData
) {	refreshData = _refreshData;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)73; }

/** 刷新数据 */
public Common.CommonFuncObj.CommonFunc_Refresh getRefreshData() { return refreshData; }
/** 刷新数据 */
public void setRefreshData(Common.CommonFuncObj.CommonFunc_Refresh _refreshData) { refreshData = _refreshData; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _refreshDataCustLen = _buf.getInt();
	int _refreshDataCurPos = _buf.position();
	refreshData.ReadUnzipBuf(_buf, _refreshDataCurPos + _refreshDataCustLen);
	_buf.position(_refreshDataCurPos + _refreshDataCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(refreshData.GetBufSize());
	refreshData.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)73);
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

