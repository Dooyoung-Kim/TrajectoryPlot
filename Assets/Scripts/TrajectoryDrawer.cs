using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TrajectoryDrawer : MonoBehaviour
{
    class TrackerNode
    {
        public float time;
        public float x;
        public float y;
        public float z;
    }

    public bool rejectFlag;

    List<TrackerNode> trackerNodes;

    double elapsedTime;
    double distance;
    public float xLim, zLim;
    int[,] grid;

    string logFileDir;
    private string filename;
    //public int participantNo;

    public int startTime;
    public int taskCompletionTime;

    private Color orange = new Color32(255, 162, 45, 255);
    private Color leaf = new Color32(3, 252, 15, 255);
    private Color brown = new Color32(94, 68, 11, 255);
    private Color purple = new Color32(77, 16, 158, 255);

    void Start()
    {
        //GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //plane.transform.localScale = new Vector3(xLim/10, 1, zLim/10);

        logFileDir = Path.GetDirectoryName(Application.dataPath) + "/logData/";
        //grid = new int[(int)(xLim * 20), (int)(zLim * 20)];
        grid = new int[(int)(xLim * 10), (int)(zLim * 10)];

        // trackerNodes = readLog(13);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.black);
        
        // trackerNodes = readLog(14);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.blue);

        // trackerNodes = readLog(15);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.cyan);

        // trackerNodes = readLog(16);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.gray);

        // trackerNodes = readLog(17);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.green);

        // trackerNodes = readLog(18);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.magenta);

        // trackerNodes = readLog(19);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.red);

        // trackerNodes = readLog(20);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(Color.yellow);

        // trackerNodes = readLog(21);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(orange);

        // trackerNodes = readLog(22);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(leaf);

        // trackerNodes = readLog(23);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(brown);

        // trackerNodes = readLog(24);
        // if (rejectFlag) rejectOutliers();
        // drawSphere(purple);
        

        for (int i=1;i<=12;i++)
        { 
            trackerNodes = readLog(i);
            if (rejectFlag) rejectOutliers();
            drawSphere(Color.blue);
            gridCount();
        }

        //exportCSV();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rejectOutliers();
            drawSphere(orange);
        }
    }

    List<TrackerNode> readLog(int i)
    {
        string filename = "SG" + i + "_log_" + i + "_VR.csv";
        return readLog(filename);
    }

    List<TrackerNode> readLog(string filename)
    {
        List<TrackerNode> newNodes = new List<TrackerNode>();

        StreamReader strReader = new StreamReader(logFileDir + filename);
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
            
            if(newNodes.Count > 1 && newNodes[newNodes.Count - 1].x == 0)
                newNodes.RemoveAt(newNodes.Count - 1);
            newNodes.Add(new TrackerNode());

            newNodes[newNodes.Count - 1].time = float.Parse(data_values[0]);
            newNodes[newNodes.Count - 1].x = float.Parse(data_values[1]);
            newNodes[newNodes.Count - 1].y = float.Parse(data_values[2]);
            newNodes[newNodes.Count - 1].z = float.Parse(data_values[3]);
            //for (int i = 0; i < data_values.Length; i++)
            //{
            //   Debug.Log($"Values {i.ToString()}: {data_values[i].ToString()}");
            //}
        }

        var timespan = newNodes[newNodes.Count - 1].time - newNodes[0].time;

        Debug.Log($"Read Complete. Total Number: {newNodes.Count}");
        Debug.Log("Time elapsed: " + timespan);

        return newNodes;
    }

    void rejectOutliers()
    {
        int count = 0;
        for (int i = 0; i < trackerNodes.Count; i++)
        {
            TrackerNode node = trackerNodes[i];

            if (Mathf.Abs(node.x) > xLim / 2 || Mathf.Abs(node.z) > zLim / 2 || node.y < 0.5 || node.y > 2)
            {
                trackerNodes.RemoveAt(i);
                i--;
                count++;
            }

        }
        Debug.Log("Rejected Outliers: " + count);

    }

    void drawSphere(Color32 lineColor)
    {
        Vector3 prevNode, currentNode;
        prevNode = new Vector3(trackerNodes[1].x, trackerNodes[1].y, trackerNodes[1].z);
        distance = 0;
        for (int i = 0; i < trackerNodes.Count; i++)
        {
            currentNode = new Vector3(trackerNodes[i].x, trackerNodes[i].y, trackerNodes[i].z);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = Vector3.one * 0.003f;
            sphere.transform.position = currentNode;
            if (i > 1)
            {
                Debug.DrawLine(currentNode, prevNode, lineColor, 100);
                distance += Vector2.Distance(new Vector2(currentNode.x, currentNode.z), new Vector2(prevNode.x, prevNode.z));
                prevNode = currentNode;
            }
        }
        Debug.Log("Distance: " + distance);
    }

    void gridCount()
    {
        for (int i = 0; i < trackerNodes.Count; i++)
        {
            //grid[(int)(20 * (trackerNodes[i].x + xLim / 2)), (int)(20 * (trackerNodes[i].z + zLim / 2))]++;
            grid[(int)(10 * (trackerNodes[i].x + xLim / 2)), (int)(10 * (trackerNodes[i].z + zLim / 2))]++;

        }
    }

    void exportCSV()
    {
        StreamWriter file = new StreamWriter("D:/Github/TrajectoryPlot/AR_2_Grid_Heatmap.csv");
        var iLength = grid.GetLength(0);
        var jLength = grid.GetLength(1);

        for (int j = 0; j < jLength; j++)
        {
            for (int i = 0; i < iLength; i++)
            {
                file.Write("{0}", grid[i, j]);
                if (i < iLength - 1)
                    file.Write(",");
            }
            if (j < jLength - 1)
                file.WriteLine();
        }
        file.Flush();
        Debug.Log("Write complete.:" + iLength + "," + jLength);
    }
}