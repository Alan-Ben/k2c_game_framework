using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ArenaObj
{

/// <summary>
/// 竞技场buff数据
/// </summary>
public class Arena_BuffList : ALBasicProtocolPack._IALProtocolStructure {
private List<Common.ArenaObj.Arena_SingleBuffInfo> buffList;


public Arena_BuffList() {
	buffList = new List<Common.ArenaObj.Arena_SingleBuffInfo>();
}

public Arena_BuffList(
	List<Common.ArenaObj.Arena_SingleBuffInfo> _buffList
) {	buffList = _buffList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public List<Common.ArenaObj.Arena_SingleBuffInfo> getBuffList() { return buffList; }
public void addBuffList(Common.ArenaObj.Arena_SingleBuffInfo _buffList) { buffList.Add(_buffList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (buffList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (buffList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buffListCount = _buf.getShort();
	for(int _i = 0; _i < _buffListCount; _i++) { 
		Common.ArenaObj.Arena_SingleBuffInfo _buffList = new Common.ArenaObj.Arena_SingleBuffInfo();
		int __buffListCustLen = _buf.getInt();
	int __buffListCurPos = _buf.getCurPos();
	_buffList.ReadUnzipBuf(_buf, __buffListCurPos + __buffListCustLen);
	_buf.setPosition(__buffListCurPos + __buffListCustLen);

		buffList.Add(_buffList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)buffList.Count);
	for(int _i = 0; _i < buffList.Count; _i++) { 
		_buf.putInt(buffList[_i].GetBufSize());
	buffList[_i].PutUnzipBuf(_buf);
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
	builder.Append("buffList").Append(":").Append(buffList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

