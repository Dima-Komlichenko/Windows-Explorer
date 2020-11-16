using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Explorer
{
    class Windows_Explorer
    {
        DirectoryInfo dir;
        FileSystemInfo[] allFiles;

        FileSystemInfo buffer;
        int copy = 0;
        public void App()
        {
            Start();
            FillAllFiles();
            Catalog();
        }

        public void Start() 
        {
            Console.Clear();
            string[] disk = new string[2];
            disk[0] = "C:";
            disk[1] = "D:";
            int x = Menu.VerticalMenu(disk);
            if (x == 0)
                dir = new DirectoryInfo("C:/");
            else if (x == 1)
                dir = new DirectoryInfo("D:/");
            else if (x == -1)
                Start();
        }

        void Normalize()
        {
            for (int j = 0; j < allFiles.Length; j++)
            {
                for (int i = 0; i < allFiles.Length - 1 - j; i++)
                {
                    if (allFiles[i] is FileInfo && allFiles[i + 1] is DirectoryInfo)
                        Swap(ref allFiles[i], ref allFiles[i + 1]);
                }
            }
        }
        void FillAllFiles()
        {
            try
            {
                allFiles = dir.GetFileSystemInfos();
                Normalize();

            }
            catch
            {
                FileOpen();
                Back();
            }


        }

        public void Catalog()
        {
            if (allFiles.GetLength(0) == 0)
            {
                Console.WriteLine(" *Clear*");
            }
            int numDirs = dir.GetDirectories().Length;
            int numFiles = dir.GetFiles().Length;

            int pos = 0;
            ACTION res = Menu.VerticalMenu(allFiles, dir.FullName, ref pos);

            switch (res)
            {
                case ACTION.TEXT_UP:
                case ACTION.TEXT_DOWN:
                case ACTION.DATE_UP:
                case ACTION.DATE_DOWN:
                case ACTION.TYPE_UP:
                case ACTION.TYPE_DOWN:
                case ACTION.SIZE_UP:
                case ACTION.SIZE_DOWN:
                    Sort(allFiles, numDirs, numFiles, res);
                    break;

                case ACTION.BACKSPASE:
                    Back();
                    FillAllFiles();
                    break;

                case ACTION.ADD:
                    FileAdd();
                    dir.Refresh();
                    FillAllFiles();
                    break;

                case ACTION.DEL:
                    FileDel(dir.FullName + '\\' + allFiles[pos].Name);
                    FillAllFiles();
                    break;

                case ACTION.COPY:
                    Copy(pos, allFiles);
                    copy = 1;
                    break;

                case ACTION.CUT:
                    Cut(pos, allFiles, dir.FullName);
                    copy = 2;
                    break;

                case ACTION.PASTE:
                    if (copy == 1)
                        PasteCopy(buffer.Name, buffer.FullName, dir.FullName.ToString());
                    else if (copy == 2)
                        PasteCut(buffer);
                    FillAllFiles();
                    copy = 0;
                    break;

                default:
                    dir = new DirectoryInfo(dir.FullName + '\\' + allFiles[pos].Name);
                    FillAllFiles();
                    break;
            }
            Catalog();
        }
        public void PasteCut(FileSystemInfo buf)
        {
            try
            {
                ((DirectoryInfo)buffer).MoveTo(dir.FullName + "\\" + buffer.Name);
            }
            catch
            {
                ((FileInfo)buffer).MoveTo(dir.FullName + "\\" + buffer.Name);
            }
        }

        void PasteCopy(string dir, string FromDir, string ToDir)
        {
            Directory.CreateDirectory(FromDir);
            PasteCopyRec(FromDir, ToDir + "\\" + Path.GetFileName(dir));
        }

        void PasteCopyRec(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                PasteCopyRec(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }

        public void Cut(int pos, FileSystemInfo[] elements, string address)
        {
            FileInfo[] files = dir.GetFiles();
            DirectoryInfo[] files2 = dir.GetDirectories();

            for (int i = 0; i < files.Length; i++)
            {
                if (elements[pos].Name == files[i].Name)
                {
                    buffer = files[i];
                }
            }

            for (int i = 0; i < files2.Length; i++)
            {
                if (elements[pos].Name == files2[i].Name)
                {
                    buffer = files2[i];
                }
            }

        }
        public void Copy(int pos, FileSystemInfo[] elements)
        {
            FileInfo[] files = dir.GetFiles();
            DirectoryInfo[] files2 = dir.GetDirectories();


            for (int i = 0; i < files.Length; i++)
            {
                if (elements[pos].Name == files[i].Name)
                {
                    buffer = files[i];
                }
            }

            for (int i = 0; i < files2.Length; i++)
            {
                if (elements[pos].Name == files2[i].Name)
                {
                    buffer = files2[i];

                }
            }

        }

        void FileOpen()
        {
            try
            {
                Process.Start(dir.FullName);
            }
            catch
            {
                Console.WriteLine("Невозможно открыть файл");
                Console.ReadKey();
            }
        }
        void FileAdd()
        {
            string[] s = { "Директория", "Файл" };
            string name = "";
            int num = Menu.MessageBox("Что вы хотите создать, директорию или файл?", s, ref name);

            switch (num)
            {
                case 0:
                    Directory.CreateDirectory(dir.FullName + '\\' + name);
                    break;
                case 1:
                    File.Create(dir.FullName + '\\' + name);
                    break;
            }
        }

        void FileDel(string address)
        {
            try
            {
                Directory.Delete(address, true);
            }
            catch
            {
                File.Delete(address);
            }
        }

        void Swap(ref FileSystemInfo s1, ref FileSystemInfo s2)
        {
            FileSystemInfo s3 = s1;
            s1 = s2;
            s2 = s3;
        }

        void SortArr(FileSystemInfo[] str, int first, int last, ACTION res)
        {
            for (int j = first; j < last; j++)
            {
                for (int i = first; i < last - 1; i++)
                {
                    int comp;

                    if (res == ACTION.TEXT_UP || res == ACTION.TEXT_DOWN)
                        comp = String.Compare(str[i].Name, str[i + 1].Name);


                    else if (res == ACTION.DATE_UP || res == ACTION.DATE_DOWN)
                        comp = DateTime.Compare(str[i].LastWriteTime, str[i + 1].LastWriteTime);


                    else if (res == ACTION.TYPE_UP || res == ACTION.TYPE_DOWN)
                        if (str[i] is DirectoryInfo)
                            comp = String.Compare(str[i].Extension, str[i + 1].Extension);
                        else return;


                    else // if (obj == SORT_OBJECT.SIZE)
                        if (str[i] is FileInfo)
                        comp = ((FileInfo)str[i]).Length > ((FileInfo)str[i + 1]).Length ? 1 : -1;
                    else return;


                    if ((res.ToString().Contains("UP") && comp != 1) || (res.ToString().Contains("DOWN") && comp == 1))
                        Swap(ref str[i], ref str[i + 1]);
                }
            }
        }

        public void Sort(FileSystemInfo[] str, int dirs, int files, ACTION res)
        {
            SortArr(str, 0, dirs, res);
            SortArr(str, dirs, dirs + files, res);
            Normalize();
        }

        public void Back()
        {
            string address = dir.FullName;
            int sizeNewAddress = dir.FullName.Length - dir.Name.ToString().Length - 1;

            if (sizeNewAddress > 0)
            {
                address = address.Remove(sizeNewAddress);
                dir = new DirectoryInfo(address + '\\');
            }
            else
                Start();
        }

    }
}
