using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 大臣套系信息
/// </summary>
public class Hero_SuitInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 套系id
/// </summary>
private long suitId;
/// <summary>
/// 套系技能列表
/// </summary>
private List<Common.HeroObj.Hero_SuitSkillInfo> suitSkillList;


public Hero_SuitInfo() {
	suitId = (long)0;
	suitSkillList = new List<Common.HeroObj.Hero_SuitSkillInfo>();
}

public Hero_SuitInfo(
	long _suitId
	, List<Common.HeroObj.Hero_SuitSkillInfo> _suitSkillList
) {	suitId = _suitId;
	suitSkillList = _suitSkillList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 套系id
/// </summary>
public long getSuitId() { return suitId; }
/// <summary>
/// 套系id
/// </summary>
public void setSuitId(long _suitId) { suitId = _suitId; }
/// <summary>
/// 套系技能列表
/// </summary>
public List<Common.HeroObj.Hero_SuitSkillInfo> getSuitSkillList() { return suitSkillList; }
/// <summary>
/// 套系技能列表
/// </summary>
public void addSuitSkillList(Common.HeroObj.Hero_SuitSkillInfo _suitSkillList) { suitSkillList.Add(_suitSkillList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (suitSkillList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (suitSkillList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	suitId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _suitSkillListCount = _buf.getShort();
	for(int _i = 0; _i < _suitSkillListCount; _i++) { 
		Common.HeroObj.Hero_SuitSkillInfo _suitSkillList = new Common.HeroObj.Hero_SuitSkillInfo();
		int __suitSkillListCustLen = _buf.getInt();
	int __suitSkillListCurPos = _buf.getCurPos();
	_suitSkillList.ReadUnzipBuf(_buf, __suitSkillListCurPos + __suitSkillListCustLen);
	_buf.setPosition(__suitSkillListCurPos + __suitSkillListCustLen);

		suitSkillList.Add(_suitSkillList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(suitId);
	_buf.putShort((short)suitSkillList.Count);
	for(int _i = 0; _i < suitSkillList.Count; _i++) { 
		_buf.putInt(suitSkillList[_i].GetBufSize());
	suitSkillList[_i].PutUnzipBuf(_buf);
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
	builder.Append("suitId").Append(":").Append(suitId.ToString()).Append(", ");
	builder.Append("suitSkillList").Append(":").Append(suitSkillList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

