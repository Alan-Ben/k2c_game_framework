using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动钻石礼包刷新推送
/// </summary>
public class GS2GC_017_059_OnActivityCrystalGiftPackRefresh : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 钻石礼包信息
/// </summary>
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

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)59; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 钻石礼包信息
/// </summary>
public Common.CommonFuncObj.CrystalGiftPack_Info getCrystalGiftPackInfo() { return crystalGiftPackInfo; }
/// <summary>
/// 钻石礼包信息
/// </summary>
public void setCrystalGiftPackInfo(Common.CommonFuncObj.CrystalGiftPack_Info _crystalGiftPackInfo) { crystalGiftPackInfo = _crystalGiftPackInfo; }


public int GetBufSize() {
	int _size = 8;
	_size += 4 + crystalGiftPackInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + crystalGiftPackInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _crystalGiftPackInfoCustLen = _buf.getInt();
	int _crystalGiftPackInfoCurPos = _buf.getCurPos();
	crystalGiftPackInfo.ReadUnzipBuf(_buf, _crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);
	_buf.setPosition(_crystalGiftPackInfoCurPos + _crystalGiftPackInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(crystalGiftPackInfo.GetBufSize());
	crystalGiftPackInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)59);
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
	builder.Append("crystalGiftPackInfo").Append(":").Append(crystalGiftPackInfo == null ? "null" : crystalGiftPackInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

