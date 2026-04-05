using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

public class GS2GC_017_006_RetActivityHotRefInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动热更配表信息
/// </summary>
private Common.ActivityObj.Activity_HotRefInfo hotRefInfo;


public GS2GC_017_006_RetActivityHotRefInfo() {
	hotRefInfo = new Common.ActivityObj.Activity_HotRefInfo();
}

public GS2GC_017_006_RetActivityHotRefInfo(
	Common.ActivityObj.Activity_HotRefInfo _hotRefInfo
) {	hotRefInfo = _hotRefInfo;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 活动热更配表信息
/// </summary>
public Common.ActivityObj.Activity_HotRefInfo getHotRefInfo() { return hotRefInfo; }
/// <summary>
/// 活动热更配表信息
/// </summary>
public void setHotRefInfo(Common.ActivityObj.Activity_HotRefInfo _hotRefInfo) { hotRefInfo = _hotRefInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + hotRefInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + hotRefInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _hotRefInfoCustLen = _buf.getInt();
	int _hotRefInfoCurPos = _buf.getCurPos();
	hotRefInfo.ReadUnzipBuf(_buf, _hotRefInfoCurPos + _hotRefInfoCustLen);
	_buf.setPosition(_hotRefInfoCurPos + _hotRefInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(hotRefInfo.GetBufSize());
	hotRefInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
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
	builder.Append("hotRefInfo").Append(":").Append(hotRefInfo == null ? "null" : hotRefInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

