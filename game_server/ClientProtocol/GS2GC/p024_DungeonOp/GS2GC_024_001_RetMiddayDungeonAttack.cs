using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_001_RetMiddayDungeonAttack : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 副本结算信息
/// </summary>
private Common.DungeonObj.MiddayDungeon_SettleInfo settleInfo;


public GS2GC_024_001_RetMiddayDungeonAttack() {
	settleInfo = new Common.DungeonObj.MiddayDungeon_SettleInfo();
}

public GS2GC_024_001_RetMiddayDungeonAttack(
	Common.DungeonObj.MiddayDungeon_SettleInfo _settleInfo
) {	settleInfo = _settleInfo;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 副本结算信息
/// </summary>
public Common.DungeonObj.MiddayDungeon_SettleInfo getSettleInfo() { return settleInfo; }
/// <summary>
/// 副本结算信息
/// </summary>
public void setSettleInfo(Common.DungeonObj.MiddayDungeon_SettleInfo _settleInfo) { settleInfo = _settleInfo; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _settleInfoCustLen = _buf.getInt();
	int _settleInfoCurPos = _buf.getCurPos();
	settleInfo.ReadUnzipBuf(_buf, _settleInfoCurPos + _settleInfoCustLen);
	_buf.setPosition(_settleInfoCurPos + _settleInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(settleInfo.GetBufSize());
	settleInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)1);
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
	builder.Append("settleInfo").Append(":").Append(settleInfo == null ? "null" : settleInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

