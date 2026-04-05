using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟建造信息列表
/// </summary>
public class Guild_ConstructList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日期
/// </summary>
private int date;
/// <summary>
/// 建造信息列表
/// </summary>
private List<Common.GuildObj.Guild_ConstructInfo> constructList;
/// <summary>
/// 捐赠进度
/// </summary>
private int rewardPoint;


public Guild_ConstructList() {
	date = 0;
	constructList = new List<Common.GuildObj.Guild_ConstructInfo>();
	rewardPoint = 0;
}

public Guild_ConstructList(
	int _date
	, List<Common.GuildObj.Guild_ConstructInfo> _constructList
	, int _rewardPoint
) {	date = _date;
	constructList = _constructList;
	rewardPoint = _rewardPoint;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日期
/// </summary>
public int getDate() { return date; }
/// <summary>
/// 日期
/// </summary>
public void setDate(int _date) { date = _date; }
/// <summary>
/// 建造信息列表
/// </summary>
public List<Common.GuildObj.Guild_ConstructInfo> getConstructList() { return constructList; }
/// <summary>
/// 建造信息列表
/// </summary>
public void addConstructList(Common.GuildObj.Guild_ConstructInfo _constructList) { constructList.Add(_constructList); }
/// <summary>
/// 捐赠进度
/// </summary>
public int getRewardPoint() { return rewardPoint; }
/// <summary>
/// 捐赠进度
/// </summary>
public void setRewardPoint(int _rewardPoint) { rewardPoint = _rewardPoint; }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (constructList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (constructList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	date = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _constructListCount = _buf.getShort();
	for(int _i = 0; _i < _constructListCount; _i++) { 
		Common.GuildObj.Guild_ConstructInfo _constructList = new Common.GuildObj.Guild_ConstructInfo();
		int __constructListCustLen = _buf.getInt();
	int __constructListCurPos = _buf.getCurPos();
	_constructList.ReadUnzipBuf(_buf, __constructListCurPos + __constructListCustLen);
	_buf.setPosition(__constructListCurPos + __constructListCustLen);

		constructList.Add(_constructList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rewardPoint = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(date);
	_buf.putShort((short)constructList.Count);
	for(int _i = 0; _i < constructList.Count; _i++) { 
		_buf.putInt(constructList[_i].GetBufSize());
	constructList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(rewardPoint);
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
	builder.Append("date").Append(":").Append(date.ToString()).Append(", ");
	builder.Append("constructList").Append(":").Append(constructList.ToString()).Append(", ");
	builder.Append("rewardPoint").Append(":").Append(rewardPoint.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

