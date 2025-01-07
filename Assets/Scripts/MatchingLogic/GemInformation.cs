using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GemInformation
{
    //public GameObject name;
    int index;
    public int[] fourIdx = new int[4];
    public Point coord;
    public GemColors color;

    public RectTransform rect;
    public Image gemImg;
    public Text debug;

    bool isDestroy;

    //오브젝트의 이동이 아님, 정보만 교환
    public void GemChange(GemInformation swapGem, Sprite[] gemSprites)
    {
        //this.x = swapGem.x;
        //this.y = swapGem.y;
        this.color = swapGem.color;
        if (gemImg != null)
        {
            //this.name.name = ((GemColors)color).ToString();
            //this.name.name = $"[{x}][{y}]";
            this.gemImg.sprite = gemSprites[(int)this.color];
        }
    }

    public void RandomGem(Sprite[] gemSprites)
    {
        int color = Random.Range(0, gemSprites.Length);
        this.color = (GemColors)color;
        if (gemImg != null)
        {
            this.gemImg.sprite = gemSprites[color];
        }
    }

    public bool isSameColor(GemInformation other)
    {
        return this.color == other.color;
    }

    public int DetermineIndex()
    {
        this.index = (coord.x * 7) + coord.y;
        FourDirectionIndex();
        return index;
    }

    void FourDirectionIndex()    // 스왑 가능한 곳의 인덱스
    {
        fourIdx[0] = (coord.x * 7) + coord.y - 1;   //left
        fourIdx[1] = (coord.x * 7) + coord.y + 1;   //right
        fourIdx[2] = ((coord.x - 1) * 7) + coord.y; //up
        fourIdx[3] = ((coord.x + 1) * 7) + coord.y; //down
    }

    public Direction SwapDirection(int idxAfterSwap)    // 스왑 방향
    {
        for (int i = 0; i < fourIdx.Length; i++)
        {
            if (fourIdx[i] == idxAfterSwap)
            {
                return (Direction)i;
            }
        }

        return Direction.passless;
    }

    public void Destroy() => isDestroy = true;
    public bool GemStatus() => isDestroy;

    public void Test()
    {
        if (gemImg != null)
        {
            this.gemImg.gameObject.SetActive(false);
        }
    }
}
