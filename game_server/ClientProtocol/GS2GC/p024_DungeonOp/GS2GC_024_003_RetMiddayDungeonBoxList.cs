using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p024_DungeonOp
{

public class GS2GC_024_003_RetMiddayDungeonBoxList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱列表
/// </summary>
private List<Common.DungeonObj.MiddayDungeon_BoxInfo> boxList;


public GS2GC_024_003_RetMiddayDungeonBoxList() {
	boxList = new List<Common.DungeonObj.MiddayDungeon_BoxInfo>();
}

public GS2GC_024_003_RetMiddayDungeonBoxList(
	List<Common.DungeonObj.MiddayDungeon_BoxInfo> _boxList
) {	boxList = _boxList;
}

public byte getMainOrder() { return (byte)24; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 宝箱列表
/// </summary>
public List<Common.DungeonObj.MiddayDungeon_BoxInfo> getBoxList() { return boxList; }
/// <summary>
/// 宝箱列表
/// </summary>
public void addBoxList(Common.DungeonObj.MiddayDungeon_BoxInfo _boxList) { boxList.Add(_boxList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < boxList.Count; _i++) {
	_size += 4 + boxList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < boxList.Count; _i++) {
	_size += 4 + boxList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _boxListCount = _buf.getShort();
	for(int _i = 0; _i < _boxListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_BoxInfo _boxList = new Common.DungeonObj.MiddayDungeon_BoxInfo();
		int __boxListCustLen = _buf.getInt();
	int __boxListCurPos = _buf.getCurPos();
	_boxList.ReadUnzipBuf(_buf, __boxListCurPos + __boxListCustLen);
	_buf.setPosition(__boxListCurPos + __boxListCustLen);

		boxList.Add(_boxList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)boxList.Count);
	for(int _i = 0; _i < boxList.Count; _i++) { 
		_buf.putInt(boxList[_i].GetBufSize());
	boxList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)3);
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
	builder.Append("boxList").Append(":").Append(boxList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

