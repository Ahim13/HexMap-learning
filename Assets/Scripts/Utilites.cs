using System;
using Models;
using Newtonsoft.Json;
using UnityEngine;


public static class Utilites
{
	public static T DeserializeJson<T>(this string json) where T : IModel
	{
		try
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
		catch (Exception e)
		{
			Debug.LogError("Could not deserialize Json! - " + e.Message + " " + e.StackTrace);
			return default;
		}
	}
}