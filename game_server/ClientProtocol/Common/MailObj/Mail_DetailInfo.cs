using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.MailObj
{

/// <summary>
/// 邮件详细信息
/// </summary>
public class Mail_DetailInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邮件唯一id
/// </summary>
private long mailUid;
/// <summary>
/// 邮件文本内容
/// </summary>
private string content;
/// <summary>
/// 替换的参数内容
/// </summary>
private List<string> contentReplace;
/// <summary>
/// 附件物品列表
/// </summary>
private List<NPCommon.NPCommon_ItemInfo> itemList;
/// <summary>
/// 额外信息类型id
/// </summary>
private int exType;
/// <summary>
/// 额外信息内容
/// </summary>
private byte[] exData;


public Mail_DetailInfo() {
	mailUid = (long)0;
	content = "";
	contentReplace = new List<string>();
	itemList = new List<NPCommon.NPCommon_ItemInfo>();
	exType = 0;
	exData = null;
}

public Mail_DetailInfo(
	long _mailUid
	, string _content
	, List<string> _contentReplace
	, List<NPCommon.NPCommon_ItemInfo> _itemList
	, int _exType
	, byte[] _exData
) {	mailUid = _mailUid;
	content = _content;
	contentReplace = _contentReplace;
	itemList = _itemList;
	exType = _exType;
	exData = _exData;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 邮件唯一id
/// </summary>
public long getMailUid() { return mailUid; }
/// <summary>
/// 邮件唯一id
/// </summary>
public void setMailUid(long _mailUid) { mailUid = _mailUid; }
/// <summary>
/// 邮件文本内容
/// </summary>
public string getContent() { return content; }
/// <summary>
/// 邮件文本内容
/// </summary>
public void setContent(string _content) { content = _content; }
/// <summary>
/// 替换的参数内容
/// </summary>
public List<string> getContentReplace() { return contentReplace; }
/// <summary>
/// 替换的参数内容
/// </summary>
public void addContentReplace(string _contentReplace) { contentReplace.Add(_contentReplace); }
/// <summary>
/// 附件物品列表
/// </summary>
public List<NPCommon.NPCommon_ItemInfo> getItemList() { return itemList; }
/// <summary>
/// 附件物品列表
/// </summary>
public void addItemList(NPCommon.NPCommon_ItemInfo _itemList) { itemList.Add(_itemList); }
/// <summary>
/// 额外信息类型id
/// </summary>
public int getExType() { return exType; }
/// <summary>
/// 额外信息类型id
/// </summary>
public void setExType(int _exType) { exType = _exType; }
/// <summary>
/// 额外信息内容
/// </summary>
public byte[] getExData() { return exData; }

/// <summary>
/// 额外信息内容
/// </summary>
public void setExData(byte[] _exData) { exData = _exData; }



public int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < contentReplace.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace[_i]);
	}

	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += 2;
for(int _i = 0; _i < contentReplace.Count; _i++) {
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace[_i]);
	}

	_size += 2;
for(int _i = 0; _i < itemList.Count; _i++) {
	_size += 4 + itemList[_i].GetBufSize();
	}

	_size += 4 + (exData == null ? 0 : exData.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	mailUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _contentReplaceCount = _buf.getShort();
	for(int _i = 0; _i < _contentReplaceCount; _i++) { 
		string _contentReplace = "";
		_contentReplace = _buf.getString();
		contentReplace.Add(_contentReplace);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _itemListCount = _buf.getShort();
	for(int _i = 0; _i < _itemListCount; _i++) { 
		NPCommon.NPCommon_ItemInfo _itemList = new NPCommon.NPCommon_ItemInfo();
		int __itemListCustLen = _buf.getInt();
	int __itemListCurPos = _buf.getCurPos();
	_itemList.ReadUnzipBuf(_buf, __itemListCurPos + __itemListCustLen);
	_buf.setPosition(__itemListCurPos + __itemListCustLen);

		itemList.Add(_itemList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exData = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(mailUid);
	_buf.putString(content);
	_buf.putShort((short)contentReplace.Count);
	for(int _i = 0; _i < contentReplace.Count; _i++) { 
		_buf.putString(contentReplace[_i]);
	}
	_buf.putShort((short)itemList.Count);
	for(int _i = 0; _i < itemList.Count; _i++) { 
		_buf.putInt(itemList[_i].GetBufSize());
	itemList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(exType);
	_buf.putByteBuffer(exData);

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
	builder.Append("mailUid").Append(":").Append(mailUid.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("contentReplace").Append(":").Append(contentReplace.ToString()).Append(", ");
	builder.Append("itemList").Append(":").Append(itemList.ToString()).Append(", ");
	builder.Append("exType").Append(":").Append(exType.ToString()).Append(", ");
	builder.Append("exData").Append(":").Append(exData == null ? "null" : exData.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

