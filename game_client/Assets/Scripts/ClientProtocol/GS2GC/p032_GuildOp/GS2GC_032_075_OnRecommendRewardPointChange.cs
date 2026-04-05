using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 推荐奖励据点变更推送
/// </summary>
public class GS2GC_032_075_OnRecommendRewardPointChange : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 推荐奖励据点
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos recommendPos;


public GS2GC_032_075_OnRecommendRewardPointChange() {
	recommendPos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
}

public GS2GC_032_075_OnRecommendRewardPointChange(
	Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos
) {	recommendPos = _recommendPos;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)75; }

/// <summary>
/// 推荐奖励据点
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getRecommendPos() { return recommendPos; }
/// <summary>
/// 推荐奖励据点
/// </summary>
public void setRecommendPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _recommendPos) { recommendPos = _recommendPos; }


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
	int _recommendPosCustLen = _buf.getInt();
	int _recommendPosCurPos = _buf.getCurPos();
	recommendPos.ReadUnzipBuf(_buf, _recommendPosCurPos + _recommendPosCustLen);
	_buf.setPosition(_recommendPosCurPos + _recommendPosCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(recommendPos.GetBufSize());
	recommendPos.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)75);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)75);
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
	builder.Append("recommendPos").Append(":").Append(recommendPos == null ? "null" : recommendPos.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

