using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

[InitializeOnLoad]
public class RemoveAutomaticallyEmptyDirectoriesOnSave : UnityEditor.AssetModificationProcessor
{
    private static string[] OnWillSaveAssets(string[] paths)
    {
        DeleteEmptyDirectories(new DirectoryInfo(Application.dataPath));
        return paths;
    }

    private static void DeleteEmptyDirectories(DirectoryInfo directory)
    {
        DirectoryInfo[] subDirectories = directory.GetDirectories();

        for (int i = subDirectories.Length-1; i >-1; i--)
            DeleteEmptyDirectories(subDirectories[i]);

        if (directory.GetDirectories().Length == 0 && DirectoryHasNoFile(directory))
            DeleteDirectory(directory);
    }

    private static void DeleteDirectory(DirectoryInfo directory)
    {
        AssetDatabase.MoveAssetToTrash(GetRelativePathFromCd(directory.FullName));
    }

    private static bool DirectoryHasNoFile(DirectoryInfo directory)
    {
        try
        {
            FileInfo noMetaFile = new List<FileInfo>(directory.GetFiles("*.*")).Find((FileInfo file) =>
            {
                return !IsMetaFile(file.Name);
            });

            return noMetaFile == null;
        } 
        catch (Exception)
        {
            Debug.Log("Exception on method DirectoryHasNoFile for folder \"" + directory.FullName + "\"");
            return false;
        } 
    }

    private static string GetRelativePathFromCd(string filespec)
    {
        return GetRelativePath( filespec, Directory.GetCurrentDirectory() );
    }

    private static string GetRelativePath(string filespec, string folder)
    {
        Uri pathUri = new Uri(filespec);

        if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            folder += Path.DirectorySeparatorChar;
        }

        Uri folderUri = new Uri(folder);
        return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
    }

    private static bool IsMetaFile(string path)
    {
        return path.EndsWith(".meta");
    }
}