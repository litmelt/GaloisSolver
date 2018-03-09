using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Collections;
using System.Xml;

namespace ClassLibrary1
{
    public static class DataBase
    {
   
            static BinaryFormatter formatter; //Для сериализации
            static Stream stream;
            public static void DoSerialization(string path, ref int[,] Spisok)
            {
                stream = File.Create(path);
                formatter = new BinaryFormatter();
                formatter.Serialize(stream, Spisok);
                stream.Close();
            }
            public static void ReadSerialization(string path, out int[,] Spisok)
            {
                stream = File.OpenRead(path);
                formatter = new BinaryFormatter();
                Spisok = (int[,])formatter.Deserialize(stream);
                stream.Close();
            }
           
        
    }
}
