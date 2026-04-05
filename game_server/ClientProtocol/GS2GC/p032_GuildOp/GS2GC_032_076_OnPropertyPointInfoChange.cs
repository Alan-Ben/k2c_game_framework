using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 属性据点信息变更推送
/// </summary>
public class GS2GC_032_076_OnPropertyPointInfoChange : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奖励据点位置
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/// <summary>
/// 属性据点信息
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo propertyPointInfo;


public GS2GC_032_076_OnPropertyPointInfoChange() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	propertyPointInfo = new Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo();
}

public GS2GC_032_076_OnPropertyPointInfoChange(
	Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointInfo
) {	pos = _pos;
	propertyPointInfo = _propertyPointInfo;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)76; }

/// <summary>
/// 奖励据点位置
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/// <summary>
/// 奖励据点位置
/// </summary>
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/// <summary>
/// 属性据点信息
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo getPropertyPointInfo() { return propertyPointInfo; }
/// <summary>
/// 属性据点信息
/// </summary>
public void setPropertyPointInfo(Common.GuildCooperateObj.GuildCooperate_PropertyPointInfo _propertyPointInfo) { propertyPointInfo = _propertyPointInfo; }


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
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _propertyPointInfoCustLen = _buf.getInt();
	int _propertyPointInfoCurPos = _buf.getCurPos();
	propertyPointInfo.ReadUnzipBuf(_buf, _propertyPointInfoCurPos + _propertyPointInfoCustLen);
	_buf.setPosition(_propertyPointInfoCurPos + _propertyPointInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putInt(propertyPointInfo.GetBufSize());
	propertyPointInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)76);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)76);
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
	builder.Append("pos").Append(":").Append(pos == null ? "null" : pos.ToString()).Append(", ");
	builder.Append("propertyPointInfo").Append(":").Append(propertyPointInfo == null ? "null" : propertyPointInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

