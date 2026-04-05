#!/usr/bin/env python
# coding=utf-8
project_root_path="../ActivitiesV02/src"
java_package="ActivitiesV02.Events"
events = \
{
    "P_NUM_MERGE_TOTAL_SCORE_CHG":
	{
		"id":2001,
		"comment":"2048总分变化",
		"field":
		[
			["SCORE","分数"],
		],
	},
    "P_NUM_MERGE_ROUND_MAX_SCORE_CHG":
	{
		"id":2002,
		"comment":"2048单轮最高分变化",
		"field":
		[
			["SCORE","分数"],
		],
	},
    "P_NUM_MERGE_NEW_BLOCK":
	{
		"id":2003,
		"comment":"2048新方块生成",
		"field":
		[
			["ID","方块等级"],
            ["COUNT","数量"],
		],
	},
}

