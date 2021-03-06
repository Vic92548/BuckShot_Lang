﻿using System;
using System.Collections.Generic;
using System.Reflection;
namespace BuckShotCompiler
{
    public class Tools
    {
        public Tools()
        {
        }
        public static WebObject.Base FindWebObjectByName(string Name, List<WebObject.Base> WebObjectList){
            foreach(WebObject.Base CurrentObject in WebObjectList){
                if(CurrentObject.GetName() == Name){
                    return CurrentObject;
                }
            }
            return new WebObject.Base("error", new WebProject(), "", "");
        }

        public static bool IsBasicFuntion(string Name){
            MethodInfo[] BasicFunctionsList = typeof(BasicFunctions).GetMethods();
            foreach(MethodInfo BasicFunc in BasicFunctionsList){
                if(BasicFunc.Name == Name){
                    return true;
                }
            }
            return false;
        }

        public static void ExecuteAllAnalyzerFunc(string[] Words, WebObject.Base CurrentObject){
            MethodInfo[] FunctionsList = typeof(Analyzer).GetMethods();
            int DNum = 0;
			foreach (MethodInfo AnalyzerFunc in FunctionsList)
			{
                if(AnalyzerFunc.Name.Remove(2) == "WO"){
                    if((bool)AnalyzerFunc.Invoke(AnalyzerFunc, new object[] { Words, CurrentObject })){
                        break;
                    }
                }
                DNum++;
			}
        }

        public static void ExecuteBasicFuntion(string Name,string[] Arguments, string ObjectName,WebObject.Base CurrentObject ){
			MethodInfo[] BasicFunctionsList = typeof(BasicFunctions).GetMethods();
			foreach (MethodInfo BasicFunc in BasicFunctionsList)
			{
				if (BasicFunc.Name == Name)
				{
                    object[] BasicArguments = new object[3];
                    BasicArguments[0] = ObjectName;
                    BasicArguments[1] = Arguments;
                    BasicArguments[2] = CurrentObject;
                    BasicFunc.Invoke(BasicFunc,BasicArguments);
				}
			}
        }

		//public static void SetObjectValue(Type PropType, string PropName, string PropValue)
		

		//public static string GetObjectValue<T>(T PropObject, string PropName)
		

        public static WebObject.Base CreateWebObject(string Name, WebObject.Base CurrentObject, string HTML_Path, string CSS_Path){
            WebObject.Base NewObject = new WebObject.Base(Name, CurrentObject.CurrentProject, HTML_Path, CSS_Path);
			NewObject.CSS.SetAllProp(CurrentObject, CurrentObject.CSS.GetAllProp());
			NewObject.HTML.SetAllProp(CurrentObject, CurrentObject.HTML.GetAllProp());
            NewObject.ClassName = CurrentObject.ClassName;
            foreach (WebObject.Base Child in CurrentObject.LocalObjectList)
			{
                NewObject.LocalObjectList.Add(Tools.CreateWebObject(Child.GetName(),Child,HTML_Path,CSS_Path));
			}
            return NewObject;
        }

        public static void CheckSousInstances(string ToAnalyze, string PreviousObjects, List<WebObject.Base> GlobalList){
            string[] PreviousNames = PreviousObjects.Split(new string[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
            WebObject.Base MasterObject = Tools.FindWebObjectByName(PreviousNames[0], GlobalList);
            List<WebObject.Base> LoadedObjects = new List<WebObject.Base>();
            LoadedObjects.Add(MasterObject);
            for (int i = 1; i < PreviousNames.Length;i++){
                WebObject.Base LocalObject = Tools.FindWebObjectByName(PreviousNames[i], LoadedObjects[LoadedObjects.Count - 1].LocalObjectList);
                LoadedObjects.Add(LocalObject);
            }
        }

        public static void SetEmbemdedProp(string LocalData,WebObject.Base NewObject, WebProject CurrentProject){
            
			//string NewLine = SyntaxTools.GetNewLine(LocalData);
            string[] LocalPropList = LocalData.Split('|');
			if (LocalPropList.Length > 0)
			{
				foreach (string LocalProp in LocalPropList)
				{
                    string[] TestString = LocalData.Split('#');
                    if(LocalProp == "-"){
                        
                    }else if(TestString[0].Split('.').Length > 1 && GetPropPos(LocalProp.Split('#')[0]) == -1){
                        
                        string[] Words = LocalProp.Split('.');
                        WebObject.Base ChildObject = Tools.FindWebObjectByName(Words[0], NewObject.LocalObjectList);
                        string[] Prop = Words[1].Split('#');
                        int CurrentPropIndex = GetPropPos(Prop[0]);
                        Console.WriteLine(LocalData + " " + Prop[0]);
                        string PropValue = Tools.GetProcessedValue(Prop[1], NewObject);
                        Console.WriteLine("Words[1] = " + Words[1] + " PropValue = " + PropValue);
                        if (CurrentPropIndex > -1)
                        {
                            NewObject.HTML.PropertiesValue[CurrentPropIndex] = Words[1];
                        }
                        string PropName = Prop[0];
                        Type LangType = CurrentProject.LangTools.PropType(PropName);
                        CurrentProject.LangTools.SetLangObjectValue(LangType, ChildObject, PropName, PropValue);
                    }else{
                        string[] Words = LocalProp.Split('#');
                        string PropValue = Tools.GetProcessedValue(Words[1], NewObject);
                        Console.WriteLine("Words[1] = " + Words[1] + " PropValue = " + PropValue);
                        int CurrentPropIndex = GetPropPos(Words[0]);
                        if(CurrentPropIndex > -1){
                            NewObject.HTML.PropertiesValue[CurrentPropIndex] = PropValue;
                        }
                        string PropName = Words[0];
                        Type LangType = CurrentProject.LangTools.PropType(PropName);
                        CurrentProject.LangTools.SetLangObjectValue(LangType, NewObject, PropName, PropValue);
                    }
				}
			}
        }

        public static string GetProcessedValue(string LocalData, WebObject.Base CurrentObject ){
            if(LocalData.Split('.').Length > 1 && Char.IsLetter(LocalData.Split('.')[1][0])){
                string LeftSide = LocalData.Split('.')[0];
                string RightSide = LocalData.Split('.')[1];
                if(LeftSide == "data"){
                    return CurrentObject.CurrentProject.GetData(RightSide);
                }else{
                    WebObject.Base SearchedObject = Tools.FindWebObjectByName(LeftSide, CurrentObject.CurrentProject.ObjectList);
                    Type LangType = CurrentObject.CurrentProject.LangTools.PropType(RightSide);
                    Console.WriteLine("Value : ");
                    Console.WriteLine(CurrentObject.CurrentProject.LangTools.GetLangObjectValue(LangType, SearchedObject, RightSide));
                    return CurrentObject.CurrentProject.LangTools.GetLangObjectValue(LangType, SearchedObject, RightSide);
                }
            }
            return LocalData;
        }

        public static int GetPropPos(string Prop){
            string[] HTMLProps = { "class", "style","link", "source" };
            for (int i = 0; i < HTMLProps.Length;i++){
                if(HTMLProps[i] == Prop){
                    return i;
                }
            }
            return -1;
        }
    }
}
