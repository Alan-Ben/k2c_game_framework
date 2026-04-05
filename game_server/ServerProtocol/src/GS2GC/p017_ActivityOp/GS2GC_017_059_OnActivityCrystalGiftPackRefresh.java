package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动钻石礼包刷新推送
 **/
public class GS2GC_017_059_OnActivityCrystalGiftPackRefresh implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 钻石礼包信息 */
private Common.CommonFuncObj.CrystalGiftPack_Info crystalGiftPackInfo;


public GS2GC_017_059_OnActivityCrystalGiftPackRefresh() {
	instanceId = (long)0;
	crystalGiftPackInfo = new Common.CommonFuncObj.CrystalGiftPack_Info();
}

public GS2GC_017_059_OnActivityCrystalGiftPackRefresh(
	 long _instanceId
	, Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo
) {	instanceId = _instanceId;
	crystalGiftPackInfo = _crystalGiftPackInfo;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)59; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 钻石礼包信息 */
public Common.CommonFuncObj.CrystalGiftPack_Info getCrystalGiftPackInfo() { return crystalGiftPackInfo; }
/** 钻石礼包信息 */
public void setCrystalGiftPackInfo(Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo) { crystalGiftPackInfo = _crystalGiftPackInfo; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + crystalGiftPackInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + crystalGiftPackInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _crystalGiftPackInfoCustLen = _buf.getInt();
	int _crystalGiftPackInfoCurPos = _buf.position();
	crystalGiftPackInfo.ReadUnzipBuf(_buf, _crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);
	_buf.position(_crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(crystalGiftPackInfo.GetBufSize());
	crystalGiftPackInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)59);
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

