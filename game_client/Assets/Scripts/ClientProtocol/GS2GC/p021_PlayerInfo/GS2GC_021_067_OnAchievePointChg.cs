using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

/// <summary>
/// 成就点变更推送
/// </summary>
public class GS2GC_021_067_OnAchievePointChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成就点信息
/// </summary>
private Common.AchieveObj.Achieve_AchievePointInfo achievePointInfo;


public GS2GC_021_067_OnAchievePointChg() {
	achievePointInfo = new Common.AchieveObj.Achieve_AchievePointInfo();
}

public GS2GC_021_067_OnAchievePointChg(
	Common.AchieveObj.Achieve_AchievePointInfo _achievePointInfo
) {	achievePointInfo = _achievePointInfo;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)67; }

/// <summary>
/// 成就点信息
/// </summary>
public Common.AchieveObj.Achieve_AchievePointInfo getAchievePointInfo() { return achievePointInfo; }
/// <summary>
/// 成就点信息
/// </summary>
public void setAchievePointInfo(Common.AchieveObj.Achieve_AchievePointInfo _achievePointInfo) { achievePointInfo = _achievePointInfo; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _achievePointInfoCustLen = _buf.getInt();
	int _achievePointInfoCurPos = _buf.getCurPos();
	achievePointInfo.ReadUnzipBuf(_buf, _achievePointInfoCurPos + _achievePointInfoCustLen);
	_buf.setPosition(_achievePointInfoCurPos + _achievePointInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(achievePointInfo.GetBufSize());
	achievePointInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)67);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
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
	builder.Append("achievePointInfo").Append(":").Append(achievePointInfo == null ? "null" : achievePointInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

