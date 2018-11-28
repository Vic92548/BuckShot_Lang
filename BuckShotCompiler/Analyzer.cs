using System;
using System.Collections.Generic;

namespace BuckShotCompiler
{
    public static class Analyzer
    {
        public static bool WOFunc(string[] Words, WebObject.Base CurrentObject)
		{
			if (Tools.IsBasicFuntion(Words[1].Split('(')[0]))
			{
				string[] FuncArguments = Words[1].Split('(')[1].Split(')')[0].Split(',');
                string FuncName = Words[1].Split('(')[0];
                Tools.ExecuteBasicFuntion(FuncName, FuncArguments, Words[0], CurrentObject);
				return true;
			}
			else
			{
				return false;
			}
		}

        public static bool WOArray(string[] Words, WebObject.Base CurrentObject )
		{
			if (Words[1].Length < 2)
			{
				return false;
			}
			else if (Words[1].Remove(1) == "[")
			{
				string LocalArguments = Words[1].Substring(1).Split(']')[0];
				string[] LocalArgumentsArray = LocalArguments.Split(',');
                WebObject.Base CreatedObject = new WebObject.Base(Words[0], CurrentObject.CurrentProject, CurrentObject.HTML_Path, CurrentObject.CSS_Path);
				foreach (string LocalData in LocalArgumentsArray)
				{
					CreatedObject.ArrayDatas.Add(LocalData);
				}
				CurrentObject.LocalObjectList.Add(CreatedObject);
				return true;
			}
			else
			{
				return false;
			}
		}

        public static bool WOCheckForSousInstances(string[] Words,WebObject.Base CurrentObject )
        {
			if (Words[0].Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
			{
				string[] LocalWords = Words[0].Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                WebObject.Base MasterObject = Tools.FindWebObjectByName(LocalWords[0], CurrentObject.CurrentProject.ObjectList);
                List<WebObject.Base> LoadedObjects = new List<WebObject.Base>();
				LoadedObjects.Add(MasterObject);
				for (int i = 1; i < LocalWords.Length; i++)
				{
					if (LocalWords[i].Split('.').Length > 1)
					{
                        WebObject.Base ChildObject = Tools.FindWebObjectByName(LocalWords[i].Split('.')[0], LoadedObjects[LoadedObjects.Count - 1].LocalObjectList);
                        string PropName = LocalWords[i].Split('.')[1];
                        Type LangType = CurrentObject.CurrentProject.LangTools.PropType(PropName);
                        CurrentObject.CurrentProject.LangTools.SetLangObjectValue(LangType, CurrentObject, PropName, Words[1]);
					}
					else
					{
                        WebObject.Base LocalObject = null;
                        if(Words[1].Length > 3){
                            if(Words[1].Remove(3) == "new"){
                                Console.WriteLine("Super coool :D " + Words[1].Substring(3) + LocalWords[i] + LocalWords[i-1]);
                                LocalObject = Tools.CreateWebObject(LocalWords[i], Tools.FindWebObjectByName(Words[1].Substring(3), CurrentObject.CurrentProject.ObjectList), CurrentObject.HTML_Path, CurrentObject.CSS_Path);
                                Tools.FindWebObjectByName(LocalWords[i - 1],CurrentObject.LocalObjectList).LocalObjectList.Add(LocalObject);
                                Console.WriteLine(Tools.FindWebObjectByName(LocalWords[i - 1], CurrentObject.LocalObjectList).LocalObjectList[0].GetName());
                            }else{
                                LocalObject = Tools.FindWebObjectByName(LocalWords[i], LoadedObjects[LoadedObjects.Count - 1].LocalObjectList);
                            }
                        }
                        else{
                            LocalObject = Tools.FindWebObjectByName(LocalWords[i], LoadedObjects[LoadedObjects.Count - 1].LocalObjectList);
                        }
						LoadedObjects.Add(LocalObject);
					}
				}
                return true;
            }else{
                return false;
            }
        }

        public static bool WOCheckForPropSetting(string[] Words,WebObject.Base CurrentObject )
        {
			if (Words[0].Split('.').Length > 1)
			{
                WebObject.Base ChildObject = Tools.FindWebObjectByName(Words[0].Split('.')[0], CurrentObject.LocalObjectList);
                string CurrentValue = Tools.GetProcessedValue(Words[1], CurrentObject);
				if (Words[0].Split('.')[1] == "link")
				{
                    ChildObject.HTML.PropertiesValue[2] = CurrentValue;
				}
				else if (Words[0].Split('.')[1] == "source")
				{
                    ChildObject.HTML.PropertiesValue[3] = CurrentValue;
				}
                string PropName = Words[0].Split('.')[1];
                Type LangType = CurrentObject.CurrentProject.LangTools.PropType(PropName);
                CurrentObject.CurrentProject.LangTools.SetLangObjectValue(LangType, ChildObject, PropName, CurrentValue);
                return true;
            }else{
                return false;
            }
        }

        public static bool WOSetPropValueWithObjectValue(string[] Words, WebObject.Base CurrentObject ){
            if (Words[1].Split('.').Length > 1 && Char.IsLetter(Words[1].Split('.')[1][0]))
            {
                string ObjName = Words[1].Split('.')[0];
                string PropName = Words[1].Split('.')[1];
                foreach (WebObject.Base CurrentObj in CurrentObject.CurrentProject.ObjectList)
                {
                    if (CurrentObj.GetName() == ObjName)
                    {
                        Type LangType = CurrentObj.CurrentProject.LangTools.PropType(PropName);
                        CurrentObj.CurrentProject.LangTools.SetLangObjectValue(LangType, CurrentObj, PropName, CurrentObj.CurrentProject.LangTools.GetLangObjectValue(LangType, CurrentObj, PropName));
                    }
                }
                return true;
            }else{
                return false;
            }
        }

        public static bool WOSetLink(string[] Words, WebObject.Base CurrentObject)
        {
			if (Words[0] == "link")
			{
                CurrentObject.HTML.PropertiesValue[2] = Words[1];
                return true;
            }else{
                return false;
            }
        }

        public static bool WOSetSource(string[] Words,WebObject.Base CurrentObject)
        {
			if (Words[0] == "source")
			{
                CurrentObject.HTML.PropertiesValue[3] = Words[1];
                return true;
            }else{
                return false;
            }
        }

        public static bool WONewObject(string[] Words, WebObject.Base CurrentObject)
        {
			if (Words[1].Length > 3)
			{
				if (Words[1].Remove(3) == "new")
				{
                    foreach (WebObject.Base LocalCurrentObject in CurrentObject.CurrentProject.ObjectList)
					{
						string MasterObjName = Words[1].Substring(3);
						if (LocalCurrentObject.GetName() == MasterObjName)
						{
                            WebObject.Base NewObject = Tools.CreateWebObject(Words[0], LocalCurrentObject, CurrentObject.HTML_Path, CurrentObject.CSS_Path);
							NewObject.ClassName = LocalCurrentObject.GetName();
                            CurrentObject.LocalObjectList.Add(NewObject);
						}
					}
				}
				else
				{
                    string PropName = Words[0];
                    Type LangType = CurrentObject.CurrentProject.LangTools.PropType(PropName);
                    CurrentObject.CurrentProject.LangTools.SetLangObjectValue(LangType, CurrentObject, PropName, Words[1]);
				}
                return true;
            }else{
                return false;
            }
        }

        public static bool WOFinal(string[] Words, WebObject.Base CurrentObject)
        {
            string PropName = Words[0];
            Type LangType = CurrentObject.CurrentProject.LangTools.PropType(PropName);
            CurrentObject.CurrentProject.LangTools.SetLangObjectValue(LangType, CurrentObject, PropName, Words[1]);
            return true;
        }
    }
}
