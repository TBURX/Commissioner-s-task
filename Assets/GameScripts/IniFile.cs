using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

public class IniFile
{
    public string path;

    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
        int size, string filePath);

    public IniFile(string INIPath)
    {
        path = INIPath;
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            DirectoryInfo direct = new DirectoryInfo(Path.GetDirectoryName(path));
            direct.Create();
        }

    }

    public void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, this.path);
    }

    public string IniReadValue(string Section, string Key, string Default)
    {
        StringBuilder temp = new StringBuilder(255);
        GetPrivateProfileString(Section, Key, Default, temp, 255, this.path);
        return temp.ToString();
    }
}

