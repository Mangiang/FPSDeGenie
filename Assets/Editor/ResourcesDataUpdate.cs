using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;

public static class ResourcesDataUpdate
{
    class ResourceHolder
    {
        public List<string> _pathList = new List<string>();
        public string _extension = "";
        public string _name = "";

        public ResourceHolder(string name, string extension)
        {
            _extension = extension;
            _name = name;
        }
    }

    static List<ResourceHolder> resourceHolders = new List<ResourceHolder>();
    static string filePath = "./Assets/Scripts/Global/ResourcesData.cs";

    [MenuItem("Assets/ResourcesDataUpdate", false, 0)]
    static void Init()
    {
        resourceHolders.Clear();
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/");
        DirectoryInfo[] tmp = dir.GetDirectories();
        getChildren(tmp);
        writeUIData();
        AssetDatabase.Refresh();
    }

    static void getChildren(DirectoryInfo[] dirs)
    {
        if (dirs.Length == 0)
            return;

        for (int i = 0; i < dirs.Length; ++i)
        {
            DirectoryInfo dir = new DirectoryInfo(dirs[i].FullName);
            DirectoryInfo[] tmp = dir.GetDirectories();
            string lowerDirsName = dirs[i].Name.ToLower();

            FileInfo[] files = dirs[i].GetFiles();

            if (files.Length > 0 && !isOnlyMeta(files))
            {
                ResourceHolder newResrouceHolder = new ResourceHolder(lowerDirsName, files[0].Extension);

                foreach (FileInfo fi in files)
                {
                    if (!fi.Name.EndsWith(".meta"))
                    {
                        int startpos = fi.FullName.LastIndexOf("\\Resources\\") + "\\Resources\\".Length;
                        newResrouceHolder._pathList.Add(fi.FullName.Substring(startpos).Replace("\\", "/").Replace(newResrouceHolder._extension, ""));
                    }
                }
                resourceHolders.Add(newResrouceHolder);
            }

            getChildren(tmp);
        }
    }

    static bool isOnlyMeta(FileInfo[] files)
    {
        foreach (FileInfo fi in files)
        {
            if (!fi.Extension.EndsWith("meta"))
                return false;
        }

        return true;
    }

    static void writeUIData()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        using (StreamWriter stream = new StreamWriter(filePath, false))
        {
            try
            {
                addHead(stream);

                foreach (var resourceHolder in resourceHolders)
                {
                    stream.Write("        _" + resourceHolder._name + " = new string[]{\n");
                    stream.Write("                                  \"" + resourceHolder._pathList[0] + "\"");

                    for (int i = 1; i < resourceHolder._pathList.Count; ++i)
                    {
                        stream.Write(",\n                                  \"" + resourceHolder._pathList[i] + "\"");
                    }

                    stream.Write("};\n\n");
                }

                stream.WriteLine("    }");
                AddGetObject(stream);
            }
            catch (Exception e)
            {
                Debug.LogError("Warning : \n" + e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                stream.WriteLine("    }");
                stream.WriteLine("}");
            }
        }
    }

    static void addHead(StreamWriter stream)
    {
        stream.WriteLine("using UnityEngine;");
        stream.WriteLine("using System.Collections.Generic;");
        stream.WriteLine("");
        stream.WriteLine("");
        AddEnum(stream, resourceHolders);
        stream.WriteLine("\n");
        stream.WriteLine("public static class ResourcesData");
        stream.WriteLine("{");
        foreach (var resourceHolder in resourceHolders)
        {
            stream.WriteLine("    public static string[] _" + resourceHolder._name + ";");
        }
        stream.WriteLine("\n");

        stream.WriteLine("    public static void Init()");
        stream.WriteLine("    {");
    }

    static void AddEnum(StreamWriter stream, List<ResourceHolder> list)
    {
        foreach (var resourceHolder in list)
        {
            stream.WriteLine("    public enum " + FirstCharToUpper(resourceHolder._name)+"Enum" + "{");
            for (int iElt = 0; iElt < resourceHolder._pathList.Count - 1; ++iElt)
            {
                stream.WriteLine("        " + ExtractName(resourceHolder._pathList[iElt]) + ",");
            }
            stream.WriteLine("        " + ExtractName(resourceHolder._pathList[resourceHolder._pathList.Count - 1]) + ",");
            stream.WriteLine("    };");
        }
    }

    static void AddGetObject(StreamWriter stream)
    {
        stream.WriteLine("    public static Object GetObject<T>(string[] arr, T enumValue)");
        stream.WriteLine("    {");
        stream.WriteLine("        string resourceName = arr[System.Convert.ToInt32(enumValue)];");
        stream.WriteLine("        return Resources.Load(resourceName);");
}

    static string FirstCharToUpper(string input)
    {

        if (String.IsNullOrEmpty(input))
            return "";
        else
        {
            char[] tmp = input.ToCharArray();
            tmp[0] = char.ToUpper(tmp[0]);
            return new string(tmp);
        }
    }

    static string ExtractName(string name)
    {
        string[] nameArr = name.Split('/');
        string enumName = nameArr[nameArr.Length - 1];
        if (enumName.Contains("."))
        {
            enumName = enumName.Split('.')[0];
        }

        enumName = enumName.Replace(" ", "_");
        enumName = enumName.Replace("-", "_");

        enumName = enumName.ToUpper();

        if (!char.IsLetter(enumName[0]))
        {
            enumName = "A" + enumName;
        }

        return enumName;
    }

    static string FirstCharToLower(string input)
    {

        if (String.IsNullOrEmpty(input))
            return "";
        else
        {
            char[] tmp = input.ToCharArray();
            tmp[0] = char.ToLower(tmp[0]);
            return new string(tmp);
        }
    }
}


