using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

/// <summary>
/// 今天点赞过玩家信息变更
/// </summary>
public class GS2GC_004_073_OnTodayLikeCidInfoChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.Common_TodayLikeCidInfo todayLikeCidInfo;


public GS2GC_004_073_OnTodayLikeCidInfoChg() {
	todayLikeCidInfo = new Common.Common_TodayLikeCidInfo();
}

public GS2GC_004_073_OnTodayLikeCidInfoChg(
	Common.Common_TodayLikeCidInfo _todayLikeCidInfo
) {	todayLikeCidInfo = _todayLikeCidInfo;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)73; }

public Common.Common_TodayLikeCidInfo getTodayLikeCidInfo() { return todayLikeCidInfo; }
public void setTodayLikeCidInfo(Common.Common_TodayLikeCidInfo _todayLikeCidInfo) { todayLikeCidInfo = _todayLikeCidInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + todayLikeCidInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + todayLikeCidInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _todayLikeCidInfoCustLen = _buf.getInt();
	int _todayLikeCidInfoCurPos = _buf.getCurPos();
	todayLikeCidInfo.ReadUnzipBuf(_buf, _todayLikeCidInfoCurPos + _todayLikeCidInfoCustLen);
	_buf.setPosition(_todayLikeCidInfoCurPos + _todayLikeCidInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(todayLikeCidInfo.GetBufSize());
	todayLikeCidInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)73);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)73);
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
	builder.Append("todayLikeCidInfo").Append(":").Append(todayLikeCidInfo == null ? "null" : todayLikeCidInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

