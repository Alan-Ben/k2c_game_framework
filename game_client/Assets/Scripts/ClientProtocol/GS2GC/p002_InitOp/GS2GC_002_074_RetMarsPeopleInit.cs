using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 火星-火星居民数据初始化
/// </summary>
public class GS2GC_002_074_RetMarsPeopleInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 居民数量
/// </summary>
private Common.MarsObj.Mars_PeopleNum peopleNum;
/// <summary>
/// 决策数据列表
/// </summary>
private List<Common.MarsObj.Mars_Intelligent> intelligentList;
/// <summary>
/// 满意度万分比
/// </summary>
private int satisfaction;
/// <summary>
/// 信件数据列表
/// </summary>
private List<Common.MarsObj.Mars_Letter> letterList;
/// <summary>
/// 求助列表
/// </summary>
private List<Common.MarsObj.Mars_Help> helpList;
/// <summary>
/// 移民数据
/// </summary>
private Common.MarsObj.Mars_PeopleImmigrant immigrant;
/// <summary>
/// 移民次数
/// </summary>
private Common.MarsObj.Mars_PeopleImmigrantCount immigrantCount;


public GS2GC_002_074_RetMarsPeopleInit() {
	peopleNum = new Common.MarsObj.Mars_PeopleNum();
	intelligentList = new List<Common.MarsObj.Mars_Intelligent>();
	satisfaction = 0;
	letterList = new List<Common.MarsObj.Mars_Letter>();
	helpList = new List<Common.MarsObj.Mars_Help>();
	immigrant = new Common.MarsObj.Mars_PeopleImmigrant();
	immigrantCount = new Common.MarsObj.Mars_PeopleImmigrantCount();
}

public GS2GC_002_074_RetMarsPeopleInit(
	Common.MarsObj.Mars_PeopleNum _peopleNum
	, List<Common.MarsObj.Mars_Intelligent> _intelligentList
	, int _satisfaction
	, List<Common.MarsObj.Mars_Letter> _letterList
	, List<Common.MarsObj.Mars_Help> _helpList
	, Common.MarsObj.Mars_PeopleImmigrant _immigrant
	, Common.MarsObj.Mars_PeopleImmigrantCount _immigrantCount
) {	peopleNum = _peopleNum;
	intelligentList = _intelligentList;
	satisfaction = _satisfaction;
	letterList = _letterList;
	helpList = _helpList;
	immigrant = _immigrant;
	immigrantCount = _immigrantCount;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)74; }

/// <summary>
/// 居民数量
/// </summary>
public Common.MarsObj.Mars_PeopleNum getPeopleNum() { return peopleNum; }
/// <summary>
/// 居民数量
/// </summary>
public void setPeopleNum(Common.MarsObj.Mars_PeopleNum _peopleNum) { peopleNum = _peopleNum; }
/// <summary>
/// 决策数据列表
/// </summary>
public List<Common.MarsObj.Mars_Intelligent> getIntelligentList() { return intelligentList; }
/// <summary>
/// 决策数据列表
/// </summary>
public void addIntelligentList(Common.MarsObj.Mars_Intelligent _intelligentList) { intelligentList.Add(_intelligentList); }
/// <summary>
/// 满意度万分比
/// </summary>
public int getSatisfaction() { return satisfaction; }
/// <summary>
/// 满意度万分比
/// </summary>
public void setSatisfaction(int _satisfaction) { satisfaction = _satisfaction; }
/// <summary>
/// 信件数据列表
/// </summary>
public List<Common.MarsObj.Mars_Letter> getLetterList() { return letterList; }
/// <summary>
/// 信件数据列表
/// </summary>
public void addLetterList(Common.MarsObj.Mars_Letter _letterList) { letterList.Add(_letterList); }
/// <summary>
/// 求助列表
/// </summary>
public List<Common.MarsObj.Mars_Help> getHelpList() { return helpList; }
/// <summary>
/// 求助列表
/// </summary>
public void addHelpList(Common.MarsObj.Mars_Help _helpList) { helpList.Add(_helpList); }
/// <summary>
/// 移民数据
/// </summary>
public Common.MarsObj.Mars_PeopleImmigrant getImmigrant() { return immigrant; }
/// <summary>
/// 移民数据
/// </summary>
public void setImmigrant(Common.MarsObj.Mars_PeopleImmigrant _immigrant) { immigrant = _immigrant; }
/// <summary>
/// 移民次数
/// </summary>
public Common.MarsObj.Mars_PeopleImmigrantCount getImmigrantCount() { return immigrantCount; }
/// <summary>
/// 移民次数
/// </summary>
public void setImmigrantCount(Common.MarsObj.Mars_PeopleImmigrantCount _immigrantCount) { immigrantCount = _immigrantCount; }


public int GetBufSize() {
	int _size = 56;
	_size += 2 + (intelligentList.Count * 20);
	_size += 2 + (letterList.Count * 29);
	_size += 2 + (helpList.Count * 33);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 58;
	_size += 2 + (intelligentList.Count * 20);
	_size += 2 + (letterList.Count * 29);
	_size += 2 + (helpList.Count * 33);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _peopleNumCustLen = _buf.getInt();
	int _peopleNumCurPos = _buf.getCurPos();
	peopleNum.ReadUnzipBuf(_buf, _peopleNumCurPos + _peopleNumCustLen);
	_buf.setPosition(_peopleNumCurPos + _peopleNumCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _intelligentListCount = _buf.getShort();
	for(int _i = 0; _i < _intelligentListCount; _i++) { 
		Common.MarsObj.Mars_Intelligent _intelligentList = new Common.MarsObj.Mars_Intelligent();
		int __intelligentListCustLen = _buf.getInt();
	int __intelligentListCurPos = _buf.getCurPos();
	_intelligentList.ReadUnzipBuf(_buf, __intelligentListCurPos + __intelligentListCustLen);
	_buf.setPosition(__intelligentListCurPos + __intelligentListCustLen);

		intelligentList.Add(_intelligentList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	satisfaction = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _letterListCount = _buf.getShort();
	for(int _i = 0; _i < _letterListCount; _i++) { 
		Common.MarsObj.Mars_Letter _letterList = new Common.MarsObj.Mars_Letter();
		int __letterListCustLen = _buf.getInt();
	int __letterListCurPos = _buf.getCurPos();
	_letterList.ReadUnzipBuf(_buf, __letterListCurPos + __letterListCustLen);
	_buf.setPosition(__letterListCurPos + __letterListCustLen);

		letterList.Add(_letterList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _helpListCount = _buf.getShort();
	for(int _i = 0; _i < _helpListCount; _i++) { 
		Common.MarsObj.Mars_Help _helpList = new Common.MarsObj.Mars_Help();
		int __helpListCustLen = _buf.getInt();
	int __helpListCurPos = _buf.getCurPos();
	_helpList.ReadUnzipBuf(_buf, __helpListCurPos + __helpListCustLen);
	_buf.setPosition(__helpListCurPos + __helpListCustLen);

		helpList.Add(_helpList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _immigrantCustLen = _buf.getInt();
	int _immigrantCurPos = _buf.getCurPos();
	immigrant.ReadUnzipBuf(_buf, _immigrantCurPos + _immigrantCustLen);
	_buf.setPosition(_immigrantCurPos + _immigrantCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _immigrantCountCustLen = _buf.getInt();
	int _immigrantCountCurPos = _buf.getCurPos();
	immigrantCount.ReadUnzipBuf(_buf, _immigrantCountCurPos + _immigrantCountCustLen);
	_buf.setPosition(_immigrantCountCurPos + _immigrantCountCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(peopleNum.GetBufSize());
	peopleNum.PutUnzipBuf(_buf);
	_buf.putShort((short)intelligentList.Count);
	for(int _i = 0; _i < intelligentList.Count; _i++) { 
		_buf.putInt(intelligentList[_i].GetBufSize());
	intelligentList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(satisfaction);
	_buf.putShort((short)letterList.Count);
	for(int _i = 0; _i < letterList.Count; _i++) { 
		_buf.putInt(letterList[_i].GetBufSize());
	letterList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)helpList.Count);
	for(int _i = 0; _i < helpList.Count; _i++) { 
		_buf.putInt(helpList[_i].GetBufSize());
	helpList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(immigrant.GetBufSize());
	immigrant.PutUnzipBuf(_buf);
	_buf.putInt(immigrantCount.GetBufSize());
	immigrantCount.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)74);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)74);
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
	builder.Append("peopleNum").Append(":").Append(peopleNum == null ? "null" : peopleNum.ToString()).Append(", ");
	builder.Append("intelligentList").Append(":").Append(intelligentList.ToString()).Append(", ");
	builder.Append("satisfaction").Append(":").Append(satisfaction.ToString()).Append(", ");
	builder.Append("letterList").Append(":").Append(letterList.ToString()).Append(", ");
	builder.Append("helpList").Append(":").Append(helpList.ToString()).Append(", ");
	builder.Append("immigrant").Append(":").Append(immigrant == null ? "null" : immigrant.ToString()).Append(", ");
	builder.Append("immigrantCount").Append(":").Append(immigrantCount == null ? "null" : immigrantCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

