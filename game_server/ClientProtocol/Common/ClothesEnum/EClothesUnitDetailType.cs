using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ClothesEnum
{

/// <summary>
/// 时装单品详细类型
/// </summary>
public enum EClothesUnitDetailType {
	NONE, //0 ==== 
	[InspectorName("HAIRSTYLE - idx[1] - 发型")]
	HAIRSTYLE, //1 ==== 发型
	[InspectorName("DRESS - idx[2] - 连衣裙")]
	DRESS, //2 ==== 连衣裙
	[InspectorName("COAT - idx[3] - 外套")]
	COAT, //3 ==== 外套
	[InspectorName("TOP - idx[4] - 上衣")]
	TOP, //4 ==== 上衣
	[InspectorName("BOTTOMS - idx[5] - 下装")]
	BOTTOMS, //5 ==== 下装
	[InspectorName("SOCKS - idx[6] - 袜子")]
	SOCKS, //6 ==== 袜子
	[InspectorName("SHOES - idx[7] - 鞋子")]
	SHOES, //7 ==== 鞋子
	[InspectorName("HAT - idx[8] - 帽子")]
	HAT, //8 ==== 帽子
	[InspectorName("FACE_ACCESSORY - idx[9] - 面饰")]
	FACE_ACCESSORY, //9 ==== 面饰
	[InspectorName("EAR_ACCESSORY - idx[10] - 耳饰")]
	EAR_ACCESSORY, //10 ==== 耳饰
	[InspectorName("NECK_ACCESSORY - idx[11] - 颈饰")]
	NECK_ACCESSORY, //11 ==== 颈饰
	[InspectorName("GLOVES - idx[12] - 手套")]
	GLOVES, //12 ==== 手套
	[InspectorName("HANDHELD - idx[13] - 手持物")]
	HANDHELD, //13 ==== 手持物
	[InspectorName("HAIR_ACCESSORY - idx[14] - 发饰")]
	HAIR_ACCESSORY, //14 ==== 发饰
	[InspectorName("WRIST_ACCESSORY - idx[15] - 腕饰")]
	WRIST_ACCESSORY, //15 ==== 腕饰
	[InspectorName("RING - idx[16] - 戒指")]
	RING, //16 ==== 戒指
	[InspectorName("WING - idx[17] - 翅膀")]
	WING, //17 ==== 翅膀
	[InspectorName("EYEBROW_MAKEUP - idx[18] - 眉妆")]
	EYEBROW_MAKEUP, //18 ==== 眉妆
	[InspectorName("COSMETIC_CONTACT_LENSES - idx[19] - 美瞳")]
	COSMETIC_CONTACT_LENSES, //19 ==== 美瞳
	[InspectorName("EYE_SHADOW - idx[20] - 眼影")]
	EYE_SHADOW, //20 ==== 眼影
	[InspectorName("EYE_LIINER - idx[21] - 眼线")]
	EYE_LIINER, //21 ==== 眼线
	[InspectorName("EYELASH - idx[22] - 睫毛")]
	EYELASH, //22 ==== 睫毛
	[InspectorName("ROUGE - idx[23] - 腮红")]
	ROUGE, //23 ==== 腮红
	[InspectorName("LIP_MAKEUP - idx[24] - 唇妆")]
	LIP_MAKEUP, //24 ==== 唇妆
	[InspectorName("FACE_TATTOOS - idx[25] - 纹面")]
	FACE_TATTOOS, //25 ==== 纹面
	[InspectorName("TAIL - idx[26] - 尾巴")]
	TAIL, //26 ==== 尾巴
	[InspectorName("BACK_ACCESSORY - idx[27] - 背饰")]
	BACK_ACCESSORY, //27 ==== 背饰
	[InspectorName("CROSSBODY - idx[28] - 斜挎")]
	CROSSBODY, //28 ==== 斜挎
	[InspectorName("FLOATING - idx[29] - 悬浮")]
	FLOATING, //29 ==== 悬浮
	[InspectorName("TATTOO - idx[30] - 纹身")]
	TATTOO, //30 ==== 纹身
}

public class EClothesUnitDetailTypeComparer : IEqualityComparer<EClothesUnitDetailType>{
	public bool Equals(EClothesUnitDetailType x, EClothesUnitDetailType y) { return x == y; }
	public int GetHashCode(EClothesUnitDetailType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 31;
}
}

