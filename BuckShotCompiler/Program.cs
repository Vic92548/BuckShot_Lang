using System;
using System.IO;
using System.Collections.Generic;
namespace BuckShotCompiler
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            CheckCommand("", "", new List<string>(), 0);
        }

        public static void CompileAFile(string File, List<string> CommandHistory, int HistoryNav){
            List<string> ExitFile = new List<string>();
            Compiler LocalCompiler = new Compiler(File);
            LocalCompiler.GlobalCompile();
			Console.WriteLine("Compile finished");
            CheckCommand("",File, CommandHistory, HistoryNav);
        }

        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void CheckCommand(string Command,string File, List<string> CommandHistory, int HistoryNav){
            string Line = "";
            if(Command == ""){
                Console.WriteLine("Waiting for user input");
            }else{
                Line = Command;
                foreach(char CurrentChar in Command){
                    Console.Write(CurrentChar);
                }
            }
            ConsoleKeyInfo CurrentKey = Console.ReadKey();
            while(CurrentKey.Key != ConsoleKey.Enter){
                if(CurrentKey.Key == ConsoleKey.UpArrow){
                    if(CommandHistory.Count > HistoryNav){
                        HistoryNav++;
                        ClearCurrentConsoleLine();
                        CheckCommand(CommandHistory[CommandHistory.Count - HistoryNav],File,CommandHistory,HistoryNav);
                        break;
                    }else{
                        ClearCurrentConsoleLine();
                        CheckCommand(CommandHistory[CommandHistory.Count - HistoryNav], File, CommandHistory, HistoryNav);
                        break;
                    }
                }else if (CurrentKey.Key == ConsoleKey.DownArrow)
                {
                    if (HistoryNav > 0)
                    {
                        HistoryNav--;
                        ClearCurrentConsoleLine();
                        CheckCommand(CommandHistory[CommandHistory.Count - HistoryNav], File, CommandHistory, HistoryNav);
                        break;
                    }
                    else
                    {
                        ClearCurrentConsoleLine();
                        CheckCommand(CommandHistory[CommandHistory.Count - HistoryNav], File, CommandHistory, HistoryNav);
                        break;
                    }
                }else{
                    Line += CurrentKey.KeyChar;
                }
                CurrentKey = Console.ReadKey();
            }
            string[] Words = Line.Split(' ');
            HistoryNav = 0;
            if(Words[0] == "c"){
                if(Words.Length == 1 && File != ""){
                    CommandHistory.Add(Line);
                    CompileAFile(File, CommandHistory, HistoryNav);
                }else if(Words.Length > 1){
                    File = Words[1];
                    CommandHistory.Add(Line);
                    CompileAFile(File, CommandHistory, HistoryNav);
                }else{
                    Console.WriteLine("Fichier invalide");
                    CheckCommand("",File, CommandHistory, HistoryNav);
                }
            }else if(Words[0] == "lines"){
                string FilesLoc = "../..";
                int Lines = 0;
                string[] Files = Directory.GetFiles(FilesLoc);
                foreach(string LocalFile in Files){
                    Console.WriteLine(LocalFile);
                    if(LocalFile.Split('.').Length > 1){
                        if(LocalFile.Split('.')[LocalFile.Split('.').Length - 1] == "cs"){
                            Lines += System.IO.File.ReadAllLines(LocalFile).Length;
                            Console.WriteLine(LocalFile);
                        }
                    }
                }
                Console.WriteLine("Lignes = " + Lines);
                CommandHistory.Add(Line);
                CheckCommand("",File, CommandHistory, HistoryNav);
            }else{
                CheckCommand("",File, CommandHistory, HistoryNav);
            }
        }
    }
}
