using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟展示信息
/// </summary>
public class Guild_ShowInfo : ALBasicProtocolPack._IALProtocolStructure {
private long guidId;
private long flagId;
private string name;
private string simpleName;
/// <summary>
/// 宣言
/// </summary>
private string declaration;
private long leaderId;
private int level;
private long exp;
/// <summary>
/// 成员总赚速
/// </summary>
private long totalEarnings;
/// <summary>
/// 加入类型
/// </summary>
private Common.GuildEnum.EGuildJoinType joinType;
/// <summary>
/// 加入限制信息
/// </summary>
private List<Common.GuildObj.Guild_JoinLimitInfo> joinLimitInfo;


public Guild_ShowInfo() {
	guidId = (long)0;
	flagId = (long)0;
	name = "";
	simpleName = "";
	declaration = "";
	leaderId = (long)0;
	level = 0;
	exp = (long)0;
	totalEarnings = (long)0;
	joinType = 0;
	joinLimitInfo = new List<Common.GuildObj.Guild_JoinLimitInfo>();
}

public Guild_ShowInfo(
	long _guidId
	, long _flagId
	, string _name
	, string _simpleName
	, string _declaration
	, long _leaderId
	, int _level
	, long _exp
	, long _totalEarnings
	, Common.GuildEnum.EGuildJoinType _joinType
	, List<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfo
) {	guidId = _guidId;
	flagId = _flagId;
	name = _name;
	simpleName = _simpleName;
	declaration = _declaration;
	leaderId = _leaderId;
	level = _level;
	exp = _exp;
	totalEarnings = _totalEarnings;
	joinType = _joinType;
	joinLimitInfo = _joinLimitInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getGuidId() { return guidId; }
public void setGuidId(long _guidId) { guidId = _guidId; }
public long getFlagId() { return flagId; }
public void setFlagId(long _flagId) { flagId = _flagId; }
public string getName() { return name; }
public void setName(string _name) { name = _name; }
public string getSimpleName() { return simpleName; }
public void setSimpleName(string _simpleName) { simpleName = _simpleName; }
/// <summary>
/// 宣言
/// </summary>
public string getDeclaration() { return declaration; }
/// <summary>
/// 宣言
/// </summary>
public void setDeclaration(string _declaration) { declaration = _declaration; }
public long getLeaderId() { return leaderId; }
public void setLeaderId(long _leaderId) { leaderId = _leaderId; }
public int getLevel() { return level; }
public void setLevel(int _level) { level = _level; }
public long getExp() { return exp; }
public void setExp(long _exp) { exp = _exp; }
/// <summary>
/// 成员总赚速
/// </summary>
public long getTotalEarnings() { return totalEarnings; }
/// <summary>
/// 成员总赚速
/// </summary>
public void setTotalEarnings(long _totalEarnings) { totalEarnings = _totalEarnings; }
/// <summary>
/// 加入类型
/// </summary>
public Common.GuildEnum.EGuildJoinType getJoinType() { return joinType; }
/// <summary>
/// 加入类型
/// </summary>
public void setJoinType(Common.GuildEnum.EGuildJoinType _joinType) { joinType = _joinType; }
/// <summary>
/// 加入限制信息
/// </summary>
public List<Common.GuildObj.Guild_JoinLimitInfo> getJoinLimitInfo() { return joinLimitInfo; }
/// <summary>
/// 加入限制信息
/// </summary>
public void addJoinLimitInfo(Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo) { joinLimitInfo.Add(_joinLimitInfo); }


public int GetBufSize() {
	int _size = 48;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);
	_size += 2 + (joinLimitInfo.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);
	_size += 2 + (joinLimitInfo.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	guidId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	flagId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	simpleName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	declaration = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	leaderId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	level = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalEarnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	joinType = (Common.GuildEnum.EGuildJoinType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _joinLimitInfoCount = _buf.getShort();
	for(int _i = 0; _i < _joinLimitInfoCount; _i++) { 
		Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo = new Common.GuildObj.Guild_JoinLimitInfo();
		int __joinLimitInfoCustLen = _buf.getInt();
	int __joinLimitInfoCurPos = _buf.getCurPos();
	_joinLimitInfo.ReadUnzipBuf(_buf, __joinLimitInfoCurPos + __joinLimitInfoCustLen);
	_buf.setPosition(__joinLimitInfoCurPos + __joinLimitInfoCustLen);

		joinLimitInfo.Add(_joinLimitInfo);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(guidId);
	_buf.putLong(flagId);
	_buf.putString(name);
	_buf.putString(simpleName);
	_buf.putString(declaration);
	_buf.putLong(leaderId);
	_buf.putInt(level);
	_buf.putLong(exp);
	_buf.putLong(totalEarnings);
	_buf.putInt((int)joinType);

	_buf.putShort((short)joinLimitInfo.Count);
	for(int _i = 0; _i < joinLimitInfo.Count; _i++) { 
		_buf.putInt(joinLimitInfo[_i].GetBufSize());
	joinLimitInfo[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("guidId").Append(":").Append(guidId.ToString()).Append(", ");
	builder.Append("flagId").Append(":").Append(flagId.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("simpleName").Append(":").Append(simpleName.ToString()).Append(", ");
	builder.Append("declaration").Append(":").Append(declaration.ToString()).Append(", ");
	builder.Append("leaderId").Append(":").Append(leaderId.ToString()).Append(", ");
	builder.Append("level").Append(":").Append(level.ToString()).Append(", ");
	builder.Append("exp").Append(":").Append(exp.ToString()).Append(", ");
	builder.Append("totalEarnings").Append(":").Append(totalEarnings.ToString()).Append(", ");
	builder.Append("joinType").Append(":").Append(joinType.ToString()).Append(", ");
	builder.Append("joinLimitInfo").Append(":").Append(joinLimitInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

