using System;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    public class Compiler
    {
        public string HTML_Path;
        public string CSS_Path;
        public string FileName;
        private List<string> FileLines;
        public Compiler(string File)
        {
            this.FileName = File;
        }
        public string GlobalCompile(){
            string[] FileText = System.IO.File.ReadAllLines(FileName);
            string FileNamePath = "";
            if(FileName.Split('/').Length > 1){
                FileNamePath = FileName.Remove(FileName.Length - FileName.Split('/')[FileName.Split('/').Length - 1].Length - 1);
            }else{
                FileNamePath = FileName.Remove(FileName.Length - FileName.Split('\\')[FileName.Split('\\').Length - 1].Length - 1);
            }
            string[] ConfigLines = System.IO.File.ReadAllLines(FileNamePath + "/config.bk");
            string[] DataLines = {};
            WebProject CurrenProject = new WebProject();
            CurrenProject.ProjectStopWatch.Start();
            if(System.IO.File.Exists(FileNamePath + "/data.bk")){
                DataLines = System.IO.File.ReadAllLines(FileNamePath + "/data.bk");
                for (int i = 0; i < DataLines.Length;i++){
                    string[] LocalLine = SyntaxTools.GetNewLine(DataLines[i]).Split('=');
                    CurrenProject.AddData(LocalLine[0],LocalLine[1]);
                }
            }
            this.HTML_Path = FileNamePath + ConfigLines[0].Split('=')[1];
            VerifyFolder(this.HTML_Path);
            this.CSS_Path = FileNamePath + ConfigLines[1].Split('=')[1];
            VerifyFolder(this.CSS_Path);
			bool Started = false;
            WebObject CurrentObject = new WebObject("default", CurrenProject, "","");
            FileLines = new List<string>();
            AddFileLines(FileText,FileNamePath);
            for (int i = 0; i < FileLines.Count; i++)
			{
				string[] LinesWords = FileLines[i].Split(' ');

                if (LinesWords[0] == "page")
				{
                    CurrentObject = new page(LinesWords[1], CurrenProject, HTML_Path, CSS_Path);
					Started = true;
				}
				else if (LinesWords[0] == "object")
				{
                    if(LinesWords[1].Split(':').Length > 1){
                        string[] LocalWords = LinesWords[1].Split(':');
                        CurrentObject = new WebObject(LocalWords[0], CurrenProject, HTML_Path, CSS_Path);
                        foreach(WebObject LocalObject in CurrenProject.ObjectList){
                            if(LocalObject.GetName() == LocalWords[1]){
                                CurrentObject.CSS.SetAllProp(LocalObject,LocalObject.CSS.GetAllProp());
                            }
                        }
                    }else{
                        CurrentObject = new WebObject(LinesWords[1], CurrenProject, HTML_Path, CSS_Path);
                    }
					Started = true;
				}
				else if (Started && LinesWords[0] == "end")
				{
					CurrentObject.CompileObject();
                    CurrenProject.ObjectList.Add(CurrentObject);
					Started = false;
				}
				else if (Started)
				{
					CurrentObject.AddLine(FileLines[i]);
				}
			}
            CurrenProject.DisplayCompileTime();
            return "ExitCode = 0";
        }
        public void VerifyFolder(string Path){
            if(!System.IO.Directory.Exists(Path)){
                System.IO.Directory.CreateDirectory(Path);
            }
        }

        public void AddFileLines(string[] FileText, string Path){
			for (int i = 0; i < FileText.Length; i++)
			{
				string[] LinesWords = FileText[i].Split(' ');
				if (LinesWords[0] == "include")
				{
                    string[] DataToInclude = System.IO.File.ReadAllLines(Path + '/' + LinesWords[1]);
                    AddFileLines(DataToInclude, Path);
                }else if(LinesWords[0] == "include_all"){
                    List<string> BrutDataToInclude = new List<string>();
                    List<string> FilesName = SyntaxTools.GetFunctionDatas(LinesWords[1]);
                    for (int j = 1; j < FilesName.Count;j++){
                        string[] CurrentFilesPath = System.IO.Directory.GetFiles(Path + '/' + FilesName[j]);
                        for (int k = 0; k < CurrentFilesPath.Length;k++){
                            string[] CurrentLines = System.IO.File.ReadAllLines(CurrentFilesPath[k]);
                            for (int l = 0; l < CurrentLines.Length;l++){
                                BrutDataToInclude.Add(CurrentLines[l]);
                            }
                        }
                    }
                    string[] DataToInclude = BrutDataToInclude.ToArray();
                    AddFileLines(DataToInclude, Path);
                }
				else
				{
					FileLines.Add(FileText[i]);
				}
			}
        }

    }
}
