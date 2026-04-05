using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟派遣信息
/// </summary>
public class Guild_DispatchData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 相性派遣列表
/// </summary>
private List<Common.GuildObj.Guild_AttrDispatchInfo> dispatchList;


public Guild_DispatchData() {
	dispatchList = new List<Common.GuildObj.Guild_AttrDispatchInfo>();
}

public Guild_DispatchData(
	List<Common.GuildObj.Guild_AttrDispatchInfo> _dispatchList
) {	dispatchList = _dispatchList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 相性派遣列表
/// </summary>
public List<Common.GuildObj.Guild_AttrDispatchInfo> getDispatchList() { return dispatchList; }
/// <summary>
/// 相性派遣列表
/// </summary>
public void addDispatchList(Common.GuildObj.Guild_AttrDispatchInfo _dispatchList) { dispatchList.Add(_dispatchList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < dispatchList.Count; _i++) {
	_size += 4 + dispatchList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < dispatchList.Count; _i++) {
	_size += 4 + dispatchList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dispatchListCount = _buf.getShort();
	for(int _i = 0; _i < _dispatchListCount; _i++) { 
		Common.GuildObj.Guild_AttrDispatchInfo _dispatchList = new Common.GuildObj.Guild_AttrDispatchInfo();
		int __dispatchListCustLen = _buf.getInt();
	int __dispatchListCurPos = _buf.getCurPos();
	_dispatchList.ReadUnzipBuf(_buf, __dispatchListCurPos + __dispatchListCustLen);
	_buf.setPosition(__dispatchListCurPos + __dispatchListCustLen);

		dispatchList.Add(_dispatchList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)dispatchList.Count);
	for(int _i = 0; _i < dispatchList.Count; _i++) { 
		_buf.putInt(dispatchList[_i].GetBufSize());
	dispatchList[_i].PutUnzipBuf(_buf);
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
	builder.Append("dispatchList").Append(":").Append(dispatchList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

