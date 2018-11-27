using System;
using System.Collections.Generic;

namespace BuckShotCompiler
{
    public static class Analyzer
    {
		public static bool WOFunc(string[] Words, WebObject CurrentObject)
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

		public static bool WOArray(string[] Words, WebObject CurrentObject)
		{
			if (Words[1].Length < 2)
			{
				return false;
			}
			else if (Words[1].Remove(1) == "[")
			{
				string LocalArguments = Words[1].Substring(1).Split(']')[0];
				string[] LocalArgumentsArray = LocalArguments.Split(',');
				WebObject CreatedObject = new WebObject(Words[0], CurrentObject.CurrentProject, CurrentObject.HTML_Path, CurrentObject.CSS_Path);
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

        public static bool WOCheckForSousInstances(string[] Words,WebObject CurrentObject){
			if (Words[0].Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries).Length > 1)
			{
				string[] LocalWords = Words[0].Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                WebObject MasterObject = Tools.FindWebObjectByName(LocalWords[0], CurrentObject.CurrentProject.ObjectList);
				List<WebObject> LoadedObjects = new List<WebObject>();
				LoadedObjects.Add(MasterObject);
				for (int i = 1; i < LocalWords.Length; i++)
				{
					if (LocalWords[i].Split('.').Length > 1)
					{
						WebObject ChildObject = Tools.FindWebObjectByName(LocalWords[i].Split('.')[0], LoadedObjects[LoadedObjects.Count - 1].LocalObjectList);
						Tools.SetObjectValue<CSSObject>(ChildObject.CSS, LocalWords[i].Split('.')[1], Words[1]);
						Tools.SetObjectValue<HTMLObject>(ChildObject.HTML, LocalWords[i].Split('.')[1], Words[1]);
					}
					else
					{
                        WebObject LocalObject = null;
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

        public static bool WOCheckForPropSetting(string[] Words,WebObject CurrentObject){
			if (Words[0].Split('.').Length > 1)
			{
				WebObject ChildObject = Tools.FindWebObjectByName(Words[0].Split('.')[0], CurrentObject.LocalObjectList);
                string CurrentValue = Tools.GetProcessedValue(Words[1], CurrentObject);
				if (Words[0].Split('.')[1] == "link")
				{
                    ChildObject.HTML.PropertiesValue[2] = CurrentValue;
				}
				else if (Words[0].Split('.')[1] == "source")
				{
                    ChildObject.HTML.PropertiesValue[3] = CurrentValue;
				}
                Tools.SetObjectValue<HTMLObject>(ChildObject.HTML, Words[0].Split('.')[1], CurrentValue);
                Tools.SetObjectValue<CSSObject>(ChildObject.CSS, Words[0].Split('.')[1], CurrentValue);
                return true;
            }else{
                return false;
            }
        }

        public static bool WOSetPropValueWithObjectValue(string[] Words, WebObject CurrentObject){
            if (Words[1].Split('.').Length > 1 && Char.IsLetter(Words[1].Split('.')[1][0]))
            {
                string ObjName = Words[1].Split('.')[0];
                string PropName = Words[1].Split('.')[1];
                foreach (WebObject CurrentObj in CurrentObject.CurrentProject.ObjectList)
                {
                    if (CurrentObj.GetName() == ObjName)
                    {
                        if (CurrentObject.CSS.IsCSS(PropName))
                        {
                            Tools.SetObjectValue(CurrentObject.CSS, Words[0], Tools.GetObjectValue<CSSObject>(CurrentObj.CSS, PropName));
                        }
                    }
                }
                return true;
            }else{
                return false;
            }
        }

        public static bool WOSetLink(string[] Words, WebObject CurrentObject){
			if (Words[0] == "link")
			{
                CurrentObject.HTML.PropertiesValue[2] = Words[1];
                return true;
            }else{
                return false;
            }
        }

        public static bool WOSetSource(string[] Words,WebObject CurrentObject){
			if (Words[0] == "source")
			{
                CurrentObject.HTML.PropertiesValue[3] = Words[1];
                return true;
            }else{
                return false;
            }
        }

        public static bool WONewObject(string[] Words, WebObject CurrentObject){
			if (Words[1].Length > 3)
			{
				if (Words[1].Remove(3) == "new")
				{
                    foreach (WebObject LocalCurrentObject in CurrentObject.CurrentProject.ObjectList)
					{
						string MasterObjName = Words[1].Substring(3);
						if (LocalCurrentObject.GetName() == MasterObjName)
						{
                            WebObject NewObject = Tools.CreateWebObject(Words[0], LocalCurrentObject, CurrentObject.HTML_Path, CurrentObject.CSS_Path);
							NewObject.ClassName = LocalCurrentObject.GetName();
                            CurrentObject.LocalObjectList.Add(NewObject);
						}
					}
				}
				else
				{
					Tools.SetObjectValue<HTMLObject>(CurrentObject.HTML, Words[0], Words[1]);
					Tools.SetObjectValue<CSSObject>(CurrentObject.CSS, Words[0], Words[1]);
				}
                return true;
            }else{
                return false;
            }
        }

        public static bool WOFinal(string[] Words, WebObject CurrentObject){
            Tools.SetObjectValue<HTMLObject>(CurrentObject.HTML, Words[0], Words[1]);
			Tools.SetObjectValue<CSSObject>(CurrentObject.CSS, Words[0], Words[1]);
            return true;
        }
    }
}
