using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动基金-新增推送
/// </summary>
public class GS2GC_017_067_OnActivityFundAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基金信息
/// </summary>
private Common.ActivityFundObj.ActivityFund_Info fundInfo;


public GS2GC_017_067_OnActivityFundAdd() {
	fundInfo = new Common.ActivityFundObj.ActivityFund_Info();
}

public GS2GC_017_067_OnActivityFundAdd(
	Common.ActivityFundObj.ActivityFund_Info _fundInfo
) {	fundInfo = _fundInfo;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)67; }

/// <summary>
/// 基金信息
/// </summary>
public Common.ActivityFundObj.ActivityFund_Info getFundInfo() { return fundInfo; }
/// <summary>
/// 基金信息
/// </summary>
public void setFundInfo(Common.ActivityFundObj.ActivityFund_Info _fundInfo) { fundInfo = _fundInfo; }


public int GetBufSize() {
	int _size = 44;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _fundInfoCustLen = _buf.getInt();
	int _fundInfoCurPos = _buf.getCurPos();
	fundInfo.ReadUnzipBuf(_buf, _fundInfoCurPos + _fundInfoCustLen);
	_buf.setPosition(_fundInfoCurPos + _fundInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(fundInfo.GetBufSize());
	fundInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)67);
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
	builder.Append("fundInfo").Append(":").Append(fundInfo == null ? "null" : fundInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

