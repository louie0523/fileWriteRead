using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
public class MapLocation
{
    public int x;
    public int z;
}



public class Maze : MonoBehaviour
{
    public static Maze instance;

    System.Random rng = new System.Random();
    [SerializeField]
    public List<MapLocation> directions = new List<MapLocation>();
    public int width = 30;
    public int depth = 30;
    public int[,] map;
    public int scale = 6;

    public string MazeFoler = "Maze";
    public string MazeSaveFileName = "MazeSave.text";

    string folderPath;
    string mazePath;


    // Start is called before the first frame update
    void Start()
    {
        folderPath = Path.Combine(Application.dataPath, MazeFoler);
        mazePath = Path.Combine(Application.dataPath, MazeFoler, MazeSaveFileName);

        InitialiseMap();

        SetMapLocation();


    }

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        } else
        {
            Destroy(gameObject);
        }
    }

    public void CreateMap()
    {


        Generate(5, 5);

        DrawMap();
    }

    void InitialiseMap()
    {
        map = new int[width, depth];
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1; //1�� ��, 0�� ���
            }
        }

        Debug.Log(map.GetLength(0).ToString() + " X " + map.GetLength(1).ToString());
    }

    void SetMapLocation()
    {
        //��
        MapLocation location0 = new MapLocation();
        location0.x = 1;
        location0.z = 0;
        //Ŭ���� �� �Լ��� �� ���
        //location0.SetMapLocation(1,0);
        directions.Add(location0);
        //��
        MapLocation location1 = new MapLocation();
        location1.x = 0;
        location1.z = 1;
        //Ŭ���� �� �Լ��� �� ���
        //location0.SetMapLocation(0,1);
        directions.Add(location1);
        //��
        MapLocation location2 = new MapLocation();
        location2.x = -1;
        location2.z = 0;
        //Ŭ���� �� �Լ��� �� ���
        //location0.SetMapLocation(-1,0);
        directions.Add(location2);
        //�Ʒ�
        MapLocation location3 = new MapLocation();
        location3.x = 0;
        location3.z = -1;
        directions.Add(location3);
        //Ŭ���� �� �Լ��� �� ���
        //��
        MapLocation location4 = new MapLocation();
        location4.x = 1;
        location4.z = 1;
        directions.Add(location4);
        //��
        MapLocation location5 = new MapLocation();
        location5.x = -1;
        location5.z = 1;
        directions.Add(location5);
        //��
        MapLocation location6 = new MapLocation();
        location6.x = 1;
        location6.z = -1;
        directions.Add(location6);
        //��
        MapLocation location7 = new MapLocation();
        location6.x = -1;
        location6.z = -1;
        directions.Add(location7);


    }


    void GenerateList(int x, int z)
    {
        List<MapLocation> mapDatas = new List<MapLocation>();
        map[x, z] = 0;
        MapLocation mapData = new MapLocation();
        mapData.x = x;
        mapData.z = z;
        mapDatas.Add(mapData);

        while (mapDatas.Count > 0)
        {
            MapLocation current = mapDatas[0];
            Shuffle(directions);
            //�̵��� ������ ������ Ȯ��
            bool moved = false;

            foreach (MapLocation dir in directions)
            {
                int changeX = current.x + dir.x;
                int changeZ = current.z + dir.z;

                if (!(CountSquareNeighbours(changeX, changeZ) >= 2|| map[changeX, changeZ] == 0))
                {
                    map[changeX, changeZ] = 0;
                    MapLocation tempData = new MapLocation();
                    tempData.x = changeX;
                    tempData.z = changeZ;
                    mapDatas.Insert(0, tempData);
                    moved = true;
                    break;
                }
            }
            if (!moved)
            {
                mapDatas.RemoveAt(0);
            }
        }

    }

    void Generate(int x, int z)
    {
        Stack<Vector2Int> mapDatas = new Stack<Vector2Int>();
        int startX = x;
        int StartZ = z;
        map[startX, StartZ] = 0;
        mapDatas.Push(new Vector2Int(startX, StartZ));

        while (mapDatas.Count > 0)
        {
            Vector2Int current = mapDatas.Peek();
            Shuffle(directions);
            //�̵��� ������ ������ Ȯ��
            bool moved = false;

            foreach (MapLocation dir in directions)
            {
                int changeX = current.x + dir.x;
                int changeZ = current.y + dir.z;

                if (!(CountSquareNeighbours(changeX, changeZ) >= 2 || map[changeX, changeZ] == 0 ))
                {
                    map[changeX, changeZ] = 0;
                    mapDatas.Push(new Vector2Int(changeX, changeZ));
                    moved = true;
                    break;
                }
            }
            if (!moved)
            {
                mapDatas.Pop();
            }
        }

    }

    void Generate8(int x, int z)
    {
        Stack<Vector2Int> mapDatas = new Stack<Vector2Int>();
        int startX = x;
        int StartZ = z;
        map[startX, StartZ] = 0;
        mapDatas.Push(new Vector2Int(startX, StartZ));

        while (mapDatas.Count > 0)
        {
            Vector2Int current = mapDatas.Peek();
            Shuffle(directions);
            //�̵��� ������ ������ Ȯ��
            bool moved = false;

            foreach (MapLocation dir in directions)
            {
                int changeX = current.x + dir.x;
                int changeZ = current.y + dir.z;

                if (!((CountSquareNeighbours(changeX, changeZ) + CountSleepSqaureNeighbours(changeX, changeZ)) >= 4 || map[changeX, changeZ] == 0))
                {
                    map[changeX, changeZ] = 0;
                    mapDatas.Push(new Vector2Int(changeX, changeZ));
                    moved = true;
                    break;
                }
            }
            if (!moved)
            {
                mapDatas.Pop();
            }
        }

    }


    public void Shuffle(List<MapLocation> mapLocations)
    {
        int n = mapLocations.Count;
        //����Ʈ�� ũ�⸸ŭ �ݺ�
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            MapLocation value = mapLocations[k];
            mapLocations[k] = mapLocations[n];
            mapLocations[n] = value;
        }
    }

    void DrawMap()
    {
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z * scale);
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
        }
    }

    /// <summary>
    ///4���� ������ �˻��Ѵ�. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;
        return count;
    }

    /// <summary>
    /// �밢�� 4���� ������ �˻��Ѵ�. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int CountSleepSqaureNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z + 1] == 0) count++;
        if (map[x + 1, z + 1] == 0) count++;
        if (map[x - 1, z - 1] == 0) count++;
        if (map[x + 1, z - 1] == 0) count++;
        return count;
    }

    public void CreateFolder()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("�̷� ���� ���� �Ϸ�");
        }
    }

    public void MazeSave()
    {
        CreateFolder();

        string mapData = "";
        mapData += $"{depth}, {width},\n";
        for(int z = 0; z <depth; z++)
        {
            for(int x = 0; x < width; x++)
            {
                mapData += map[x, z].ToString() + ",";
            }
            Debug.Log(mapData);
            mapData += "\n";
        }

        File.WriteAllText(mazePath, mapData);

        Debug.Log("�̷� ���� ���� �Ϸ�");
    }

    public string LoadMap()
    {
        if (File.Exists(mazePath))
        {
            return File.ReadAllText(mazePath);
        }
        Debug.Log("�̷� ���� �ε� ����");
        return null;
    }

    public void LoadMaze()
    {
        string result = LoadMap();

        string depthstr = result.Split(',')[0];
        Debug.Log(depthstr);
        depth = int.Parse(depthstr);
        string widhtstr = result.Split(",")[1];
        Debug.Log(widhtstr);
        width = int.Parse(widhtstr);
        Debug.Log(depth + " : " + width);
        InitialiseMap();

        for(int z = 0; z < depth;z++)
        {
            string ZmapStr = result.Split('\n')[z + 1];
            for(int x = 0; x < width;x++)
            {
                string mapStr = ZmapStr.Split(",")[x];
                ZmapStr += mapStr + ",";
                map[x, z] = int.Parse(mapStr); 
            }
            Debug.Log(ZmapStr);
        }

        DrawMap();
    }

}