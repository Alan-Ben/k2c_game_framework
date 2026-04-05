using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_030_RetCuteActorInit : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.NpPlayerInfoObj.PlayerInfo_CuteActor> cuteActorList;


public GS2GC_002_030_RetCuteActorInit() {
	cuteActorList = new List<Common.NpPlayerInfoObj.PlayerInfo_CuteActor>();
}

public GS2GC_002_030_RetCuteActorInit(
	List<Common.NpPlayerInfoObj.PlayerInfo_CuteActor> _cuteActorList
) {	cuteActorList = _cuteActorList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)30; }

public List<Common.NpPlayerInfoObj.PlayerInfo_CuteActor> getCuteActorList() { return cuteActorList; }
public void addCuteActorList(Common.NpPlayerInfoObj.PlayerInfo_CuteActor _cuteActorList) { cuteActorList.Add(_cuteActorList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (cuteActorList.Count * 17);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cuteActorList.Count * 17);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cuteActorListCount = _buf.getShort();
	for(int _i = 0; _i < _cuteActorListCount; _i++) { 
		Common.NpPlayerInfoObj.PlayerInfo_CuteActor _cuteActorList = new Common.NpPlayerInfoObj.PlayerInfo_CuteActor();
		int __cuteActorListCustLen = _buf.getInt();
	int __cuteActorListCurPos = _buf.getCurPos();
	_cuteActorList.ReadUnzipBuf(_buf, __cuteActorListCurPos + __cuteActorListCustLen);
	_buf.setPosition(__cuteActorListCurPos + __cuteActorListCustLen);

		cuteActorList.Add(_cuteActorList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)cuteActorList.Count);
	for(int _i = 0; _i < cuteActorList.Count; _i++) { 
		_buf.putInt(cuteActorList[_i].GetBufSize());
	cuteActorList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)30);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)30);
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
	builder.Append("cuteActorList").Append(":").Append(cuteActorList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

