using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 联盟协作奖励据点击败推送
/// </summary>
public class GS2GC_032_077_OnRewardPointDefeat : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 奖励据点位置
/// </summary>
private Common.GuildCooperateObj.GuildCooperate_RewardPointPos pos;
/// <summary>
/// 解锁据点时的盟主CID
/// </summary>
private long leaderCid;


public GS2GC_032_077_OnRewardPointDefeat() {
	pos = new Common.GuildCooperateObj.GuildCooperate_RewardPointPos();
	leaderCid = (long)0;
}

public GS2GC_032_077_OnRewardPointDefeat(
	Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos
	, long _leaderCid
) {	pos = _pos;
	leaderCid = _leaderCid;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)77; }

/// <summary>
/// 奖励据点位置
/// </summary>
public Common.GuildCooperateObj.GuildCooperate_RewardPointPos getPos() { return pos; }
/// <summary>
/// 奖励据点位置
/// </summary>
public void setPos(Common.GuildCooperateObj.GuildCooperate_RewardPointPos _pos) { pos = _pos; }
/// <summary>
/// 解锁据点时的盟主CID
/// </summary>
public long getLeaderCid() { return leaderCid; }
/// <summary>
/// 解锁据点时的盟主CID
/// </summary>
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leaderCid = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putLong(leaderCid);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)77);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)77);
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
	builder.Append("leaderCid").Append(":").Append(leaderCid.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

