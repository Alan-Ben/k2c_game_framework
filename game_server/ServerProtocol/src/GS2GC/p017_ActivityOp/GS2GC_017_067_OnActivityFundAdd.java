package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动基金-新增推送
 **/
public class GS2GC_017_067_OnActivityFundAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 基金信息 */
private Common.ActivityFundObj.ActivityFund_Info fundInfo;


public GS2GC_017_067_OnActivityFundAdd() {
	fundInfo = new Common.ActivityFundObj.ActivityFund_Info();
}

public GS2GC_017_067_OnActivityFundAdd(
	 Common.ActivityFundObj.ActivityFund_Info _fundInfo
) {	fundInfo = _fundInfo;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)67; }

/** 基金信息 */
public Common.ActivityFundObj.ActivityFund_Info getFundInfo() { return fundInfo; }
/** 基金信息 */
public void setFundInfo(Common.ActivityFundObj.ActivityFund_Info _fundInfo) { fundInfo = _fundInfo; }


public final int GetBufSize() {
	int _size = 44;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _fundInfoCustLen = _buf.getInt();
	int _fundInfoCurPos = _buf.position();
	fundInfo.ReadUnzipBuf(_buf, _fundInfoCurPos + _fundInfoCustLen);
	_buf.position(_fundInfoCurPos + _fundInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(fundInfo.GetBufSize());
	fundInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)67);
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

