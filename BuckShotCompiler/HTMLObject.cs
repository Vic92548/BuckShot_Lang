using System;
using System.Reflection;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    public class HTMLObject
    {
        public string type = "";
        public string content = "";
        public string[] PropertiesName = { "class", "style", "href", "src" };
        public string[] PropertiesValue = { null, null, null, null };

        public string GetCompiledCode(){
            string CompiledHTML = "";
            if(type != ""){
			    CompiledHTML = "<" + this.type;
				for (int i = 0; i < this.PropertiesName.Length; i++)
				{
					if (PropertiesValue[i] != null)
					{
						CompiledHTML += ' ' + PropertiesName[i] + "=\"" + PropertiesValue[i] + '"';
					}
				}
				CompiledHTML += '>' + this.content + "</" + this.type + ">\n";
            }
            return CompiledHTML;
        }

		public List<FieldInfo> GetAllProp()
		{
			List<FieldInfo> PropList = new List<FieldInfo>();
			FieldInfo[] HTMLProps = this.GetType().GetFields();
			foreach (FieldInfo HTMLProp in HTMLProps)
			{
				PropList.Add(HTMLProp);
			}
			return PropList;
		}

		public void SetAllProp(WebObject MasterObject, List<FieldInfo> PropList)
		{
			FieldInfo[] HTMLProps = this.GetType().GetFields();
			foreach (FieldInfo LocalProp in PropList)
			{
                if(LocalProp.Name == "PropertiesValue"){
                    string[] MasterPropValueList = (string[])LocalProp.GetValue(MasterObject.HTML);
                    for (int i = 0; i < MasterPropValueList.Length;i++){
                        this.PropertiesValue[i] = MasterPropValueList[i];
                    }
                }else{
                    HTMLProps[PropList.IndexOf(LocalProp)].SetValue(this, LocalProp.GetValue(MasterObject.HTML));
                }
			}
		}
    }
}
