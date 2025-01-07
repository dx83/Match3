using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject gemBoard;
    public Sprite[] gemSprites;

    List<GameObject> gemObject;
    List<GemInformation> gems;
    GemInformation temp;
    GemImmutableInfo gemImmutableInfo;

    void Start()
    {
        temp = new GemInformation();
        GemGenerator();
    }

    bool test = false;
    Coroutine coroutine;
    List<GemInformation> tempList = new List<GemInformation>();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            test = !test;

            //GemMatchingInfomation.IsMatchingTheGem(gems);
            //GemMatchingInfomation.Test(gems);

            //if (test)
            //{
            //    coroutine = StartCoroutine(GemMatchingInfomation.FlickerGemEffect(gems));
            //}
            //else
            //{
            //    if (coroutine != null)
            //    {
            //        StopCoroutine(coroutine);
            //        List<GemInformation> temp = gems.FindAll(e => e.coord.x == 3);
            //        foreach (GemInformation g in temp)  // 코루틴 끝나면 효과 초기화
            //            g.gemImg.color = Color.white;
            //    }
            //}
            tempList = GemMatchingInformation.SearchForHint(gems);
            //foreach (GemInformation e in tempList)
            //    Debug.Log(e.DetermineIndex());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))// 매칭 검수
        {
            //GemBoardCheckAndShuffle();
            //foreach (GemInformation e in gems)
            //{
            //    if (e.GemStatus()) e.Test();
            //}
            GemMatchingInformation.FlickerGemEffect(tempList);
        }

        //if (test)// 이동, dotween 같은 것 이용할 필요가 있음
        //{
        //    moveTime += Time.deltaTime;
        //    float t = moveTime / 30f;
        //    Vector2 start = gems[0].rect.localPosition;
        //    Vector2 target = gemImmutableInfo.position[gems[0].x + 6, gems[0].y];
        //
        //    gems[0].rect.localPosition = Vector2.Lerp(start, target, t);
        //}

        foreach (GemInformation g in gems)
        {
            //g.debug.text = g.color.ToString();
            g.debug.text = g.GemStatus() ? "D" : "";
            //g.debug.text = g.index.ToString();
        }
    }

    //float excuteTime = 9.0f;
    //float nextTime = 0.0f;
    private void FixedUpdate()
    {
        if (test)
        {
            //if (Time.time > nextTime) // 일정시간 계속 렌덤 판짜기
            //{
            //    nextTime = Time.time + excuteTime;
            //    GemBoardNewShuffle();
            //    GemBoardCheckAndShuffle();
            //}

            // DOTWeen
            // https://maintaining.tistory.com/entry/Unity-UI-%EA%B9%9C%EB%B9%A1%EC%9E%84-%ED%9A%A8%EA%B3%BC-%EB%84%A3%EA%B8%B0
        }
    }

    void GemGenerator()
    {
        gemImmutableInfo = CreateGemImmutableInfo();

        gemObject = new List<GameObject>();
        gems = new List<GemInformation>();

        for (int i = 0; i < Constants.RAWS; i++)
        {
            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                gemObject.Add(CreateRandomGem(i, j));
            }
        }

        GemBoardCheckAndShuffle();
    }

    GemImmutableInfo CreateGemImmutableInfo()   // 변하지않는 젬 공통 속성 생성
    {
        GemImmutableInfo obj = new GemImmutableInfo();

        RectTransform rect = gemBoard.GetComponent<RectTransform>();

        obj.width = rect.rect.width / 7;
        obj.height = rect.rect.height / 7;
        obj.paddingLeft = obj.width * 0.1f;
        obj.paddingTop = obj.height * 0.1f;
        obj.size = new Vector2(obj.width, obj.height);
        obj.imgSize = new Vector2(obj.width - obj.paddingTop * 2, obj.height - obj.paddingLeft * 2);
        obj.ImgPosition = new Vector3(obj.paddingLeft, -obj.paddingTop, 0);

        for (int i = 0; i < Constants.RAWS; i++)
        {
            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                obj.position[i, j] = new Vector3(j * obj.width, i * -obj.height);
            }
        }

        return obj;
    }

    GameObject CreateRandomGem(int x, int y)    // 개별 쳄 오브젝트와 정보 생성
    {
        GameObject obj = new GameObject();
        obj.name = "GEM";

        RectTransform objRect = obj.AddComponent<RectTransform>();
        objRect.pivot = new Vector2(0, 1);
        objRect.SetParent(gemBoard.transform);

        GemImmutableInfo info = gemImmutableInfo;
        objRect.sizeDelta = info.size;
        objRect.localPosition = info.position[x, y];

        GameObject imgObj = new GameObject();
        imgObj.name = "image";

        Image img = imgObj.AddComponent<Image>();
        //int color = (int)colorForTest[(x * 7) + y];
        int color = Random.Range(0, gemSprites.Length - 1);
        img.sprite = gemSprites[color];

        RectTransform rt = imgObj.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0, 1);
        rt.SetParent(obj.transform);

        rt.sizeDelta = info.imgSize;
        rt.localPosition = info.ImgPosition;

        // Debug
        GameObject textObject = new GameObject();
        textObject.name = "Debug";
        textObject.transform.SetParent(obj.transform);
        textObject.layer = LayerMask.NameToLayer("UI");

        textObject.AddComponent<CanvasRenderer>();

        RectTransform textRt = textObject.AddComponent<RectTransform>();
        textRt.localPosition = Vector3.zero;
        textRt.localScale = Vector3.one;
        textRt.anchorMin = new Vector2(0f, 0f);
        textRt.anchorMax = new Vector2(1f, 1f);
        textRt.pivot = new Vector2(0.5f, 0.5f);
        textRt.offsetMin = new Vector2(0, 0);
        textRt.offsetMax = new Vector2(1, 1);

        Text text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 20;
        text.supportRichText = true;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        text.resizeTextForBestFit = false;
        // =========

        //gems.Add(new GemInformation { name = Obj, index = gemObject.Count, x = x, y = y, 
        //    color = (GemColors)color, rect = objRect, gemImg = imgObj.GetComponent<Image>() });
        gems.Add(new GemInformation
        {
            coord = new Point(x, y),
            color = (GemColors)color,
            rect = objRect,
            gemImg = img,
            debug = text,   // Debug
        });
        //Obj.name = ((GemColors)color).ToString();
        //Obj.name = $"[{x}][{y}]";

        return obj;
    }

    void GemBoardCheckAndShuffle()    // 배치 직후 즉시 매칭되는 젬이 있는지 검수하고 교환
    {
        bool Loop = true;
        int count = 0;
        while (Loop)
        {
            Loop = false;

            for (int i = 0; i < Constants.RAWS; i++)
            {
                GemInformation[] info = gems.FindAll(e => e.coord.x == i).ToArray();
                
                // 색깔이 연속 3개째일때 다른
                for (int j = 0; j < Constants.COLUMNS - 2; j++)
                {
                    if (j + 2 < Constants.COLUMNS &&
                        info[j].color == info[j + 1].color &&
                        info[j].color == info[j + 2].color)
                    {
                        temp.GemChange(info[j + 2], gemSprites);

                        GemInformation[] swap = gems.FindAll(e => e.coord.x != i && e.color != info[j + 2].color).ToArray();
                        int random = Random.Range(0, swap.Length);
                        info[j + 2].GemChange(swap[random], gemSprites);
                        swap[random].GemChange(temp, gemSprites);

                        Loop = true;
                    }
                }
            }

            for (int i = 0; i < Constants.COLUMNS; i++)
            {
                GemInformation[] info = gems.FindAll(e => e.coord.y == i).ToArray();

                // 색깔이 연속 3개째일때 다른
                for (int j = 0; j < Constants.RAWS - 2; j++)
                {
                    if (j + 2 < Constants.RAWS &&
                        info[j].color == info[j + 1].color &&
                        info[j].color == info[j + 2].color)
                    {
                        temp.GemChange(info[j + 2], gemSprites);

                        GemInformation[] swap = gems.FindAll(e => e.coord.y != i && e.color != info[j + 2].color).ToArray();
                        int random = Random.Range(0, swap.Length);
                        info[j + 2].GemChange(swap[random], gemSprites);

                        swap[random].GemChange(temp, gemSprites);

                        Loop = true;
                    }
                }
            }

            if (count++ > Constants.LOOP)   // 검수가 길어지면 그냥 다시 새판 짜기
            {
                count = 0;
                Loop = true;
                GemBoardNewShuffle();
            }
        }
    }

    void GemBoardNewShuffle()   // 새판 짜기
    {
        foreach (GemInformation g in gems)
        {
            g.RandomGem(gemSprites);
        }
    }

    

    List<GemColors> colorForTest = new List<GemColors>() // 테스트 코드
    {
        //GemColors.Red, GemColors.Red, GemColors.Green, GemColors.Blue, GemColors.Purple, GemColors.Yellow, GemColors.Blue,
        //GemColors.Green, GemColors.Green, GemColors.Green, GemColors.Red, GemColors.Red, GemColors.Purple, GemColors.Blue,
        //GemColors.Purple, GemColors.Purple, GemColors.Green, GemColors.Yellow, GemColors.Stone, GemColors.Red, GemColors.Red,
        //GemColors.Yellow, GemColors.Yellow, GemColors.Red, GemColors.Blue, GemColors.Yellow, GemColors.Purple, GemColors.Purple,
        //GemColors.Blue, GemColors.Red, GemColors.Green, GemColors.Red, GemColors.Green, GemColors.Black, GemColors.Black,
        //GemColors.Red, GemColors.Yellow, GemColors.Yellow, GemColors.Black, GemColors.Blue, GemColors.Purple, GemColors.Stone,
        //GemColors.Yellow, GemColors.Black, GemColors.Blue, GemColors.Stone, GemColors.Yellow, GemColors.Green, GemColors.Purple,

        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green,
        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green,
        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green,
        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green,
        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green,
        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green,
        GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Red, GemColors.Green, GemColors.Green,
    };

    /* 무작위 배치와 매칭 검수
    
    반드시 최소 하나의 플레이 가능한 매칭이 있어야 함 == 힌트 시스템
    매칭이 하나도 없다면 => 다시 새판 짜도록 코드 작성


    다시 새판짤 때 아래로 다떨어지고 다시 채우는 효과

    ### 좀 더 생각이 필요하다... 매칭 검수 예
    (보드판 전체) 현재 즉시 매칭이 되는지 검수하고 파괴와 채우기 실행
    스왑 실행 후 매칭이 되는지 검수하고 [젬되돌기] 또는 [파괴와 채우기] 실행
    */
}

public class GemImmutableInfo
{
    public float width;
    public float height;
    public float paddingTop;
    public float paddingLeft;
    public Vector2 size;
    public Vector2 imgSize;
    public Vector2 ImgPosition;
    public Vector2[,] position = new Vector2[Constants.RAWS, Constants.COLUMNS];
}

public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

