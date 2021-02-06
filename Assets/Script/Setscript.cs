using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setscript : MonoBehaviour
{
    public TextAsset csvFile; // CSVファイル

    public string str = "";
    public string strget = "";

    public GameObject Blue;
    public GameObject wall;
    public GameObject Red;
    public GameObject Black;//ブロックを置くための設定　青　赤　黒
    public GameObject Player;
    public GameObject Goal;
    public GameObject BlockPut;
    public GameObject PlayerPut;
    public GameObject GoalPut;
    public GameObject BlockParents;
    public GameObject Camera;

    public List<GameObject> ObjList = new List<GameObject>();
    public List<int> posList = new List<int>();
    public int listnum;

    public int gyou = 7;  //CSVデータの行数　縦
    public int retu = 7;  //CSVデータの列数　横

    public int[,] map = new int[20, 20];   //マップ番号を格納するマップ用変数
    public int[,] firstmap = new int[20, 20];
    int[] iDat = new int[15];       //文字検索用

    int a = 0;    //濫用　数値型変数
    int b = 0;    //濫用　数値型変数

    int ix = 0;     //マップ置くX座標　初期位置
    int iy = 0;     //マップ置くY座標　初期位置
    int iz = 0;     //マップ置くZ座標　初期位置

    int search;          
    int senum;
    int research;
    int resenum;
    GameObject reseobj;
    int resetype;
    public int putX;

    Blockscript script2;
    public Vector3 retra;

    Blockscript anyscript;
    public GameObject anyobj;
    public int anynum;
    int anyse;
    public int anytype;//プレイヤーが動くか判定用
    public bool playerdo = false;
    public GameObject playerblock;
    Blockscript playerscript;

    public int Goalpos;
    public int playerpos;
    public GameObject winner;

    public int CSVnum;
    public string strCSV;

    // Use this for initialization
    void Start()
    {
        CSVnum = SelectScript.GetStage();
        strCSV = CSVnum.ToString("00");
        //----------↓　ここでCSVデータをstrに保存　↓
        csvFile = Resources.Load(strCSV) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            str = str + "," + line;
        }

        str = str + ",";        //最後に検索文字列の","を追記。これがないと最後の文字を取りこぼす。
                                //----------↑　ここでCSVデータをstrに保存　↑

        //----------↓　ここでCSVデータをマップ配列変数mapに保存　↓
        for (int c = 0; c < gyou; c++)
        {
            for (int i = 0; i < retu; i++)
            {
                try
                {
                    iDat[0] = str.IndexOf(",", iDat[0]);   //","を検索
                }
                catch { break; }

                try
                {
                    iDat[1] = str.IndexOf(",", iDat[0] + 1);   //次の","を検索
                }
                catch { break; }

                iDat[2] = iDat[1] - iDat[0] - 1;                //何文字取り出すか決定

                try
                {
                    strget = str.Substring(iDat[0] + 1, iDat[2]);   //iDat[2]文字ぶんだけ取り出す
                }
                catch { break; }

                try
                {
                    iDat[3] = int.Parse(strget);        //取り出した文字列を数値型に変換
                }
                catch { break; }

                map[a, b] = iDat[3];
                firstmap[a, b] = iDat[3];//マップ用変数に保存。１とか６とか数字が入るよ
                b++;                            //一つ右のマップ用変数へ
                iDat[0]++;                      //次のインデックスへ
            }

            a++;                                //一つ下のマップチップへ
            b = 0;                              //マップチップ格納を一番左に戻す。
        }
        ix = 0;   //マップ置くX座標　初期位置
        iy = 0;   //マップ置くY座標　初期位置
        iz = 0;   //マップ置くZ座標　初期位置
        a = 0;
        b = 0;
        PUT();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Check();
            for (int roop = 1; roop < gyou; roop++)
            {

                a = 0;//縦
                b = 0;//横
                for (int r = 0; r < gyou; r++)//行をすべてやるまで繰り返す
                {
                    for (int p = 0; p < retu; p++)//列をすべてやるまで繰り返す
                    {
                        search = a * 10 + b;//動かすブロックの一つ上 検索用
                        research = a * 10 + b + 10;//動かすブロック検索用

                        senum = posList.IndexOf(search);//一つ上のブロックがリストの何番目か
                        if (senum == -1)//上にブロックが存在しなければ処理を実行
                        {
                            resenum = posList.IndexOf(research);//動かすブロックがリストの何番目か
                            if (resenum != -1)//動かすブロックが存在すれば処理を行う
                            {
                                reseobj = ObjList[resenum];//動かすブロックのオブジェクト
                                script2 = reseobj.GetComponent<Blockscript>();
                                resetype = script2.type;
                                if (resetype != 3 & resetype != 5)     //処理を行うブロックが3(動かない)以外なら処理を実行
                                {
                                    if (resetype == 4)
                                    {
                                        if (playerdo == true)
                                        {
                                            map[a, b] = map[a + 1, b];
                                            map[a + 1, b] = 0;
                                            posList[resenum] = search;
                                            script2.bY -= 1;
                                            retra = reseobj.transform.position;
                                            retra.y += 1;
                                            reseobj.transform.position = retra;
                                        }

                                    }
                                    else
                                    {
                                        map[a, b] = map[a + 1, b];
                                        map[a + 1, b] = 0;
                                        posList[resenum] = search;
                                        script2.bY -= 1;
                                        retra = reseobj.transform.position;
                                        retra.y += 1;
                                        reseobj.transform.position = retra;
                                    }
                                }
                            }
                        }

                        b++;
                    }
                    a++;
                    b = 0;
                }
            }
            GoalCheck();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Check();
            for (int roop = 1; roop < gyou; roop++)
            {
                a = gyou - 1;
                b = 0;
                for (int r = 0; r < gyou; r++)//行をすべてやるまで繰り返す
                {
                    for (int p = 0; p < retu; p++)//列をすべてやるまで繰り返す
                    {
                        search = a * 10 + b;//動かすブロックの一つ下 検索用
                        research = a * 10 + b - 10;//動かすブロック検索用

                        senum = posList.IndexOf(search);//一つ下のブロックがリストの何番目か
                        if (senum == -1)//下にブロックが存在しなければ処理を実行
                        {
                            resenum = posList.IndexOf(research);//動かすブロックがリストの何番目か
                            if (resenum != -1)//動かすブロックが存在すれば処理を行う
                            {
                                reseobj = ObjList[resenum];//動かすブロックのオブジェクト
                                script2 = reseobj.GetComponent<Blockscript>();
                                resetype = script2.type;
                                if (resetype != 3 & resetype != 5)     //処理を行うブロックが3(動かない)以外なら処理を実行
                                {
                                    if (resetype == 4)
                                    {
                                       
                                        if (playerdo == true)
                                        {
                                            map[a, b] = map[a - 1, b];
                                            map[a - 1, b] = 0;
                                            posList[resenum] = search;
                                            script2.bY += 1;
                                            retra = reseobj.transform.position;
                                            retra.y -= 1;
                                            reseobj.transform.position = retra;
                                        }
                                    }
                                    else
                                    {
                                        map[a, b] = map[a - 1, b];
                                        map[a - 1, b] = 0;
                                        posList[resenum] = search;
                                        script2.bY += 1;
                                        retra = reseobj.transform.position;
                                        retra.y -= 1;
                                        reseobj.transform.position = retra;
                                    }
                                    
                                }
                            }
                        }

                        b++;
                    }
                    a--;
                    b = 0;
                }
            }
            GoalCheck();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Check();
            for (int roop = 1; roop < retu; roop++)
            {
                a = 0;//縦
                b = 0;//横
                for (int r = 0; r < retu; r++)//列(横)をすべてやるまで繰り返す
                {
                    for (int p = 0; p < gyou; p++)//行(縦)をすべてやるまで繰り返す
                    {
                        search = a * 10 + b;//動かすブロックの一つ左 検索用
                        research = a * 10 + b + 1;//動かすブロック検索用

                        senum = posList.IndexOf(search);//一つ左のブロックがリストの何番目か
                        if (senum == -1)//左にブロックが存在しなければ処理を実行
                        {
                            resenum = posList.IndexOf(research);//動かすブロックがリストの何番目か
                            if (resenum != -1)//動かすブロックが存在すれば処理を行う
                            {
                                reseobj = ObjList[resenum];//動かすブロックのオブジェクト
                                script2 = reseobj.GetComponent<Blockscript>();
                                resetype = script2.type;
                                if (resetype != 3 & resetype != 5)     //処理を行うブロックが3(動かない)以外なら処理を実行
                                {
                                    if (resetype == 4)
                                    {
                                        if (playerdo == true)
                                        {
                                            map[a, b] = map[a, b + 1];
                                            map[a, b + 1] = 0;
                                            posList[resenum] = search;
                                            script2.bX -= 1;
                                            retra = reseobj.transform.position;
                                            retra.x -= 1;
                                            reseobj.transform.position = retra;
                                        }
                                    }
                                    else
                                    {
                                        map[a, b] = map[a, b + 1];
                                        map[a, b + 1] = 0;
                                        posList[resenum] = search;
                                        script2.bX -= 1;
                                        retra = reseobj.transform.position;
                                        retra.x -= 1;
                                        reseobj.transform.position = retra;
                                    }
                                    
                                }
                            }
                        }

                        a++;
                    }
                    b++;
                    a = 0;
                }
            }
            GoalCheck();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Check();
            for (int roop = 1; roop < retu; roop++)
            {
                a = 0;//縦
                b = retu -1;//横
                for (int r = 0; r < retu; r++)//列(横)をすべてやるまで繰り返す
                {
                    for (int p = 0; p < gyou; p++)//行(縦)をすべてやるまで繰り返す
                    {
                        search = a * 10 + b;//動かすブロックの一つ右 検索用
                        research = a * 10 + b - 1;//動かすブロック検索用

                        senum = posList.IndexOf(search);//一つ右のブロックがリストの何番目か
                        if (senum == -1)//右にブロックが存在しなければ処理を実行
                        {
                            resenum = posList.IndexOf(research);//動かすブロックがリストの何番目か
                            if (resenum != -1)//動かすブロックが存在すれば処理を行う
                            {
                                reseobj = ObjList[resenum];//動かすブロックのオブジェクト
                                script2 = reseobj.GetComponent<Blockscript>();
                                resetype = script2.type;
                                if (resetype != 3 & resetype != 5)     //処理を行うブロックが3(動かない)以外なら処理を実行
                                {
                                    if (resetype == 4)
                                    {
                                        if (playerdo == true)
                                        {
                                            map[a, b] = map[a, b - 1];
                                            map[a, b - 1] = 0;
                                            posList[resenum] = search;
                                            script2.bX += 1;
                                            retra = reseobj.transform.position;
                                            retra.x += 1;
                                            reseobj.transform.position = retra;
                                        }
                                    }
                                    else
                                    {
                                        map[a, b] = map[a, b - 1];
                                        map[a, b - 1] = 0;
                                        posList[resenum] = search;
                                        script2.bX += 1;
                                        retra = reseobj.transform.position;
                                        retra.x += 1;
                                        reseobj.transform.position = retra;
                                    }
                                }
                            }
                        }

                        a++;
                    }
                    b--;
                    a = 0;
                }
            }
            GoalCheck();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            foreach (Transform child in BlockParents.transform)
            {
               Destroy(child.gameObject);
                ObjList.Clear();
                posList.Clear();
            }
            PUT();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene("stageselect");
        }
    }
    void PUT()
    {
        ix = 0;   //マップ置くX座標　初期位置
        iy = 0;   //マップ置くY座標　初期位置
        iz = 0;   //マップ置くZ座標　初期位置

        int at = 0; //縦
        int bt = 0; //横

        listnum = 0;
        for (int c = 0; c < gyou; ++c)
        {
            for (int i = 0; i < retu; ++i)
            {
                putX = 0;
                if (firstmap[at, bt] % 10 == 1)     //マップ番号の一桁目が１ならBlueを配置
                {
                    BlockPut = Instantiate(Blue) as GameObject;   
                    BlockPut.transform.position = new Vector3(ix, iy, iz);
                    Blockscript number = BlockPut.GetComponent<Blockscript>();
                    number.num = listnum;
                    listnum += 1;
                    putX = at * 10;
                    putX += bt;
                    posList.Add(putX);
                    number.bX = bt;
                    number.bY = at;
                    number.type = 1;
                    ObjList.Add(BlockPut);
                    BlockPut.transform.parent = BlockParents.transform;
                }
                if (firstmap[at, bt] % 10 == 2)     //2ならRedを配置
                {
                    BlockPut = Instantiate(Red) as GameObject;   
                    BlockPut.transform.position = new Vector3(ix, iy, iz);
                    Blockscript number = BlockPut.GetComponent<Blockscript>();
                    number.num = listnum;
                    listnum += 1;
                    putX = at * 10;
                    putX += bt;
                    posList.Add(putX);
                    number.bX = bt;
                    number.bY = at;
                    number.type = 2;
                    ObjList.Add(BlockPut);
                    BlockPut.transform.parent = BlockParents.transform;
                }
                if (firstmap[at, bt] % 10 == 3)     //3ならBlackを配置
                {
                    BlockPut = Instantiate(Black) as GameObject;
                    BlockPut.transform.position = new Vector3(ix, iy, iz);
                    Blockscript number = BlockPut.GetComponent<Blockscript>();
                    number.num = listnum;
                    listnum += 1;
                    putX = at * 10;
                    putX += bt;
                    posList.Add(putX);
                    number.bX = bt;
                    number.bY = at;
                    number.type = 3;
                    ObjList.Add(BlockPut);
                    BlockPut.transform.parent = BlockParents.transform;
                }
                if (firstmap[at, bt] % 10 == 4)     //4ならPlayerを配置
                {
                    PlayerPut = Instantiate(Player) as GameObject;
                    PlayerPut.transform.position = new Vector3(ix, iy, iz);
                    Blockscript number = PlayerPut.GetComponent<Blockscript>();
                    number.num = listnum;
                    listnum += 1;
                    putX = at * 10;
                    putX += bt;
                    posList.Add(putX);
                    number.bX = bt;
                    number.bY = at;
                    number.type = 4;
                    ObjList.Add(PlayerPut);
                    playerblock = PlayerPut;
                    PlayerPut.transform.parent = BlockParents.transform;
                }
                if (firstmap[at, bt] % 10 == 5)     //5ならWallを配置
                {
                    BlockPut = Instantiate(wall) as GameObject;
                    BlockPut.transform.position = new Vector3(ix, iy, iz);
                    Blockscript number = BlockPut.GetComponent<Blockscript>();
                    number.num = listnum;
                    listnum += 1;
                    putX = at * 10;
                    putX += bt;
                    posList.Add(putX);
                    number.bX = bt;
                    number.bY = at;
                    number.type = 5;
                    ObjList.Add(BlockPut);
                    BlockPut.transform.parent = BlockParents.transform;
                }
                if (firstmap[at, bt] / 10 == 1)     //二桁目が1ならGoalを配置
                {
                    GoalPut = Instantiate(Goal) as GameObject;  
                    GoalPut.transform.position = new Vector3(ix, iy, iz);
                    Goalpos = at * 10 + bt;
                    GoalPut.transform.parent = BlockParents.transform;
                }
                ++bt;            //次の右のマップ番号を読み込む
                ix += 1;    //配置位置を右に移動
            }
            ++at;            //一行終了。下の段のマップ番号を読み込んでいく
            bt = 0;          //行の始めに戻るからb=0
            iy -= 1;    //下の段に配置していくからY座標をー1する
            ix = 0;       //一段下の左端からまた配置していくからX座標は初期位置になる 
        }//読み込んで配置する
    } 
    void Check()
    {
        playerdo = false;
        playerscript = playerblock.GetComponent<Blockscript>();
        playerpos = playerscript.bY * 10 + playerscript.bX;
        anyse = playerpos - 10;//上のブロック
        anynum = posList.IndexOf(anyse);
        //Debug.Log(anyse);
        //Debug.Log(research);
        if (anynum != -1)
        {
            anyobj = ObjList[anynum];
            anyscript = anyobj.GetComponent<Blockscript>();
            anytype = anyscript.type;
            if (anytype == 1)
            {
                playerdo = true;
            }
        }
        anyse = playerpos + 10;  //下のブロック
        anynum = posList.IndexOf(anyse);
        if (anynum != -1)
        {
            anyobj = ObjList[anynum];
            anyscript = anyobj.GetComponent<Blockscript>();
            anytype = anyscript.type;
            if (anytype == 1)
            {
                playerdo = true;
            }
        }
        anyse = playerpos + 1; //右のブロック
        anynum = posList.IndexOf(anyse);
        if (anynum != -1)
        {
            anyobj = ObjList[anynum];
            anyscript = anyobj.GetComponent<Blockscript>();
            anytype = anyscript.type;
            if (anytype == 1)
            {
                playerdo = true;
            }
        }
        anyse = playerpos - 1;//左のブロック 
        anynum = posList.IndexOf(anyse);
        if (anynum != -1)
        {
            anyobj = ObjList[anynum];
            anyscript = anyobj.GetComponent<Blockscript>();
            anytype = anyscript.type;
            if (anytype == 1)
            {
                playerdo = true;
            }
        }
    }

    void GoalCheck()
    {
        playerpos = playerscript.bY * 10 + playerscript.bX;
        if (playerpos == Goalpos)
        {
            winner.SetActive(true);
            Invoke("Back", 3f);
        }
    }
    void Back()
    {
        SceneManager.LoadScene("stageselect");
    }
}
