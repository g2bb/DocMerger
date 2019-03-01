using System;
using System.IO;
using System.Collections;

namespace DocMerger {
    class Program {
        static void Main() {
            Console.WriteLine("===============\nDocument Merger\n===============");
            bool running = true;
            while (running) {
                ArrayList fileNames = RequestFiles();
                string nameMerged = MergeNames(fileNames);
                try {
                    int charCount = MergeFiles(fileNames, nameMerged);
                    Console.WriteLine("Document successfully saved as: {0} and contains {1} characters", nameMerged, charCount);
                } catch (Exception error) {
                    Console.WriteLine(error.Message);
                }
                Console.WriteLine("Would you like to merge more files? (y/n)");
                running = Console.ReadLine().ToLower().Equals("y");
            }
        }

        static ArrayList RequestFiles() {
            ArrayList titles = new ArrayList();
            Console.WriteLine();
            do {
                Console.WriteLine("Enter the name of file #{0} (or leave blank if you are done):", titles.Count + 1);
                string fileName = Console.ReadLine();
                if (string.IsNullOrEmpty(fileName)) {
                    return titles;
                } else if (File.Exists(fileName)) {
                    titles.Add(fileName);
                } else {
                    Console.WriteLine("Provided document either does not exist or can not be found.\nPlease check your spelling and try again.");
                }
            } while (true);
        }

        static string MergeNames(ArrayList titles) {
            string defaultTitle = MergeList(titles);
            Console.WriteLine("Enter a new name (default {0}):", defaultTitle);
            string NewTitle = Console.ReadLine();
            if (NewTitle.Length.Equals(0)) {
                return defaultTitle;
            } else {
                return NewTitle.EndsWith(".txt") ? NewTitle : NewTitle + ".txt";
            }
        }

        static string MergeList(ArrayList titles) {
            string AutoMergedName = "";
            foreach (string title in titles) {
                AutoMergedName += title.Replace(".txt", "");
            }
            return AutoMergedName + ".txt";
        }

        static int MergeFiles(ArrayList titles, string saveName) {
            StreamWriter sw = null;
            int charCount = 0;
            try {
                sw = new StreamWriter(saveName);
                foreach (string title in titles) {
                    charCount += WriteLines(ReadText(title), sw);
                }
            } finally {
                if (sw != null) {
                    sw.Close();
                }
            }
            return charCount;
        }

        static ArrayList ReadText(string fileName) {
            ArrayList lines = new ArrayList();
            StreamReader sr = null;
            try {
                sr = new StreamReader(fileName);
                string line = sr.ReadLine();
                while (line != null) {
                    lines.Add(line);
                    line = sr.ReadLine();
                }
                return lines;
            } finally {
                if (sr != null) {
                    sr.Close();
                }
            }
        }

        static int WriteLines(ArrayList lines, StreamWriter sw) {
            int charCount = 0;
            foreach (string line in lines) {
                charCount += line.Length;
                sw.WriteLine(line);
            }
            return charCount;
        }
    }
}
