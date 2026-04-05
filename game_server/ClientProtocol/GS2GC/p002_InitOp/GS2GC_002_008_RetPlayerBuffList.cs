using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_008_RetPlayerBuffList : ALBasicProtocolPack._IALProtocolStructure {
private List<NPCommon.NPCommon_PlayerBuffInfo> playerBuffList;


public GS2GC_002_008_RetPlayerBuffList() {
	playerBuffList = new List<NPCommon.NPCommon_PlayerBuffInfo>();
}

public GS2GC_002_008_RetPlayerBuffList(
	List<NPCommon.NPCommon_PlayerBuffInfo> _playerBuffList
) {	playerBuffList = _playerBuffList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)8; }

public List<NPCommon.NPCommon_PlayerBuffInfo> getPlayerBuffList() { return playerBuffList; }
public void addPlayerBuffList(NPCommon.NPCommon_PlayerBuffInfo _playerBuffList) { playerBuffList.Add(_playerBuffList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (playerBuffList.Count * 32);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (playerBuffList.Count * 32);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _playerBuffListCount = _buf.getShort();
	for(int _i = 0; _i < _playerBuffListCount; _i++) { 
		NPCommon.NPCommon_PlayerBuffInfo _playerBuffList = new NPCommon.NPCommon_PlayerBuffInfo();
		int __playerBuffListCustLen = _buf.getInt();
	int __playerBuffListCurPos = _buf.getCurPos();
	_playerBuffList.ReadUnzipBuf(_buf, __playerBuffListCurPos + __playerBuffListCustLen);
	_buf.setPosition(__playerBuffListCurPos + __playerBuffListCustLen);

		playerBuffList.Add(_playerBuffList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)playerBuffList.Count);
	for(int _i = 0; _i < playerBuffList.Count; _i++) { 
		_buf.putInt(playerBuffList[_i].GetBufSize());
	playerBuffList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)8);
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
	builder.Append("playerBuffList").Append(":").Append(playerBuffList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

