using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 设置推荐奖励据点
/// </summary>
public class GC2GS_032_042_ReqSetRecommendRewardPoint : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 推荐奖励据点位置
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;


public GC2GS_032_042_ReqSetRecommendRewardPoint() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
}

public GC2GS_032_042_ReqSetRecommendRewardPoint(
	Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
) {	pos = _pos;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)42; }

/// <summary>
/// 推荐奖励据点位置
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/// <summary>
/// 推荐奖励据点位置
/// </summary>
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)42);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)42);
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
	builder.Append("}");
	return builder.ToString();
}

}

}

