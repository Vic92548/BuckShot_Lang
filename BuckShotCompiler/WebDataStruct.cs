using System;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    public struct WebDataStruct
    {
        public string CatName;
        public string Key;
        public string Value;
        public List<WebDataStruct> ChildsData;
        public List<WebDataStruct> ChildsCategories;

        public static WebDataStruct FindCategory(WebDataStruct ParentData,string SearchedCatName){
            foreach(WebDataStruct LocalData in ParentData.ChildsCategories){
                if(LocalData.CatName == SearchedCatName){
                    return LocalData;
                }
            }
            throw new Exception("Category not found from FindCategory");
        }

        public static WebDataStruct FindCategoryFromFullKey(WebDataStruct ParentData, string FullKey){
            string[] Args = FullKey.Split('.');
            WebDataStruct CurrentData = ParentData;
            for (int i = 0; i < Args.Length - 2; i++)
            {
                CurrentData = WebDataStruct.FindCategory(CurrentData, Args[i]);
            }
            return CurrentData;
        }

        public static WebDataStruct FindDataFromFullKey(WebDataStruct ParentData, string FullKey){
            string[] KeyArgs = FullKey.Split('.');
            WebDataStruct LocalCat = WebDataStruct.FindCategoryFromFullKey(ParentData, FullKey);
            foreach(WebDataStruct LocalData in LocalCat.ChildsData){
                if(LocalData.Key == KeyArgs[KeyArgs.Length - 1]){
                    return LocalData;
                }
            }
            throw new Exception("Data not found from FindDataFromFullKey");
        }
    }
}
