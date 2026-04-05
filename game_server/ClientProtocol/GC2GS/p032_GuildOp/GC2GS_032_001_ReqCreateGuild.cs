using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p032_GuildOp
{

/// <summary>
/// 请求创建联盟
/// </summary>
public class GC2GS_032_001_ReqCreateGuild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 旗帜id
/// </summary>
private long flagId;
/// <summary>
/// 名称
/// </summary>
private string name;
/// <summary>
/// 简称
/// </summary>
private string simpleName;
/// <summary>
/// 宣言
/// </summary>
private string declaration;
/// <summary>
/// 是否允许自由加入
/// </summary>
private bool canFreeJoin;


public GC2GS_032_001_ReqCreateGuild() {
	flagId = (long)0;
	name = "";
	simpleName = "";
	declaration = "";
	canFreeJoin = false;
}

public GC2GS_032_001_ReqCreateGuild(
	long _flagId
	, string _name
	, string _simpleName
	, string _declaration
	, bool _canFreeJoin
) {	flagId = _flagId;
	name = _name;
	simpleName = _simpleName;
	declaration = _declaration;
	canFreeJoin = _canFreeJoin;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 旗帜id
/// </summary>
public long getFlagId() { return flagId; }
/// <summary>
/// 旗帜id
/// </summary>
public void setFlagId(long _flagId) { flagId = _flagId; }
/// <summary>
/// 名称
/// </summary>
public string getName() { return name; }
/// <summary>
/// 名称
/// </summary>
public void setName(string _name) { name = _name; }
/// <summary>
/// 简称
/// </summary>
public string getSimpleName() { return simpleName; }
/// <summary>
/// 简称
/// </summary>
public void setSimpleName(string _simpleName) { simpleName = _simpleName; }
/// <summary>
/// 宣言
/// </summary>
public string getDeclaration() { return declaration; }
/// <summary>
/// 宣言
/// </summary>
public void setDeclaration(string _declaration) { declaration = _declaration; }
/// <summary>
/// 是否允许自由加入
/// </summary>
public bool getCanFreeJoin() { return canFreeJoin; }
/// <summary>
/// 是否允许自由加入
/// </summary>
public void setCanFreeJoin(bool _canFreeJoin) { canFreeJoin = _canFreeJoin; }


public int GetBufSize() {
	int _size = 9;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	flagId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	name = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	simpleName = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	declaration = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	canFreeJoin = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(flagId);
	_buf.putString(name);
	_buf.putString(simpleName);
	_buf.putString(declaration);
	_buf.put(canFreeJoin?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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
	builder.Append("flagId").Append(":").Append(flagId.ToString()).Append(", ");
	builder.Append("name").Append(":").Append(name.ToString()).Append(", ");
	builder.Append("simpleName").Append(":").Append(simpleName.ToString()).Append(", ");
	builder.Append("declaration").Append(":").Append(declaration.ToString()).Append(", ");
	builder.Append("canFreeJoin").Append(":").Append(canFreeJoin.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

