using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_082_RetGuildBoxInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宝箱列表
/// </summary>
private List<Common.GuildObj.Guild_BoxInfo> boxList;
private long activePoint;
private int targetLvl;
/// <summary>
/// 分享宝箱匿名
/// </summary>
private bool isGuildBoxShareAnonymous;


public GS2GC_002_082_RetGuildBoxInit() {
	boxList = new List<Common.GuildObj.Guild_BoxInfo>();
	activePoint = (long)0;
	targetLvl = 0;
	isGuildBoxShareAnonymous = false;
}

public GS2GC_002_082_RetGuildBoxInit(
	List<Common.GuildObj.Guild_BoxInfo> _boxList
	, long _activePoint
	, int _targetLvl
	, bool _isGuildBoxShareAnonymous
) {	boxList = _boxList;
	activePoint = _activePoint;
	targetLvl = _targetLvl;
	isGuildBoxShareAnonymous = _isGuildBoxShareAnonymous;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)82; }

/// <summary>
/// 宝箱列表
/// </summary>
public List<Common.GuildObj.Guild_BoxInfo> getBoxList() { return boxList; }
/// <summary>
/// 宝箱列表
/// </summary>
public void addBoxList(Common.GuildObj.Guild_BoxInfo _boxList) { boxList.Add(_boxList); }
public long getActivePoint() { return activePoint; }
public void setActivePoint(long _activePoint) { activePoint = _activePoint; }
public int getTargetLvl() { return targetLvl; }
public void setTargetLvl(int _targetLvl) { targetLvl = _targetLvl; }
/// <summary>
/// 分享宝箱匿名
/// </summary>
public bool getIsGuildBoxShareAnonymous() { return isGuildBoxShareAnonymous; }
/// <summary>
/// 分享宝箱匿名
/// </summary>
public void setIsGuildBoxShareAnonymous(bool _isGuildBoxShareAnonymous) { isGuildBoxShareAnonymous = _isGuildBoxShareAnonymous; }


public int GetBufSize() {
	int _size = 13;
	_size += 2 + (boxList.Count * 36);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;
	_size += 2 + (boxList.Count * 36);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _boxListCount = _buf.getShort();
	for(int _i = 0; _i < _boxListCount; _i++) { 
		Common.GuildObj.Guild_BoxInfo _boxList = new Common.GuildObj.Guild_BoxInfo();
		int __boxListCustLen = _buf.getInt();
	int __boxListCurPos = _buf.getCurPos();
	_boxList.ReadUnzipBuf(_buf, __boxListCurPos + __boxListCustLen);
	_buf.setPosition(__boxListCurPos + __boxListCustLen);

		boxList.Add(_boxList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	activePoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetLvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isGuildBoxShareAnonymous = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)boxList.Count);
	for(int _i = 0; _i < boxList.Count; _i++) { 
		_buf.putInt(boxList[_i].GetBufSize());
	boxList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(activePoint);
	_buf.putInt(targetLvl);
	_buf.put(isGuildBoxShareAnonymous?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)82);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)82);
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
	builder.Append("activePoint").Append(":").Append(activePoint.ToString()).Append(", ");
	builder.Append("targetLvl").Append(":").Append(targetLvl.ToString()).Append(", ");
	builder.Append("isGuildBoxShareAnonymous").Append(":").Append(isGuildBoxShareAnonymous.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

