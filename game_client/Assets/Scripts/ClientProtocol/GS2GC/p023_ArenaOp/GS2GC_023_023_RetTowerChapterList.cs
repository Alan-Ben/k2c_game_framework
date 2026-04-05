using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_023_RetTowerChapterList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 对手列表
/// </summary>
private List<Common.TowerObj.Tower_OpponentInfo> opponentList;


public GS2GC_023_023_RetTowerChapterList() {
	opponentList = new List<Common.TowerObj.Tower_OpponentInfo>();
}

public GS2GC_023_023_RetTowerChapterList(
	List<Common.TowerObj.Tower_OpponentInfo> _opponentList
) {	opponentList = _opponentList;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)23; }

/// <summary>
/// 对手列表
/// </summary>
public List<Common.TowerObj.Tower_OpponentInfo> getOpponentList() { return opponentList; }
/// <summary>
/// 对手列表
/// </summary>
public void addOpponentList(Common.TowerObj.Tower_OpponentInfo _opponentList) { opponentList.Add(_opponentList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (opponentList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (opponentList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _opponentListCount = _buf.getShort();
	for(int _i = 0; _i < _opponentListCount; _i++) { 
		Common.TowerObj.Tower_OpponentInfo _opponentList = new Common.TowerObj.Tower_OpponentInfo();
		int __opponentListCustLen = _buf.getInt();
	int __opponentListCurPos = _buf.getCurPos();
	_opponentList.ReadUnzipBuf(_buf, __opponentListCurPos + __opponentListCustLen);
	_buf.setPosition(__opponentListCurPos + __opponentListCustLen);

		opponentList.Add(_opponentList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)opponentList.Count);
	for(int _i = 0; _i < opponentList.Count; _i++) { 
		_buf.putInt(opponentList[_i].GetBufSize());
	opponentList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)23);
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
	builder.Append("opponentList").Append(":").Append(opponentList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

