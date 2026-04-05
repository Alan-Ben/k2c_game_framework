using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_StringList : ALBasicProtocolPack._IALProtocolStructure {
private List<string> valueList;


public Common_StringList() {
	valueList = new List<string>();
}

public Common_StringList(
	List<string> _valueList
) {	valueList = _valueList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public List<string> getValueList() { return valueList; }
public void addValueList(string _valueList) { valueList.Add(_valueList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < valueList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(valueList[_i]);
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < valueList.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(valueList[_i]);
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _valueListCount = _buf.getShort();
	for(int _i = 0; _i < _valueListCount; _i++) { 
		string _valueList = "";
		_valueList = _buf.getString();
		valueList.Add(_valueList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)valueList.Count);
	for(int _i = 0; _i < valueList.Count; _i++) { 
		_buf.putString(valueList[_i]);
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
	builder.Append("valueList").Append(":").Append(valueList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

