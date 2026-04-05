using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p041_MarsExploreOp
{

public class GS2GC_041_017_RetGuildMineShareList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 分享矿列表
/// </summary>
private List<Common.GuildObj.Guild_MineShareInfo> mineShareList;


public GS2GC_041_017_RetGuildMineShareList() {
	mineShareList = new List<Common.GuildObj.Guild_MineShareInfo>();
}

public GS2GC_041_017_RetGuildMineShareList(
	List<Common.GuildObj.Guild_MineShareInfo> _mineShareList
) {	mineShareList = _mineShareList;
}

public byte getMainOrder() { return (byte)41; }

public byte getSubOrder() { return (byte)17; }

/// <summary>
/// 分享矿列表
/// </summary>
public List<Common.GuildObj.Guild_MineShareInfo> getMineShareList() { return mineShareList; }
/// <summary>
/// 分享矿列表
/// </summary>
public void addMineShareList(Common.GuildObj.Guild_MineShareInfo _mineShareList) { mineShareList.Add(_mineShareList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (mineShareList.Count * 36);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (mineShareList.Count * 36);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _mineShareListCount = _buf.getShort();
	for(int _i = 0; _i < _mineShareListCount; _i++) { 
		Common.GuildObj.Guild_MineShareInfo _mineShareList = new Common.GuildObj.Guild_MineShareInfo();
		int __mineShareListCustLen = _buf.getInt();
	int __mineShareListCurPos = _buf.getCurPos();
	_mineShareList.ReadUnzipBuf(_buf, __mineShareListCurPos + __mineShareListCustLen);
	_buf.setPosition(__mineShareListCurPos + __mineShareListCustLen);

		mineShareList.Add(_mineShareList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)mineShareList.Count);
	for(int _i = 0; _i < mineShareList.Count; _i++) { 
		_buf.putInt(mineShareList[_i].GetBufSize());
	mineShareList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)41);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)41);
	_recBuf.put((byte)17);
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
	builder.Append("mineShareList").Append(":").Append(mineShareList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

