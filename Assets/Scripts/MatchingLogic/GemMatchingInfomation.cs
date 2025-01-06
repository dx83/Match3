using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
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

        foreach (var list in rawMatches)
            list.Clear();
        foreach (var list in colMatches)
            list.Clear();
    }

    static List<GemInformation> mergeList = new List<GemInformation>();
    static public void IsMatchingAfterSwap(List<GemInformation> gems, GemInformation target, bool hint = false)
    {
        int index = 0;
        if (potentialMatches.Count == 0)
        {
            for (int i = 0; i < 4; i++)
                potentialMatches.Add(new List<GemInformation>());
        }

        // left
        index = target.index - 1;
        SearchForMatcing(gems, target, index, Direction.Left);
        if (matchingList.Count < 3)
            matchingList.Clear();
        else
            potentialMatches[0].AddRange(matchingList);

        SearchForMatcing(gems, target, index, Direction.Up);
        mergeList = matchingList.ToList();
        SearchForMatcing(gems, target, index, Direction.Down);
        mergeList.AddRange(matchingList);

        if (mergeList.Count < 3)
            mergeList.Clear();
        else
            potentialMatches[0].AddRange(mergeList);
        mergeList.Clear();

        if (hint && potentialMatches[0].Count > 2)
            return;

        // right
        //index = target.index + 1;
        //potentialMatches[0] = SearchForMatcing(gems, target, index, Direction.Right);
        //potentialMatches[1] = SearchForMatcing(gems, target, index, Direction.Up);
        //potentialMatches[1].AddRange(SearchForMatcing(gems, target, index, Direction.Down));
        // up
        //index = target.index - Constants.COLUMNS;
        //potentialMatches[0] = SearchForMatcing(gems, target, index, Direction.Up);
        //potentialMatches[1] = SearchForMatcing(gems, target, index, Direction.Left);
        //potentialMatches[1].AddRange(SearchForMatcing(gems, target, index, Direction.Right));
        // down
        //index = target.index + Constants.COLUMNS;
        //potentialMatches[0] = SearchForMatcing(gems, target, index, Direction.Down);
        //potentialMatches[1] = SearchForMatcing(gems, target, index, Direction.Left);
        //potentialMatches[1].AddRange(SearchForMatcing(gems, target, index, Direction.Right));
    }

    static List<GemInformation> matchingList = new List<GemInformation>();// potentialMatches, ����Ʈ ���� �ʿ�
    static void SearchForMatcing(List<GemInformation> gems, GemInformation target, int index, Direction direction)
    {
        matchingList.Clear();

        int start = 0;
        int loop = 0;
        int sign = 0;
        switch(direction)
        {
            case Direction.Left:
                {
                    start = index - 1;
                    loop = index % Constants.COLUMNS;
                    sign = -1;
                    break;
                }
            case Direction.Right:
                {
                    start = index + 1;
                    loop = Constants.COLUMNS - index % Constants.COLUMNS - 1;
                    sign = 1;
                    break;
                }
            case Direction.Up:
                {
                    start = index - Constants.COLUMNS;
                    loop = index / Constants.RAWS;
                    sign = -1 * Constants.RAWS;
                    break;
                }
            case Direction.Down:
                {
                    start = index + Constants.COLUMNS;
                    loop = Constants.RAWS - index / Constants.RAWS - 1;
                    sign = 1 * Constants.RAWS;
                    break;
                }
        }

        for (int i = 0; i < loop; i++)
        {
            if (target.isSameColor(gems[start + (i * sign)]))
            {
                matchingList.Add(gems[i]);
            }
            else
                break;
        }
    }


    static public void Test(List<GemInformation> gems) // �׽�Ʈ �ı�
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
