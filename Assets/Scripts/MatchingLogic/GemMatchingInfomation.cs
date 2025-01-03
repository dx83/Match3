using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GemMatchingInfomation
{
    static public void SearchForMatching(List<GemInformation> gems, GemColors[] favorColors, bool isPlayer = true)
    {
        bool Loop = true;
        //int count = 0;
        while (Loop)
        {
            Loop = false;

            for (int i = 0; i < Constants.RAWS; i++)
            {

            }
        }
    }

    static public void SearchForMatching(List<GemInformation> gems)
    {
        bool Loop = true;
        //int count = 0;
        while (Loop)
        {
            Loop = false;

            
        }

        for (int i = 0; i < Constants.RAWS; i++)
        {
            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                GemInformation g = gems[(i * 7) + j];

                if (g.coord.x - 1 > 0)
                {

                }
            }
        }
    }

    //static List<GemInformation> concat = new List<GemInformation>();
    static List<List<GemInformation>> matches = new List<List<GemInformation>>();
    static public void IsMatchingTheGem(List<GemInformation> gems)
    {
        for (int i = 0; i < Constants.RAWS; i++)
        {
            GemInformation[] g = gems.FindAll(e => e.coord.x == i).OrderBy(e => e.coord.y).ToArray(); ;

            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                matches.Add(new List<GemInformation>());    // ����ȭ �ʿ�
                matches[j].Add(g[j]);

                for (int k = j + 1; k < Constants.COLUMNS; k++)
                {
                    if (g[j].isSameColor(g[k]))
                    {
                        matches[j].Add(g[k]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (matches[j].Count < 3)
                    matches[j].Clear();
            }

            for (int j = 0; j < Constants.COLUMNS; j++)
            {
                foreach (GemInformation e in matches[j])
                    e.isDestroy = true;
            }
        }
        /*
            �� �྿ : ��Ī�� �̷������ ������ ���� ����Ʈ
            �� ���� �Լ�( ���� ���� ���� ������ ����Ʈ�� ��� => �ٸ��� ������ for�� ���� => ����Ʈ�� ���� 3�� �̸��̸� ����Ʈ ��� => ����)
            ����Ʈ �ߺ� �����ϰ� �� ���鿡 isDestroy = true �ϸ� �ɵ�

            ���྿�� �غ���???
         */
    }

    static public void Test(List<GemInformation> gems)
    {
        for (int i = 0; i < Constants.COLUMNS; i++)
            gems[i].Test();
    }
}

/*  ### ��Ī ã�� (��Ʈ, !! ��Ī�Ǵ°� ���ٸ� ���� ¥�� !!)
    
    ���� ��� ����
    
    �÷��̾��� ��� : �������� ������
    1. ���� ���� �߿��� ���� ����
    2. 0, 3, 6 �߿��� ���� ����
    3. �ű⼭ ���� ������ ���������� �˻�
    4. ��Ʈ ȿ���� 3�� �ִٰ� �����ֱ�
    
    AI�� ��� : ������ ������ ��Ī �����ϵ��� ����
    1. ������ ���� �˻��ؼ� ���� ������ �� ����
    2. ��ȣ���� ���ٸ� ���� ������ ���� (�迭���� ���� ó���� �ִ� ��)

    ��Ī�Ǵ°� �ϳ��� ���ٸ� GameManager���� ���� ¥�� ���� -> ��Ʈ ã��

    ### ���� ���� �� ��Ī �˼� �Լ� -> �� �ı�, �� ����, �� ä���
*/
