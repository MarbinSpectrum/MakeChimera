using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUtility
{
	public static List<string> Split(string input, List<char> check)
	{
		List<string> answer = new List<string>();
		input += check[0];
		string temp = "";
		for (int i = 0; i < input.Length; i++)
		{
			bool flag = false;
			for (int j = 0; j < check.Count; j++)
				if (input[i] == check[j])
				{
					flag = true;
					break;
				}
			if (flag && temp.Length == 0)
				continue;
			if (flag && temp.Length > 0)
			{
				answer.Add(temp);
				temp = "";
			}
			else
			{
				temp += input[i];
			}
		}
		return answer;
	}

	public static List<string> Split(string input, char check)
	{
		List<char> temp = new List<char>();
		temp.Add(check);
		return Split(input, temp);		
	}

	public static bool RandomResult(float input,float range)
    {
		float result = Random.Range(0, range);
		return result < input;
    }
}
