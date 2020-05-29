using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace Easy_Backup {
    class EasyBackup {
        static void Main(string[] args) {
            char keyPressed = 'A';
            string destination;
            string[] lines;
            string[] sources;

            string currDirectory = Directory.GetCurrentDirectory();
            string fileDir = @currDirectory;
            string fileName = "fileSets.txt";
            string file = System.IO.Path.Combine(fileDir, fileName);

            Console.WriteLine("----------------------");
            Console.WriteLine("Welcome to Easy Backup");
            Console.WriteLine("----------------------");
            Console.WriteLine();

            while (keyPressed != 'Q' && keyPressed != 'q') {
                if (System.IO.File.Exists(file)) {
                    lines = System.IO.File.ReadAllLines(@file);

                    for (int i = 0; i < lines.Length; i++) {
                        if (lines[i] == "DESTINATION:")
                            break;

                        Console.WriteLine((i + 1) + " " + lines[i]);
                    }

                } else {
                    System.IO.File.Create(file);
                    lines = System.IO.File.ReadAllLines(@file);

                    for (int i = 0; i < lines.Length; i++) {
                        Console.WriteLine((i + 1) + " " + lines[i]);
                    }
                }

                Console.WriteLine("\nDestination Folder:");
                Console.WriteLine(lines[lines.Length - 1]);

                Console.WriteLine();
                Console.WriteLine("To change the destination, press [D]. To remove a source file, press [R]. To add a source file, press [A]. To Quit, press [Q]");
                Console.WriteLine("To begin the backup, press any key. This may take SEVERAL minutes.");
                keyPressed = Console.ReadKey().KeyChar;
                
                switch (keyPressed) {
                    case 'a':
                    case 'A':
                        Console.WriteLine("case");
                        break;

                    default:
                        Console.WriteLine();
                        destination = lines[lines.Length - 1];
                        sources = getSources(lines);
                        EasyBackup.backup(sources, destination, 0);
                        Console.WriteLine("Backup Complete");
                        Console.WriteLine();
                        break;
                }
            }
        }

        private static void backup(string[] sources, string destination, int indent) {
            string source = null;
            string fileName;
            string newFile;
            string newFolder;
            int percentComplete;
            int lastNumTick = 0;
            int tickDifference;
            string[] files;
            string[] subDirs;

            // Loop through all the sources and backup
            for (int i = 0; i < sources.Length; i++) {
                source = sources[i];                                          // Get the current source
                files = System.IO.Directory.GetFiles(source);                 // Get the files from that source
                subDirs = System.IO.Directory.GetDirectories(source);         // Get the sub directories from that source
                newFolder = source.Substring(source.LastIndexOf('\\'));       // Get the name of the source
                System.IO.Directory.CreateDirectory(destination + newFolder); // Creat the directory that the source will be copied to

                // Write an indent for sub directories
                for (int j = 0; j < indent * 3; j++) {
                    Console.Write(" ");
                }

                Console.Write("Copying " + newFolder + "... [");

                // Loop through all the files in the source and copy each file
                for (int j = 0; j < files.Length; j++) {
                    fileName = System.IO.Path.GetFileName(files[j]);                     // Get the file name
                    newFile = System.IO.Path.Combine(destination + newFolder, fileName); // Create a new file path
                    System.IO.File.Copy(files[j], newFile, true);                        // Copy the file

                    // Calculate the percentage complete (divide by 5 becasue each tick will represent 5%)
                    percentComplete = 100 * (j + 1) / (files.Length * 5);
                    // Calculate the difference between the last loop ticks and the current loo ticks
                    tickDifference = percentComplete - lastNumTick;

                    // loop will add ticks to compensate for the difference
                    for (int k = tickDifference; k > 0; k--) {
                        Console.Write("-");
                        lastNumTick++; // Keep track of how many ticks there are 
                    }
                }

                // if there are no files in the directory, then the previous loop will not print any ticks. if statement will fill in the gap
                if (files.Length == 0) {
                    Console.Write("--------------------");
                }

                // reset tick num and end that loading bar
                lastNumTick = 0;
                Console.WriteLine("] -> 100%");

                // if there are sub directories, backup sub directories
                if (subDirs.Length > 0) {
                    backup(subDirs, destination + newFolder, indent + 1);
                }
            }
        }

        private static string[] getSources(string[] lines) {
            int i = 0;
            string currline = lines[i];

            while (currline != "DESTINATION:") {
                currline = lines[++i];
            }

            string[] sources = new string[i];

            for (int j = 0; j < i; j++) {
                sources[j] = lines[j];
            }

            return sources;
        }
    }
}
