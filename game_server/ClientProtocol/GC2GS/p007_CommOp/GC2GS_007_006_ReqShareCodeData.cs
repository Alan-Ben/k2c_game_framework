using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 请求分享码信息
/// </summary>
public class GC2GS_007_006_ReqShareCodeData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 类型
/// </summary>
private CommonEnum.EShareCodeType type;
/// <summary>
/// 分享码
/// </summary>
private string shareCode;


public GC2GS_007_006_ReqShareCodeData() {
	type = 0;
	shareCode = "";
}

public GC2GS_007_006_ReqShareCodeData(
	CommonEnum.EShareCodeType _type
	, string _shareCode
) {	type = _type;
	shareCode = _shareCode;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 类型
/// </summary>
public CommonEnum.EShareCodeType getType() { return type; }
/// <summary>
/// 类型
/// </summary>
public void setType(CommonEnum.EShareCodeType _type) { type = _type; }
/// <summary>
/// 分享码
/// </summary>
public string getShareCode() { return shareCode; }
/// <summary>
/// 分享码
/// </summary>
public void setShareCode(string _shareCode) { shareCode = _shareCode; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(shareCode);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(shareCode);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (CommonEnum.EShareCodeType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	shareCode = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putString(shareCode);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)6);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("shareCode").Append(":").Append(shareCode.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

