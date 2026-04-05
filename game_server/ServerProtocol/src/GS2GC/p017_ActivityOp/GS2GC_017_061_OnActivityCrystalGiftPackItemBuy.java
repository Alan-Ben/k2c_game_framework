package GS2GC.p017_ActivityOp;

import java.nio.ByteBuffer;
/*********
 * 活动钻石礼包购买记录推送
 **/
public class GS2GC_017_061_OnActivityCrystalGiftPackItemBuy implements ALBasicProtocolPack._IALProtocolStructure {
/** 活动实例id */
private long instanceId;
/** 组id */
private long groupId;
/** 购买记录 */
private Common.CommonFuncObj.CrystalGiftPack_BuyRecord buyRecord;


public GS2GC_017_061_OnActivityCrystalGiftPackItemBuy() {
	instanceId = (long)0;
	groupId = (long)0;
	buyRecord = new Common.CommonFuncObj.CrystalGiftPack_BuyRecord();
}

public GS2GC_017_061_OnActivityCrystalGiftPackItemBuy(
	 long _instanceId
	, long _groupId
	, Common.CommonFuncObj.CrystalGiftPack_BuyRecord _buyRecord
) {	instanceId = _instanceId;
	groupId = _groupId;
	buyRecord = _buyRecord;
}

public final byte getMainOrder() { return (byte)17; }

public final byte getSubOrder() { return (byte)61; }

/** 活动实例id */
public long getInstanceId() { return instanceId; }
/** 活动实例id */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 组id */
public long getGroupId() { return groupId; }
/** 组id */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 购买记录 */
public Common.CommonFuncObj.CrystalGiftPack_BuyRecord getBuyRecord() { return buyRecord; }
/** 购买记录 */
public void setBuyRecord(Common.CommonFuncObj.CrystalGiftPack_BuyRecord _buyRecord) { buyRecord = _buyRecord; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _buyRecordCustLen = _buf.getInt();
	int _buyRecordCurPos = _buf.position();
	buyRecord.ReadUnzipBuf(_buf, _buyRecordCurPos + _buyRecordCustLen);
	_buf.position(_buyRecordCurPos + _buyRecordCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(groupId);
	_buf.putInt(buyRecord.GetBufSize());
	buyRecord.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)61);
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

