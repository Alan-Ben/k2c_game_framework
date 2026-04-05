using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_001_RetPlayerInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private string cname;
private List<NPCommon.NPCommon_PlayerParam> playerParams;


public GS2GC_002_001_RetPlayerInfo() {
	cid = (long)0;
	cname = "";
	playerParams = new List<NPCommon.NPCommon_PlayerParam>();
}

public GS2GC_002_001_RetPlayerInfo(
	long _cid
	, string _cname
	, List<NPCommon.NPCommon_PlayerParam> _playerParams
) {	cid = _cid;
	cname = _cname;
	playerParams = _playerParams;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)1; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public string getCname() { return cname; }
public void setCname(string _cname) { cname = _cname; }
public List<NPCommon.NPCommon_PlayerParam> getPlayerParams() { return playerParams; }
public void addPlayerParams(NPCommon.NPCommon_PlayerParam _playerParams) { playerParams.Add(_playerParams); }


public int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 2 + (playerParams.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 2 + (playerParams.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cname = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _playerParamsCount = _buf.getShort();
	for(int _i = 0; _i < _playerParamsCount; _i++) { 
		NPCommon.NPCommon_PlayerParam _playerParams = new NPCommon.NPCommon_PlayerParam();
		int __playerParamsCustLen = _buf.getInt();
	int __playerParamsCurPos = _buf.getCurPos();
	_playerParams.ReadUnzipBuf(_buf, __playerParamsCurPos + __playerParamsCustLen);
	_buf.setPosition(__playerParamsCurPos + __playerParamsCustLen);

		playerParams.Add(_playerParams);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putString(cname);
	_buf.putShort((short)playerParams.Count);
	for(int _i = 0; _i < playerParams.Count; _i++) { 
		_buf.putInt(playerParams[_i].GetBufSize());
	playerParams[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("cname").Append(":").Append(cname.ToString()).Append(", ");
	builder.Append("playerParams").Append(":").Append(playerParams.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

