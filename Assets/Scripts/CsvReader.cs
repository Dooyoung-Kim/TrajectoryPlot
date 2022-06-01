using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CsvReader : MonoBehaviour
{
    void Start()
    {
        ReadCSVFile();
    }

    void ReadCSVFile()
    {
        StreamReader strReader = new StreamReader("D:/Github/TrajectoryPlot/logData/PG1_log_1_AR_1110_122435.csv");
        bool endofFile = false;
        while(!endofFile)
        {
            string data_string = strReader.ReadLine();
            if(data_string == null)
            {
                endofFile = true;
                break;
            }

            var data_values = data_string.Split(',');
            for (int i = 0; i < data_values.Length; i++)
            {
                Debug.Log($"Values {i.ToString()}: {data_values[i].ToString()}");
            }
        }
    }
}