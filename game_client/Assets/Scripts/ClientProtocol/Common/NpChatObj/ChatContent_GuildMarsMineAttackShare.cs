using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 联盟成员火星矿被攻击分享
/// </summary>
public class ChatContent_GuildMarsMineAttackShare : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 攻击玩家昵称
/// </summary>
private string attackCname;
/// <summary>
/// 攻击玩家的联盟简称
/// </summary>
private string attackGuildSimpleName;
/// <summary>
/// 联盟被攻击玩家昵称
/// </summary>
private string cname;
/// <summary>
/// 火星矿实例ID
/// </summary>
private long mineInstanceId;


public ChatContent_GuildMarsMineAttackShare() {
	attackCname = "";
	attackGuildSimpleName = "";
	cname = "";
	mineInstanceId = (long)0;
}

public ChatContent_GuildMarsMineAttackShare(
	string _attackCname
	, string _attackGuildSimpleName
	, string _cname
	, long _mineInstanceId
) {	attackCname = _attackCname;
	attackGuildSimpleName = _attackGuildSimpleName;
	cname = _cname;
	mineInstanceId = _mineInstanceId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 攻击玩家昵称
/// </summary>
public string getAttackCname() { return attackCname; }
/// <summary>
/// 攻击玩家昵称
/// </summary>
public void setAttackCname(string _attackCname) { attackCname = _attackCname; }
/// <summary>
/// 攻击玩家的联盟简称
/// </summary>
public string getAttackGuildSimpleName() { return attackGuildSimpleName; }
/// <summary>
/// 攻击玩家的联盟简称
/// </summary>
public void setAttackGuildSimpleName(string _attackGuildSimpleName) { attackGuildSimpleName = _attackGuildSimpleName; }
/// <summary>
/// 联盟被攻击玩家昵称
/// </summary>
public string getCname() { return cname; }
/// <summary>
/// 联盟被攻击玩家昵称
/// </summary>
public void setCname(string _cname) { cname = _cname; }
/// <summary>
/// 火星矿实例ID
/// </summary>
public long getMineInstanceId() { return mineInstanceId; }
/// <summary>
/// 火星矿实例ID
/// </summary>
public void setMineInstanceId(long _mineInstanceId) { mineInstanceId = _mineInstanceId; }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackCname);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackGuildSimpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackCname);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(attackGuildSimpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackCname = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackGuildSimpleName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cname = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mineInstanceId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(attackCname);
	_buf.putString(attackGuildSimpleName);
	_buf.putString(cname);
	_buf.putLong(mineInstanceId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("attackCname").Append(":").Append(attackCname.ToString()).Append(", ");
	builder.Append("attackGuildSimpleName").Append(":").Append(attackGuildSimpleName.ToString()).Append(", ");
	builder.Append("cname").Append(":").Append(cname.ToString()).Append(", ");
	builder.Append("mineInstanceId").Append(":").Append(mineInstanceId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

