// class is taken from cherry's recording on 4/25/2024

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Logger
{
    // call with Logger.Log("<message>");
    public static void Log(string message, [CallerFilePath] string file ="", [CallerLineNumber] int line =0, [CallerMemberName] string methodName ="") {
        File.WriteAllText("log.txt", $"file:{file}\nline:{line}\nmethod: {methodName}\nmessage: {message}");
    }

    public static void Clear() {

    }
}
