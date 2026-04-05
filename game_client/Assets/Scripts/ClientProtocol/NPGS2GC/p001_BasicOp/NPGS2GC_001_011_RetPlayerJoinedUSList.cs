using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPGS2GC.p001_BasicOp
{

public class NPGS2GC_001_011_RetPlayerJoinedUSList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 最后一次登录的US服务器逻辑id
/// </summary>
private int lastJoinUSLogicId;
/// <summary>
/// 玩家登录过的服务及服务器上的角色信息
/// </summary>
private List<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> joinedUSList;
/// <summary>
/// 是否账号被封禁
/// </summary>
private bool isFreeze;
/// <summary>
/// 封禁结束时间
/// </summary>
private long freezeTimeMs;


public NPGS2GC_001_011_RetPlayerJoinedUSList() {
	lastJoinUSLogicId = 0;
	joinedUSList = new List<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo>();
	isFreeze = false;
	freezeTimeMs = (long)0;
}

public NPGS2GC_001_011_RetPlayerJoinedUSList(
	int _lastJoinUSLogicId
	, List<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> _joinedUSList
	, bool _isFreeze
	, long _freezeTimeMs
) {	lastJoinUSLogicId = _lastJoinUSLogicId;
	joinedUSList = _joinedUSList;
	isFreeze = _isFreeze;
	freezeTimeMs = _freezeTimeMs;
}

public byte getMainOrder() { return (byte)1; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 最后一次登录的US服务器逻辑id
/// </summary>
public int getLastJoinUSLogicId() { return lastJoinUSLogicId; }
/// <summary>
/// 最后一次登录的US服务器逻辑id
/// </summary>
public void setLastJoinUSLogicId(int _lastJoinUSLogicId) { lastJoinUSLogicId = _lastJoinUSLogicId; }
/// <summary>
/// 玩家登录过的服务及服务器上的角色信息
/// </summary>
public List<Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo> getJoinedUSList() { return joinedUSList; }
/// <summary>
/// 玩家登录过的服务及服务器上的角色信息
/// </summary>
public void addJoinedUSList(Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinedUSList) { joinedUSList.Add(_joinedUSList); }
/// <summary>
/// 是否账号被封禁
/// </summary>
public bool getIsFreeze() { return isFreeze; }
/// <summary>
/// 是否账号被封禁
/// </summary>
public void setIsFreeze(bool _isFreeze) { isFreeze = _isFreeze; }
/// <summary>
/// 封禁结束时间
/// </summary>
public long getFreezeTimeMs() { return freezeTimeMs; }
/// <summary>
/// 封禁结束时间
/// </summary>
public void setFreezeTimeMs(long _freezeTimeMs) { freezeTimeMs = _freezeTimeMs; }


public int GetBufSize() {
	int _size = 13;
	_size += 2;
for(int _i = 0; _i < joinedUSList.Count; _i++) {
	_size += 4 + joinedUSList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;
	_size += 2;
for(int _i = 0; _i < joinedUSList.Count; _i++) {
	_size += 4 + joinedUSList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastJoinUSLogicId = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _joinedUSListCount = _buf.getShort();
	for(int _i = 0; _i < _joinedUSListCount; _i++) { 
		Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinedUSList = new Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo();
		int __joinedUSListCustLen = _buf.getInt();
	int __joinedUSListCurPos = _buf.getCurPos();
	_joinedUSList.ReadUnzipBuf(_buf, __joinedUSListCurPos + __joinedUSListCustLen);
	_buf.setPosition(__joinedUSListCurPos + __joinedUSListCustLen);

		joinedUSList.Add(_joinedUSList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFreeze = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	freezeTimeMs = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lastJoinUSLogicId);
	_buf.putShort((short)joinedUSList.Count);
	for(int _i = 0; _i < joinedUSList.Count; _i++) { 
		_buf.putInt(joinedUSList[_i].GetBufSize());
	joinedUSList[_i].PutUnzipBuf(_buf);
	}
	_buf.put(isFreeze?(byte)1:(byte)0);
	_buf.putLong(freezeTimeMs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)11);
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
	builder.Append("lastJoinUSLogicId").Append(":").Append(lastJoinUSLogicId.ToString()).Append(", ");
	builder.Append("joinedUSList").Append(":").Append(joinedUSList.ToString()).Append(", ");
	builder.Append("isFreeze").Append(":").Append(isFreeze.ToString()).Append(", ");
	builder.Append("freezeTimeMs").Append(":").Append(freezeTimeMs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

