using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-额外设置-大学学习
/// </summary>
public class WeekCard_ExtraInfo_CollegeStudy : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣列表
/// </summary>
private List<long> heroList;
/// <summary>
/// 是否关闭 关闭则需要给玩家留下可以收获的
/// </summary>
private bool isClose;


public WeekCard_ExtraInfo_CollegeStudy() {
	heroList = new List<long>();
	isClose = false;
}

public WeekCard_ExtraInfo_CollegeStudy(
	List<long> _heroList
	, bool _isClose
) {	heroList = _heroList;
	isClose = _isClose;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大臣列表
/// </summary>
public List<long> getHeroList() { return heroList; }
/// <summary>
/// 大臣列表
/// </summary>
public void addHeroList(long _heroList) { heroList.Add(_heroList); }
/// <summary>
/// 是否关闭 关闭则需要给玩家留下可以收获的
/// </summary>
public bool getIsClose() { return isClose; }
/// <summary>
/// 是否关闭 关闭则需要给玩家留下可以收获的
/// </summary>
public void setIsClose(bool _isClose) { isClose = _isClose; }


public int GetBufSize() {
	int _size = 1;
	_size += 2 + (heroList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 3;
	_size += 2 + (heroList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		long _heroList = (long)0;
		_heroList = _buf.getLong();
		heroList.Add(_heroList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isClose = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)heroList.Count);
	for(int _i = 0; _i < heroList.Count; _i++) { 
		_buf.putLong(heroList[_i]);
	}
	_buf.put(isClose?(byte)1:(byte)0);
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
	builder.Append("heroList").Append(":").Append(heroList.ToString()).Append(", ");
	builder.Append("isClose").Append(":").Append(isClose.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

