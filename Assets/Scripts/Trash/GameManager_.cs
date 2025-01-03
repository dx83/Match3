using UnityEngine;
using UnityEngine.UI;


public class GameManager_ : MonoBehaviour
{
    public GameObject gemBoard;
    public Sprite[] gemSprites;
    public GameObject[] gems;
    public GemPositionInfo[] gemPosInfo;

    void Start()
    {
        GemGenerator();
    }

    void Update()
    {

    }

    void GemGenerator()
    {
        gemPosInfo = CreateGemPositionInfo();

        gems = new GameObject[49];

        for (int i = 0; i < 49; i++)
        {
            gems[i] = GetRandomGem(i);
        }

    }

    GameObject GetRandomGem(int index)
    {
        GameObject gemObj = new GameObject();
        gemObj.name = "GEM";

        RectTransform gemRect = gemObj.AddComponent<RectTransform>();
        gemRect.pivot = new Vector2(0, 1);
        gemRect.SetParent(gemBoard.transform);

        GemPositionInfo pos = gemPosInfo[index];
        gemRect.sizeDelta = pos.size;
        gemRect.localPosition = pos.position;

        GameObject g = new GameObject();
        g.name = "image";

        Image img = g.AddComponent<Image>();
        img.sprite = gemSprites[Random.Range(0, gemSprites.Length)];

        RectTransform rt = g.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0, 1);
        rt.SetParent(gemObj.transform);

        rt.sizeDelta = pos.imgSize;
        rt.localPosition = pos.ImgPosition;

        return gemObj;
    }

    GemPositionInfo[] CreateGemPositionInfo()
    {
        GemPositionInfo[] info = new GemPositionInfo[49];

        RectTransform rect = gemBoard.GetComponent<RectTransform>();

        float width = rect.rect.width / 7;
        float height = rect.rect.height / 7;


        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                int index = (i * 7) + j;

                info[index] = new GemPositionInfo();

                GemPositionInfo g = info[index];

                g.width = width;
                g.height = height;
                g.paddingLeft = width * 0.1f;
                g.paddingTop = height * 0.1f;
                g.size = new Vector2(width, height);
                g.position = new Vector3(j * width, i * -height);
                g.imgSize = new Vector2(g.width - g.paddingTop * 2, g.height - g.paddingLeft * 2);
                g.ImgPosition = new Vector3(g.paddingLeft, -g.paddingTop, 0);
            }
        }
        return info;
    }

    public class GemPositionInfo
    {
        public float width;
        public float height;
        public float paddingTop;
        public float paddingLeft;
        public Vector2 size;
        public Vector2 position;
        public Vector2 imgSize;
        public Vector2 ImgPosition;
    }

    // �� ����
    // ���� ��ġ�� ������, �� ��ġ ������ ���� ��� �־�� �ұ�
    // �� ���� �������� ������ �����ϰ� ��ġ ������ ������
    
    // �� ����
    // ����ġ �迭�� ����� ����ġ�� �־� ��
    // ��ġ�� ���ؾ� �� �� �ε����� �����ؼ� �ٲ� ��ġ�� �ҷ����� �ɵ�?
}
