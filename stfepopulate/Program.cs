using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace STFEpopulate
{
    public class Program
    {
        private static void printList(string title, List<string> items)
        {
            Console.WriteLine("\n\n"+title);
            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }


        private static string Captialise(string text, bool allWords = true)
        {
            var words = text.Split(' ');
            List<string> cleanArray = new List<string>();
            for (int i = 0; i < words.Length; i++)
            {
                if ((allWords == true) || (i == 0))
                {
                    cleanArray.Add(char.ToUpper(words[i][0]) + words[i].Substring(1));
                }
                else
                {
                    cleanArray.Add(words[i]);
                }
            }
            return String.Join(" ", cleanArray);
        }


        private static string getArtwork(string path, bool vids = false)
        {
            string[] folders = { "gps", "hgps", "attract" };
            long fileSize = 0;
            string largestFile = "";
            foreach (var folder in folders)
            {
                string currentDir = Path.Combine(path, folder);
                if (Directory.Exists(currentDir))
                {
                    if (vids)
                    {
                        var artPaths = Directory.GetFiles(currentDir)
                            .Where(file => file.ToLower().EndsWith("swf")
                            || file.ToLower().EndsWith("avi"));
                        foreach (var item in artPaths)
                        {
                            FileInfo fi1 = new FileInfo(item);
                            if (fi1.Length > fileSize)
                            {
                                fileSize = fi1.Length;
                                largestFile = fi1.FullName;
                            }
                        }
                    }
                    if (!vids || fileSize == 0)
                    {
                        var artPaths = Directory.GetFiles(currentDir)
                            .Where(file => file.ToLower().EndsWith("bmp")
                            || file.ToLower().EndsWith("jpg")
                            || file.ToLower().EndsWith("png")
                            || file.ToLower().EndsWith("jpeg"));
                        foreach (var item in artPaths)
                        {
                            FileInfo fi1 = new FileInfo(item);
                            if (fi1.Length > fileSize)
                            {
                                fileSize = fi1.Length;
                                largestFile = fi1.FullName;
                            }
                        }
                    }
                }
            }
            return largestFile;
        }

        [STAThread]
        static void Main(string[] args)
        {
            var appFolder = Application.StartupPath;
            var tempFolder = Path.Combine(appFolder, "Config");
            if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, recursive: true);
            int counter = 1;
            bool attractMode = false;
            MessageBox.Show("Select the root directory where your games are stored");
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            List<string> categories = new List<string>();
            List<string> emuCleanup = new List<string>();
            List<string> missingArtList = new List<string>();
            List<string> skippedList = new List<string>();
            List<string> successList = new List<string>();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Directory.CreateDirectory(tempFolder);
                DialogResult useVids = MessageBox.Show("Do you want to use videos for your icons?\nStatic pictures will be set where no videos are available.", "Use video attract mode", MessageBoxButtons.YesNo);
                if (useVids == DialogResult.Yes) attractMode = true;
                DialogResult allGames = MessageBox.Show("Do you want to add an \"All Games\" menu?", "Optional ALL GAMES Menu", MessageBoxButtons.YesNo);
                if (allGames == DialogResult.Yes)
                {
                    counter = 2;
                    Directory.CreateDirectory(Path.Combine(tempFolder, "Config01"));
                }
                var rootPath = fbd.SelectedPath;
                bool useCaps = false;
                DialogResult caps = MessageBox.Show("Do you want the first letter of each word in the game names to be uppercase?", "Capitalise First Letters", MessageBoxButtons.YesNo);
                if (caps == DialogResult.Yes) useCaps = true;

                foreach (var path in Directory.GetDirectories(rootPath))
                {
                    string dirName = System.IO.Path.GetFileName(path);
                    string confNum = counter.ToString();
                    if (confNum.Length == 1) confNum = "0" + confNum;
                    var configFolder = Path.Combine(tempFolder, "Config" + confNum);
                    Directory.CreateDirectory(configFolder);
                    if (dirName.StartsWith("- "))
                    {
                        categories.Add(dirName.Substring(2));
                        foreach (var emuPath in Directory.GetDirectories(path))
                        {
                            // Loop through the emulator folders within genre folder
                            string emuName = System.IO.Path.GetFileName(emuPath);
                            foreach (var gamePath in Directory.GetDirectories(emuPath))
                            {
                                string gameName = System.IO.Path.GetFileName(gamePath);

                                // Get all lnk and exe files then concatenate the lists together
                                var lnkFiles = Directory.GetFiles(gamePath, "*.lnk");
                                var exeFiles = Directory.GetFiles(gamePath, "*.exe");
                                var result = lnkFiles.Concat(exeFiles);
                                List<string> resultList = result.ToList();
                                var link = "";
                                foreach (var f in resultList)
                                {
                                    var lnkName = System.IO.Path.GetFileNameWithoutExtension(f);
                                    var ext = System.IO.Path.GetExtension(f);
                                    if (lnkName == emuName) emuCleanup.Add(System.IO.Path.GetFullPath(f)); // emulator found in game dir
                                    if (lnkName == gameName) link = System.IO.Path.GetFullPath(f);
                                    if ((ext == "lnk") && (lnkName.ToLower().StartsWith("shortcut to"))) link = System.IO.Path.GetFullPath(f);
                                    if ((link == "") && (lnkName == gameName)) link = System.IO.Path.GetFullPath(f);
                                }
                                if (link == "")
                                {
                                    OpenFileDialog customFile = new OpenFileDialog();
                                    customFile.InitialDirectory = gamePath;
                                    customFile.Title = "Select executable file for " + gameName;
                                    if (customFile.ShowDialog() == DialogResult.OK) link = customFile.FileName;
                                    else skippedList.Add(gameName);
                                }
                                if (link != "")
                                {
                                    string paramaters = "";
                                    string cleanGameName = Captialise(gameName,useCaps);
                                    string img = getArtwork(gamePath, attractMode);
                                    if (img == "") missingArtList.Add(link);
                                    var preGame = Path.Combine(rootPath, emuName);
                                    var iniFile = Path.Combine(tempFolder, "Config" + confNum, cleanGameName + ".ini");
                                    string[] details = {
                                        "[GAME]",
                                        "DisplayName="+cleanGameName,
                                        "Program="+link,
                                        "Parameters="+paramaters,
                                        "PreGameProgram="+preGame+".exe",
                                        "HidePreProgram=1",
                                        "Image="+img,
                                        "Wait=1",
                                        "Launch2=0",
                                        "UseKillFunction=0",
                                        "UseStartFunction=0"
                                    };
                                    successList.Add(gameName);
                                    System.IO.File.WriteAllLines(iniFile, details);
                                    if (allGames == DialogResult.Yes)
                                    {
                                        var conf01 = Path.Combine(tempFolder, "Config01", gameName + ".ini");
                                        System.IO.File.WriteAllLines(conf01, details);
                                    }
                                }
                            }
                        }
                        counter++;
                    }
                    if (dirName.Contains("shortcuts"))
                    {
                        var lnkFiles = Directory.GetFiles(path, "*.lnk");
                        foreach (var f in lnkFiles)
                        {
                            var gameName = System.IO.Path.GetFileNameWithoutExtension(f);

                            var ext = System.IO.Path.GetExtension(f);
                            var link = System.IO.Path.GetFullPath(f);
                            var img = link.Replace(".lnk", ".jpg");
                            string cleanGameName = gameName;
                            if (gameName.ToLower().StartsWith("shortcut to")) cleanGameName = gameName.Substring(12);
                            cleanGameName = Captialise(cleanGameName, useCaps);
                            var iniFile = Path.Combine(tempFolder, "Config" + confNum, cleanGameName + ".ini");
                            string[] details = {
                                        "[GAME]",
                                        "DisplayName="+cleanGameName,
                                        "Program="+link,
                                        "Parameters=",
                                        "PreGameProgram=",
                                        "HidePreProgram=1",
                                        "Image="+img,
                                        "Wait=1",
                                        "Launch2=0",
                                        "UseKillFunction=0",
                                        "UseStartFunction=0"
                                    };
                            successList.Add(cleanGameName);
                            System.IO.File.WriteAllLines(iniFile, details);
                            if (allGames == DialogResult.Yes)
                            {
                                var conf01 = Path.Combine(tempFolder, "Config01", cleanGameName + ".ini");
                                System.IO.File.WriteAllLines(conf01, details);
                            }
                        }
                        counter++;
                    }
                }
            }
            if (emuCleanup.Count > 0)
            {
                var message = "The following emulators have been found, would you like to remove them ?\n\n";
                message += string.Join(Environment.NewLine, emuCleanup);
                DialogResult dialogResult = MessageBox.Show(message, "Remove Emulators", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    foreach (var path in emuCleanup)
                    {
                        File.Delete(path);
                    }
                }
            }
            if (successList.Count > 0) printList("Successfully created configs for these games:", successList);
            if (skippedList.Count > 0) printList("Skipped configs for these games:", skippedList);
            if (missingArtList.Count > 0) printList("No artwork/videos found for these games:", missingArtList);
            if ((successList.Count == 0) && (skippedList.Count == 0) && (missingArtList.Count == 0))
                Console.WriteLine("No games found");
            Console.WriteLine("Press ENTER to quit");
            Console.ReadLine();
        }
    }
}