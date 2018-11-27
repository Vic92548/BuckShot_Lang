using System;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    public class SyntaxTools
    {
        public SyntaxTools()
        {
            
        }

		public static string GetNewLine(string Line)
		{
			string newLine = "";
			bool String = false;
			foreach (char c in Line)
			{
				if (c == '"')
				{
					if (String)
					{
						String = false;
					}
					else
					{
						String = true;
					}
				}
				else if (String)
				{
					newLine += c;
				}
				else if (c != ' ' && c != '\t')
				{
					newLine += c;
				}
			}
			return newLine;
		}

        public static List<string> GetFunctionDatas(string FunctionString){
            List<string> Result = new List<string>();
            Result.Add(FunctionString.Split('(')[0]);
            if(FunctionString.Split('(')[1].Split(',').Length > 1){
                foreach(string Argument in FunctionString.Split('(')[1].Split(',')){
                    if(Argument[Argument.Length - 1] == ')'){
                        Result.Add(Argument.Remove(Argument.Length - 1));
                    }else{
                        Result.Add(Argument);
                    }

                }
            }
            return Result;
        }
    }
}
