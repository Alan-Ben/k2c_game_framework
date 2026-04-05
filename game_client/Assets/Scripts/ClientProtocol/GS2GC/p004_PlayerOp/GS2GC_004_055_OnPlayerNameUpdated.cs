using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_055_OnPlayerNameUpdated : ALBasicProtocolPack._IALProtocolStructure {
private string newName;


public GS2GC_004_055_OnPlayerNameUpdated() {
	newName = "";
}

public GS2GC_004_055_OnPlayerNameUpdated(
	string _newName
) {	newName = _newName;
}

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)55; }

public string getNewName() { return newName; }
public void setNewName(string _newName) { newName = _newName; }


public int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newName);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(newName);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	newName = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putString(newName);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)55);
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
	builder.Append("newName").Append(":").Append(newName.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

