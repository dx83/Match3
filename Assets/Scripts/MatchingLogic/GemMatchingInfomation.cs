using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UIElements;

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
    /*
        1. ���� ���� �߿��� ���� ����
        2. 0, 3, 6 �߿��� ���� ����
        3. �ű⼭ ���� ������ ���ʿ��� ���������� �˻�
        4. ��Ʈ ȿ���� 3�� �ִٰ� �����ֱ�

        ������ �����̵��� ����, �� ������ ������ ����
        ##*####
        **#**##
        ##*####
        #######
        4�������� �̵��� ���� ���� ��� ��Ī�� ���� ȿ��
    */
    static List<List<GemInformation>> potentialMatches = new List<List<GemInformation>>();
    static GemInformation targetGem = new GemInformation();
    static public void IsMatchingForHint(List<GemInformation> gems)
    {
        if (potentialMatches.Count == 0)
        {
            for (int i = 0; i < 4; i++)
                potentialMatches.Add(new List<GemInformation>());
        }

        //int cond0 = Random.Range(0, 2);
        //int cond1 = Random.Range(0, 3);
        //switch(cond0)
        //{
        //    case 0:     break;
        //
        //}

        // �̵��� �ش� ������ ������ ���� üũ
        for (int i = 0; i < gems.Count; i++) // ��� �� ���
        {
            targetGem.color = gems[i].color;
            targetGem.coord = gems[i].coord;
            // �˼��� �������� �׻� Ÿ������ �˻� �ؾߵǴµ�...
            // left
            if ((i % Constants.COLUMNS) != 0)
            {
                int max = i - targetGem.coord.x * Constants.COLUMNS;

                for (int j = 0; j < max; j++)     // üũ�� ���� �����ִ� �� �˼�
                {
                    int current = targetGem.coord.x * Constants.COLUMNS + j;
                    potentialMatches[j].Add(gems[current]);
                    
                    for (int k = 0; k < max; k++)                   // ��Ī Ȯ��
                    {
                        int next = current + k + 1;
                        if (gems[current].isSameColor(gems[next]))  // ���� ���� ���ӵǴ��� Ȯ��
                        {
                            potentialMatches[j].Add(gems[next]);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (potentialMatches[j].Count < 3)    // 2�� ������ ��� ����Ʈ ���
                        potentialMatches[j].Clear();
                }
                //for (int j = 0; j < Constants.COLUMNS - 2; j++)
                //{
                //    int current = i * Constants.COLUMNS + j;
                //    potentialMatches[j].Add(gems[current]);
                //
                //    for (int k = 0; k < Constants.COLUMNS; k++)     // ��Ī Ȯ��
                //    {
                //        int next = current + k + 1;
                //        if (gems[current].isSameColor(gems[next]))  // ���� ���� ���ӵǴ��� Ȯ��
                //        {
                //            rawMatches[j].Add(gems[next]);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //
                //    if (rawMatches[j].Count < 3)    // 2�� ������ ��� ����Ʈ ���
                //        rawMatches[j].Clear();
                //}
                //
                //for (int j = 0; j < Constants.COLUMNS - 2; j++)     // ���� ���� �� ������
                //{
                //    foreach (GemInformation e in rawMatches[j])     // ��Ī�Ǿ� �ı��Ǵ� �� ��� ǥ��
                //        e.isDestroy = true;
                //    rawMatches[j].Clear();          // ���� ���� ���� ����Ʈ ���
                //}
            }
        }
    }

    static public IEnumerator FlickerGemEffect(List<GemInformation> potentialMatches)   // �� ������ ȿ��
    {
        List<GemInformation> temp = potentialMatches.FindAll(e => e.coord.x == 3).ToList();
        while(true)
        {
            for (float i = 0.95f; i >= 0.55f; i -= 0.1f)
            {
                // ��Ʈ�� �ش�Ǵ� �� ������
                foreach (var item in temp)//potentialMatches)
                {
                    if (item == null)
                        yield break;
                    // ���İ��� 1f ���� 0.3f ���� ���� : ���� ��������
                    Color c = item.gemImg.color;
                    c.a = i;
                    item.gemImg.color = c;
                }
                // ������ ��ġ��ŭ ������
                yield return new WaitForSeconds(0.15f);
            }
            for (float i = 0.55f; i <= 0.95f; i += 0.1f)
            {
                // ��Ʈ�� �ش�Ǵ� �� ������
                foreach (var item in temp)//potentialMatches)
                {
                    if (item == null)
                        yield break;
                    // ���İ��� 0.3f ���� 1f ���� ���� : ���� �ٽ� ���ƿ�
                    Color c = item.gemImg.color;
                    c.a = i;
                    item.gemImg.color = c;
                }
                // ������ ��ġ��ŭ ������
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    // ��� ��Ī ã��
    static List<List<GemInformation>> rawMatches = new List<List<GemInformation>>();
    static List<List<GemInformation>> colMatches = new List<List<GemInformation>>();
    static public void IsMatchingTheGem(List<GemInformation> gems)
    {
        if (rawMatches.Count == 0)
        {
            for (int i = 0; i < Constants.COLUMNS - 2; i++)
                rawMatches.Add(new List<GemInformation>()); // �� ���� �� �� ��Ī ����Ʈ
            for (int i = 0; i < Constants.RAWS - 2; i++)
                colMatches.Add(new List<GemInformation>()); // �� ���� �� �� ��Ī ����Ʈ
        }

        for (int i = 0; i < Constants.RAWS; i++)                // �� �྿
        {
            for (int j = 0; j < Constants.COLUMNS - 2; j++)     // �� ������
            {
                int current = i * Constants.COLUMNS + j;
                rawMatches[j].Add(gems[current]);
                
                for (int k = 0; k < Constants.COLUMNS; k++)     // ��Ī Ȯ��
                {
                    int next = current + k + 1;
                    if (gems[current].isSameColor(gems[next]))  // ���� ���� ���ӵǴ��� Ȯ��
                    {
                        rawMatches[j].Add(gems[next]);
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (rawMatches[j].Count < 3)    // 2�� ������ ��� ����Ʈ ���
                    rawMatches[j].Clear();
            }

            for (int j = 0; j < Constants.COLUMNS - 2; j++)     // ���� ���� �� ������
            {
                foreach (GemInformation e in rawMatches[j])     // ��Ī�Ǿ� �ı��Ǵ� �� ��� ǥ��
                    e.isDestroy = true;
                rawMatches[j].Clear();          // ���� ���� ���� ����Ʈ ���
            }
        }

        for (int i = 0; i < Constants.COLUMNS; i++)             // �� ����
        {
            for (int j = 0; j < Constants.RAWS - 2; j++)        // �� ������
            {
                int current = i + (j * Constants.RAWS);
                colMatches[j].Add(gems[current]);

                for (int k = 0; k < Constants.RAWS; k++)        // ��Ī Ȯ��
                {
                    int next = current + (k + 1) * Constants.RAWS;
                    if (gems[current].isSameColor(gems[next]))  // ���� ���� ���ӵǴ��� Ȯ��
                    {
                        colMatches[j].Add(gems[next]);
                    }
                    else
                    {
                        break;
                    }
                }

                if (colMatches[j].Count < 3)    // 2�� ������ ��� ����Ʈ ���
                    colMatches[j].Clear();
            }

            for (int j = 0; j < Constants.RAWS - 2; j++)        // ���� ���� �� ������
            {
                foreach (GemInformation e in colMatches[j])     // ��Ī�Ǿ� �ı��Ǵ� �� ��� ǥ��
                    e.isDestroy = true;
                colMatches[j].Clear();          // ���� ���� ���� ����Ʈ ���
            }
        }
    }

    static public void Test(List<GemInformation> gems) // �ı�
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
