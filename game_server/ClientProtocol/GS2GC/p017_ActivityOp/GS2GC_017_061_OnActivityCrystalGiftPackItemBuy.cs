using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动钻石礼包购买记录推送
/// </summary>
public class GS2GC_017_061_OnActivityCrystalGiftPackItemBuy : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 组id
/// </summary>
private long groupId;
/// <summary>
/// 购买记录
/// </summary>
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

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)61; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 购买记录
/// </summary>
public Common.CommonFuncObj.CrystalGiftPack_BuyRecord getBuyRecord() { return buyRecord; }
/// <summary>
/// 购买记录
/// </summary>
public void setBuyRecord(Common.CommonFuncObj.CrystalGiftPack_BuyRecord _buyRecord) { buyRecord = _buyRecord; }


public int GetBufSize() {
	int _size = 36;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _buyRecordCustLen = _buf.getInt();
	int _buyRecordCurPos = _buf.getCurPos();
	buyRecord.ReadUnzipBuf(_buf, _buyRecordCurPos + _buyRecordCustLen);
	_buf.setPosition(_buyRecordCurPos + _buyRecordCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(groupId);
	_buf.putInt(buyRecord.GetBufSize());
	buyRecord.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)61);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("buyRecord").Append(":").Append(buyRecord == null ? "null" : buyRecord.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

