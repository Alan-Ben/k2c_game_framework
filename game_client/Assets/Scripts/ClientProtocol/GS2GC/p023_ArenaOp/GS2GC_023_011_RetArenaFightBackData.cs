using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_011_RetArenaFightBackData : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.ArenaObj.Arena_FightBackInfo> fightBackList;


public GS2GC_023_011_RetArenaFightBackData() {
	fightBackList = new List<Common.ArenaObj.Arena_FightBackInfo>();
}

public GS2GC_023_011_RetArenaFightBackData(
	List<Common.ArenaObj.Arena_FightBackInfo> _fightBackList
) {	fightBackList = _fightBackList;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)11; }

public List<Common.ArenaObj.Arena_FightBackInfo> getFightBackList() { return fightBackList; }
public void addFightBackList(Common.ArenaObj.Arena_FightBackInfo _fightBackList) { fightBackList.Add(_fightBackList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (fightBackList.Count * 41);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (fightBackList.Count * 41);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _fightBackListCount = _buf.getShort();
	for(int _i = 0; _i < _fightBackListCount; _i++) { 
		Common.ArenaObj.Arena_FightBackInfo _fightBackList = new Common.ArenaObj.Arena_FightBackInfo();
		int __fightBackListCustLen = _buf.getInt();
	int __fightBackListCurPos = _buf.getCurPos();
	_fightBackList.ReadUnzipBuf(_buf, __fightBackListCurPos + __fightBackListCustLen);
	_buf.setPosition(__fightBackListCurPos + __fightBackListCustLen);

		fightBackList.Add(_fightBackList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)fightBackList.Count);
	for(int _i = 0; _i < fightBackList.Count; _i++) { 
		_buf.putInt(fightBackList[_i].GetBufSize());
	fightBackList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
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
	builder.Append("fightBackList").Append(":").Append(fightBackList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

